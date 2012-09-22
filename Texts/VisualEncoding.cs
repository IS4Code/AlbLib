using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using AlbLib.Internal;

namespace AlbLib.Texts
{
	public class VisualEncoding : Encoding
	{
		private Dictionary<char,byte> c2b;
		private Dictionary<byte,char> b2c;
		
		public VisualEncoding() : this(null)
		{
		}
		
		public VisualEncoding(string tree)
		{
			c2b = new Dictionary<char, byte>();
			b2c = new Dictionary<byte, char>();
			c2b.Add('\0', 0);
			b2c.Add(0, '\0');
			AddAll("default");
			if(tree != null)AddAll(tree);
		}
		
		private void AddAll(string tree)
		{
			XDocument table = Resources.CharTable;
			foreach(XElement elem in table.Root.Element(tree).Elements("pair"))
			{
				char ch = elem.Attribute("char").Value[0];
				byte b = Byte.Parse(elem.Attribute("code").Value);
				if(elem.Attribute("nochar")==null)
					c2b[ch] = b;
				if(elem.Attribute("nocode")==null)
					b2c[b] = ch;
			}
		}
		
		public override bool IsSingleByte{
			get{
				return true;
			}
		}
		
		public override int GetByteCount(char[] chars, int index, int count)
		{
			int c = 0;
			for(int i = 0; i < count; i++)
			{
				char ch = chars[i+index];
				if(c2b.ContainsKey(ch))c += 1;
			}
			return c;
		}
		
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			Console.WriteLine(String.Join("", chars));
			Console.WriteLine(charIndex);
			Console.WriteLine(charCount);
			Console.WriteLine(byteIndex);
			for(int i = 0; i < charCount; i++)
			{
				char ch = chars[i+charIndex];
				byte code;
				if(!c2b.TryGetValue(ch, out code))
				{
					continue;
				}
				bytes[i+byteIndex] = code;
			}
			return charCount;
		}
		
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			int c = 0;
			for(int i = 0; i < count; i++)
			{
				byte b = bytes[i+index];
				if(b2c.ContainsKey(b))c += 1;
			}
			return c;
		}
		
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			for(int i = 0; i < byteCount; i++)
			{
				byte b = bytes[i+byteIndex];
				char ch;
				if(!b2c.TryGetValue(b, out ch))
				{
					continue;
				}
				chars[i+charIndex] = ch;
			}
			return byteCount;
		}
		
		public override int GetMaxByteCount(int charCount)
		{
			return charCount;
		}
		
		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}
