using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files.MeshStructures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MESH_HEADER
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Header;
        public int VertexCount;
        public int IndexCount;
        public int MeshType;
    }
}
