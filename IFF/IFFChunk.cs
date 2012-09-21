namespace AlbLib.IFF
{
	/// <summary>
	/// Structure representing chunk header.
	/// </summary>
	public struct IFFChunk
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