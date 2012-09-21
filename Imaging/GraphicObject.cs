using System;
using System.Drawing;

namespace AlbLib.Imaging
{
	/// <summary>
	/// Object lieing on graphic plane.
	/// </summary>
	public class GraphicObject
	{
		/// <summary>
		/// Location of object.
		/// </summary>
		public Point Location{
			get;set;
		}
		
		/// <param name="image">
		/// Source image.
		/// </param>
		/// <param name="location">
		/// Location of object.
		/// </param>
		public GraphicObject(ImageBase image, Point location)
		{
			this.image = image;
			Location = location;
		}
		
		private ImageBase image;
		
		/// <summary>
		/// Source image.
		/// </summary>
		public ImageBase Image{
			get{
				return image;
			}
			set{
				if(value == null)throw new ArgumentNullException("value");
				image = value;
			}
		}
		
		/// <summary>
		/// Type of transparency.
		/// </summary>
		public TransparencyType Transparency{
			get;set;
		}
		
		/// <summary>
		/// Transparent color index.
		/// </summary>
		public int TransparentIndex{
			get;set;
		}
	}
}