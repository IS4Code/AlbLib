using System;
using System.IO;
using AlbLib.Caching;
using AlbLib.Imaging;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	public static class LabGraphics
	{
		public static IndexedCache<RawImage,int> floorscache = new IndexedCache<RawImage,int>(LoadFloor, Cache.ZeroNull);
		public static IndexedCache<RawImage,int> wallscache = new IndexedCache<RawImage,int>(LoadWall, Cache.ZeroNull);
		public static IndexedCache<RawImage,int> objectscache = new IndexedCache<RawImage,int>(LoadObject, Cache.ZeroNull);
		
		private static RawImage LoadFloor(int floor, int labdata)
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
				return new RawImage(nav, 64, length/64);
			}
		}
			
		public static RawImage GetFloor(int labdata, int floor)
		{
			return floorscache.Get(floor, labdata);
		}
		
		private static RawImage LoadWall(int wall, int labdata)
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
				return new RawImage(nav, walldata.TextureHeight, walldata.TextureWidth);
			}
		}
		
		public static RawImage GetWall(int labdata, int wall)
		{
			return wallscache.Get(wall, labdata);
		}
		
		private static RawImage LoadObject(int obj, int labdata)
		{
			LabData ld = LabData.GetLabData(labdata);
			ObjectInfo objectdata = ld.GetObjectInfo(obj);
			
			int texture = objectdata.Texture;
			int fx, sx;
			if(!Common.E(texture, out fx, out sx))return null;
			using(FileStream stream = new FileStream(Paths._3DObjectsN.Format(fx), FileMode.Open))
			{
				XLDNavigator nav = XLDNavigator.ReadToIndex(stream, (short)sx);
				int length = nav.SubfileLength;
				return new RawImage(nav, objectdata.TextureHeight, objectdata.TextureWidth);
			}
		}
		
		public static RawImage GetObject(int labdata, int obj)
		{
			return objectscache.Get(obj, labdata);
		}
	}
}
