using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AlbLib.Imaging
{
	public abstract partial class ImagePalette
	{
		private sealed class JoinPalette : ImagePalette
		{
            public JoinPalette(ImagePalette a, ImagePalette b)
            {
                ColorArray = a.ColorArray.Concat(b.ColorArray).ToArray();
			}
		}
	}
}
