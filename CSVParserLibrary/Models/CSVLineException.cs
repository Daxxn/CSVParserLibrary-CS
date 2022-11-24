using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CSVParserLibrary.Models;

public class CSVLineException : Exception
{
   public int LineNumber { get; set; }
   public int PropertyCount { get; set; }
   public CSVLineException(int lineNumber, int propertyCount)
   {
      LineNumber = lineNumber;
      PropertyCount = propertyCount;
   }
   public CSVLineException(string? message) : base(message) { }
   public CSVLineException(string? message, int lineNumber, int propertyCount) : base(message)
   {
      LineNumber = lineNumber;
      PropertyCount = propertyCount;
   }
   public CSVLineException(string? message, int lineNumber, int propertyCount, Exception? innerException) : base(message, innerException)
   {
      LineNumber = lineNumber;
      PropertyCount = propertyCount;
   }

   public override string ToString() => $"LN: {LineNumber} Props:{PropertyCount} - {Message}";
}
