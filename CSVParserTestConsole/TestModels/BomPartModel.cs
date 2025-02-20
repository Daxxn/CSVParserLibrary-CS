using CSVParserLibrary.Models;

namespace CSVParserTestConsole.TestModels;

public class BomPartModel
{
   #region Local Props
   private string _ref = "";
   private string _partNumber = "";
   private string _value = "";
   private string _datasheet = "";
   private string _footprint = "";
   private string? _dnp = "";
   private string _color = "";

   private int? _part = null;
   #endregion

   #region Constructors
   public BomPartModel() { }
   #endregion

   #region Methods
   public override string ToString() => $"BOM Model - {ReferenceDesignator} - {PartNumber} - {Value}";
   #endregion

   #region Full Props
   [CSVProperty("Reference")]
   public string ReferenceDesignator
   {
      get => _ref;
      set
      {
         _ref = value;
      }
   }

   [CSVProperty("PartNumber")]
   public string PartNumber
   {
      get => _partNumber;
      set
      {
         _partNumber = value;
      }
   }

   [CSVProperty("Value")]
   public string Value
   {
      get => _value;
      set
      {
         _value = value;
      }
   }

   [CSVProperty("Datasheet")]
   public string Datasheet
   {
      get => _datasheet;
      set
      {
         _datasheet = value;
      }
   }

   [CSVProperty("Footprint")]
   public string Footprint
   {
      get => _footprint;
      set
      {
         _footprint = value;
      }
   }

   public string? DNP
   {
      get => _dnp;
      set
      {
         _dnp = value;
      }
   }

   public string Color
   {
      get => _color;
      set
      {
         _color = value;
      }
   }

   [CSVIgnore]
   public int? Part
   {
      get => _part;
      set
      {
         _part = value;
      }
   }
   #endregion
}
