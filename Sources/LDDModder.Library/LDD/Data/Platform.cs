using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Data
{
    public class Platform
    {
        [JsonProperty]
        public int ID { get; set; }
        [JsonProperty]
        public string Name { get; set; }

        [JsonIgnore]
        public string Display => $"{ID} - {Name}";

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

        public override string ToString()
        {
            return $"{ID} - {Name}";
        }

        //public static readonly Platform None = new Platform(0, "None");
        //public static readonly Platform System = new Platform(200, "SYSTEM");
        //public static readonly Platform Technic = new Platform(300, "TECHNIC");
        //public static readonly Platform ActionFigures = new Platform(500, "ACTION FIGURES");
    }
}
