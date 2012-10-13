using System;
using System.Drawing;
using AlbLib.Mapping;

namespace AlbLib.Imaging
{
	public class MinimapPalette : ImagePalette
	{
		private ImagePalette source;
		private ImagePalette minimap;
		
		public override Color this[int index]{
			get{
				if(152 <= index && index <= 191)
				{
					return minimap[index];
				}else if(source.Length <= index && index <= 191)
				{
					return Color.Empty;
				}else{
					return source[index];
				}
			}
		}
		
		public MinimapPalette(ImagePalette source) : this(source, MinimapType.Classic)
		{}
		
		public MinimapPalette(ImagePalette source, MinimapType type) : this(source, type==MinimapType.Classic?ImagePalette.GetPalette(21):ImagePalette.GetPalette(46))
		{}
		
		public MinimapPalette(ImagePalette source, ImagePalette minimap)
		{
			this.source = source;
			this.minimap = minimap;
		}
		
		public override int Length{
			get{
				if(source.Length > 192)return source.Length;
				return 192;
			}
		}
	}
}
