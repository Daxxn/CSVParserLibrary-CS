namespace CSVParserLibrary.Models;

/// <summary>
/// Assign a property to a column in the CSV file.
/// </summary>
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

   /// <summary>
   /// Property to match in the CSV file.
   /// </summary>
   public string PropertyString => _propertyString;
}
