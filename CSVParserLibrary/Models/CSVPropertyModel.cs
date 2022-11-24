using System.Reflection;

namespace CSVParserLibrary.Models
{
   public class CSVPropertyModel
   {
      public int Index { get; set; }
      public PropertyInfo Property { get; set; } = null!;
      public string CSVProperty { get; set; } = null!;

      public CSVPropertyModel(int index, PropertyInfo propInfo, string csvPropName)
      {
         Index = index;
         Property = propInfo;
         CSVProperty = csvPropName;
      }
   }
}
