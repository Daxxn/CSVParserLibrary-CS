using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSVParserLibrary.Models;

namespace CSVParserTestConsole.TestModels;

public class CsvTestModel
{
   public string Name { get; set; } = "";

   public int? Number { get; set; }

   [CSVIgnore]
   public string Text { get; set; } = "";

   [CSVProperty("Desc")]
   public string Description { get; set; } = "";

   public override string ToString() => $"{Name} - Num: {Number} - TXT: {Text} - Desc: {Description}";
}
