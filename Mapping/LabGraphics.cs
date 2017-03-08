using System;
using System.Drawing;
using System.IO;
using AlbLib.Caching;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	public static class LabGraphics
	{
		public static RawImage GetFloor(int labdata, int floor)
		{
			LabData ld = LabData.GetLabData(labdata);
			FloorData floordata = ld.GetFloor(floor);
			
			int texture = floordata.Texture;
			return GameData.Floors3D.Open(texture);
		}
		
		public static RawImage GetWall(int labdata, int wall)
		{
			LabData ld = LabData.GetLabData(labdata);
			WallData walldata = ld.GetWall(wall);
			
			int texture = walldata.Texture;
			var bg = GameData.Walls3D.Open(texture);
			bg.Width = walldata.TextureWidth;
			bg.Height = walldata.TextureHeight;
			var plane = new GraphicPlane(bg.Width, bg.Height);
			plane.Background = bg;
			/*foreach(var ovrl in walldata.Overlays)
			{
				var img = GameData.Overlays3D.Open(ovrl.Texture);
				img.Width = ovrl.TextureWidth;
				img.Height = ovrl.TextureHeight;
				plane.Objects.Add(new GraphicObject(img, new Point(ovrl.X, ovrl.Y)));
			}*/
			plane.Bake();
			return (RawImage)plane.Background;
		}
		
		public static RawImage GetObject(int labdata, int obj)
		{
			LabData ld = LabData.GetLabData(labdata);
			ObjectInfo objectdata = ld.GetObjectInfo(obj);
			
			int texture = objectdata.Texture;
			var img = GameData.Objects3D.Open(texture);
			img.Width = objectdata.TextureWidth;
			img.Height = objectdata.TextureHeight;
			return img;
		}
	}
}
