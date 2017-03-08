/* Date: 17.12.2016, Time: 16:39 */
using System;
using System.IO;

namespace AlbLib.Sounds
{
	[Serializable]
	public class WaveLib : IGameResource
	{
		public Header[] Headers{get; private set;}
		public RawPCMSound Sound{get; set;}
		
		public WaveLib(Stream stream)
		{
			Headers = new Header[512];
			BinaryReader reader = new BinaryReader(stream);
			for(int i = 0; i < 512; i++)
			{
				Headers[i] = new Header(reader);
			}
			Sound = new RawPCMSound(stream, (int)(stream.Length-stream.Position));
		}
		
		public int Save(Stream output)
		{
			throw new NotImplementedException();
		}
		
		public struct Header
		{
			public int Index;
			public int Start;
			public int Length;
			public int SampleRate;
			
			public Header(BinaryReader reader)
			{
				reader.ReadInt32();
				Index = reader.ReadInt32();
				reader.ReadInt32();
				Start = reader.ReadInt32();
				Length = reader.ReadInt32();
				reader.ReadInt32();
				reader.ReadInt32();
				SampleRate = reader.ReadInt32();
			}
		}
	}
}
