using System;
using System.IO;

namespace AlbLib.Mapping
{
	public class Event
	{
		public byte Type{get;set;}
		public byte Byte1{get;set;}
		public byte Byte2{get;set;}
		public byte Byte3{get;set;}
		public byte Byte4{get;set;}
		public byte Byte5{get;set;}
		public short Word6{get;set;}
		public short Word8{get;set;}
		public short Next{get;set;}
		
		public Event(Stream stream) : this(new BinaryReader(stream))
		{}
		
		public Event(BinaryReader reader)
		{
			Type = reader.ReadByte();
			Byte1 = reader.ReadByte();
			Byte2 = reader.ReadByte();
			Byte3 = reader.ReadByte();
			Byte4 = reader.ReadByte();
			Byte5 = reader.ReadByte();
			Word6 = reader.ReadInt16();
			Word8 = reader.ReadInt16();
			Next = reader.ReadInt16();
		}
	}
}
