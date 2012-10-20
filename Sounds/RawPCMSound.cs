using System;
using System.IO;

namespace AlbLib.Sounds
{
	[Serializable]
	public class RawPCMSound : SoundBase
	{
		/// <summary>
		/// Frequency of PCM sound.
		/// </summary>
		public int Rate{get; set;}
		
		/// <summary>
		/// Raw sound data.
		/// </summary>
		public byte[] SoundData{get; private set;}
		
		private RawPCMSound()
		{
			Rate = 11025;
		}
		
		/// <summary>
		/// Loads sound from byte array.
		/// </summary>
		/// <param name="data">
		/// Raw PCM data.
		/// </param>
		public RawPCMSound(byte[] data) : this()
		{
			SoundData = data;
		}
		
		/// <summary>
		/// Loads sound from stream.
		/// </summary>
		/// <param name="stream">
		/// Input stream.
		/// </param>
		/// <param name="length">
		/// Length of
		/// </param>
		public RawPCMSound(Stream stream, int length) : this()
		{
			SoundData = new byte[length];
			stream.Read(SoundData, 0, length);
		}
		
		/// <summary>
		/// Converts sound to wave format.
		/// </summary>
		/// <returns>
		/// Byte array containing whole WAVE data.
		/// </returns>
		public byte[] ToWAVE()
		{
			byte[] wave = new byte[44+SoundData.Length];
			GetWAVEHeader(Rate, SoundData.Length).CopyTo(wave, 0);
			SoundData.CopyTo(wave, 44);
			return wave;
		}
		
		public int WriteWAVEToStream(Stream output)
		{
			WriteWAVEHeader(output, Rate, SoundData.Length);
			output.Write(SoundData, 0, SoundData.Length);
			output.Flush();
			return 44+SoundData.Length;
		}
		
		public MemoryStream ToWAVEStream()
		{
			return new MemoryStream(ToWAVE());
		}
	}
}