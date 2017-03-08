/* Date: 28.8.2014, Time: 0:00 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AlbLib
{
	public abstract class Repository<T> : IRepository, IList<T>, IEnumerable<T> where T : IGameResource
	{
		private T[] cacheBuffer;
		public bool Cache{get;set;}
		
		public Repository()
		{
			cacheBuffer = new T[0];
			Cache = true;
		}
		
		Type IRepository.DataType{
			get{
				return typeof(T);
			}
		}
		object IRepository.Open(object id)
		{
			return this.Open(id);
		}
		object IRepository.Open(int id)
		{
			return this.Open(id);
		}
		
		public abstract PathInfo Path{get;}
		public virtual T Open(object id)
		{
			if(id is int) return Open((int)id);
			throw new NotImplementedException();
		}
		public T Open(int id)
		{
			if(Cache)
			{
				if(cacheBuffer.Length <= id)
				{
					Array.Resize(ref cacheBuffer, id+1);
				}else{
					if(cacheBuffer[id] != null)
					{
						return cacheBuffer[id];
					}
				}
				return cacheBuffer[id] = GetEntry(id);
			}
			return GetEntry(id);
		}
		protected abstract T GetEntry(int id);
		public IEnumerator<T> GetEnumerator()
		{
			foreach(var pair in this.IndexEnumerate())
			{
				yield return pair.Value;
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		
		public T this[int id]
		{
			get{
				return Open(id);
			}
			set{
				throw new NotImplementedException();
			}
		}
		
		public virtual int Count {
			get {
				int c = 0;
				foreach(var e in this)c++;
				return c;
			}
		}
		
		bool ICollection<T>.IsReadOnly {
			get {
				return true;
			}
		}
		
		int IList<T>.IndexOf(T item)
		{
			throw new NotImplementedException();
		}
		
		void IList<T>.Insert(int index, T item)
		{
			throw new NotImplementedException();
		}
		
		void IList<T>.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}
		
		void ICollection<T>.Add(T item)
		{
			throw new NotImplementedException();
		}
		
		void ICollection<T>.Clear()
		{
			throw new NotImplementedException();
		}
		
		bool ICollection<T>.Contains(T item)
		{
			foreach(var elem in this)
			{
				if(elem.Equals(item)) return true;
			}
			return false;
		}
		
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			foreach(var pair in this.IndexEnumerate())
			{
				array[arrayIndex+pair.Key-1] = pair.Value;
			}
		}
		
		bool ICollection<T>.Remove(T item)
		{
			throw new NotImplementedException();
		}
		
		public IEnumerable<KeyValuePair<int,T>> IndexEnumerate()
		{
			int last = 0;
			foreach(var pair in GetPairEnumerator().OrderBy(p => p.Key).Distinct(PairEqualityComparer.Instance))
			{
				if(pair.Key > last+1)
				{
					for(int i = last+1; i < pair.Key; i++)
					{
						yield return new KeyValuePair<int,T>(i, default(T));
					}
				}
				yield return pair;
				last = pair.Key;
			}
		}
		
		private class PairEqualityComparer : IEqualityComparer<KeyValuePair<int,T>>
		{
			public static readonly PairEqualityComparer Instance = new PairEqualityComparer();
			
			private PairEqualityComparer()
			{
				
			}
			
			public int GetHashCode(KeyValuePair<int,T> obj)
			{
				return obj.Key;
			}
			
			public bool Equals(KeyValuePair<int, T> x, KeyValuePair<int, T> y)
			{
				return x.Key == y.Key;
			}
		}
		
		protected abstract IEnumerable<KeyValuePair<int,T>> GetPairEnumerator();
		
		private static readonly XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
		private static readonly XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
		public XDocument ToXml()
		{
			string type = null;
			return SetType(new XDocument(
				new XElement(
					"repository",
					new XAttribute(XNamespace.Xmlns+"xsi", xsi),
					new XAttribute(XNamespace.Xmlns+"xsd", xsd),
					this.IndexEnumerate().SelectMany(p => ToXml(p.Key, p.Value, out type))
				)
			), type);
		}
		
		private XDocument SetType(XDocument doc, string type)
		{
			doc.Root.SetAttributeValue("type", type);
			return doc;
		}
		
		private List<XNode> ToXml(int index, T entry, out string type)
		{
			var ret = new List<XNode>();
			
			var elem = SerializeEntry(entry);
			type = null;
			ret.Add(new XComment(index.ToString()));
			if(elem == null)
			{
				ret.Add(new XElement("entry", new XAttribute(xsi+"nil", true)));
			}else{
				type = elem.Name.ToString();
				foreach(var attr in elem.Attributes(XNamespace.Xmlns+"xsi").Concat(elem.Attributes(XNamespace.Xmlns+"xsd")))
				{
					attr.Remove();
				}
				elem.Name = "entry";
				ret.Add(elem);
			}
			return ret;
		}
		
		private XElement SerializeEntry(T entry)
		{
			if(entry == null) return null;
			if(entry is StringRepository.StringResource) return new XElement("string", entry.ToString());
			XmlSerializer ser = new XmlSerializer(typeof(T));
			MemoryStream buffer = new MemoryStream();
			XmlWriter writer = XmlWriter.Create(buffer);
			ser.Serialize(writer, entry);
			buffer.Seek(0, SeekOrigin.Begin);
			XmlReader reader = XmlReader.Create(buffer);
			return XElement.Load(reader);
		}
	}
}
