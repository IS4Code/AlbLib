using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Plane containing more graphic objects.
	/// </summary>
	[Serializable]
	public class GraphicPlane : IRenderable
	{
		/// <summary>
		/// Main background image.
		/// </summary>
		public ImageBase Background{
			get;set;
		}
		
		/// <summary>
		/// Collection of all objects.
		/// </summary>
		public ICollection<GraphicObject> Objects{
			get;set;
		}
		
		/// <summary>
		/// Main palette.
		/// </summary>
		public ImagePalette Palette{
			get;set;
		}
		
		/// <summary>
		/// Transparency table used when blending colors.
		/// </summary>
		public TransparencyTable TransparencyTable{
			get;set;
		}
		
		/// <summary>
		/// Sets 
		/// </summary>
		public int PaletteID{
			set{
				Palette = ImagePalette.GetFullPalette(value);
				TransparencyTable = TransparencyTable.GetTransparencyTable(value);
			}
		}
		
		/// <summary>
		/// Creates empty graphic plane.
		/// </summary>
		public GraphicPlane()
		{
			Objects = new Collection<GraphicObject>();
		}
		
		/// <summary>
		/// Creates graphic plane with predefined background.
		/// </summary>
		/// <param name="width">
		/// Width of graphic plane.
		/// </param>
		/// <param name="height">
		/// Height of graphic plane.
		/// </param>
		public GraphicPlane(int width, int height) : this()
		{
			Background = new RawImage(new byte[width*height], width, height);
		}
		
		/// <summary>
		/// Draws plane to bitmap using other palette.
		/// </summary>
		public Image Render(byte palette)
		{
			return Render(ImagePalette.GetFullPalette(palette));
		}
		
		/// <summary>
		/// Draws plane to bitmap using palette and render options.
		/// </summary>
		public Image Render(byte palette, RenderOptions options)
		{
			return Render(new RenderOptions(options){Palette = options.Palette??ImagePalette.GetFullPalette(palette)});
		}
		
		/// <summary>
		/// Draws plane to bitmap.
		/// </summary>
		public Image Render()
		{
			byte[] baked = GetBaked();
			return Drawing.DrawBitmap(baked, Background.GetWidth(), Background.GetHeight(), Palette);
		}
		
		/// <summary>
		/// Draws plane to bitmap using other palette.
		/// </summary>
		public Image Render(RenderOptions options)
		{
			byte[] baked = GetBaked();
			return Drawing.DrawBitmap(baked, Background.GetWidth(), Background.GetHeight(), options);
		}
		
		/// <summary>
		/// Inserts all objects into background.
		/// </summary>
		public void Bake()
		{
			ApplyBake(Background.ImageData);
			Objects.Clear();
		}
		
		private byte[] GetBaked()
		{
			byte[] baked = new byte[Background.ImageData.Length];
			Background.ImageData.CopyTo(baked, 0);
			ApplyBake(baked);
			return baked;
		}
		
		private void ApplyBake(byte[] tobake)
		{
			int width = Background.GetWidth();
			int height = Background.GetHeight();
			TransparencyTable trans = TransparencyTable;
			foreach(GraphicObject obj in Objects)
			{
				if(obj == null || obj.Image == null)continue;
				for(int y = 0; y < obj.Image.GetHeight(); y++)
				for(int x = 0; x < obj.Image.GetWidth(); x++)
				{
					if(x+obj.Location.X >= 0 && y+obj.Location.Y >= 0 && x+obj.Location.X < width && y+obj.Location.Y < height)
					{
						byte color = obj.Image.ImageData[obj.Image.GetWidth()*y+x];
						if(color == obj.TransparentIndex)continue;
						int index = width*(y+obj.Location.Y)+obj.Location.X+x;
						if(trans == null || obj.Transparency == TransparencyType.None)
						{
							tobake[index] = color;
						}else{
							tobake[index] = trans.GetResultingColorIndex(color, tobake[index], obj.Transparency);
						}
					}
				}
			}
		}
	}
}