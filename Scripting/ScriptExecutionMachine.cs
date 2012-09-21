using System;
namespace AlbLib.Scripting
{
	/// <summary>
	/// Virtual machine which parses script text.
	/// </summary>
	public abstract class ScriptExecutionMachine : IScriptExecutor
	{
		/// <summary>
		/// Default debug executor.
		/// </summary>
		public static readonly DebugExecutor DebugExecutor = new DebugExecutor();
		
		/// <summary>
		/// Occurs when a comment is found.
		/// </summary>
		/// <param name="comment">
		/// Found comment.
		/// </param>
		public virtual void OnComment(string comment){}
		
		/// <summary>
		/// Occurs when a function is called.
		/// </summary>
		/// <param name="function">
		/// Found function name.
		/// </param>
		/// <param name="args">
		/// Found function arguments.
		/// </param>
		public abstract void OnFunction(string function, int[] args);
		
		/// <summary>
		/// Executes a script.
		/// </summary>
		/// <param name="script">
		/// Script text.
		/// </param>
		/// <param name="exception">
		/// Thrown exception, if any, will be stored there.
		/// </param>
		/// <returns>
		/// True on success. False on failure.
		/// </returns>
		public bool Execute(string script, out ScriptExecutionException exception)
		{
			string line = null;
			int i = -1;
			try{
				string[] lines = script.Split('\n');
				for(i = 0; i < lines.Length; i++)
				{
					line = lines[i].Trim();
					if(line.Length == 0)
					{
						continue;
					}else if(line[0] == ';')
					{
						OnComment(line.Substring(1));
					}else{
						string[] parts = line.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
						int[] args = new int[parts.Length-1];
						for(int j = 1; j < parts.Length; j++)
						{
							if(!int.TryParse(parts[j], out args[j-1]))
							{
								throw new ScriptExecutionException(i+1, line, string.Format("Cannot parse argument \"{0}\".", parts[j]), null);
							}
						}
						OnFunction(parts[0], args);
					}
				}
				exception = null;
				return true;
			}catch(ScriptExecutionException e)
			{
				exception = e;
				return false;
			}catch(Exception e)
			{
				exception = new ScriptExecutionException(i+1, line, e.Message, e);
				return false;
			}
		}
		
		/// <summary>
		/// Executes a script. Does not throw any exceptions.
		/// </summary>
		/// <param name="script">
		/// Script text.
		/// </param>
		/// <returns>
		/// True on success. False on failure.
		/// </returns>
		public virtual bool Execute(string script)
		{
			ScriptExecutionException exception;
			return Execute(script, out exception);
		}
	}
}