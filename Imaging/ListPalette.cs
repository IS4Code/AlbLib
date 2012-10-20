using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlbLib.Imaging
{
	public abstract partial class ImagePalette : IList<Color>
	{
		private sealed class ListPalette : ImagePalette
		{
			private readonly IList<Color> list;
			public ListPalette(IList<Color> list)
			{
				this.list = list;
			}
			
			public override int Length{
				get{
					return list.Count;
				}
			}
			
			public override Color this[int index]{
				get{
					return list[index];
				}
			}
			
			public override void CopyTo(Color[] array, int index)
			{
				list.CopyTo(array, index);
			}
			
			public override IEnumerator<Color> GetEnumerator()
			{
				return list.GetEnumerator();
			}
		}
	}
}
