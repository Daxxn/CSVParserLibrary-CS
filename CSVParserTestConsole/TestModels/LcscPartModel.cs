using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSVParserLibrary.Models;

namespace CSVParserTestConsole.TestModels
{
   public class LcscPartModel
   {
      #region Local Props
      [CSVProperty("Order Qty.")]
      public int Quantity { get; set; }

      [CSVProperty("LCSC Part Number")]
      public string? PartNumber { get; set; }

      [CSVProperty("Manufacture Part Number")]
      public string? MfrPartNumber { get; set; }

      [CSVProperty("Manufacturer")]
      public string? Manufacturer { get; set; }

      [CSVProperty("Description")]
      public string? Desc { get; set; }

      [CSVProperty("Unit Price($)")]
      public double UnitPrice { get; set; }
      #endregion

      #region Constructors
      public LcscPartModel() { }
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
