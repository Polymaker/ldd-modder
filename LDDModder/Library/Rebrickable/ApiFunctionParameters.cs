﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LDDModder.Rebrickable
{
    public class ApiFunctionParameters
    {
        const string API_FORMAT = "xml";

        [ApiParameter("key")]
        protected string Key
        {
            get { return RebrickableAPIv2.API_KEY; }
        }

        [ApiParameter("format")]
        protected string Format
        {
            get { return API_FORMAT; }
        }

        protected List<Tuple<string, string>> GetParameters()
        {
            var paramValList = new List<Tuple<string, string>>();
            foreach (var propInfo in GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attrs = (ApiParameterAttribute[])propInfo.GetCustomAttributes(typeof(ApiParameterAttribute), true);
                if (attrs.Length == 0)
                    continue;

                if (attrs.Length == 1)
                {
                    string paramValue = GetParamValue(propInfo);
                    if (string.IsNullOrEmpty(paramValue) && !attrs[0].Optional)
                        throw new ArgumentNullException(attrs[0].ParamName);
                    paramValList.Add(new Tuple<string, string>(attrs[0].ParamName, paramValue));
                }
            }
            return paramValList;
        }

        private string GetParamValue(PropertyInfo propInfo)
        {
            object propVal = propInfo.GetValue(this, null);
            if (propInfo.PropertyType == typeof(bool))
                return (((bool)propVal == true) ? 1 : 0).ToString();
            return propVal.ToString();
        }

        public string GetParamsUrl()
        {
            string getUrl = string.Empty;
            var paramList = GetParameters();
            for (int i = 0; i < paramList.Count; i++)
            {
                getUrl += string.Format("{0}={1}", paramList[i].Item1, paramList[i].Item2);
                if (i < paramList.Count - 1)
                    getUrl += "&";
            }
            return getUrl;
        }

        public NameValueCollection GetPostParams()
        {
            var post = new NameValueCollection();
            var paramList = GetParameters();
            for (int i = 0; i < paramList.Count; i++)
                post[paramList[i].Item1] = paramList[i].Item2;
            return post;
        }

    }
}
