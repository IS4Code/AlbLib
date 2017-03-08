using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlbLib.XLD
{
	/// <summary>
	/// This class holds all information about XLD file.
	/// </summary>
	[Serializable]
	public class XLDFile : IEnumerable<XLDSubfile>
	{
		/// <summary>
		/// XLD header signature as string.
		/// </summary>
		/// <returns>
		/// XLD0I\0
		/// </returns>
		public const  string Signature = "XLD0I\0";
		/// <summary>
		/// XLD header signature as char array.
		/// </summary>
		/// <returns>
		/// X, L, D, 0, I, \0
		/// </returns>
		public static char[] SignatureChars{get{return Signature.ToCharArray();}}
		/// <summary>
		/// XLD header signature as byte array.
		/// </summary>
		/// <returns>
		/// 58-4C-44-30-49-00
		/// </returns>
		public static byte[] SignatureBytes{get{return Encoding.ASCII.GetBytes(Signature);}}
		
		/// <summary>
		/// Count of subfiles in this file.
		/// </summary>
		public int Count{
			get{
				return Subfiles.Count;
			}
		}
		
		/// <summary>
		/// List of all subfiles.
		/// </summary>
		public IList<XLDSubfile> Subfiles{get;private set;}
		
		/// <summary></summary>
		/// <param name="subfiles">
		/// List of subfiles.
		/// </param>
		public XLDFile(IList<XLDSubfile> subfiles)
		{
			Subfiles = subfiles;
		}
		
		/// <summary>
		/// Enumerates through all subfiles.
		/// </summary>
		/// <returns>
		/// Subfile enumerator.
		/// </returns>
		public IEnumerator<XLDSubfile> GetEnumerator()
		{
			return Subfiles.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Subfiles.GetEnumerator();
		}
		
		/// <summary>
		/// Writes XLD to stream.
		/// </summary>
		/// <param name="output">
		/// Output stream to write.
		/// </param>
		/// <returns>
		/// Count of bytes written.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Thrown when <see cref="Count"/> is not in range between <see cref="UInt16.MinValue"/> and <see cref="UInt16.MaxValue"/>.
		/// </exception>
		public int Save(Stream output)
		{
			if(Count > ushort.MaxValue || Count < ushort.MinValue)
			{
				throw new ArgumentOutOfRangeException("Count", "Count of subfiles is too big or negative. XLD cannot be created.");
			}
			BinaryWriter writer = new BinaryWriter(output);
			writer.Write(SignatureBytes);
			writer.Write((ushort)Count);
			int written = 8;
			foreach(XLDSubfile subfile in Subfiles)
			{
				if(subfile == null)
				{
					writer.Write(0);
					written += 4;
				}else{
					writer.Write(subfile.Length);
					written += 4 + subfile.Length;
				}
			}
			foreach(XLDSubfile subfile in Subfiles)
			{
				if(subfile == null)continue;
				if(subfile.Data != null)
					writer.Write(subfile.Data);
			}
			return written;
		}
		
		/// <summary>
		/// Writes XLD to memory and return.
		/// </summary>
		/// <returns>
		/// Byte array containing XLD.
		/// </returns>
		/// <seealso cref="Save"/>
		public byte[] ToByteArray()
		{
			MemoryStream stream = new MemoryStream();
			Save(stream);
			return stream.ToArray();
		}
		
		/*/// <summary>
		/// Simply reads to a subfile at the given index. Stream must be set to beginning of XLD format.
		/// </summary>
		/// <param name="stream">
		/// Input stream. There must start the XLD.
		/// </param>
		/// <param name="index">
		/// Zero-based index of subfile.
		/// </param>
		/// <returns>
		/// Length of subfile.
		/// </returns>
		public static int ReadToIndex(Stream stream, int index)
		{
			if(index < 0)throw new ArgumentOutOfRangeException("index");
			int lastEntry = -1;
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			short nentries = 0;
			int[] entrylen = null;
			
			string sig = new string(reader.ReadChars(6));
			if(sig != "XLD0I\0")
			{
				throw new Exception("This is not valid XLD file.");
			}
			nentries = reader.ReadInt16();
			if(index >= nentries)throw new ArgumentOutOfRangeException("index", "Argument is greater than subfiles count.");
			entrylen = new int[nentries];
			for(int i = 0; i < nentries; i++)
			{
				entrylen[i] = reader.ReadInt32();
			}
			for(int i = 0; i < nentries; i++)
			{
				lastEntry = i;
				if(i == index)
				{
					return entrylen[i];
				}else{
					reader.ReadBytes(entrylen[i]);
				}
			}
			return -1;
		}*/
		
		/// <summary>
		/// Reads subfile at the given index.
		/// </summary>
		/// <param name="stream">
		/// Input stream. There must start the XLD.
		/// </param>
		/// <param name="index">
		/// Zero-based index of subfile.
		/// </param>
		/// <returns>
		/// Byte array containing the content of subfile.
		/// </returns>
		public static XLDSubfile ReadSubfile(Stream stream, int index)
		{
			int lastEntry = -1;
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			short nentries = 0;
			int[] entrylen = null;
			
			string sig = new string(reader.ReadChars(6));
			if(sig != "XLD0I\0")
			{
				throw new Exception("This is not valid XLD file.");
			}
			nentries = reader.ReadInt16();
			entrylen = new int[nentries];
			for(int i = 0; i < nentries; i++)
			{
				entrylen[i] = reader.ReadInt32();
			}
			for(int i = 0; i < nentries; i++)
			{
				lastEntry = i;
				if(i == index)
				{
					return new XLDSubfile(stream, entrylen[i], (short)i);
				}else{
					reader.ReadBytes(entrylen[i]);
				}
			}
			return null;
		}
		
		/// <summary>
		/// Enumerates through all subfiles.
		/// </summary>
		/// <param name="file">
		/// Path to XLD file.
		/// </param>
		/// <returns>
		/// Enumerable object containing subfile contents.
		/// </returns>
		public static IEnumerable<XLDSubfile> EnumerateSubfiles(string file)
		{
			return EnumerateSubfiles(new FileStream(file, FileMode.Open), true);
		}
		
		/// <summary>
		/// Enumerates through all subfiles.
		/// </summary>
		/// <param name="stream">
		/// Input stream. There must start the XLD.
		/// </param>
		/// <returns>
		/// Enumerable object containing subfile contents.
		/// </returns>
		public static IEnumerable<XLDSubfile> EnumerateSubfiles(Stream stream)
		{
			return EnumerateSubfiles(stream, false);
		}
		
		/// <summary>
		/// Enumerates through all subfiles.
		/// </summary>
		/// <param name="stream">
		/// Input stream. There must start the XLD.
		/// </param>
		/// <param name="close">
		/// If true, stream will be closed.
		/// </param>
		/// <returns>
		/// Enumerable object containing subfile contents.
		/// </returns>
		public static IEnumerable<XLDSubfile> EnumerateSubfiles(Stream stream, bool close)
		{
			try{
				int lastEntry = -1;
				BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
				short nentries = 0;
				int[] entrylen = null;
				
				string sig = new string(reader.ReadChars(6));
				if(sig != "XLD0I\0")
				{
					throw new ArgumentException("This is not valid XLD file.", "stream");
				}
				nentries = reader.ReadInt16();
				entrylen = new int[nentries];
				for(short i = 0; i < nentries; i++)
				{
					entrylen[i] = reader.ReadInt32();
				}
				for(short i = 0; i < nentries; i++)
				{
					lastEntry = i;
					yield return new XLDSubfile(stream, entrylen[i], i);
				}
			}finally{
				if(close)stream.Close();
			}
		}
		
		/// <summary>
		/// Parses data in XLD format.
		/// </summary>
		/// <param name="data">
		/// Byte array containing file data.
		/// </param>
		/// <returns>
		/// Parsed XLD file.
		/// </returns>
		public static XLDFile Parse(byte[] data)
		{
			using(MemoryStream stream = new MemoryStream(data))
			{
				return Parse(stream);
			}
		}
		
		/// <summary>
		/// Parses data in XLD format.
		/// </summary>
		/// <param name="path">
		/// Path to file containing XLD data.
		/// </param>
		/// <returns>
		/// Parsed XLD file.
		/// </returns>
		public static XLDFile Parse(string path)
		{
			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				return Parse(stream);
			}
		}
		
		/// <summary>
		/// Parses data in XLD format.
		/// </summary>
		/// <param name="stream">
		/// Input stream. There must start the XLD.
		/// </param>
		/// <returns>
		/// Parsed XLD file.
		/// </returns>
		public static XLDFile Parse(Stream stream)
		{
			int lastEntry = -1;
			BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
			short nentries = 0;
			int[] entrylen = null;
			string sig = new string(reader.ReadChars(6));
			if(sig != "XLD0I\0")
			{
				throw new Exception("This is not valid XLD file.");
			}
			nentries = reader.ReadInt16();
			entrylen = new int[nentries];
			for(int i = 0; i < nentries; i++)
			{
				entrylen[i] = reader.ReadInt32();
			}
			XLDSubfile[] entries = new XLDSubfile[nentries];
			for(int i = 0; i < nentries; i++)
			{
				lastEntry = i;
				entries[i] = new XLDSubfile(stream, entrylen[i], (short)i);
			}
			return new XLDFile(entries);
		}
		
		/// <summary>
		/// Moves stream to position of first XLD file to occur.
		/// </summary>
		/// <param name="stream">
		/// Input stream.
		/// </param>
		/// <returns>
		/// True if XLD file was found. Otherwise false.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Thrown when input <paramref name="stream"/> does not support seeking.
		/// </exception>
		public static bool ReadToXLD(Stream stream)
		{
			if(!stream.CanSeek)
				throw new ArgumentException("This stream cannot seek, operation is not possible.");
			while(stream.Position < stream.Length)
			{
				if(stream.ReadByte() == 0x58 &&
				   stream.ReadByte() == 0x4C &&
				   stream.ReadByte() == 0x44 &&
				   stream.ReadByte() == 0x30 &&
				   stream.ReadByte() == 0x49 &&
				   stream.ReadByte() == 0x00)
				{
					stream.Seek(-6, SeekOrigin.Current);
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Lists all XLD files in a file.
		/// </summary>
		/// <param name="path">
		/// File where to search for XLDs.
		/// </param>
		/// <returns>
		/// List of all XLD files.
		/// </returns>
		public static List<XLDFile> FindXLDs(string path)
		{
			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				return FindXLDs(stream);
			}
		}
		
		/// <summary>
		/// Lists all XLD files in a <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">
		/// Input stream.
		/// </param>
		/// <returns>
		/// List of all XLD files.
		/// </returns>
		public static List<XLDFile> FindXLDs(Stream stream)
		{
			List<XLDFile> entries = new List<XLDFile>();
			while(ReadToXLD(stream))
			{
				entries.Add(XLDFile.Parse(stream));
			}
			return entries;
		}
		
		/// <summary>
		/// Finds <paramref name="n"/>-th XLD file in a <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">
		/// Input stream.
		/// </param>
		/// <param name="n">
		/// Number of XLD file in <paramref name="stream"/>.
		/// </param>
		/// <returns>
		/// <paramref name="n"/>-th XLD file in a stream.
		/// </returns>
		public static XLDFile FindXLD(Stream stream, int n)
		{
			int offset;
			return FindXLD(stream, n, out offset);
		}
		
		/// <summary>
		/// Finds <paramref name="n"/>-th XLD file in a <paramref name="stream"/> and stores <paramref name="offset"/>.
		/// </summary>
		/// <param name="stream">
		/// Input stream.
		/// </param>
		/// <param name="n">
		/// Number of XLD file in <paramref name="stream"/>.
		/// </param>
		/// <param name="offset">
		/// Offset where the XLD begins.
		/// </param>
		/// <returns>
		/// <paramref name="n"/>-th XLD file in a stream.
		/// </returns>
		public static XLDFile FindXLD(Stream stream, int n, out int offset)
		{
			int pos = 0;
			while(ReadToXLD(stream))
			{
				if(pos++ == n)
				{
					offset = (int)stream.Position;
					return XLDFile.Parse(stream);
				}else{
					stream.ReadByte();
				}
			}
			offset = -1;
			return null;
		}
	}
}