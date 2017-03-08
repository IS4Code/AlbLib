/* Date: 11.8.2014, Time: 14:48 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace AlbLib.Texts
{
	public class TextLibrary : List<string>, IGameResource, IXmlSerializable
	{
		public TextLibrary()
		{
			
		}
		
		public TextLibrary(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			ushort count = reader.ReadUInt16();
			ushort[] lengths = new ushort[count];
			for(int i = 0; i < count; i++)
			{
				lengths[i] = reader.ReadUInt16();
			}
			for(int i = 0; i < count; i++)
			{
				this.Add(new string(reader.ReadChars(lengths[i])).Split('\0')[0]);
			}
		}
		
		public int Save(Stream output)
		{
			BinaryWriter writer = new BinaryWriter(output);
			writer.Write((ushort)this.Count);
			foreach(string str in this)
			{
				writer.Write((ushort)(str.Length+1));
			}
			foreach(string str in this)
			{
				writer.Write((str+"\0").ToCharArray());
			}
			return 2+this.Count*2+this.Sum(s => s.Length+1);
		}
		
		public bool Equals(IGameResource obj)
		{
			return Equals((object)obj);
		}
		
		public override bool Equals(object obj)
		{
			if(obj is TextLibrary)
			{
				return ((TextLibrary)obj).SequenceEqual(this);
			}else{
				return false;
			}
		}
		
		public override int GetHashCode()
		{
			return this.ToArray().GetHashCode();
		}
			
		public static bool operator ==(TextLibrary lhs, TextLibrary rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(TextLibrary lhs, TextLibrary rhs)
		{
			return !(lhs == rhs);
		}
		
	    public void WriteXml(XmlWriter writer)
	    {
	    	for(int i = 0; i < this.Count; i++)
	    	{
	    		writer.WriteComment(i.ToString());
	    		writer.WriteElementString("string", this[i]);
	    	}
	    }
	    public XmlSchema GetSchema()
	    {
	        throw new NotImplementedException();
	    }
	
	    public void ReadXml(XmlReader reader)
	    {
	        throw new NotImplementedException();
	    }
	}
}
