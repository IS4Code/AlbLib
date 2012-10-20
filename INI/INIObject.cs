using System;

namespace AlbLib.INI
{
	public abstract class INIObject
	{
		private readonly string name;
		public string Name
		{
			get{
				return name;
			}
		}
		
		protected INIObject(string name)
		{
			this.name = name;
		}
	}
}
