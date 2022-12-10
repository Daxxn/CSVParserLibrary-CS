namespace CSVParserLibrary.Models;

public class CSVParseResult<T>
{
   /// <summary>
   /// All data parsed from the file.
   /// </summary>
   public IEnumerable<T> Values { get; init; } = null!;
   /// <summary>
   /// Errors that occured during parsing.
   /// <para/>
   /// Only used when <see cref="ICSVParserOptions.IgnoreLineParseErrors"/> is set to <see langword="true"/>.
   /// </summary>
   public IEnumerable<Exception>? Errors { get; init; }

   public CSVParseResult(IEnumerable<T> values, IEnumerable<Exception>? errors)
   {
      Values = values;
      Errors = errors;
   }
}
