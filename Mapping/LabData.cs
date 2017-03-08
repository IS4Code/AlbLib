using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlbLib.XLD;

namespace AlbLib.Mapping
{
	[Serializable]
	public class LabData : IGameResource
	{
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
			return GameData.LabData.Open(index);
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
		
		public IEnumerable<Overlay> GetOverlays()
		{
			return Walls.SelectMany(w => w.Overlays);
		}
		
		int IGameResource.Save(Stream output)
		{
			throw new NotImplementedException();
		}
		
		public bool Equals(IGameResource other)
		{
			return Equals((object)other);
		}
		
		public override bool Equals(object obj)
		{
			LabData other = obj as LabData;
			if (other == null)
				return false;
			return this.Scale1 == other.Scale1 && this.CameraHeight == other.CameraHeight && this.CameraAngle == other.CameraAngle && this.Background == other.Background && this.FogDistance == other.FogDistance && this.MaxLightStrength == other.MaxLightStrength && this.Scale2 == other.Scale2 && this.ViewDistance == other.ViewDistance && object.Equals(this.Objects, other.Objects) && object.Equals(this.Floors, other.Floors) && object.Equals(this.ObjectInfos, other.ObjectInfos) && object.Equals(this.Walls, other.Walls);
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * Scale1.GetHashCode();
				hashCode += 1000000009 * CameraHeight.GetHashCode();
				hashCode += 1000000021 * CameraAngle.GetHashCode();
				hashCode += 1000000033 * Background.GetHashCode();
				hashCode += 1000000087 * FogDistance.GetHashCode();
				hashCode += 1000000093 * MaxLightStrength.GetHashCode();
				hashCode += 1000000097 * Scale2.GetHashCode();
				hashCode += 1000000103 * ViewDistance.GetHashCode();
				if (Objects != null)
					hashCode += 1000000123 * Objects.GetHashCode();
				if (Floors != null)
					hashCode += 1000000181 * Floors.GetHashCode();
				if (ObjectInfos != null)
					hashCode += 1000000207 * ObjectInfos.GetHashCode();
				if (Walls != null)
					hashCode += 1000000223 * Walls.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(LabData lhs, LabData rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(LabData lhs, LabData rhs)
		{
			return !(lhs == rhs);
		}

	}
}
