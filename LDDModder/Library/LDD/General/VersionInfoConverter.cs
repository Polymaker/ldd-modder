using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;

namespace LDDModder.LDD.General
{
    public class VersionInfoConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null || !(value is VersionInfo))
                return base.ConvertTo(context, culture, value, destinationType);
            var vInfo = (VersionInfo)value;

            if (destinationType == typeof(string))
            {
                return string.Format("{0}.{1}", vInfo.Major, vInfo.Minor);
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                var ctroInfo = typeof(VersionInfo).GetConstructor(new Type[] { typeof(int), typeof(int) });
                return new InstanceDescriptor(ctroInfo, new object[] { vInfo.Major, vInfo.Minor });
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                var versionStr = (string)value;
                var match = Regex.Match(versionStr, "(\\d+)\\.(\\d+)");
                if (match.Success)
                {
                    return new VersionInfo(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
