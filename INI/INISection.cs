using System;
using System.Collections;
using System.Collections.Generic;

namespace AlbLib.INI
{
	public class INISection : INIObject, IDictionary<string,string>, ICollection<INIProperty>
	{
		protected readonly List<INIProperty> props;
		
		public INISection(string name) : base(name)
		{
			props = new List<INIProperty>();
		}
		
		public INISection(string name, IDictionary<string,string> properties) : this(name)
		{
			if(properties != null)
			{
				foreach(KeyValuePair<string,string> pair in properties)
				{
					props.Add(new INIProperty(pair.Key, pair.Value, this));
				}
			}
		}
		
		public INISection(string name, IList<INIProperty> properties) : this(name)
		{
			if(properties != null)
			{
				foreach(INIProperty prop in properties)
				{
					props.Add(prop);
				}
			}
		}
		
		public bool ContainsKey(string key)
		{
			foreach(INIProperty prop in props)
			{
				if(prop.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))return true;
			}
			return false;
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		
		public IEnumerator<INIProperty> GetEnumerator()
		{
			return props.GetEnumerator();
		}
		
		IEnumerator<KeyValuePair<string,string>> IEnumerable<KeyValuePair<string,string>>.GetEnumerator()
		{
			foreach(INIProperty prop in props)
			{
				yield return new KeyValuePair<string,string>(prop.Name, prop.Value);
			}
		}
		
		public bool IsReadOnly{
			get{
				return false;
			}
		}
		
		public int Count{
			get{
				return props.Count;
			}
		}
		
		public bool Remove(INIProperty item)
		{
			return props.Remove(item);
		}
		
		public void CopyTo(INIProperty[] array, int arrayIndex)
		{
			props.CopyTo(array, arrayIndex);
		}
		
		public bool Contains(INIProperty item)
		{
			return props.Contains(item);
		}
		
		public void Clear()
		{
			props.Clear();
		}
		
		public void Add(INIProperty item)
		{
			props.Add(item);
		}
		
		public INIProperty this[string key]
		{
			get{
				foreach(INIProperty prop in props)
				{
					if(prop.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))
					{
						return prop;
					}
				}
				throw new KeyNotFoundException();
			}
		}
		
		bool ICollection<KeyValuePair<string,string>>.Remove(KeyValuePair<string,string> item)
		{
			foreach(INIProperty prop in props)
			{
				if(prop.Name.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase) && prop.Value.Equals(item.Value, StringComparison.CurrentCultureIgnoreCase))
				{
					return props.Remove(prop);
				}
			}
			return false;
		}
		
		void ICollection<KeyValuePair<string,string>>.CopyTo(KeyValuePair<string,string>[] array, int arrayIndex)
		{
			for(int i = 0; i < props.Count; i++)
			{
				array[arrayIndex+i] = new KeyValuePair<string, string>(props[i].Name, props[i].Value);
			}
		}
		
		bool ICollection<KeyValuePair<string,string>>.Contains(KeyValuePair<string,string> item)
		{
			foreach(INIProperty prop in props)
			{
				if(prop.Name.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase) && prop.Value.Equals(item.Value, StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
		
		void ICollection<KeyValuePair<string,string>>.Add(KeyValuePair<string,string> item)
		{
			props.Add(new INIProperty(item.Key, item.Value, this));
		}
		
		ICollection<string> IDictionary<string,string>.Values{
			get{
				string[] values = new string[props.Count];
				for(int i = 0; i < props.Count; i++)
				{
					values[i] = props[i].Value;
				}
				return values;
			}
		}
		
		ICollection<string> IDictionary<string,string>.Keys{
			get{
				string[] keys = new string[props.Count];
				for(int i = 0; i < props.Count; i++)
				{
					keys[i] = props[i].Name;
				}
				return keys;
			}
		}
		
		string IDictionary<string,string>.this[string key]
		{
			get{
				foreach(INIProperty prop in props)
				{
					if(prop.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))return prop.Value;
				}
				throw new KeyNotFoundException();
			}
			set{
				foreach(INIProperty prop in props)
				{
					if(prop.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))prop.Value = value;
					return;
				}
				props.Add(new INIProperty(key, value));
			}
		}
		
		bool IDictionary<string,string>.TryGetValue(string key, out string value)
		{
			value = null;
			foreach(INIProperty prop in props)
			{
				if(prop.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))
				{
					value = prop.Value;
					return true;
				}
			}
			return false;
		}
		
		public void Add(string key, string value)
		{
			props.Add(new INIProperty(key, value, this));
		}
		
		public bool Remove(string key)
		{
			foreach(INIProperty prop in props)
			{
				if(prop.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))
				{
					return props.Remove(prop);
				}
			}
			return false;
		}
		
		public override string ToString()
		{
			return "["+Name+"]";
		}
	}
}
