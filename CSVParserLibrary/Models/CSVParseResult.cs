namespace CSVParserLibrary.Models;

public class CSVParseResult<T>
{
   public IEnumerable<T> Values { get; init; } = null!;
   public IEnumerable<Exception>? Errors { get; init; }

   public CSVParseResult(IEnumerable<T> values, IEnumerable<Exception>? errors)
   {
      Values = values;
      Errors = errors;
   }
}
