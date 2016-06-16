using LDDModder.LDD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace LDDModder
{
    //[Serializable]
    class PreferenceEntry
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public LDDLocation Location { get; set; }

        public string Serialize()
        {
            return string.Format("{2}){0}={1}", Key, Value, (int)Location);
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    new BinaryFormatter().Serialize(stream, this);
            //    return Convert.ToBase64String(stream.ToArray());
            //}
        }

        public static PreferenceEntry Deserialize(string entryStr)
        {
            string locStr = entryStr.Substring(0, entryStr.IndexOf(')'));
            entryStr = entryStr.Substring(locStr.Length + 1);
            string key = entryStr.Substring(0, entryStr.IndexOf('='));
            string value = entryStr.Substring(key.Length + 1);
            return new PreferenceEntry() 
            { 
                Key = key, 
                Value = value, 
                Location = (LDDLocation)int.Parse(locStr)
            };
            //byte[] bytes = Convert.FromBase64String(str);

            //using (MemoryStream stream = new MemoryStream(bytes))
            //{
            //    return (PreferenceEntry)new BinaryFormatter().Deserialize(stream);
            //}
        }
    }
}
