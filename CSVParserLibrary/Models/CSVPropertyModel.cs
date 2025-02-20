using System.Reflection;

namespace CSVParserLibrary.Models;

internal class CSVPropertyModel
{
   public int Index { get; set; } = -1;
   public PropertyInfo Property { get; set; } = null!;
   public string CSVProperty { get; set; } = null!;
   public bool Ignore { get; set; } = false;

   public CSVPropertyModel(int index, PropertyInfo propInfo, string csvPropName, bool ignore = false)
   {
      Index = index;
      Property = propInfo;
      CSVProperty = csvPropName;
      Ignore = ignore;
   }

   public override string ToString() => $"CSV Prop {Index} - {Property.Name} - PropName: {CSVProperty} - Ignore: {Ignore}";
}
