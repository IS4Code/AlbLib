using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AlbLib
{
	namespace IFF
	{
		/// <summary>
		/// Class respresenting single file in IFF format.
		/// </summary>
		public sealed class IFFFileNode : IFFNode
		{
			/// <summary>
			/// List of all entries or nodes in file.
			/// </summary>
			public ReadOnlyCollection<IFFContentNode> Nodes{
				get; private set;
			}
			
			/// <summary>
			/// 4-character format ID.
			/// </summary>
			public string FormatID{
				get; private set;
			}
			
			/// <summary>
			/// Initializes new instance using file name.
			/// </summary>
			/// <param name="file">
			/// File path.
			/// </param>
			public IFFFileNode(string file):this(new FileStream(file, FileMode.Open))
			{}
			
			/// <summary>
			/// Initializes new instance using stream.
			/// </summary>
			/// <param name="input">
			/// Input stream.
			/// </param>
			public IFFFileNode(Stream input) : base(input)
			{
				Apply(reader.ReadFileHeader());
				var nodes = new List<IFFContentNode>();
				int read = 0;
				while(read < Length)
				{
					var node = new IFFContentNode(input);
					read += node.Length;
					nodes.Add(node);
				}
				Nodes = new ReadOnlyCollection<IFFContentNode>(nodes);
			}
			
			private void Apply(IFFFile file)
			{
				this.TypeID = file.TypeID;
				this.Length = file.Length;
				this.FormatID = file.FormatID;
			}
		}
		
		/// <summary>
		/// Class representing single node in IFF file.
		/// </summary>
		public class IFFContentNode : IFFNode
		{
			/// <summary>
			/// Node's content as byte array.
			/// </summary>
			public byte[] Content{
				get; protected set;
			}
			
			/// <summary>
			/// Initializes new instance using stream.
			/// </summary>
			/// <param name="input">
			/// Input stream.
			/// </param>
			public IFFContentNode(Stream input) : base(input)
			{
				Apply(reader.ReadChunkHeader());
				Content = new byte[Length];
				Length = input.Read(Content, 0, Length);
			}
			
			private void Apply(IFFChunk chunk)
			{
				this.TypeID = chunk.TypeID;
				this.Length = chunk.Length;
			}
		}
		
		/// <summary>
		/// Represent data container in IFF format. File or node.
		/// </summary>
		public abstract class IFFNode
		{
			/// <summary>
			/// Type of data.
			/// </summary>
			public string TypeID{
				get; protected set;
			}
			
			/// <summary>
			/// Length of data in bytes.
			/// </summary>
			public int Length{
				get; protected set;
			}
			
			/// <summary>
			/// IFF reader.
			/// </summary>
			protected IFFReader reader;
			
			/// <summary>
			/// Initializes new instance using stream.
			/// </summary>
			/// <param name="input">
			/// Input stream.
			/// </param>
			protected IFFNode(Stream input)
			{
				reader = new IFFReader(input);
			}
		}
		
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
			
			public byte[] ReadUnpack(int size)
			{
				int read;
				byte[] bytes = Unpack(input, size, out read);
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
			
			public static byte[] Unpack(Stream input, int size, out int read)
			{
				using(MemoryStream stream = new MemoryStream())
				{
					read = Unpack(input, stream, size);
					return stream.ToArray();
				}
			}
			
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
		
		/// <summary>
		/// Structure representing file header.
		/// </summary>
		public struct IFFFile
		{
			/// <summary>
			/// Type ID of data.
			/// </summary>
			/// <returns>
			/// FORM
			/// </returns>
			public readonly string TypeID;
			
			/// <summary>
			/// File subformat.
			/// </summary>
			public readonly string FormatID;
			
			/// <summary>
			/// File length.
			/// </summary>
			public readonly int Length;
			
			/// <summary>
			/// Initializes new instance.
			/// </summary>
			public IFFFile(string typeid, string formatid, int length)
			{
				TypeID = typeid;
				FormatID = formatid;
				Length = length;
			}
		}
		
		/// <summary>
		/// Structure representing chunk header.
		/// </summary>
		public struct IFFChunk
		{
			/// <summary>
			/// Type ID of data.
			/// </summary>
			public readonly string TypeID;
			
			/// <summary>
			/// Chunk length.
			/// </summary>
			public readonly int Length;
			
			/// <summary>
			/// Initializes new instance.
			/// </summary>
			public IFFChunk(string typeid, int length)
			{
				TypeID = typeid;
				Length = length;
			}
		}
	}
}