using System;
using System.IO;

namespace AlbLib.Scripting
{
	/// <summary>
	/// Dummy implementation of <see cref="ScriptExecutionMachine"/> which writes parsed functions to an output.
	/// </summary>
	[Serializable]
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
}