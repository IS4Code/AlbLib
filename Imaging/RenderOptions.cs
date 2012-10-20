using System;

namespace AlbLib.Imaging
{
	[Serializable]
	public class RenderOptions : IEquatable<RenderOptions>, ICloneable
	{
		public static readonly RenderOptions Empty = new RenderOptions();
		
		public virtual int TransparentIndex{get;set;}
		public virtual ImagePalette Palette{get;set;}
		
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
		}
		
		public static implicit operator RenderOptions(ImagePalette pal)
		{
			return new RenderOptions(pal);
		}
		
		public virtual bool Equals(RenderOptions other)
		{
			return
				this.TransparentIndex == other.TransparentIndex &&
				this.Palette == other.Palette;
		}

		
		public virtual object Clone()
		{
			return new RenderOptions(this);
		}
	}
}
