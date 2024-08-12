using CSVParserLibrary;

using CSVParserTestConsole.TestModels;

namespace CSVParserTestConsole
{
   internal class Program
   {
      public static string TestFilePath1 { get; } = @"F:\Electrical\PartInvoices\DigiKey\70297680.csv";
      public static string TestFilePath2 { get; } = @"C:\Users\Daxxn\Downloads\LCSC_Exported__20240812_075612.csv";
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

         var results1 = parser.ParseFile<DigiKeyPartModel>(TestFilePath1, options);

         foreach (var result in results1)
         {
            Console.WriteLine(result);
         }

         var results2 = parser.ParseFile<LcscPartModel>(TestFilePath2, options);

         foreach (var result in results2)
         {
            Console.WriteLine(result);
         }

         Console.ReadLine();
      }
   }
}
