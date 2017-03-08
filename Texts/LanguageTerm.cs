using System;
using System.IO;

namespace AlbLib.Texts
{
	public struct LanguageTerm : IEquatable<LanguageTerm>
	{
		public static readonly LanguageTerm Empty = new LanguageTerm("","","");
		
		public string German{get;set;}
		public string English{get;set;}
		public string French{get;set;}
		
		public string this[Language lang]
		{
			get{
				switch(lang)
				{
					case Language.German:
						return German;
					case Language.English:
						return English;
					case Language.French:
						return French;
					default:
						return this[TextCore.DefaultLanguage];
				}
			}
			set{
				switch(lang)
				{
					case Language.German:
						German = value; break;
					case Language.English:
						English = value; break;
					case Language.French:
						French = value; break;
					default:
						this[TextCore.DefaultLanguage] = value; break;
				}
			}
		}
		
		public LanguageTerm(string german, string english, string french) : this()
		{
			German = german;
			English = english;
			French = french;
		}
		
		public LanguageTerm(Stream stream) : this(new BinaryReader(stream, TextCore.DefaultEncoding))
		{
			
		}
		
		public LanguageTerm(BinaryReader reader) : this(reader.ReadNullTerminatedString(16), reader.ReadNullTerminatedString(16), reader.ReadNullTerminatedString(16))
		{
			
		}
		
		public override string ToString()
		{
			return String.Format("[German={0}, English={1}, French={2}]", German, English, French);
		}

		public bool IsEmpty{
			get{
				return German.Length == 0 && English.Length == 0 && French.Length == 0;
			}
		}
		
		public string UnifiedName{
			get{
				return English.Length > 0 ? English : German.Length > 0 ? German : French;
			}
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			return (obj is LanguageTerm) && Equals((LanguageTerm)obj);
		}
		
		public bool Equals(LanguageTerm other)
		{
			return this.German == other.German && this.English == other.English && this.French == other.French;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (German != null)
					hashCode += 1000000007 * German.GetHashCode();
				if (English != null)
					hashCode += 1000000009 * English.GetHashCode();
				if (French != null)
					hashCode += 1000000021 * French.GetHashCode();
			}
			return hashCode;
		}
		
		public static bool operator ==(LanguageTerm lhs, LanguageTerm rhs)
		{
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(LanguageTerm lhs, LanguageTerm rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
}
