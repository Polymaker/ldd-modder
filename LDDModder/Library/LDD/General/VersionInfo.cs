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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
