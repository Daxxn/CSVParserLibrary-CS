using CSVParserLibrary.Models;

namespace CSVParserLibrary
{
   /// <summary>
   /// Parse CSV files. See README for usage.
   /// </summary>
   public interface ICSVParser
   {
      /// <summary>
      /// Remove all exclusion functions.
      /// </summary>
      void ClearExclusionFunctions();

      /// <summary>
      /// Parse the provided CSV stream and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="stream">Stream to read from.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      /// <exception cref="Exception"></exception>
      CSVParseResult<T> ParseFile<T>(Stream stream) where T : class, new();

      /// <summary>
      /// Parse the provided CSV file stream and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="reader">File stream to read from.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      /// <exception cref="Exception"/>
      /// <exception cref="CSVLineException"/>
      /// <exception cref="AggregateException"/>
      CSVParseResult<T> ParseFile<T>(StreamReader reader) where T : class, new();

      /// <summary>
      /// Parse the provided CSV file stream and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="stream">File stream to read from.</param>
      /// <param name="options">Options to use instead of the defaults.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      /// <exception cref="Exception"/>
      /// <exception cref="CSVLineException"/>
      /// <exception cref="AggregateException"/>
      CSVParseResult<T> ParseFile<T>(StreamReader stream, ICSVParserOptions options) where T : class, new();

      /// <summary>
      /// Open and parse the provided CSV file and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="filePath">file path to open.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      /// <exception cref="AggregateException"/>
      CSVParseResult<T> ParseFile<T>(string filePath) where T : class, new();

      /// <summary>
      /// Open and parse the provided CSV file and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="filePath">file path to open.</param>
      /// <param name="options">Options to use instead of the defaults.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      CSVParseResult<T> ParseFile<T>(string filePath, ICSVParserOptions options) where T : class, new();

      /// <summary>
      /// Parse the provided CSV stream and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="stream">Stream to read from.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      /// <exception cref="Exception"></exception>
      Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream) where T : class, new();

      /// <summary>
      /// Open and parse the provided CSV file and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="filePath">file path to open.</param>
      /// <param name="options">Options to use instead of the defaults.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream, ICSVParserOptions options) where T : class, new();

      /// <summary>
      /// Parse CSV file asyncronously.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="filePath">File to parse.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath) where T : class, new();

      /// <summary>
      /// Parse CSV file asyncronously.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="filePath">File to parse.</param>
      /// <param name="options">Change the options used during parsing.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath, ICSVParserOptions options) where T : class, new();

      /// <summary>
      /// Parse multiple files in parallel.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="filePaths"><see cref="Array"/> of files to parse.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      IEnumerable<CSVParseResult<T>> ParseFilesParallel<T>(string[] filePaths, ICSVParserOptions options) where T : class, new();

      /// <summary>
      /// Add a new exclusion function to the parsing chain.
      /// <para/>
      /// Exclusion function:
      /// <list type="table">
      ///   <item>
      ///      <term>Input : <see cref="Array"/> of <see cref="string"/>s</term>
      ///      <description>List of properties from the current line.</description>
      ///   </item>
      ///   <item>
      ///      <term>Return : <see cref="bool"/></term>
      ///      <description><see langword="true"/> to exclude the current line from further parsing.</description>
      ///   </item>
      /// </list>
      /// </summary>
      /// <param name="id">Function ID</param>
      /// <param name="func">Exclusion function</param>
      void RegisterExclusionFunc(string id, Func<string[], bool> func);

      /// <summary>
      /// Remove a registered exclusion function.
      /// </summary>
      /// <param name="id">ID of the exclusion function.</param>
      void RemoveExclusionFunc(string id);
      /// <summary>
      /// Reset all options to defaults.
      /// </summary>
      void ResetOptions();
      /// <summary>
      /// Replace current options.
      /// </summary>
      /// <param name="options">New options.</param>
      void UpdateOptions(ICSVParserOptions options);
   }
}