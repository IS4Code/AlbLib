using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AlbLib.Imaging
{
	public abstract partial class ImagePalette
	{
		private sealed class GrayscalePalette : ImagePalette
		{
            public GrayscalePalette()
            {
				ColorArray = (from i in Enumerable.Range(0, 256) 
					select Color.FromArgb(i, i, i)).ToArray();
			}
        }
	}
}
