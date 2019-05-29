using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Data
{
    public class Platform
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Platform()
        {
        }

        public Platform(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is Platform platform &&
                   ID == platform.ID &&
                   Name == platform.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 1479869798;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + Name.GetHashCode();
            return hashCode;
        }

        public static readonly Platform System = new Platform(200, "SYSTEM");


    }
}
