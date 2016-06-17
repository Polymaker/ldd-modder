using LDDModder.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.PaletteMaker.Rebrickable
{
    public class ApiFunction<P, R> where P : ApiFunctionParameters
    {
        // Fields...
        private string _FunctionName;
        private XmlSerializer ResultParser;
        private bool SkipRoot = false;
        private WebMethod Method = WebMethod.GET;

        public string FunctionName
        {
            get { return _FunctionName; }
        }
 
        internal ApiFunction(string functionName)
        {
            _FunctionName = functionName;
            ResultParser = new XmlSerializer(typeof(R));
        }

        public ApiFunction(string functionName, bool skipRoot)
            :this(functionName)
        {
            SkipRoot = skipRoot;
        }

        public ApiFunction(string functionName, WebMethod method)
            : this(functionName)
        {
            Method = method;
        }

        public ApiFunction(string functionName, bool skipRoot, WebMethod method)
            : this(functionName)
        {
            SkipRoot = skipRoot;
            Method = method;
        }

        public R Execute(P funcParam)
        {
            byte[] resultData = null;
            string funcUrl = string.Format("{0}{1}", RebrickableAPI.API_URL, FunctionName);

            if (Method == WebMethod.GET)
            {
                funcUrl += string.Format("?{0}", funcParam.GetParamsUrl());
                resultData = RebrickableAPI.DownloadWebPage(funcUrl);
            }
            else
            {
                resultData = RebrickableAPI.DownloadWebPage(funcUrl, funcParam.GetPostParams());
            }

            if (resultData == null || resultData.Length <= 20)
                return default(R);

            if (typeof(R) == typeof(string))
                return (R)((object)Encoding.UTF8.GetString(resultData));

            using (var ms = new MemoryStream(resultData))
            {
                ms.Seek(0, SeekOrigin.Begin);
                if (SkipRoot)
                {
                    var doc = XDocument.Load(ms);
                    return XSerializationHelper.DefaultDeserialize<R>(doc.Root.Elements().First());
                }
                return (R)ResultParser.Deserialize(ms);
            }
        }
    }

    public class ApiFunction<R>
    {
        // Fields...
        private string _FunctionName;
        private XmlSerializer ResultParser;
        private bool SkipRoot = false;
        public string FunctionName
        {
            get { return _FunctionName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiFunction"/> class.
        /// </summary>
        /// <param name="functionName"></param>
        internal ApiFunction(string functionName)
        {
            _FunctionName = functionName;
            ResultParser = new XmlSerializer(typeof(R));
        }

        public ApiFunction(string functionName, bool skipRoot)
            : this(functionName)
        {
            SkipRoot = skipRoot;
        }

        public R Execute()
        {
            var funcUrl = string.Format("{0}{1}?key={2}", RebrickableAPI.API_URL, FunctionName, RebrickableAPI.API_KEY);
            var resultData = RebrickableAPI.DownloadWebPage(funcUrl);

            if (resultData == null || resultData.Length <= 20)
                return default(R);

            using (var ms = new MemoryStream(resultData))
            {
                ms.Seek(0, SeekOrigin.Begin);
                if (SkipRoot)
                {
                    var doc = XDocument.Load(ms);
                    return XSerializationHelper.DefaultDeserialize<R>(doc.Root.Elements().First());
                }
                return (R)ResultParser.Deserialize(ms);
            }
        }
    }
}
