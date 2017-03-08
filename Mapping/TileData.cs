using System;
using System.IO;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Data about map tile.
	/// </summary>
	[Serializable]
	public struct TileData : IGameResource, IEquatable<TileData>
	{
		/// <summary>
		/// Assigned id.
		/// </summary>
		public readonly int Id;
		
		/// <summary>
		/// TODO.
		/// </summary>
		public readonly byte Type;
		
		/// <summary>
		/// TODO.
		/// </summary>
		public readonly byte Collision;
		
		/// <summary>
		/// TODO.
		/// </summary>
		public readonly short Info;
		
		/// <summary>
		/// TODO.
		/// </summary>
		public readonly short GrID;
		
		/// <summary>
		/// TODO.
		/// </summary>
		public readonly byte FramesCount;
		
		public readonly byte Unknown1;
		
		public bool IsEmpty{
			get{
				return FramesCount == 0;
			}
		}
		
		public bool Discard{
			get{
				return IsEmpty || (Info & 0x20) != 0;
			}
		}
	
		/// <param name="id">
		/// Id to assign.
		/// </param>
		/// <param name="stream">
		/// Source stream.
		/// </param>
		public TileData(int id, Stream stream)
		{
			Id = id;
			
			BinaryReader reader = new BinaryReader(stream);
			Type = reader.ReadByte();
			Collision = reader.ReadByte();
			Info = reader.ReadInt16();
			GrID = reader.ReadInt16();
			FramesCount = reader.ReadByte();
			Unknown1 = reader.ReadByte();
		}
		
		public int Save(Stream output)
		{
			BinaryWriter writer = new BinaryWriter(output);
			writer.Write(Type);
			writer.Write(Collision);
			writer.Write(Info);
			writer.Write(GrID);
			writer.Write(FramesCount);
			writer.Write(Unknown1);
			return 8;
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return (obj is TileData) && Equals((TileData)obj);
		}
		
		public bool Equals(TileData other)
		{
			return this.Id == other.Id && this.Type == other.Type && this.Collision == other.Collision && this.Info == other.Info && this.GrID == other.GrID && this.FramesCount == other.FramesCount && this.Unknown1 == other.Unknown1;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * Id.GetHashCode();
				hashCode += 1000000009 * Type.GetHashCode();
				hashCode += 1000000021 * Collision.GetHashCode();
				hashCode += 1000000033 * Info.GetHashCode();
				hashCode += 1000000087 * GrID.GetHashCode();
				hashCode += 1000000093 * FramesCount.GetHashCode();
				hashCode += 1000000097 * Unknown1.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(TileData lhs, TileData rhs)
		{
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(TileData lhs, TileData rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
}