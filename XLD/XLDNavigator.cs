/*
 * Created by SharpDevelop.
 * User: Illidan
 * Date: 12.10.2012
 * Time: 19:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace AlbLib.XLD
{
	/// <summary>
	/// Description of XLDNavigator.
	/// </summary>
	[Serializable]
	public class XLDNavigator : Stream, IDisposable
	{
		private readonly Stream baseStream;
		private readonly short nEntries;
		private readonly int[] entriesLengths;
		private readonly int[] entriesPos;
		private readonly long streamStart;
		private int remaining;
		
		public Stream BaseStream{
			get{
				return baseStream;
			}
		}
		
		public short NumSubfiles{
			get{
				return nEntries;
			}
		}
		
		public short CurrentSubfile
		{
			get; private set;
		}
		
		public XLDNavigator(string file) : this(new FileStream(file, FileMode.Open))
		{
		}
		
		public XLDNavigator(Stream input)
		{
			if(input == null)throw new ArgumentNullException("input");
			baseStream = input;
			BinaryReader reader = new BinaryReader(input);
			if(XLDFile.Signature != new String(reader.ReadChars(6)))
			{
				input.Close();
				throw new InvalidDataException("This is not valid XLD file.");
			}
			nEntries = reader.ReadInt16();
			entriesLengths = new int[nEntries];
			entriesPos = new int[nEntries];
			int actpos = 0;
			for(int i = 0; i < nEntries; i++)
			{
				entriesLengths[i] = reader.ReadInt32();
				entriesPos[i] = actpos;
				actpos += entriesLengths[i];
			}
			if(input.CanSeek)streamStart = input.Position;
			CurrentSubfile = 0;
			remaining = entriesLengths[0];
		}
		
		public int SubfileLength{
			get{
				return GetSubfileLength(CurrentSubfile);
			}
		}
		
		public XLDNavigator GoToSubfile(short index)
		{
			if(0 > index || index > nEntries)throw new ArgumentOutOfRangeException("index");
			if(index == CurrentSubfile)
			{
				if(remaining != SubfileLength)
					if(baseStream.CanSeek)baseStream.Position = streamStart+entriesPos[index];
					else
						throw new NotSupportedException("Can't return to previous subfile.");
			}
			
			if(baseStream.CanSeek)
			{
				baseStream.Position = streamStart+entriesPos[index];
			}else{
				if(CurrentSubfile > index)
				{
					throw new NotSupportedException("Can't return to previous subfile.");
				}
				baseStream.Skip(remaining);
				for(int i = CurrentSubfile+1; i < index; i++)
					baseStream.Skip(entriesLengths[i]);
				
			}
			CurrentSubfile = index;
			remaining = entriesLengths[index];
			return this;
		}
		
		public XLDSubfile ReadSubfile(short index)
		{
			return new XLDSubfile(GoToSubfile(index), entriesLengths[CurrentSubfile], CurrentSubfile);
		}
		
		public XLDSubfile ReadSubfile()
		{
			return new XLDSubfile(this, entriesLengths[CurrentSubfile], CurrentSubfile);
		}
		
		public int GetSubfileLength(short index)
		{
			if(0 > index || index > nEntries)throw new ArgumentOutOfRangeException("index");
			return entriesLengths[index];
		}
		
		public void Finish()
		{
			if(baseStream.CanSeek)
			{
				baseStream.Position = streamStart+entriesPos[nEntries-1]+entriesLengths[nEntries-1];
			}else{
				baseStream.Skip(remaining);
				for(int i = CurrentSubfile+1; i < nEntries; i++)
					baseStream.Skip(entriesLengths[i]);
			}
		}
		
		public override void Close()
		{
			baseStream.Close();
		}
		
		public static XLDNavigator ReadToIndex(string file, short index)
		{
			XLDNavigator nav = new XLDNavigator(file);
			return nav.GoToSubfile(index);
		}
		
		public static XLDNavigator ReadToIndex(Stream stream, short index)
		{
			XLDNavigator nav = new XLDNavigator(stream);
			return nav.GoToSubfile(index);
		}
		
		#pragma warning disable 1591
		public override int Read(byte[] buffer, int offset, int count)
		{
			int read;
			remaining -= read = baseStream.Read(buffer, offset, Math.Min(count, remaining));
			return read;
		}
		
		public override int ReadByte()
		{
			if(remaining > 0)
			{
				remaining -= 1;
				return baseStream.ReadByte();
			}else return -1;
		}
		
		public override bool CanTimeout{
			get{
				return baseStream.CanTimeout;
			}
		}
		
		public override bool CanWrite{
			get{
				return false;
			}
		}
		
		public override bool CanRead{
			get{
				return true;
			}
		}
		
		public override bool CanSeek{
			get{
				return false;
			}
		}
		
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
		
		public override void WriteByte(byte value)
		{
			throw new NotSupportedException();
		}
		
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}
		
		public override long Position{
			get{
				return SubfileLength-remaining;
			}
			set{
				throw new NotSupportedException();
			}
		}
		
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}
		
		public override void Flush()
		{
			baseStream.Flush();
		}
		
		public override long Length{
			get{
				return SubfileLength;
			}
		}
	}
}
