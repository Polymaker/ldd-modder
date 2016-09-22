using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.General
{
    [TypeConverter(typeof(VersionInfoConverter))]
    public class VersionInfo : INotifyPropertyChanged
    {
        // Fields...
        private int _Minor;
        private int _Major;
        
        public int Major
        {
        	get	{ return _Major; }
        	set
        	{
        		if(_Major == value)
        		  return;
        		_Major = value;
        		OnPropertyChanged("Major");
        	}
        }

        public int Minor
        {
        	get	{ return _Minor; }
        	set
        	{
        		if(_Minor == value)
        		  return;
        		_Minor = value;
        		OnPropertyChanged("Minor");
        	}
        }

        public VersionInfo()
        {
            _Major = 0;
            _Minor = 0;
        }

        public VersionInfo(int major, int minor)
        {
            _Major = major;
            _Minor = minor;
        }

        public static bool operator ==(VersionInfo v1, VersionInfo v2)
        {
            return v1.Major == v2.Major && v1.Minor == v2.Minor;
        }

        public static bool operator !=(VersionInfo v1, VersionInfo v2)
        {
            return !(v1 == v2);
        }

        public static bool operator >(VersionInfo v1, VersionInfo v2)
        {
            return v1.Major > v2.Major || (v1.Major == v2.Major && v1.Minor > v2.Minor);
        }

        public static bool operator >=(VersionInfo v1, VersionInfo v2)
        {
            return v1 == v2 || v1 > v2;
        }

        public static bool operator <(VersionInfo v1, VersionInfo v2)
        {
            return v1.Major < v2.Major || (v1.Major == v2.Major && v1.Minor < v2.Minor);
        }

        public static bool operator <=(VersionInfo v1, VersionInfo v2)
        {
            return v1 == v2 || v1 < v2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is VersionInfo))
                return base.Equals(obj);
            return (obj as VersionInfo) == this;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Major.GetHashCode();
                hash = hash * 23 + Minor.GetHashCode();
                return hash;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
