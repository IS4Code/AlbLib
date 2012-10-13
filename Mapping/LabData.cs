using System;
using System.IO;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	public class LabData
	{
		private static readonly LabData[] labdatas = new LabData[256];
		
		public byte Scale1{get;private set;}
		public byte CameraHeight{get;private set;}
		public byte CameraAngle{get;private set;}
		public byte Background{get;private set;}
		public byte FogDistance{get;private set;}
		public byte MaxLightStrength{get;private set;}
		public byte Scale2{get;private set;}
		public byte ViewDistance{get;private set;}
		public ObjectData[] Objects{get;set;}
		public FloorData[] Floors{get;set;}
		public ObjectInfo[] ObjectInfos{get;set;}
		public WallData[] Walls{get;set;}
		
		public LabData(Stream input)
		{
			BinaryReader reader = new BinaryReader(input);
			reader.ReadByte();
			Scale1 = reader.ReadByte();
			CameraHeight = reader.ReadByte();
			CameraAngle = reader.ReadByte();
			reader.ReadInt16();
			Background = reader.ReadByte();
			reader.ReadBytes(3);
			FogDistance = reader.ReadByte();
			reader.ReadBytes(13);
			MaxLightStrength = reader.ReadByte();
			reader.ReadByte();
			Scale2 = reader.ReadByte();
			reader.ReadBytes(3);
			ViewDistance = reader.ReadByte();
			reader.ReadBytes(7);
			
			short numobjects = reader.ReadInt16();
			Objects = new ObjectData[numobjects];
			for(int i = 0; i < numobjects; i++)
			{
				Objects[i] = new ObjectData(reader);
			}
			
			short numfloors = reader.ReadInt16();
			Floors = new FloorData[numfloors];
			for(int i = 0; i < numfloors; i++)
			{
				Floors[i] = new FloorData(reader);
			}
			
			short numoinfos = reader.ReadInt16();
			ObjectInfos = new ObjectInfo[numoinfos];
			for(int i = 0; i < numoinfos; i++)
			{
				ObjectInfos[i] = new ObjectInfo(reader);
			}
			
			short numwalls = reader.ReadInt16();
			Walls = new WallData[numwalls];
			for(int i = 0; i < numwalls; i++)
			{
				Walls[i] = new WallData(reader);
			}
		}
		
		public static LabData GetLabData(int index)
		{
			if(labdatas[index] == null)
			{
				int fx, tx;
				if(!Common.E(index, out fx, out tx))return null;
				using(FileStream stream = new FileStream(Paths.LabDataN.Format(fx), FileMode.Open))
				{
					labdatas[index] = new LabData(XLDNavigator.ReadToIndex(stream, (short)tx));
				}
			}
			return labdatas[index];
		}
		
		public FloorData GetFloor(int index)
		{
			if(index == 0)return default(FloorData);
			return Floors[index-1];
		}
		
		public ObjectData GetObject(int index)
		{
			if(index == 0 || index >= Objects.Length)return default(ObjectData);
			return Objects[index-1];
		}
		
		public ObjectInfo GetObjectInfo(int index)
		{
			if(index == 0)return default(ObjectInfo);
			return ObjectInfos[index-1];
		}
		
		public WallData GetWall(int index)
		{
			if(index == 0)return default(WallData);
			return Walls[index-1];
		}
		
		public IMinimapVisible GetMinimapForm(Block block)
		{
			if(block.IsWall)
			{
				return GetWall(block.Wall);
			}else if(block.IsObject)
			{
				return GetObject(block.Object);
			}else return null;
		}
	}
}
