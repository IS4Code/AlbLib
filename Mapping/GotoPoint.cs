using System;
using System.IO;
using AlbLib.Texts;

namespace AlbLib.Mapping
{
	/// <summary>
	/// 3D map goto-point.
	/// </summary>
	[Serializable]
	public class GotoPoint
	{
		public byte X{get;set;}
		public byte Y{get;set;}
		private short unk1;
		public string Name{get;set;}
		
		public GotoPoint(Stream input) : this(new BinaryReader(input, TextCore.DefaultEncoding))
		{}
		
		public GotoPoint(BinaryReader reader)
		{
			X = (byte)(reader.ReadByte()-1);
			Y = (byte)(reader.ReadByte()-1);
			unk1 = reader.ReadInt16();
			Name = TextCore.TrimNull(reader.ReadChars(15));
		}
	}
}
