using System;
using System.IO;

namespace AlbLib.Sounds
{
	/// <summary>
	/// Music in XMidi (XMI) format. Incomplete.
	/// </summary>
	[Serializable]
	public class XMidiMusic
	{
		/// <summary>
		/// Number of tracks.
		/// </summary>
		public short NumTracks{get;private set;}
		
		/// <summary>
		/// Creates new 
		/// </summary>
		/// <param name="xmidi">
		/// 
		/// </param>
		public XMidiMusic(byte[] xmidi)
		{
			using(MemoryStream stream = new MemoryStream(xmidi))
			{
				//ReadNext(new BinaryReader(stream, Encoding.ASCII));
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stream">
		/// 
		/// </param>
		public XMidiMusic(Stream stream)
		{
			//ReadNext(new BinaryReader(stream, Encoding.ASCII));
		}
	}
}