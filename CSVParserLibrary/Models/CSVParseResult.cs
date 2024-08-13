using System.Collections;

namespace CSVParserLibrary.Models;

public class CSVParseResult<T> : IReadOnlyList<T>
{
   /// <summary>
   /// All data parsed from the file.
   /// </summary>
   public List<T> Values { get; init; } = new();

   /// <summary>
   /// Errors that occured during parsing.
   /// <para/>
   /// Only used when <see cref="ICSVParserOptions.IgnoreLineParseErrors"/> is set to <see langword="true"/>.
   /// </summary>
   public IEnumerable<Exception>? Errors { get; init; }
   public int Count => Values.Count;

   public T this[int index] { get => Values.ElementAt(index); }

   public CSVParseResult(IEnumerable<T> values, IEnumerable<Exception>? errors)
   {
      Values = values.ToList();
      Errors = errors;
   }

   public IEnumerator<T> GetEnumerator()
   {
      return Values.GetEnumerator();
   }

   IEnumerator IEnumerable.GetEnumerator()
   {
      return Values.GetEnumerator();
   }
}
