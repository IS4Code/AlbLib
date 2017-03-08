using System;
using System.IO;

namespace AlbLib.Mapping
{
	[Serializable]
	public struct WallData : ITextured, IMinimapVisible, ICollidable
	{
		public byte Type{get;private set;}
		public byte[] Collision{get;private set;}
		public short Texture{get;private set;}
		public byte AnimationsCount{get;private set;}
		public byte MinimapType{get;private set;}
		public byte TransparentColor{get;private set;}
		public short TextureWidth{get;private set;}
		public short TextureHeight{get;private set;}
		public Overlay[] Overlays{get;private set;}
		public bool IsTransparent{get{return (Type&32)!=0 || IsTranslucent;}}
		public bool IsTranslucent{get{return (Type&64)!=0;}}
		
		public bool VisibleOnMinimap{get{return MinimapType>1;}}
		
		public WallData(Stream input) : this(new BinaryReader(input))
		{}
		
		public WallData(BinaryReader reader) : this()
		{
			Type = reader.ReadByte();
			Collision = reader.ReadBytes(3);
			Texture = reader.ReadInt16();
			AnimationsCount = reader.ReadByte();
			MinimapType = reader.ReadByte();
			TransparentColor = reader.ReadByte();
			reader.ReadByte();
			TextureHeight = reader.ReadInt16();
			TextureWidth = reader.ReadInt16();
			
			short numoverlays = reader.ReadInt16();
			Overlays = new Overlay[numoverlays];
			for(int i = 0; i < numoverlays; i++)
			{
				Overlays[i] = new Overlay(reader);
			}
		}

		public WallData(byte type, byte[] collision, short texture, byte anims, byte minimap, byte transp, short twidth, short theight, Overlay[] overlays) : this()
		{
			Type = type;
			Collision = collision;
			Texture = texture;
			AnimationsCount = anims;
			MinimapType = minimap;
			TransparentColor = transp;
			TextureWidth = twidth;
			TextureHeight = theight;
			Overlays = overlays;
		}
	}
}
