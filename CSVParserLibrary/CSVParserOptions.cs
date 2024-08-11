using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParserLibrary;

/// <summary>
/// Options to change how the <see cref="ICSVParser"/> handles parsing CSV files.
/// </summary>
public class CSVParserOptions : ICSVParserOptions
{
   /// <summary>
   /// Delimiters to use while separating each property.
   /// <para/>
   /// Default: { "," }
   /// </summary>
   public string[] Delimiters { get; set; } = new[]
   {
      "\",\""
   };

   /// <summary>
   /// If a line fails to parse, add it to the errors list and continue.
   /// </summary>
   public bool IgnoreLineParseErrors { get; set; } = false;

   /// <summary>
   /// Extra functions that check the incoming csv line for errors or unwanted data.
   /// </summary>
   public Dictionary<string, Func<string[], bool>> ExclusionFunctions { get; set; } = new();

   /// <summary>
   /// Used with CSV files that have other data stored at the end of the file.
   /// <para>For example: BOMs.</para>
   /// </summary>
   public string? EndOfFileMarker { get; set; }
}
