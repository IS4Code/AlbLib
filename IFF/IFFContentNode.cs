using System.IO;

namespace AlbLib.IFF
{
	/// <summary>
	/// Class representing single node in IFF file.
	/// </summary>
	public class IFFContentNode : IFFNode
	{
		/// <summary>
		/// Node's content as byte array.
		/// </summary>
		public byte[] Content{
			get; protected set;
		}
		
		/// <summary>
		/// Initializes new instance using stream.
		/// </summary>
		/// <param name="input">
		/// Input stream.
		/// </param>
		public IFFContentNode(Stream input) : base(input)
		{
			Apply(reader.ReadChunkHeader());
			Content = new byte[Length];
			Length = input.Read(Content, 0, Length);
		}
		
		private void Apply(IFFChunk chunk)
		{
			this.TypeID = chunk.TypeID;
			this.Length = chunk.Length;
		}
	}
}
