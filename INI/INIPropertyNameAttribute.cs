using System;

namespace AlbLib.INI
{
	public class INIPropertyNameAttribute : Attribute
	{
		public string Name{get;private set;}
		
		public INIPropertyNameAttribute(string name)
		{
			Name = name;
		}
	}
}
