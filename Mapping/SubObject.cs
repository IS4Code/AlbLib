using System;
using System.IO;

namespace AlbLib.Mapping
{
	public partial struct ObjectData : IMinimapVisible
	{
		[Serializable]
		public struct SubObject
		{
			public short X{get;private set;}
			public short Y{get;private set;}
			public short Z{get;private set;}
			public short Type{get;private set;}
			
			public bool IsEmpty{
				get{return Type==0;}
			}
			
			public SubObject(Stream input) : this(new BinaryReader(input))
			{}
			
			public SubObject(BinaryReader reader) : this()
			{
				X = reader.ReadInt16();
				Y = reader.ReadInt16();
				Z = reader.ReadInt16();
				Type = reader.ReadInt16();
			}
		}
	}
}
