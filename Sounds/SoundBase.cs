using System;
using System.IO;

namespace AlbLib.Sounds
{
	public abstract class SoundBase
	{
		private static readonly byte[] waveheader = {0x52, 0x49, 0x46, 0x46, 0x00, 0x00, 0x00, 0x00, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x08, 0x00, 0x64, 0x61, 0x74, 0x61, 0x6E, 0x3B, 0x00, 0x00};
		
		protected static void WriteWAVEHeader(Stream output, int frequency, int subchunkSize)
		{
			BinaryWriter writer = new BinaryWriter(output);
			writer.Write(waveheader, 0, 4);
			writer.Write(subchunkSize+36);
			writer.Write(waveheader, 8, 16);
			writer.Write(frequency);
			writer.Write(frequency);
			writer.Write(waveheader, 32, 8);
			writer.Write(subchunkSize);
			writer.Flush();
		}
		
		protected static byte[] GetWAVEHeader(int frequency, int subchunkSize)
		{
			byte[] wave = new byte[44];
			waveheader.CopyTo(wave, 0);
			BitConverter.GetBytes(subchunkSize+36).CopyTo(wave, 4);
			BitConverter.GetBytes(frequency).CopyTo(wave, 24);
			BitConverter.GetBytes(frequency).CopyTo(wave, 28);
			BitConverter.GetBytes(subchunkSize).CopyTo(wave, 40);
			return wave;
		}
	}
}