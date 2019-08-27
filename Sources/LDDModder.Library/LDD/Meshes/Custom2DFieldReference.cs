﻿using System.Collections.Generic;
using System.Linq;

namespace LDDModder.LDD.Meshes
{
    public class Custom2DFieldReference
    {
        /// <summary>
        /// Index of the related Custom2DField connector in the primitive XML file.
        /// </summary>
        public int ConnectorIndex { get; set; }
        public List<Custom2DFieldIndex> FieldIndices { get; set; }

        public Custom2DFieldReference()
        {
            FieldIndices = new List<Custom2DFieldIndex>();
        }

        public Custom2DFieldReference(LDD.Files.MeshStructures.STUD_2DFIELD_REF _2DFieldRef)
        {
            ConnectorIndex = _2DFieldRef.ConnectorIndex;
            FieldIndices = new List<Custom2DFieldIndex>();
            for (int i = 0; i < _2DFieldRef.Indices.Length; i++)
                FieldIndices.Add(new Custom2DFieldIndex(_2DFieldRef.Indices[i]));
        }

        public LDD.Files.MeshStructures.STUD_2DFIELD_REF Serialize()
        {
            return new Files.MeshStructures.STUD_2DFIELD_REF(ConnectorIndex, FieldIndices.Select(x=>x.Serialize()).ToArray());
        }
    }
}
