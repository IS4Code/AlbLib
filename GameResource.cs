/* Date: 27.8.2014, Time: 0:22 */
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AlbLib
{
	[StructLayout(LayoutKind.Sequential)]
	public abstract class GameResource
	{
		public GameResource()
		{
			
		}
		
		public GameResource(Stream input)
		{
			FillFrom(input);
		}
		
		protected void FillFrom(Stream input)
		{
			BinaryReader reader = new BinaryReader(input);
			Type t = this.GetType();
			foreach(PropertyInfo pi in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if(pi.GetCustomAttributes(typeof(SkipAttribute), true).Length > 0) continue;
				if(pi.GetSetMethod(true) == null) continue;
				Type ft = pi.PropertyType;
				object value = null;
				int length = 0;
				if(ft.IsArray)
				{
					MarshalAsAttribute marshal = (MarshalAsAttribute)pi.GetCustomAttributes(typeof(MarshalAsAttribute), true).FirstOrDefault();
					if(marshal != null)
					{
						length = marshal.SizeConst;
					}
					if(length <= 0)
					{
						VariableSizeAttribute varsize = (VariableSizeAttribute)pi.GetCustomAttributes(typeof(VariableSizeAttribute), true).FirstOrDefault();
						if(varsize != null)
						{
							length = varsize.GetSize(this);
						}
					}
				}
				value = ReadObject(ft, reader, length);
				pi.SetValue(this, value, null);
			}
		}
		
		private static object ReadObject(Type type, BinaryReader reader, int size)
		{
			if(type.IsEnum)
			{
				return Enum.ToObject(type, ReadObject(Enum.GetUnderlyingType(type), reader, 0));
			}if(type.IsPrimitive)
			{
				size = Marshal.SizeOf(type);
				byte[] data = reader.ReadBytes(size);
				if(data.Length < size) throw new EndOfStreamException();
				IntPtr ptr = Marshal.AllocHGlobal(size);
				try{
					Marshal.Copy(data, 0, ptr, size);
					return Marshal.PtrToStructure(ptr, type);
				}finally{
					Marshal.FreeHGlobal(ptr);
				}
			}else if(type.IsArray)
			{
				Type et = type.GetElementType();
				Array arr = Array.CreateInstance(et, size);
				for(int i = 0; i < size; i++)
				{
					arr.SetValue(ReadObject(et, reader, 0), i);
				}
				return arr;
			}
			return null;
		}
		
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
		protected class VariableSizeAttribute : Attribute
		{
			public string SizeProperty;
			public VariableSizeAttribute(string property)
			{
				SizeProperty = property;
			}
			
			public int GetSize(object instance)
			{
				Type type = instance.GetType();
				object ret = type.InvokeMember(SizeProperty, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, instance, null);
				return Convert.ToInt32(ret);
			}
		}
		
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
		protected class SkipAttribute : Attribute
		{
			
		}
	}
}
