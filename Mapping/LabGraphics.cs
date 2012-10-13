using System;
using System.IO;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	public static class LabGraphics
	{
		private static RawImage[,] floors = new RawImage[256,256];
		private static RawImage[,] walls = new RawImage[256,256];
		
		public static RawImage GetFloor(int labdata, int floor)
		{
			if(floors[labdata,floor] == null)
			{
				LabData ld = LabData.GetLabData(labdata);
				FloorData floordata = ld.GetFloor(floor);
				
				int texture = floordata.Texture;
				int fx, sx;
				if(!Common.E(texture, out fx, out sx))return null;
				using(FileStream stream = new FileStream(Paths._3DFloorN.Format(fx), FileMode.Open))
				{
					XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)sx);
					int length = nav.SubfileLength;
					floors[labdata,floor] = new RawImage(nav, 64, length/64);
				}
			}
			return floors[labdata,floor];
		}
		
		public static RawImage GetWall(int labdata, int wall)
		{
			if(walls[labdata,wall] == null)
			{
				LabData ld = LabData.GetLabData(labdata);
				WallData walldata = ld.GetWall(wall);
				
				int texture = walldata.Texture;
				int fx, sx;
				if(!Common.E(texture, out fx, out sx))return null;
				using(FileStream stream = new FileStream(Paths._3DWallsN.Format(fx), FileMode.Open))
				{
					XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)sx);
					int length = nav.SubfileLength;
					walls[labdata,wall] = new RawImage(nav, walldata.TextureHeight, walldata.TextureWidth);
				}
			}
			return walls[labdata,wall];
		}
	}
}
