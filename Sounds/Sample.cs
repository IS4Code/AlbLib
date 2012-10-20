using System;
using System.IO;

namespace AlbLib.Sounds
{
	[Serializable]
	public class Sample
	{
		public bool Used{get; set;}
		public int Index{get; set;}
		public int Type{get; set;}
		public int StartOffset{get; set;}
		public int Length{get; set;}
		public int Rate{get; set;}
		public RawPCMSound Sound{get; set;}
		
		public Sample(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);			if(reader.ReadInt32() != -1)Used = true;
			Index = reader.ReadInt32();
			Type = reader.ReadInt32();
			StartOffset = reader.ReadInt32();
			Length = reader.ReadInt32();
			reader.ReadInt32();
			reader.ReadInt32();
			Rate = reader.ReadInt32();
		}
	}
}
