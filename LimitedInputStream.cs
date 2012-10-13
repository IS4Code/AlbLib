using System;
using System.IO;

namespace AlbLib
{
	/// <summary>
	/// This stream captures base stream and limits reading.
	/// </summary>
	public class LimitedInputStream : Stream
	{
		private Stream source;
		
		/// <summary>
		/// Remaining bytes.
		/// </summary>
		public int Remaining{
			get; private set;
		}
		
		public LimitedInputStream(Stream source, int readlimit)
		{
			this.source = source;
			this.Remaining = readlimit;
		}
		
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
		
		public override int Read(byte[] buffer, int offset, int count)
		{
			if(count > Remaining)count = Remaining;
			int read = source.Read(buffer, offset, count);
			Remaining -= read;
			return read;
		}
		
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}
		
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}
		
		public override void Flush()
		{
			source.Flush();
		}
		
		public override long Position{
			get{
				throw new NotSupportedException();
			}
			set{
				throw new NotSupportedException();
			}
		}
		
		public override long Length{
			get{
				throw new NotSupportedException();
			}
		}
		
		public override bool CanWrite{
			get{
				return false;
			}
		}
		
		public override bool CanRead{
			get{
				return source.CanRead;
			}
		}
		
		public override bool CanSeek{
			get{
				return false;
			}
		}
	}
}
