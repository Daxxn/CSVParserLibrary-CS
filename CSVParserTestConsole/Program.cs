using CSVParserLibrary;

using CSVParserTestConsole.TestModels;

namespace CSVParserTestConsole
{
   internal class Program
   {
      public static int testNumber = 3;
      public static string TestFilePath1 { get; } = @"F:\Electrical\PartInvoices\DigiKey\70297680.csv";
      public static string TestFilePath2 { get; } = @"C:\Users\Daxxn\Downloads\LCSC_Exported__20240812_075612.csv";
      public static string TestFilePath3 { get; } = @"F:\Electrical\Designs\Projects\LightDrum-V2\Docs\BOMs\REV3\LightDrum-V2_Top_REV3.csv";
      public static string TestFilePath4 { get; } = @"F:\Electrical\Designs\Projects\LightDrum-V2\Docs\BOMs\REV3\CsvTest.csv";
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

               foreach (var result in results1)
               {
                  Console.WriteLine(result);
               }
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
            default:
               break;
         }

         Console.ReadLine();
      }
   }
}
