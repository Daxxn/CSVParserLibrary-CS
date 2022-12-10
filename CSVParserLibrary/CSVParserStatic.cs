using System.Collections.Concurrent;
using System.Reflection;

using CSVParserLibrary.Models;

namespace CSVParserLibrary;

public static class CSVParserStatic
{
   #region Props
   private static ICSVParserOptions Options { get; set; } = new CSVParserOptions();
   #endregion

   #region Methods

   #region Change Options Methods
   /// <summary>
   /// Replace current options.
   /// </summary>
   /// <param name="options">New options.</param>
   public static void UpdateOptions(ICSVParserOptions options) => Options = options;

   /// <summary>
   /// Reset all options to defaults.
   /// </summary>
   public static void ResetOptions() => Options = new CSVParserOptions();
   #endregion

   #region Async Methods
   /// <summary>
   /// Add a new exclusion function to the parsing chain.
   /// <para/>
   /// Exclusion function:
   /// <list type="table">
   ///   <item>
   ///      <term>Input : <see cref="Array"/> of <see cref="string"/>s</term>
   ///      <description>List of properties from the current line.</description>
   ///   </item>
   ///   <item>
   ///      <term>Return : <see cref="bool"/></term>
   ///      <description><see langword="true"/> to exclude the current line from further parsing.</description>
   ///   </item>
   /// </list>
   /// </summary>
   /// <param name="id">Function ID</param>
   /// <param name="func">Exclusion function</param>
   public static void RegisterExclusionFunc(string id, Func<string[], bool> func)
   {
      if (Options.ExclusionFunctions.ContainsKey(id))
      { Options.ExclusionFunctions[id] = func; }
      else
      {
         Options.ExclusionFunctions.Add(id, func);
      }
   }

   /// <summary>
   /// Remove a registered exclusion function.
   /// </summary>
   /// <param name="id">ID of the exclusion function.</param>
   public static void RemoveExclusionFunc(string id) => Options.ExclusionFunctions.Remove(id);

   /// <summary>
   /// Remove all exclusion functions.
   /// </summary>
   public static void ClearExclusionFunctions() => Options.ExclusionFunctions.Clear();

   /// <summary>
   /// Parse multiple files in parallel.
   /// </summary>
   /// <typeparam name="T">Model to create from each line in the file.</typeparam>
   /// <param name="filePaths"><see cref="Array"/> of files to parse.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
   public static IEnumerable<CSVParseResult<T>> ParseFilesParallel<T>(string[] filePaths, ICSVParserOptions options) where T : class, new()
   {
      Options = options;
      var results = new ConcurrentBag<CSVParseResult<T>>();
      Parallel.ForEach(filePaths, (filePath, token) => results.Add(ParseFile<T>(filePath)));
      return results;
   }

   /// <summary>
   /// Parse multiple files in parallel.
   /// </summary>
   /// <typeparam name="T">Model to create from each line in the file.</typeparam>
   /// <param name="filePaths"><see cref="Array"/> of files to parse.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
   public static IEnumerable<CSVParseResult<T>> ParseFilesParallel<T>(string[] filePaths) where T : class, new()
   {
      var results = new ConcurrentBag<CSVParseResult<T>>();
      Parallel.ForEach(filePaths, (filePath, token) => results.Add(ParseFile<T>(filePath)));
      return results;
   }

   /// <summary>
   /// Parse CSV file asyncronously.
   /// </summary>
   /// <typeparam name="T">Model to create from each line in the file.</typeparam>
   /// <param name="filePath">File to parse.</param>
   /// <param name="options">Change the options used during parsing.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
   public static async Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath, ICSVParserOptions options) where T : class, new()
   {
      Options = options;
      return await Task.Run(() => ParseFile<T>(filePath));
   }

   /// <summary>
   /// Parse CSV file asyncronously.
   /// </summary>
   /// <typeparam name="T">Model to create from each line in the file.</typeparam>
   /// <param name="filePath">File to parse.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
   public static async Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath) where T : class, new() =>
     await Task.Run(() => ParseFile<T>(filePath));
   #endregion

   /// <summary>
   /// Parse the provided CSV stream and create a <see cref="CSVParseResult{T}"/>.
   /// </summary>
   /// <typeparam name="T">Model to create. Must be a class.</typeparam>
   /// <param name="stream">Stream to read from.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
   /// <exception cref="Exception"></exception>
   public static async Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream) where T : class, new()
   {
      var reader = new StreamReader(stream);
      if (reader is null)
         throw new Exception("Unable to convert stream to a reader stream.");
      return await Task.Run(() => ParseFile<T>(reader));
   }

   /// <summary>
   /// Parse the provided CSV stream and create a <see cref="CSVParseResult{T}"/>.
   /// </summary>
   /// <typeparam name="T">Model to create. Must be a class.</typeparam>
   /// <param name="stream">Stream to read from.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
   /// <exception cref="Exception"></exception>
   public static CSVParseResult<T> ParseFile<T>(Stream stream) where T : class, new()
   {
      var reader = new StreamReader(stream);
      if (reader is null)
         throw new Exception("Unable to convert stream to a reader stream.");
      return ParseFile<T>(reader);
   }

   /// <summary>
   /// Parse the provided CSV file stream and create a <see cref="CSVParseResult{T}"/>.
   /// </summary>
   /// <typeparam name="T">Model to create. Must be a class.</typeparam>
   /// <param name="stream">File stream to read from.</param>
   /// <param name="options">Options to use instead of the defaults.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
   /// <exception cref="Exception"/>
   /// <exception cref="CSVLineException"/>
   /// <exception cref="AggregateException"/>
   public static CSVParseResult<T> ParseFile<T>(StreamReader stream, ICSVParserOptions options) where T : class, new()
   {
      Options = options;
      return ParseFile<T>(stream);
   }

   /// <summary>
   /// Open and parse the provided CSV file and create a <see cref="CSVParseResult{T}"/>.
   /// </summary>
   /// <typeparam name="T">Model to create. Must be a class.</typeparam>
   /// <param name="filePath">file path to open.</param>
   /// <param name="options">Options to use instead of the defaults.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
   public static CSVParseResult<T> ParseFile<T>(string filePath, ICSVParserOptions options) where T : class, new()
   {
      Options = options;
      return ParseFile<T>(filePath);
   }

   /// <summary>
   /// Open and parse the provided CSV file and create a <see cref="CSVParseResult{T}"/>.
   /// </summary>
   /// <typeparam name="T">Model to create. Must be a class.</typeparam>
   /// <param name="filePath">file path to open.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
   /// <exception cref="AggregateException"/>
   public static CSVParseResult<T> ParseFile<T>(string filePath) where T : class, new()
   {
      using StreamReader reader = new(filePath);
      return ParseFile<T>(reader);
   }

   /// <summary>
   /// Parse the provided CSV file stream and create a <see cref="CSVParseResult{T}"/>.
   /// </summary>
   /// <typeparam name="T">Model to create. Must be a class.</typeparam>
   /// <param name="reader">File stream to read from.</param>
   /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
   /// <exception cref="Exception"/>
   /// <exception cref="CSVLineException"/>
   /// <exception cref="AggregateException"/>
   public static CSVParseResult<T> ParseFile<T>(StreamReader reader) where T : class, new()
   {
      var output = new List<T>();

      CSVPropertyCollection props = null!;
      var errors = new List<Exception>();
      int lineCount = 0;

      if (!reader.EndOfStream)
      {
         var line = reader.ReadLine() ?? "";
         lineCount++;
         props = BuildProperties<T>(line);
      }
      else
      {
         throw new Exception("File read error. No lines found in file.");
      }

      while (!reader.EndOfStream)
      {
         try
         {
            var line = reader.ReadLine() ?? "";
            lineCount++;
            var propData = ParseLine(line);
            if (Options.ExclusionFunctions.Values.Any(func => func(propData)))
            { continue; }
            if (Options.EndOfFileMarker != null)
            {
               if (propData[0] == Options.EndOfFileMarker)
               { break; }
            }
            if (propData.Length != props.TotalPropertyCount)
            {
               throw new CSVLineException("Length of properties is longer than the number of properties.", lineCount, propData.Length);
            }
            var newModel = new T();
            foreach (var prop in props)
            {
               prop.Property.SetValue(newModel, ParseType(propData[prop.Index], prop.Property.PropertyType));
            }
            output.Add(newModel);
         }
         catch (Exception e)
         {
            errors.Add(e);
         }
      }
      return errors.Count > 0 && !Options.IgnoreLineParseErrors
         ? throw new AggregateException("Some lines failed to parse.", errors)
         : (new(output, errors.Count > 0 ? errors : null));
   }

   #region Utility Methods
   /// <summary>
   /// Split the line and remove excess characters.
   /// </summary>
   /// <param name="propsLine">Line to parse.</param>
   /// <returns><see cref="Array"/> of cleaned <seealso cref="string"/></returns>
   /// <exception cref="ArgumentNullException"/>
   private static string[] ParseLine(string propsLine)
   {
      if (string.IsNullOrEmpty(propsLine))
         throw new ArgumentNullException(nameof(propsLine));
      var split = propsLine.Split(Options.Delimiters, StringSplitOptions.None);

      for (int i = 0; i < split.Length; i++)
      {
         split[i] = split[i].Trim().Trim('"');
      }

      return split;
   }

   /// <summary>
   /// Parse property line and match each property to the corresponding <see cref="CSVProperty"/> attribute.
   /// </summary>
   /// <typeparam name="T">Model to check for properties.</typeparam>
   /// <param name="firstLine">property line to parse.</param>
   /// <returns></returns>
   private static CSVPropertyCollection BuildProperties<T>(string firstLine) where T : class, new()
   {
      string[] propStrings = ParseLine(firstLine);
      var output = new List<CSVPropertyModel>();
      var modelProps = new T().GetType().GetProperties();
      for (int i = 0; i < propStrings.Length; i++)
      {
         var foundProp = modelProps.FirstOrDefault(p => p.GetCustomAttribute<CSVProperty>()?.CompareProperty(propStrings[i], Options.IgnoreCase) == true);
         if (foundProp != null)
         {
            output.Add(new(i, foundProp, propStrings[i]));
         }
      }
      return new(output, propStrings.Length);
   }

   /// <summary>
   /// Parse the data based on the property type.
   /// </summary>
   /// <param name="data">Raw string data</param>
   /// <param name="type">Property type to parse as.</param>
   /// <returns>The parsed value, or <see langword="null"/> if unable.</returns>
   private static object? ParseType(string data, Type type)
   {
      if (type.Name == "String")
      {
         return string.IsNullOrEmpty(data) ? null : data;
      }
      else if (type.Name == "Int32")
      {
         if (int.TryParse(data.Replace(",", ""), out int i))
         {
            return i;
         }
      }
      else if (type.Name == "Double")
      {
         if (double.TryParse(data, out double d))
         {
            return d;
         }
      }
      else if (type.Name == "Decimal")
      {
         if (decimal.TryParse(data, out decimal dm))
         {
            return dm;
         }
      }
      else if (type.IsClass)
      {
         if (type.GetConstructor(new[] { typeof(string) }) is ConstructorInfo ctr)
         {
            return ctr.Invoke(new[] { data });
         }
      }
      return null;
   }
   #endregion
   #endregion

   #region Full Props

   #endregion
}
