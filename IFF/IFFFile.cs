namespace AlbLib.IFF
{
	/// <summary>
	/// Structure representing file header.
	/// </summary>
	public struct IFFFile
	{
		/// <summary>
		/// Type ID of data.
		/// </summary>
		/// <returns>
		/// FORM
		/// </returns>
		public readonly string TypeID;
		
		/// <summary>
		/// File subformat.
		/// </summary>
		public readonly string FormatID;
		
		/// <summary>
		/// File length.
		/// </summary>
		public readonly int Length;
		
		/// <summary>
		/// Initializes new instance.
		/// </summary>
		public IFFFile(string typeid, string formatid, int length)
		{
			TypeID = typeid;
			FormatID = formatid;
			Length = length;
		}
	}
}