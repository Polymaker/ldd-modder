using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Data
{
    public class MainGroup
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public MainGroup()
        {
        }

        public MainGroup(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is MainGroup group &&
                   ID == group.ID &&
                   Name == group.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = 1479869798;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + Name.GetHashCode();
            return hashCode;
        }

        public static readonly MainGroup Bricks = new MainGroup(201, "BRICKS");

        public override string ToString()
        {
            return $"{ID} - {Name}";
        }
    }
}
