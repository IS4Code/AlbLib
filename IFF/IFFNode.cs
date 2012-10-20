using System;
using System.IO;

namespace AlbLib.IFF
{
	/// <summary>
	/// Represent data container in IFF format. File or node.
	/// </summary>
	[Serializable]
	public abstract class IFFNode
	{
		/// <summary>
		/// Type of data.
		/// </summary>
		public string TypeID{
			get; protected set;
		}
		
		/// <summary>
		/// Length of data in bytes.
		/// </summary>
		public int Length{
			get; protected set;
		}
		
		/// <summary>
		/// IFF reader.
		/// </summary>
		protected IFFReader reader;
		
		/// <summary>
		/// Initializes new instance using stream.
		/// </summary>
		/// <param name="input">
		/// Input stream.
		/// </param>
		protected IFFNode(Stream input)
		{
			reader = new IFFReader(input);
		}
	}
}