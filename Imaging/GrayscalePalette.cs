using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlbLib.Imaging
{
	public abstract partial class ImagePalette : IList<Color>
	{
		private sealed class GrayscalePalette : ImagePalette
		{
			public override int Length{
				get{
					return 256;
				}
			}
			
			public override Color this[int index]{
				get{
					return Color.FromArgb(index,index,index);
				}
			}
		}
	}
}
