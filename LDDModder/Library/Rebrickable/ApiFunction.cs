﻿using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LDDModder.Rebrickable
{
    public class ApiFunction<P, R> where P : ApiFunctionParameters
    {
        // Fields...
        private string _FunctionName;
        private RequestMethod _Method;

        public string FunctionName
        {
            get { return _FunctionName; }
        }

        public RequestMethod Method
        {
            get { return _Method; }
        }

        public string FunctionUrl
        {
            get { return RebrickableAPI.API_URL + FunctionName; }
        }

        public ApiFunction(string functionName)
        {
            _FunctionName = functionName;
            _Method = RequestMethod.GET;
        }

        public ApiFunction(string functionName, RequestMethod method)
        {
            _FunctionName = functionName;
            _Method = method;
        }

        public R Execute(P funcParam)
        {
            R result;
            Execute(funcParam, out result);
            return result;
        }

        class BreakException : Exception { }

        /// <summary>
        /// Returns true if the function succeeded. If the function failed, see <see cref="RebrickableAPI.GetLastError()"/>
        /// </summary>
        /// <param name="funcParam"></param>
        /// <param name="result"></param>
        /// <returns>Returns true if the function succeeded. If the function failed, see <see cref="RebrickableAPI.GetLastError()"/></returns>
        public bool Execute(P funcParam, out R result)
        {
            RebrickableAPI.LastError = null;
            try
            {
                byte[] resultData = null;
                var timer = new Stopwatch();
                timer.Start();
                if (Method == RequestMethod.GET)
                {
                    resultData = RebrickableAPI.DownloadWebPage(string.Format("{0}?{1}", FunctionUrl, funcParam.GetParamsUrl()));
                }
                else
                {
                    resultData = RebrickableAPI.DownloadWebPage(FunctionUrl, funcParam.GetPostParams());
                }

                timer.Stop();
                Trace.WriteLine("Executed and received Rebrickable API function result in " + timer.Elapsed);

                string resultText = Encoding.UTF8.GetString(resultData);

                if (resultText.Length < 10)
                {
                    RebrickableAPI.LastError = ApiError.FindError(resultText);
                    if (RebrickableAPI.LastError != null)
                        throw new BreakException();
                }

                if (typeof(R) == typeof(string))
                {
                    result = (R)((object)resultText);
                    return true;
                }
                timer.Restart();
                var xDoc = XDocument.Parse(resultText);
                string rootName = LDDModder.Serialization.XSerializationHelper.GetTypeXmlRootName(typeof(R));
                var resultElem = xDoc.Root;

                if (resultElem.Name.LocalName != rootName)
                    resultElem = xDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == rootName);

                if (resultElem == null)
                {
                    RebrickableAPI.LastError = new ApiError(string.Empty, "Error parsing xml");
                    throw new BreakException();
                }

                result = LDDModder.Serialization.XSerializationHelper.DefaultDeserialize<R>(resultElem);
                timer.Stop();
                Trace.WriteLine("Parsed Rebrickable API function result in " + timer.Elapsed);

                return true;

            }
            catch (BreakException ex)
            {
                result = default(R);
                return RebrickableAPI.LastError == null;
            }
            catch (Exception ex)
            {
                RebrickableAPI.LastError = new ApiError(ex.GetType().Name, ex.Message);
            }

            result = default(R);

            return RebrickableAPI.LastError == null;
        }
    }

    public class ApiFunction<R> : ApiFunction<ApiFunctionParameters, R>
    {

        public ApiFunction(string functionName)
            : base(functionName)
        {
            
        }

        public ApiFunction(string functionName, RequestMethod method)
            : base(functionName, method)
        {
            
        }

        public R Execute()
        {
            return Execute(new ApiFunctionParameters());
        }

        public bool Execute(out R result)
        {
            return Execute(new ApiFunctionParameters(), out result);
        }
    }
}