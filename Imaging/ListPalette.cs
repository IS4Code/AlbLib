using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AlbLib.Imaging
{
	public abstract partial class ImagePalette : IGameResource
	{
		private sealed class ListPalette : ImagePalette
		{
			public ListPalette(IList<Color> list)
            {
                ColorArray = list.ToArray();
            }
		}
	}
}
