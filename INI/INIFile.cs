using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlbLib.INI
{
	public class INIFile : INISection, ICollection<INIObject>
	{
		private readonly IList<INISection> sections;
		
		public INIFile(string file) : base(file)
		{
			sections = new List<INISection>();
			using(StreamReader reader = File.OpenText(file))
			{
				string line;
				INISection actsection = null;
				while((line = reader.ReadLine()) != null)
				{
					if(line.Length == 0 || line[0] == ';')continue;
					line = line.Trim(' ', '\t');
					if(line[0] == '[')
					{
						actsection = new INISection(line.Substring(1, line.Length-2));
						sections.Add(actsection);
					}else{
						string[] sep = line.Split('=');
						var prop = new INIProperty(sep[0], sep[1], actsection);
						if(actsection == null)
						{
							base.Add(prop);
						}else{
							actsection.Add(prop);
						}
					}
				}
			}
		}
		
		public INIFile() : base(null)
		{
			this.sections = new List<INISection>();
		}
		
		public INIFile(IList<INISection> sections) : base(null)
		{
			if(sections != null)
			{
				this.sections = sections;
			}else{
				this.sections = new List<INISection>();
			}
		}
		
		public new INIObject this[string key]
		{
			get{
				foreach(INISection sec in sections)
				{
					if(sec.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))return sec;
				}
				foreach(INIProperty prop in props)
				{
					if(prop.Name.Equals(key, StringComparison.CurrentCultureIgnoreCase))return prop;
				}
				throw new KeyNotFoundException();
			}
		}
		
		public INIProperty this[string section, string property]
		{
			get{
				if(section == null)
				{
					foreach(INIProperty prop in props)
					{
						if(prop.Name.Equals(property, StringComparison.CurrentCultureIgnoreCase))
						{
							return prop;
						}
					}
				}else{
					foreach(INISection sec in sections)
					{
						if(sec.Name.Equals(section, StringComparison.CurrentCultureIgnoreCase))
						{
							foreach(INIProperty prop in sec)
							{
								if(prop.Name.Equals(property, StringComparison.CurrentCultureIgnoreCase))
								{
									return prop;
								}
							}
						}
					}
				}
				throw new KeyNotFoundException();
			}
		}
		
		public new IEnumerator<INIObject> GetEnumerator()
		{
			return sections.GetEnumerator();
		}
		
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach(INIProperty prop in props)
			{
				builder.AppendLine(prop.ToString());
			}
			foreach(INISection sec in sections)
			{
				builder.AppendLine(sec.ToString());
				foreach(INIProperty prop in sec)
				{
					builder.AppendLine(prop.ToString());
				}
			}
			return builder.ToString();
		}
		
		public void Save(string file)
		{
			using(StreamWriter writer = File.CreateText(file))
			{
				foreach(INIProperty prop in props)
				{
					writer.WriteLine(prop.ToString());
				}
				foreach(INISection sec in sections)
				{
					writer.WriteLine(sec.ToString());
					foreach(INIProperty prop in sec)
					{
						writer.WriteLine(prop.ToString());
					}
				}
			}
		}
		
		public List<INIProperty> GetAllProperties()
		{
			return new List<INIProperty>(AllProperties);
		}
		
		public IEnumerable<INIProperty> AllProperties{
			get{
				foreach(INIProperty prop in props)
				{
					yield return prop;
				}
				foreach(INISection sec in sections)
				{
					foreach(INIProperty prop in sec)
					{
						yield return prop;
					}
				}
			}
		}
		
		public bool Remove(INIObject item)
		{
			if(item is INISection)
			{
				return sections.Remove((INISection)item);
			}else if(item is INIProperty)
			{
				return props.Remove((INIProperty)item);
			}
			return false;
		}
		
		public void CopyTo(INIObject[] array, int arrayIndex)
		{
			props.CopyTo((INIProperty[])array, arrayIndex);
			sections.CopyTo((INISection[])array, arrayIndex+props.Count);
		}
		
		public bool Contains(INIObject item)
		{
			if(item is INISection)
			{
				return sections.Contains((INISection)item);
			}else if(item is INIProperty)
			{
				return props.Contains((INIProperty)item);
			}
			return false;
		}
		
		public void Add(INIObject item)
		{
			if(item is INISection)
			{
				sections.Add((INISection)item);
			}else if(item is INIProperty)
			{
				props.Add((INIProperty)item);
			}
		}
	}
}
