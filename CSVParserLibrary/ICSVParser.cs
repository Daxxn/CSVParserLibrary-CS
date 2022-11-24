using CSVParserLibrary.Models;

namespace CSVParserLibrary
{
   public interface ICSVParser
   {
      void ClearExclusionFunctions();
      CSVParseResult<T> ParseFile<T>(Stream stream) where T : class, new();
      CSVParseResult<T> ParseFile<T>(StreamReader reader) where T : class, new();
      CSVParseResult<T> ParseFile<T>(StreamReader stream, ICSVParserOptions options) where T : class, new();
      CSVParseResult<T> ParseFile<T>(string filePath) where T : class, new();
      CSVParseResult<T> ParseFile<T>(string filePath, ICSVParserOptions options) where T : class, new();
      Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream) where T : class, new();
      Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream, ICSVParserOptions options) where T : class, new();
      Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath) where T : class, new();
      Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath, ICSVParserOptions options) where T : class, new();
      IEnumerable<CSVParseResult<T>> ParseFilesParallel<T>(string[] filePaths, ICSVParserOptions options) where T : class, new();
      void RegisterExclusionFunc(string id, Func<string[], bool> func);
      void RemoveExclusionFunc(string id);
      void ResetOptions();
      void UpdateOptions(ICSVParserOptions options);
   }
}