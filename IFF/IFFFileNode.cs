using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AlbLib.IFF
{
	/// <summary>
	/// Class respresenting single file in IFF format.
	/// </summary>
	[Serializable]
	public sealed class IFFFileNode : IFFNode
	{
		/// <summary>
		/// List of all entries or nodes in file.
		/// </summary>
		public ReadOnlyCollection<IFFContentNode> Nodes{
			get; private set;
		}
		
		/// <summary>
		/// 4-character format ID.
		/// </summary>
		public string FormatID{
			get; private set;
		}
		
		/// <summary>
		/// Initializes new instance using file name.
		/// </summary>
		/// <param name="file">
		/// File path.
		/// </param>
		public IFFFileNode(string file):this(new FileStream(file, FileMode.Open))
		{}
		
		/// <summary>
		/// Initializes new instance using stream.
		/// </summary>
		/// <param name="input">
		/// Input stream.
		/// </param>
		public IFFFileNode(Stream input) : base(input)
		{
			Apply(reader.ReadFileHeader());
			var nodes = new List<IFFContentNode>();
			int read = 0;
			while(read < Length)
			{
				var node = new IFFContentNode(input);
				read += node.Length;
				nodes.Add(node);
			}
			Nodes = new ReadOnlyCollection<IFFContentNode>(nodes);
		}
		
		private void Apply(IFFFile file)
		{
			this.TypeID = file.TypeID;
			this.Length = file.Length;
			this.FormatID = file.FormatID;
		}
	}
}