using System;

namespace AlbLib.Mapping
{
	[Flags]
	public enum WallForm
	{
		Close = 0,
		OpenTop = 1,
		OpenTopRight = 3,
		OpenRight = 2,
		OpenBottomRight = 6,
		OpenBottom = 4,
		OpenBottomLeft = 12,
		OpenLeft = 8,
		OpenTopLeft = 9,
		Open = 15
	}
}
