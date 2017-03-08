using System;

namespace AlbLib.Imaging
{
	[Serializable]
	public class RenderOptions : IEquatable<RenderOptions>, ICloneable
	{
		public static RenderOptions Empty{get{return new RenderOptions();}}
		
		public virtual int TransparentIndex{get;set;}
		public virtual ImagePalette Palette{get;set;}
		public virtual int Frame{get;set;}
		
		public RenderOptions()
		{
			TransparentIndex = ImagePalette.TransparentIndex;
		}
		
		public RenderOptions(ImagePalette pal) : this()
		{
			Palette = pal;
		}
		
		public RenderOptions(RenderOptions source) : this()
		{
			TransparentIndex = source.TransparentIndex;
			Palette = source.Palette;
			Frame = source.Frame;
		}
		
		public static implicit operator RenderOptions(ImagePalette pal)
		{
			return new RenderOptions(pal);
		}
		
		public virtual bool Equals(RenderOptions other)
		{
			return
				this.TransparentIndex == other.TransparentIndex &&
				this.Palette == other.Palette &&
				this.Frame == other.Frame;
		}

		
		public virtual object Clone()
		{
			return new RenderOptions(this);
		}
		
		public static RenderOptions Transparent(int palette)
		{
			return new RenderOptions(ImagePalette.GetFullPalette(palette)){TransparentIndex = 0};
		}
	}
}
