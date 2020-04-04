using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlbLib.Scripting
{
    public class EventSet : IGameResource
    {
        public List<int> ActiveBlocks { get; } = new List<int>();
        public List<Block> Blocks { get; } = new List<Block>();

        public EventSet(Stream input)
        {
            var reader = new BinaryReader(input);
            int numActive = reader.ReadInt16();
            int numBlocks = reader.ReadInt16();
            for(int i = 0; i < numActive; i++)
            {
                ActiveBlocks.Add(reader.ReadInt16());
            }
            for(int i = 0; i < numBlocks; i++)
            {
                Blocks.Add(new Block(reader));
            }
        }

        public int Save(Stream output)
        {
            throw new NotImplementedException();
        }

        public class Block
        {
            public byte Type { get; set; }
            public byte Field1;
            public byte Field2;
            public byte Field3;
            public byte Field4;
            public byte Field5;
            public short Field6;
            public short Field7;
            public short Next { get; }

            public Block(BinaryReader reader)
            {
                Type = reader.ReadByte();
                Field1 = reader.ReadByte();
                Field2 = reader.ReadByte();
                Field3 = reader.ReadByte();
                Field4 = reader.ReadByte();
                Field5 = reader.ReadByte();
                Field6 = reader.ReadInt16();
                Field7 = reader.ReadInt16();
                Next = reader.ReadInt16();
            }
        }
    }
}
