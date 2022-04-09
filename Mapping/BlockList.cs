using System;
using System.Collections.Generic;
using System.IO;

namespace AlbLib.Mapping
{
    public class BlockList : IGameResource
    {
        public readonly int Id;

        public List<Block2D> Blocks { get; }

        public BlockList(int id, Stream blocks)
        {
            Blocks = new List<Block2D>();
            while(blocks.Position < blocks.Length)
            {
                Blocks.Add(new Block2D(blocks));
            }
        }

        public int Save(Stream output)
        {
            throw new NotImplementedException();
        }
    }
}
