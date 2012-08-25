using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AlbLib
{
	namespace Sounds
	{
		/// <summary>
		/// Music in XMidi (XMI) format. Incomplete.
		/// </summary>
		public class XMidiMusic
		{
			#pragma warning disable 1591
			public short NumTracks{get;private set;}
			
			public XMidiMusic(byte[] xmidi)
			{
				using(MemoryStream stream = new MemoryStream(xmidi))
				{
					//ReadNext(new BinaryReader(stream, Encoding.ASCII));
				}
			}
			
			public XMidiMusic(Stream stream)
			{
				//ReadNext(new BinaryReader(stream, Encoding.ASCII));
			}
		}
		
		public abstract class SoundBase
		{
			protected static readonly byte[] WaveHeader = {0x52, 0x49, 0x46, 0x46, 0x92, 0x3B, 0x00, 0x00, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x11, 0x2B, 0x00, 0x00, 0x88, 0x58, 0x01, 0x00, 0x08, 0x00, 0x08, 0x00, 0x64, 0x61, 0x74, 0x61, 0x6E, 0x3B, 0x00, 0x00};
		}
		
		public class RawPCMSound : SoundBase
		{
			public int Frequency{get; set;}
			public byte[] SoundData{get;private set;}
			
			private RawPCMSound()
			{
				Frequency = 11025;
			}
			
			public RawPCMSound(byte[] data) : this()
			{
				SoundData = data;
			}
			
			public RawPCMSound(Stream stream, int length) : this()
			{
				BinaryReader reader = new BinaryReader(stream);
				SoundData = reader.ReadBytes(length);
			}
			
			public byte[] ToWAVE()
			{
				byte[] wave = new byte[44+SoundData.Length];
				int chunkSize = SoundData.Length+36;
				int subchunkSize = SoundData.Length;
				WaveHeader.CopyTo(wave, 0);
				BitConverter.GetBytes(chunkSize).CopyTo(wave, 4);
				BitConverter.GetBytes(Frequency).CopyTo(wave, 24);
				BitConverter.GetBytes(subchunkSize).CopyTo(wave, 40);
				SoundData.CopyTo(wave, 44);
				return wave;
			}
			
			public MemoryStream ToWAVEStream()
			{
				MemoryStream stream = new MemoryStream(ToWAVE());
				return stream;
			}
		}
		
		public class HeaderedPCMSound : SoundBase
		{
			
		}
	}
}