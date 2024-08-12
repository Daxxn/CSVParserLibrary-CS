namespace CSVParserLibrary
{
   /// <summary>
   /// Options to change how the <see cref="ICSVParser"/> handles parsing CSV files.
   /// </summary>
   public interface ICSVParserOptions
   {
      /// <summary>
      /// List of characters that divide the string into the values for each line.
      /// <para/>
      /// default = <c>,</c>
      /// </summary>
      char[] Delimiters { get; set; }

      /// <summary>
      /// A list of characters that bypass the delimiters when parsing lines. These are needed when the data contains characters used as the <see cref="Delimiters"/>
      /// <para>These are usually double quotes <c>"</c></para>
      /// <para/>
      /// default = <c>"</c>
      /// </summary>
      char[] IgnoreDelimiters { get; set; }

      /// <summary>
      /// Ignores all errors while parsing file.
      /// <para/>
      /// True = Will continue to parse file and add all errors will be in the <see cref="Models.CSVParseResult{T}"/>.
      /// <para/>
      /// False = Will throw an exception immediately when an error occurs.
      /// </summary>
      bool IgnoreLineParseErrors { get; set; }

      /// <summary>
      /// Extra functions while parsing lines.
      /// <para/>
      /// Return true to exclude the line.
      /// <list type="table">
      ///   <item>
      ///      <term>Input : <see cref="Array"/> of <see cref="string"/>s</term>
      ///      <description>List of properties from the current line.</description>
      ///   </item>
      ///   <item>
      ///      <term>Return : <see cref="bool"/></term>
      ///      <description>Return <see langword="true"/> to exclude this line.</description>
      ///   </item>
      /// </list>
      /// </summary>
      Dictionary<string, Func<string[], bool>> ExclusionFunctions { get; set; }

      /// <summary>
      /// Custom end of file marker string.
      /// </summary>
      public string? EndOfFileMarker { get; set; }
   }
}