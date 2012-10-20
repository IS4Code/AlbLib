using System;
using System.Globalization;

namespace AlbLib.INI
{
	public class INIProperty : INIObject
	{
		public string Value{get;set;}
		public INISection Section{get;internal set;}
		
		public INIProperty(string name, INISection section) : this(name, null, section)
		{}
		
		public INIProperty(string name, string value, INISection section) : this(name, value)
		{
			Section = section;
		}
		
		public INIProperty(string name) : this(name, (string)null)
		{}
		
		public INIProperty(string name, string value) : base(name)
		{
			Value = value;
		}
		
		public override string ToString()
		{
			return Name+"="+Value;
		}
		
		public object ObjectValue{
			get{
				if(Value.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}else if(Value.Equals("N", StringComparison.CurrentCultureIgnoreCase))
				{
					return false;
				}else{
					int output;
					if(Int32.TryParse(Value, out output))
					{
						return output;
					}else{
						return Value;
					}
				}
			}
			set{
				if(value is bool)
				{
					if(value.Equals(true))
					{
						Value = "Y";
					}else{
						Value = "N";
					}
				}else if(value is int)
				{
					Value = ((int)value).ToString(NumberFormatInfo.InvariantInfo);
				}else{
					Value = value.ToString();
				}
			}
		}
	}
}
