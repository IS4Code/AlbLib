using System;
using System.IO;

namespace AlbLib.Mapping
{
	public struct ObjectData : IMinimapVisible
	{
		public byte MinimapType{get;private set;}
		public SubObject[] SubObjects{get;private set;}
		
		public bool VisibleOnMinimap{get{return MinimapType>1;}}
		
		public ObjectData(Stream input) : this(new BinaryReader(input))
		{}
		
		public ObjectData(BinaryReader reader) : this()
		{
			MinimapType = reader.ReadByte();
			reader.ReadByte();
			
			SubObjects = new SubObject[8];
			for(int i = 0; i < 8; i++)
			{
				SubObjects[i] = new SubObject(reader);
			}
		}
		
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
