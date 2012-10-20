using System;

namespace AlbLib.Imaging
{
	public static partial class MainExecutableImages
	{
		struct ImageLocationInfo
		{
			public long Position;
			public short Width;
			public short Height;
			
			public ImageLocationInfo(long pos, short width, short height)
			{
				Position = pos;
				Width = width;
				Height = height;
			}
		}
	}
}
