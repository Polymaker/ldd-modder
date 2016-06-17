﻿using LDDModder.Utilities;
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
        internal bool SkipRoot = false;
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

        public R Execute(P funcParam)
        {
            var funcUrl = string.Format("{0}{1}?{2}", RebrickableAPI.API_URL, FunctionName, funcParam.GetParamsUrl());
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

    public class ApiFunction<R>
    {
        // Fields...
        private string _FunctionName;
        private XmlSerializer ResultParser;
        internal bool SkipRoot = false;
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
