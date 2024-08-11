using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSVParserLibrary.Models;

namespace CSVParserTestConsole.TestModels
{
   public class DigiKeyPartModel
   {
      #region Local Props
      [CSVProperty("QUANTITY", true)]
      public int Quantity { get; set; }

      [CSVProperty("PART NUMBER", true)]
      public string? PartNumber { get; set; }

      [CSVProperty("MANUFACTURER PART NUMBER", true)]
      public string? MfrPartNumber { get; set; }

      [CSVProperty("DESCRIPTION", true)]
      public string? Desc { get; set; }

      [CSVProperty("UNIT PRICE", true)]
      public double UnitPrice { get; set; }
      #endregion

      #region Constructors
      public DigiKeyPartModel() { }
      #endregion

      #region Methods
      public override string ToString()
      {
         return $"QTY: {Quantity} PN: {PartNumber} MFR-PN: {MfrPartNumber} DESC: {Desc} UNIT-PRICE: {UnitPrice}";
      }
      #endregion

      #region Full Props

      #endregion
   }
}
