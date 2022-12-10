namespace CSVParserLibrary
{
   /// <summary>
   /// Options to change how the <see cref="ICSVParser"/> handles parsing CSV files.
   /// </summary>
   public interface ICSVParserOptions
   {
      /// <summary>
      /// Delimiters used to parse props.
      /// <para/>
      /// default - <c>","</c>
      /// </summary>
      string[] Delimiters { get; set; }
      /// <summary>
      /// Ignore all letter casing while parsing props.
      /// </summary>
      bool IgnoreCase { get; set; }
      /// <summary>
      /// Ignores all errors while parsing file.
      /// <para/>
      /// True - Will continue to parse file and add errors to the output.
      /// <para/>
      /// False - Will throw an exception immediately when an error occurs.
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
      ///      <description><see langword="true"/> to exclude the current line from further parsing.</description>
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