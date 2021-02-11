using System;
using System.IO;
using System.Text;
using AlbLib.Caching;

namespace AlbLib.Texts
{
	/// <summary>
	/// Core texts class.
	/// </summary>
	public static class TextCore
	{
		//private static string[][] ItemNames;
		private static IndexedCache<LanguageTerm, RefEq<Encoding>> NameCache = new IndexedCache<LanguageTerm, RefEq<Encoding>>(LoadItemName);
		
		
		private static Language _defaultLanguage;
		
		/// <summary>
		/// Default language in common localization operations.
		/// </summary>
		public static Language DefaultLanguage{
			get{
				return _defaultLanguage;
			}
			set{
				switch(value)
				{
					case Language.German:case Language.English:case Language.French:
						_defaultLanguage = value;
						return;
					default:
						throw new ArgumentException("Default language cannot be undefined or invariant.", "value");
				}
			}
		}
		
		private static string deflang;
		public static string DefaultLanguageFolder{
			get{
				return deflang??"ENGLISH";
			}
			set{
				if(value == null) throw new ArgumentNullException("value");
				deflang = value;
			}
		}
		
		/// <summary>
		/// Encoding used in all text-related readings.
		/// </summary>
		public static Encoding DefaultEncoding{
			get;set;
		}
		
		static TextCore()
		{
			DefaultLanguage = Language.English;
			DefaultEncoding = Encoding.ASCII;
		}
		
		private static string ReadString(BinaryReader reader)
		{
			return TrimNull(reader.ReadChars(20));
		}
		
		private static LanguageTerm LoadItemName(int index, RefEq<Encoding> encoding)
		{
			if(index == 0)return LanguageTerm.Empty;
			index -= 1;
			using(FileStream stream = new FileStream(Paths.ItemName, FileMode.Open))
			{
				stream.Seek(index*60, SeekOrigin.Begin);
				BinaryReader reader = new BinaryReader(stream, encoding.Value);
				return new LanguageTerm(ReadString(reader), ReadString(reader), ReadString(reader));
			}
		}
		
		/// <summary>
		/// Gets localized item name for <paramref name="type"/> using default language.
		/// </summary>
		/// <param name="type">
		/// Item type.
		/// </param>
		/// <returns>
		/// The localized name.
		/// </returns>
		public static string GetItemName(int type)
		{
			return GetItemName(type, DefaultLanguage);
		}
		
		/// <summary>
		/// Gets localized item name for <paramref name="type"/> and specified <paramref name="language"/>.
		/// </summary>
		/// <param name="type">
		/// Item type.
		/// </param>
		/// <param name="language">
		/// Language of the name.
		/// </param>
		/// <returns>
		/// The localized name.
		/// </returns>
		public static string GetItemName(int type, Language language)
		{
			if(type == 0)return String.Empty;
			return NameCache[type,TextCore.DefaultEncoding][language];
		}
		
		/// <summary>
		/// Trims a string to first null character.
		/// </summary>
		/// <param name="str">
		/// String to trim.
		/// </param>
		/// <returns>
		/// Trimmed string.
		/// </returns>
		public static string TrimNull(string str)
		{
			return str.Substring(0, str.IndexOf('\0'));
		}
		
		/// <summary>
		/// Creates a string from character array up to first null character.
		/// </summary>
		/// <param name="chars">
		/// Source character array.
		/// </param>
		/// <returns>
		/// New string.
		/// </returns>
		public static string TrimNull(char[] chars)
		{
			StringBuilder str = new StringBuilder(chars.Length);
			foreach(char ch in chars)
			{
				if(ch == '\0')break;
				str.Append(ch);
			}
			return str.ToString();
		}
		
		public static string ReadNullTerminatedString(this BinaryReader reader, int length)
		{
			StringBuilder builder = new StringBuilder(length);
			for(int i = 0; i < length; i++)
			{
				char ch = reader.ReadChar();
				if(ch == '\0')
				{
					reader.ReadBytes(length-i-1);
					break;
				}
				builder.Append(ch);
			}
			return builder.ToString();
		}

        public static void ClearCache()
        {
            NameCache.Clear();
        }
	}
}