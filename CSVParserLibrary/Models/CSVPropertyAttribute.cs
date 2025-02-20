namespace CSVParserLibrary.Models;

/// <summary>
/// Assign a property to a column in the CSV file.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class CSVPropertyAttribute : Attribute
{
   /// <summary>
   /// Assign a property to a column in the CSV file that matches the property string.
   /// </summary>
   /// <param name="propertyString">The name of the property in the CSV file.</param>
   public CSVPropertyAttribute(string propertyString)
   {
      PropertyString = propertyString;
   }

   /// <summary>
   /// Assign a property to a column in the CSV file that matches the property string.
   /// <para/>
   /// Also, set if the property name is case sensitive.
   /// </summary>
   /// <param name="propertyString">The name of the property in the CSV file.</param>
   /// <param name="ingoreCase">Set to true to ignore any case differences.</param>
   public CSVPropertyAttribute(string propertyString, bool ingoreCase)
   {
      PropertyString = propertyString;
      IgnoreCase = ingoreCase;
   }

   /// <summary>
   /// Compare the CSV file column name to the property string.
   /// </summary>
   /// <param name="prop">The column name from the CSV file.</param>
   /// <returns>True if the column name matches the property string.</returns>
   public bool CompareProperty(string prop) =>
      IgnoreCase ? prop.ToLower() == PropertyString.ToLower() : prop == PropertyString;

   /// <inheritdoc/>
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
