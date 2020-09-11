using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Utilities
{
    public class FlagManager
    {
        private Dictionary<string, bool> mFlags;

        //TODO: implement multithreading read-write lock

        public bool Any { get { return mFlags.Values.Any(v => v); } }

        public bool All { get { return mFlags.Values.All(v => v); } }

        public IReadOnlyDictionary<string, bool> Flags => mFlags;

        public bool this[string flagName]
        {
            get => IsSet(flagName);
            set => Set(flagName, value);
        }

        public FlagManager()
        {
            mFlags = new Dictionary<string, bool>();
        }

        private KeyValuePair<string, bool> GetFlag(string name)
        {
            return mFlags.FirstOrDefault(x => 
            x.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool IsSet(string flagName)
        {
            var flag = GetFlag(flagName);
            return flag.Value;
        }

        public void Set(string flagName)
        {
            var flag = GetFlag(flagName);
            if (!string.IsNullOrEmpty(flag.Key))
                flagName = flag.Key;
            mFlags[flagName] = true;
        }

        public void Unset(string flagName)
        {
            var flag = GetFlag(flagName);
            if (!string.IsNullOrEmpty(flag.Key))
                mFlags[flagName] = false;
        }

        public void Set(string flagName, bool value)
        {
            var flag = GetFlag(flagName);
            if (!string.IsNullOrEmpty(flag.Key))
                flagName = flag.Key;
            mFlags[flagName] = value;
        }

        public void Toggle(string flagName)
        {
            var flag = GetFlag(flagName);
            if (string.IsNullOrEmpty(flag.Key))
                mFlags.Add(flagName, true);
            else
                mFlags[flag.Key] = !flag.Value;
        }

        public IDisposable UseFlag(string name)
        {
            Set(name, true);
            return new TempFlag(this, name);
        }

        private class TempFlag : IDisposable
        {
            public FlagManager Manager { get; }
            public string FlagName { get; }

            public TempFlag(FlagManager manager, string flagName)
            {
                Manager = manager;
                FlagName = flagName;
            }

            public void Dispose()
            {
                Manager.Set(FlagName, false);
            }
        }
    }
}
