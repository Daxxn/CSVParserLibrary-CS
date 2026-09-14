using CSVParserLibrary;

using CSVParserTestConsole.TestModels;

namespace CSVParserTestConsole
{
  internal class Program
  {
    public static int testNumber = 5;
    public static string TestFilePath1 { get; } = @"F:\Electrical\PartInvoices\DigiKey\70297680.csv";
    public static string TestFilePath2 { get; } = @"C:\Users\Daxxn\Downloads\LCSC_Exported__20240812_075612.csv";
    public static string TestFilePath3 { get; } = @"F:\Electrical\Designs\Projects\LightDrum-V2\Docs\BOMs\REV3\LightDrum-V2_Top_REV3.csv";
    public static string TestFilePath4 { get; } = @"F:\Electrical\Designs\Projects\LightDrum-V2\Docs\BOMs\REV3\CsvTest.csv";
    public static string TestFilePath5 { get; } = @"F:\Electrical\PartInvoices\DigiKey\96892037.csv";
    public static string TestFilePath6 { get; } = @"C:\Users\Daxxn\Documents\WorkStuff\Cycle Counting\Locations\B10_Locations_2-10-26.csv";
    static void Main(string[] args)
    {
      Console.WriteLine("CSV Parser Library Testing");

      CSVParserOptions options = new()
      {
        ExclusionFunctions =
            {
               { "subtotal-exlusion", (string[] values) => values.Any(val => val.Contains("Subtotal"))}
            }
      };

      CSVParser parser = new CSVParser(options);

      switch (testNumber)
      {
        case 0:
          var results1 = parser.ParseFile<DigiKeyPartModel>(TestFilePath1, options);
          break;
        case 1:
          var results2 = parser.ParseFile<LcscPartModel>(TestFilePath2, options);

          foreach (var result in results2)
          {
            Console.WriteLine(result);
          }
          break;
        case 2:
          var results3 = parser.ParseFile<BomPartModel>(TestFilePath3, options);

          foreach (var result in results3)
          {
            Console.WriteLine(result);
          }
          break;
        case 3:
          var results4 = parser.ParseFile<CsvTestModel>(TestFilePath4);

          foreach (var result in results4)
          {
            Console.WriteLine(result);
          }
          break;
        case 4:
          var results5 = parser.ParseFile<DigiKeyPartModel>(TestFilePath5, options);

          foreach (var result in results5)
          {
            Console.WriteLine(result);
          }
          break;
        case 5:
          var sageParseOptions = new CSVParserOptions()
          {
            Delimiters = [';'],
            IgnoreDelimiters = ['"'],
            DiscardCharacters = ['='],
          };
          var result6 = parser.ParseFile<SagePartModel>(TestFilePath6, sageParseOptions);

          foreach (var sagePart in result6)
          {
            Console.WriteLine(sagePart);
          }
          break;
        default:
          break;
      }

      Console.ReadLine();
    }
  }
}
