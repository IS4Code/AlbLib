using System;
using System.IO;

namespace AlbLib.Mapping
{
	public struct Overlay : ITextured
	{
		public short Texture{get;private set;}
		public byte AnimationsCount{get;private set;}
		public bool IsTransparent{get;private set;}
		public short X{get;private set;}
		public short Y{get;private set;}
		public short TextureWidth{get;private set;}
		public short TextureHeight{get;private set;}
		
		public Overlay(Stream input) : this(new BinaryReader(input))
		{}
		
		public Overlay(BinaryReader reader) : this()
		{
			Texture = reader.ReadInt16();
			AnimationsCount = reader.ReadByte();
			IsTransparent = reader.ReadByte()!=0;
			X = reader.ReadInt16();
			Y = reader.ReadInt16();
			TextureWidth = reader.ReadInt16();
			TextureHeight = reader.ReadInt16();
		}
	}
}
