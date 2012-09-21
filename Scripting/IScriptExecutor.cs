namespace AlbLib.Scripting
{
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
}