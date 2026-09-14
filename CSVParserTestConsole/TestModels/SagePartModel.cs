using CSVParserLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParserTestConsole.TestModels
{
  internal enum UnitOfMeasure
  {
    EA,
    FT,
    IN,
    RL,
  }

  internal class SagePartModel
  {
    [CSVProperty("Product", true)]
    public string PartNumber { get; set; }

    [CSVProperty("Location", true)]
    public string Location { get; set; }

    [CSVProperty("Description 1", true)]
    public string Description { get; set; }

    [CSVProperty("STK quantity", true)]
    public double Quantity { get; set; }

    [CSVProperty("Unit", true)]
    public UnitOfMeasure UoM { get; set; }

    public override string ToString()
    {
      return $"{Location} - {PartNumber} - {Quantity} {UoM} {Description}";
    }
  }
}
