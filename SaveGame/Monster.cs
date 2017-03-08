/* Date: 26.8.2014, Time: 0:12 */
using System;
using System.IO;
using AlbLib.Texts;

namespace AlbLib.SaveGame
{
	public class Monster : Character
	{
		public byte Sprite{get;set;}
		private byte unknown1;
		private byte[] unknown2;
		public byte OffsetY{get;set;}
		public byte ScreenOffset{get;set;}
		public short Width{get;set;}
		public short Height{get;set;}
		
		public Monster() : base()
		{
			unknown2 = new byte[266];
		}
		
		public Monster(Stream stream) : this(new BinaryReader(stream, TextCore.DefaultEncoding))
		{
			
		}
		
		public Monster(BinaryReader reader) : base(reader)
		{
			Sprite = reader.ReadByte();
			unknown1 = reader.ReadByte();
			unknown2 = reader.ReadBytes(266);
			OffsetY = reader.ReadByte();
			ScreenOffset = reader.ReadByte();
			Width = reader.ReadInt16();
			Height = reader.ReadInt16();
		}
	}
}
