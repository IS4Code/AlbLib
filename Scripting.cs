using System;
using System.IO;
using System.Text;
using AlbLib.XLD;

namespace AlbLib
{
	namespace Scripting
	{
		/// <summary>
		/// Core scripting class.
		/// </summary>
		public static class Scripts
		{
			/// <summary>
			/// Returns script text.
			/// </summary>
			/// <param name="index">
			/// Zero-based script index.
			/// </param>
			/// <returns>
			/// Script.
			/// </returns>
			public static string GetScript(int index)
			{
				int subindex = index%100;
				int fileindex = index/100;
				using(FileStream stream = new FileStream(String.Format(Paths.ScriptsN, fileindex), FileMode.Open))
				{
					byte[] content = XLDFile.ReadSubfile(stream, subindex);
					return Encoding.ASCII.GetString(content);
				}
			}
			
			/// <summary>
			/// Executes script.
			/// </summary>
			/// <param name="script">
			/// The script text to execute.
			/// </param>
			/// <param name="executor">
			/// Virtual machine which executes the script.
			/// </param>
			/// <returns>
			/// True on success. False on failure.
			/// </returns>
			public static bool RunScript(string script, IScriptExecutor executor)
			{
				return executor.Execute(script);
			}
			
			/// <summary>
			/// Executes script.
			/// </summary>
			/// <param name="index">
			/// The script index to execute.
			/// </param>
			/// <param name="executor">
			/// Virtual machine which executes the script.
			/// </param>
			/// <returns>
			/// True on success. False on failure.
			/// </returns>
			public static bool RunScript(int index, IScriptExecutor executor)
			{
				return executor.Execute(GetScript(index));
			}
			
			/// <summary>
			/// Executes script.
			/// </summary>
			/// <param name="script">
			/// The script text to execute.
			/// </param>
			/// <param name="handler">
			/// Delegate which is called.
			/// </param>
			/// <returns>
			/// True on success. False on failure.
			/// </returns>
			public static bool RunScript(string script, ExecuteHandler handler)
			{
				return handler(script);
			}
			
			/// <summary>
			/// Executes script.
			/// </summary>
			/// <param name="index">
			/// The script index to execute.
			/// </param>
			/// <param name="handler">
			/// Delegate which is called.
			/// </param>
			/// <returns>
			/// True on success. False on failure.
			/// </returns>
			public static bool RunScript(int index, ExecuteHandler handler)
			{
				return handler(GetScript(index));
			}
		}
		
		/// <summary>
		/// Dummy implementation of <see cref="ScriptExecutionMachine"/> which writes parsed functions to an output.
		/// </summary>
		public class DebugExecutor : ScriptExecutionMachine
		{
			/// <summary>
			/// Output where to write debug information.
			/// </summary>
			public TextWriter Output{get;set;}
			
			/// <summary>
			/// Occurs when a comment is found.
			/// </summary>
			/// <param name="comment">
			/// Found comment.
			/// </param>
			public override void OnComment(string comment)
			{
				Output.WriteLine("//"+comment);
			}
			
			/// <summary>
			/// Occurs when a function is called.
			/// </summary>
			/// <param name="function">
			/// Found function name.
			/// </param>
			/// <param name="args">
			/// Found function arguments.
			/// </param>
			public override void OnFunction(string function, int[] args)
			{
				Output.WriteLine("{0}({1})", function, String.Join(", ", args));
			}
			
			/// <summary>
			/// Initializes new instance using <see cref="Console.Out"/> as an output.
			/// </summary>
			public DebugExecutor() : this(Console.Out)
			{}
			
			/// <summary>
			/// Initializes new instance using <paramref name="output"/> as an output.
			/// </summary>
			/// <param name="output">
			/// Output where debug information will be written.
			/// </param>
			public DebugExecutor(TextWriter output)
			{
				Output = output;
			}
			
			/// <summary>
			/// Executes a script.
			/// </summary>
			/// <param name="script">
			/// Script text.
			/// </param>
			/// <returns>
			/// True on success.
			/// </returns>
			/// <exception cref="ScriptExecutionException">
			/// When any exception raises in script execution.
			/// </exception>
			public override bool Execute(string script)
			{
				ScriptExecutionException exception;
				if(!Execute(script, out exception))
				{
					throw exception;
				}else{
					return true;
				}
			}
		}
		
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
		
		/// <summary>
		/// Delegate which handles scripts.
		/// </summary>
		public delegate bool ExecuteHandler(string script);
		
		/// <summary>
		/// Interface which handles scripts.
		/// </summary>
		public interface IScriptExecutor
		{
			/// <summary>
			/// Executes a script.
			/// </summary>
			/// <param name="script">
			/// Script text.
			/// </param>
			/// <returns>
			/// True on success. If handled, false on failure.
			/// </returns>
			/// <exception cref="ScriptExecutionException">
			/// In some implementations when exception occurs during parsing or at runtime.
			/// </exception>
			bool Execute(string script);
		}
		
		/// <summary>
		/// Exception which stores additional information when normal exception raises.
		/// </summary>
		public class ScriptExecutionException : Exception
		{
			/// <summary>
			/// Line number.
			/// </summary>
			public int LinePosition{get;private set;}
			
			/// <summary>
			/// Actual line.
			/// </summary>
			public string CurrentLine{get;private set;}
			
			/// <summary>
			/// Initializes new instance of this exception.
			/// </summary>
			/// <param name="line">
			/// Line number where did exception occur.
			/// </param>
			/// <param name="errorline">
			/// Actual line.
			/// </param>
			/// <param name="message">
			/// Message which was thrown.
			/// </param>
			/// <param name="innerException">
			/// Inner exception if any.
			/// </param>
			public ScriptExecutionException(int line, string errorline, string message, Exception innerException) : base(message, innerException)
			{
				LinePosition = line;
				CurrentLine = errorline;
			}
		}
	}
}