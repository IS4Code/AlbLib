using System;
using System.Reflection;

namespace AlbLib.INI
{
	[Serializable]
	public abstract class Config
	{
		public string FileName{get;set;}
		
		public Config()
		{
		}
		
		public Config(string file) : this()
		{
			Type iniPropertyNameAttribute = typeof(INIPropertyNameAttribute);
			INIFile ini = new INIFile(file);
			foreach(PropertyInfo sectprop in this.GetType().GetProperties())
			{
				object[] sectattr = sectprop.GetCustomAttributes(iniPropertyNameAttribute, false);
				if(sectattr.Length != 0)
				{
					string sectname = ((INIPropertyNameAttribute)((sectattr)[0])).Name;
					INISection section = ini[sectname] as INISection;
					object sectobj = sectprop.GetValue(this, null);
					if(sectobj == null)
					{
						sectobj = Activator.CreateInstance(sectprop.PropertyType);
						sectprop.SetValue(this, sectobj, null);
					}
					if(section != null)
					{
						foreach(PropertyInfo prop in sectprop.PropertyType.GetProperties())
						{
							object[] attr = prop.GetCustomAttributes(iniPropertyNameAttribute, false);
							if(attr.Length != 0)
							{
								string name = ((INIPropertyNameAttribute)((attr)[0])).Name;
								switch(Type.GetTypeCode(prop.PropertyType))
								{
									case TypeCode.Int32:
										prop.SetValue(sectobj, section.GetProperty(name).ToInt32(), null);
										break;
									case TypeCode.Boolean:
										prop.SetValue(sectobj, section.GetProperty(name).ToBoolean(), null);
										break;
									case TypeCode.String:
										prop.SetValue(sectobj, section.GetProperty(name).Value, null);
										break;
								}
							}
						}
					}
				}
			}
			FileName = file;
		}
		
		public virtual void Save()
		{
			Save(FileName);
		}
		
		public virtual void Save(string file)
		{
			Type iniPropertyNameAttribute = typeof(INIPropertyNameAttribute);
			INIFile ini = new INIFile();
			foreach(PropertyInfo sectprop in this.GetType().GetProperties())
			{
				object[] sectattr = sectprop.GetCustomAttributes(iniPropertyNameAttribute, false);
				if(sectattr.Length != 0)
				{
					string sectname = ((INIPropertyNameAttribute)((sectattr)[0])).Name;
					INISection section = new INISection(sectname);
					object sectobj = sectprop.GetValue(this, null);
					if(section != null)
					{
						foreach(PropertyInfo prop in sectprop.PropertyType.GetProperties())
						{
							object[] attr = prop.GetCustomAttributes(iniPropertyNameAttribute, false);
							if(attr.Length != 0)
							{
								string name = ((INIPropertyNameAttribute)((attr)[0])).Name;
								section.Add(new INIProperty(name, prop.GetValue(sectobj, null)));
							}
						}
					}
					ini.Add(section);
				}
			}
			ini.Save(file);
		}
	}
}
