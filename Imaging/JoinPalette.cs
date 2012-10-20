using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlbLib.Imaging
{
	public abstract partial class ImagePalette : IList<Color>
	{
		private sealed class JoinPalette : ImagePalette
		{
			private readonly ImagePalette left;
			private readonly ImagePalette right;
			
			public JoinPalette(ImagePalette a, ImagePalette b)
			{
				left = a;
				right = b;
			}
			
			public override int Length{
				get{
					return left.Length+right.Length;
				}
			}
			
			public override Color this[int index]{
				get{
					if(index<left.Length)
					{
						return left[index];
					}else{
						return right[index-left.Length];
					}
				}
			}
			
			public override IEnumerator<Color> GetEnumerator()
			{
				foreach(Color c in left)
					yield return c;
				foreach(Color c in right)
					yield return c;
			}
		}
	}
}
