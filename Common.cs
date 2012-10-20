using System;
using System.IO;

namespace AlbLib
{
	/// <summary>
	/// Contains various common functions and magic contants.
	/// </summary>
	public static class Common
	{
		/// <summary>
		/// 63×ColorConversion = 255
		/// </summary>
		public const double ColorConversion = 4.047619047619047619047619047619;
		
		/// <summary>
		/// Converts nullable index to file index and subfile index
		/// </summary>
		/// <param name="index">
		/// Nullable index. (0 = null, 1 - 99 => 0 - 99, 100+ => 100+)
		/// </param>
		/// <param name="fileIndex">
		/// File index.
		/// </param>
		/// <param name="subfileIndex">
		/// Subfile index.
		/// </param>
		/// <returns>
		/// True if <paramref name="index"/> is non-zero.
		/// </returns>
		public static bool E(int index, out int fileIndex, out int subfileIndex)
		{
			if(index == 0)
			{
				fileIndex = 0; subfileIndex = 0;
				return false;
			}
			fileIndex = index/100;
			subfileIndex = index<100?index-1:index%100;
			return true;
		}
		
		/// <summary>
		/// Converts file and subfile index to nullable index.
		/// </summary>
		/// <param name="fileIndex">
		/// File index.
		/// </param>
		/// <param name="subfileIndex">
		/// Subfile index.
		/// </param>
		/// <returns>
		/// Nullable index. (0 = null, 1 - 99 => 0 - 99, 100+ => 100+)
		/// </returns>
		public static int E(int fileIndex, int subfileIndex)
		{
			return fileIndex==0?subfileIndex+1:fileIndex*100+subfileIndex;
		}
		
		private static readonly byte[] skipBuffer = new byte[4096];
		
		/// <summary>
		/// Skips <paramref name="bytes"/> from stream.
		/// </summary>
		/// <param name="input">
		/// Input stream.
		/// </param>
		/// <param name="bytes">
		/// Number of bytes to skip.
		/// </param>
		/// <returns>
		/// Number of skipped bytes.
		/// </returns>
		public static int Skip(this Stream input, int bytes)
		{
			int read = 0;
			do{
				if(bytes > 4096)
				{
					read += input.Read(skipBuffer, 0, 4096);
					bytes -= 4096;
				}else{
					read += input.Read(skipBuffer, 0, bytes);
					bytes = 0;
				}
			}while(bytes != 0);
			return read;
		}
	}
}
