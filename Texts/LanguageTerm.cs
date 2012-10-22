using System;

namespace AlbLib.Texts
{
	public class LanguageTerm
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
		
		public LanguageTerm(string german, string english, string french)
		{
			German = german;
			English = english;
			French = french;
		}
	}
}
