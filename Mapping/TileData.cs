using System;
using System.IO;

namespace AlbLib.Mapping
{
	/// <summary>
	/// Data about map tile.
	/// </summary>
	[Serializable]
	public struct TileData
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
	}
}