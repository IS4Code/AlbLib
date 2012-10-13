using System;
using System.IO;

namespace AlbLib.Mapping
{
	public struct ObjectInfo : ITextured, ICollidable
	{
		public byte Type{get;private set;}
		public byte[] Collision{get;private set;}
		public short Texture{get;private set;}
		public byte AnimationsCount{get;private set;}
		public short TextureWidth{get;private set;}
		public short TextureHeight{get;private set;}
		public short MapWidth{get;private set;}
		public short MapHeight{get;private set;}
		public bool IsTransparent{get{return true;}}
		
		public ObjectInfo(Stream input) : this(new BinaryReader(input))
		{}
		
		public ObjectInfo(BinaryReader reader) : this()
		{
			Type = reader.ReadByte();
			Collision = reader.ReadBytes(3);
			Texture = reader.ReadInt16();
			AnimationsCount = reader.ReadByte();
			reader.ReadByte();
			TextureWidth = reader.ReadInt16();
			TextureHeight = reader.ReadInt16();
			MapWidth = reader.ReadInt16();
			MapHeight = reader.ReadInt16();
		}
	}
}
