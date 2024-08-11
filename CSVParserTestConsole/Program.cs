using CSVParserLibrary;

using CSVParserTestConsole.TestModels;

namespace CSVParserTestConsole
{
   internal class Program
   {
      public static string TestFilePath { get; } = @"F:\Electrical\PartInvoices\DigiKey\70297680.csv";
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

         var results = parser.ParseFile<DigiKeyPartModel>(TestFilePath, options);

         foreach (var result in results)
         {
            Console.WriteLine(result);
         }

         Console.ReadLine();
      }
   }
}
