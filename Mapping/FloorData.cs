using System;
using System.IO;

namespace AlbLib.Mapping
{
	public struct FloorData : ITextured, ICollidable
	{
		public byte[] Collision{get;private set;}
		public byte AnimationsCount{get;private set;}
		public short Texture{get;private set;}
		public short TextureWidth{get{return 64;}}
		public short TextureHeight{get{return 64;}}
		public bool IsTransparent{get{return false;}}
		
		public FloorData(Stream input) : this(new BinaryReader(input))
		{}
		
		public FloorData(BinaryReader reader) : this()
		{
			Collision = reader.ReadBytes(3);
			reader.ReadByte();
			AnimationsCount = reader.ReadByte();
			reader.ReadByte();
			Texture = reader.ReadInt16();
			reader.ReadInt16();
		}
	}
}
