using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AlbLib.Mapping;

namespace AlbLib.Imaging
{
	[Serializable]
	public class MinimapPalette : ImagePalette
	{
		public MinimapPalette(ImagePalette source) : this(source, MinimapType.Classic)
		{}
		
		public MinimapPalette(ImagePalette source, MinimapType type) : this(source, type==MinimapType.Classic?ImagePalette.GetPalette(21):ImagePalette.GetPalette(46))
		{}
		
		public MinimapPalette(ImagePalette source, ImagePalette minimap)
        {
            ColorArray = TakeWithPadding(source, 152, Color.Empty)
                .Concat(TakeWithPadding(minimap, 192-152, Color.Empty))
                .Concat(source.TakeWhile((color, i) => i>=192))
                .ToArray();
		}

        private IEnumerable<Color> TakeWithPadding(ImagePalette list, int count, Color padding)
        {
            return list.TakeWhile((color, i) => i < count)
                .Concat(Enumerable.Repeat(Color.Empty, Math.Max(0, count - list.Count)));

        }
	}
}
