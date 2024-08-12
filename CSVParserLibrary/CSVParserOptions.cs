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
   /// List of characters that divide the string into the values for each line.
   /// <para/>
   /// default = <c>,</c>
   /// </summary>
   public char[] Delimiters { get; set; } = new[]
   {
      ',',
   };

   /// <summary>
   /// A list of characters that bypass the delimiters when parsing lines. These are needed when the data contains characters used as the <see cref="Delimiters"/>
   /// <para>These are usually double quotes <c>"</c></para>
   /// <para/>
   /// default = <c>"</c>
   /// </summary>
   public char[] IgnoreDelimiters { get; set; } = new[]
   {
      '"'
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
