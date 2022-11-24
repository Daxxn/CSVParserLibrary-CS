namespace CSVParserLibrary.Models;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class CSVProperty : Attribute
{
   readonly string _propertyString;

   public CSVProperty(string propertyString)
   {
      _propertyString = propertyString;
   }

   public bool CompareProperty(string prop, bool ignoreCase) =>
      ignoreCase ? prop.ToLower() == _propertyString.ToLower() : prop == _propertyString;

   public override string ToString() => $"CSVProperty {PropertyString}";

    public string PropertyString => _propertyString;
}
