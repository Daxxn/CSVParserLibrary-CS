using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CSVParserLibrary.Models;

/// <summary>
/// An exception thrown when parsing a CSV line.
/// </summary>
public class CSVLineException : Exception
{
   /// <summary>
   /// The line number where the exception was thrown.
   /// </summary>
   public int LineNumber { get; set; }
   /// <summary>
   /// The number of items parsed from the line.
   /// </summary>
   public int PropertyCount { get; set; }
   /// <summary>
   /// An exception thrown when parsing a CSV line.
   /// </summary>
   public CSVLineException(int lineNumber, int propertyCount)
   {
      LineNumber = lineNumber;
      PropertyCount = propertyCount;
   }
   /// <summary>
   /// An exception thrown when parsing a CSV line.
   /// </summary>
   public CSVLineException(string? message) : base(message) { }
   /// <summary>
   /// An exception thrown when parsing a CSV line.
   /// </summary>
   public CSVLineException(string? message, int lineNumber, int propertyCount) : base(message)
   {
      LineNumber = lineNumber;
      PropertyCount = propertyCount;
   }
   /// <summary>
   /// An exception thrown when parsing a CSV line.
   /// </summary>
   public CSVLineException(string? message, int lineNumber, int propertyCount, Exception? innerException) : base(message, innerException)
   {
      LineNumber = lineNumber;
      PropertyCount = propertyCount;
   }

   /// <inheritdoc/>
   public override string ToString() => $"LN: {LineNumber} Props:{PropertyCount} - {Message}";
}
