using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlbLib.IFF
{
	/// <summary>
	/// Class used when reading files in IFF format.
	/// </summary>
	public class IFFReader
	{
		Stream input;
		BinaryReader reader;
		int rest;
		int filelength;
		int fileread;
		
		/// <summary>
		/// Initializes new instance using stream.
		/// </summary>
		/// <param name="input">
		/// Input stream.
		/// </param>
		public IFFReader(Stream input)
		{
			reader = new BinaryReader(this.input = input, Encoding.ASCII);
		}
		
		/// <summary>
		/// Reads file header on beginning of data.
		/// </summary>
		/// <returns>
		/// File header.
		/// </returns>
		public IFFFile ReadFileHeader()
		{
			string typeid = new String(reader.ReadChars(4));
			filelength = ToLittleEndian(reader.ReadInt32());
			if(typeid != "FORM")
			{
				throw new NotSupportedException("This is not IFF file.");
			}
			fileread += 4;
			return new IFFFile(typeid, new String(reader.ReadChars(4)), filelength);
		}
		
		/// <summary>
		/// Reads chunk header.
		/// </summary>
		/// <returns>
		/// Chunk header.
		/// </returns>
		public IFFChunk ReadChunkHeader()
		{
			fileread += 8;
			return new IFFChunk(new String(reader.ReadChars(4)), rest = ToLittleEndian(reader.ReadInt32()));
		}
		
		/// <summary>
		/// Reads one byte.
		/// </summary>
		/// <returns>
		/// One byte.
		/// </returns>
		public byte ReadByte()
		{
			rest -= 1;
			fileread += 1;
			return reader.ReadByte();
		}
		
		/// <summary>
		/// Reads one int16. Automatically converted to little endian.
		/// </summary>
		/// <returns>
		/// One int16.
		/// </returns>
		public short ReadInt16()
		{
			rest -= 2;
			fileread += 2;
			return ToLittleEndian(reader.ReadInt16());
		}
		
		
		/// <summary>
		/// Reads one int32. Automatically converted to little endian.
		/// </summary>
		/// <returns>
		/// One int32.
		/// </returns>
		public int ReadInt32()
		{
			rest -= 4;
			fileread += 4;
			return ToLittleEndian(reader.ReadInt32());
		}
		
		
		/// <summary>
		/// Reads one uint16. Automatically converted to little endian.
		/// </summary>
		/// <returns>
		/// One uint16.
		/// </returns>
		public ushort ReadUInt16()
		{
			rest -= 2;
			fileread += 2;
			return ToLittleEndian(reader.ReadUInt16());
		}
		
		
		/// <summary>
		/// Reads one uint32. Automatically converted to little endian.
		/// </summary>
		/// <returns>
		/// One uint32.
		/// </returns>
		public uint ReadUInt32()
		{
			rest -= 4;
			fileread += 4;
			return ToLittleEndian(reader.ReadUInt32());
		}
		
		/// <summary>
		/// Reads all remaining bytes from chunk.
		/// </summary>
		public void ReadRest()
		{
			fileread += rest;
			reader.ReadBytes(rest);
			rest = 0;
		}
		
		/*public void Advance(int count)
		{
			rest -= count;
		}*/
		
		/// <summary>
		/// Reads bytes with specified <paramref name="count"/>.
		/// </summary>
		/// <param name="count">
		/// Bytes count.
		/// </param>
		/// <returns>
		/// Byte array.
		/// </returns>
		public byte[] ReadBytes(int count)
		{
			fileread += count;
			rest -= count;
			return reader.ReadBytes(count);
		}
		
		/// <summary>
		/// Reads packed bytes with specified <paramref name="count"/>.
		/// </summary>
		/// <param name="count">
		/// Bytes count.
		/// </param>
		/// <returns>
		/// Byte array.
		/// </returns>
		public byte[] ReadUnpack(int count)
		{
			int read;
			byte[] bytes = Unpack(input, count, out read);
			fileread += read;
			rest -= read;
			return bytes;
		}
		
		/// <summary>
		/// Enumerates through all chunks in file.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IFFChunk> ReadAll()
		{
			while(fileread < filelength)
			{
				IFFChunk chunk = ReadChunkHeader();
				yield return chunk;
				if(rest>0)ReadRest();
			}
		}
		
		/// <summary>
		/// Converts value in big endian to current endian.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Converted value.
		/// </returns>
		public static int ToLittleEndian(int value)
		{
			return unchecked((int)ToLittleEndian((uint)value));
		}
		
		/// <summary>
		/// Converts value in big endian to current endian.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Converted value.
		/// </returns>
		public static uint ToLittleEndian(uint value)
		{
			if(BitConverter.IsLittleEndian)
			{
				return ((value&0xFF)<<24)|((value&0xFF00)<<8)|((value&0xFF0000)>>8)|((value&0xFF000000)>>24);
			}
			return value;
		}
		
		/// <summary>
		/// Converts value in big endian to current endian.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Converted value.
		/// </returns>
		public static short ToLittleEndian(short value)
		{
			return unchecked((short)ToLittleEndian((ushort)value));
		}
		
		/// <summary>
		/// Converts value in big endian to current endian.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Converted value.
		/// </returns>
		public static ushort ToLittleEndian(ushort value)
		{
			if(BitConverter.IsLittleEndian)
			{
				return (ushort)(((value&0xFF)<<8)|((value&0xFF00)>>8));
			}
			return value;
		}
		
		/// <summary>
		/// Reads packed data from stream.
		/// </summary>
		/// <param name="input">
		/// Input stream.
		/// </param>
		/// <param name="size">
		/// Data size.
		/// </param>
		/// <param name="read">
		/// Bytes read.
		/// </param>
		/// <returns>
		/// Uncompressed data.
		/// </returns>
		public static byte[] Unpack(Stream input, int size, out int read)
		{
			using(MemoryStream stream = new MemoryStream())
			{
				read = Unpack(input, stream, size);
				return stream.ToArray();
			}
		}
		
		/// <summary>
		/// Reads packed data from stream.
		/// </summary>
		/// <param name="input">
		/// Input stream.
		/// </param>
		/// <param name="output">
		/// Output stream.
		/// </param>
		/// <param name="size">
		/// Data size.
		/// </param>
		/// <returns>
		/// Bytes read.
		/// </returns>
		public static int Unpack(Stream input, Stream output, int size)
		{
			int read = 0;
			while(read < size)
			{
				int n = input.ReadByte();
				read += 1;
				if(0 <= n && n <= 127)
				{
					byte[] seq = new byte[n+1];
					read += input.Read(seq, 0, n+1);
					output.Write(seq, 0, n+1);
				}else if(129 <= n && n <= 255)
				{
					int b = input.ReadByte();
					if(b == -1)return read;
					read += 1;
					for(int i = 0; i < 257-n; i++)
					{
						output.WriteByte((byte)b);
					}
				}
			}
			if(size%2!=0)
			{
				input.ReadByte();
				read += 1;
			}
			return read;
		}
	}
}