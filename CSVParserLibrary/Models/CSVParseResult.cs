using System.Collections;

namespace CSVParserLibrary.Models;

/// <summary>
/// The results from a CSV file. Contains the results and per-line errors.
/// </summary>
/// <typeparam name="T">The model to use when parsing the CSV file.</typeparam>
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

   /// <inheritdoc/>
   public int Count => Values.Count;

   /// <inheritdoc/>
   public T this[int index] { get => Values.ElementAt(index); }

   /// <inheritdoc/>
   public CSVParseResult(IEnumerable<T> values, IEnumerable<Exception>? errors)
   {
      Values = values.ToList();
      Errors = errors;
   }

   /// <inheritdoc/>
   public IEnumerator<T> GetEnumerator()
   {
      return Values.GetEnumerator();
   }

   IEnumerator IEnumerable.GetEnumerator()
   {
      return Values.GetEnumerator();
   }
}
