namespace CSVParserLibrary.Models;

/// <summary>
/// Assign a property to a column in the CSV file.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class CSVPropertyAttribute : Attribute
{
   public CSVPropertyAttribute(string propertyString)
   {
      PropertyString = propertyString;
   }

   public CSVPropertyAttribute(string propertyString, bool ingoreCase)
   {
      PropertyString = propertyString;
      IgnoreCase = ingoreCase;
   }

   public bool CompareProperty(string prop) =>
      IgnoreCase ? prop.ToLower() == PropertyString.ToLower() : prop == PropertyString;

   public override string ToString() => $"CSVProperty {PropertyString}{(IgnoreCase ? " ignore case" : "")}";

   /// <summary>
   /// Property to match in the CSV file.
   /// </summary>
   public string PropertyString { get; init; }

   /// <summary>
   /// Ingore letter casing when matching the name.
   /// </summary>
   public bool IgnoreCase { get; init; } = false;
}
