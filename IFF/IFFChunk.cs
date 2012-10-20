using System;
namespace AlbLib.IFF
{
	/// <summary>
	/// Structure representing chunk header.
	/// </summary>
	[Serializable]
	public class IFFChunk
	{
		/// <summary>
		/// Type ID of data.
		/// </summary>
		public readonly string TypeID;
		
		/// <summary>
		/// Chunk length.
		/// </summary>
		public readonly int Length;
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public IFFChunk(string typeid, int length)
		{
			TypeID = typeid;
			Length = length;
		}
	}
}