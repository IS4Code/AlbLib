using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlbLib.Imaging
{
	public abstract partial class ImagePalette : IList<Color>
	{
		private sealed class MonochromePalette : ImagePalette
		{
			public override int Length{
				get{
					return 256;
				}
			}
			
			public override Color this[int index]{
				get{
                    return index == 0 ? Color.Black : Color.White;
				}
			}
		}
	}
}
