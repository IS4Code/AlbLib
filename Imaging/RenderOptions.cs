using System;

namespace AlbLib.Imaging
{
	public class RenderOptions
	{
		public static readonly RenderOptions Empty = new RenderOptions();
		
		public virtual int TransparentIndex{get;set;}
		
		public RenderOptions()
		{
			TransparentIndex = ImagePalette.TransparentIndex;
		}
	}
}
