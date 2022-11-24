namespace CSVParserLibrary
{
   public interface ICSVParserOptions
   {
      string[] Delimiters { get; set; }
      bool IgnoreCase { get; set; }
      bool IgnoreLineParseErrors { get; set; }
      Dictionary<string, Func<string[], bool>> ExclusionFunctions { get; set; }
      public string? EndOfFileMarker { get; set; }
   }
}