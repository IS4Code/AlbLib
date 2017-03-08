/* Date: 28.8.2014, Time: 0:01 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AlbLib.Texts;

namespace AlbLib
{
	public class SysTextRepository : StringRepository
	{
		public static readonly Regex LineRegex = new Regex(@"\[([0-9]{4}):(.*?)\]", RegexOptions.Compiled);
		
		private readonly Func<PathInfo> pathGet;
		public override PathInfo Path{get{return pathGet();}}
		
		public SysTextRepository(PathInfo path) : this(()=>path)
		{
			
		}
		
		public SysTextRepository(Func<PathInfo> pathGet)
		{
			this.pathGet = pathGet;
		}
		
		protected override string OpenImpl(int id)
		{
			return IndexEnumerate().FirstOrDefault(p => p.Key == id).Value;
		}
 
		public new string Open(int id)
		{
			return OpenImpl(id);
		}
		
		public new IEnumerable<KeyValuePair<int,string>> IndexEnumerate()
		{
			return base.IndexEnumerate().Select(p => new KeyValuePair<int,string>(p.Key, p.Value==null?null:p.Value.Value));
		}
		
		protected override IEnumerable<KeyValuePair<int, StringRepository.StringResource>> GetPairEnumerator()
		{
			using(StreamReader reader = File.OpenText(Path))
			{
				string line;
				while((line = reader.ReadLine()) != null)
				{
					var match = LineRegex.Match(line);
					if(match.Success)
					{
						int id = Int32.Parse(match.Groups[1].Value);
						string value = match.Groups[2].Value;
						yield return new KeyValuePair<int,StringRepository.StringResource>(id,new StringRepository.StringResource(value));
					}
				}
			}
		}
	}
}
