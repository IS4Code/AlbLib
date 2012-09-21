using System;
namespace AlbLib.Scripting
{
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