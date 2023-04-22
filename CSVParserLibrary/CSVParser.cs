using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CSVParserLibrary.Models;

namespace CSVParserLibrary
{
   public class CSVParser : ICSVParser
   {
      #region Local Props
      private ICSVParserOptions _options;
      #endregion

      #region Constructors
      public CSVParser(ICSVParserOptions options) => _options = options;
      #endregion

      #region Methods

      #region Setup Methods
      public void UpdateOptions(ICSVParserOptions options) => _options = options;
      public void ResetOptions() => _options = new CSVParserOptions();
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
      public void RegisterExclusionFunc(string id, Func<string[], bool> func)
      {
         if (_options.ExclusionFunctions.ContainsKey(id))
         { _options.ExclusionFunctions[id] = func; }
         else
         {
            _options.ExclusionFunctions.Add(id, func);
         }
      }

      /// <summary>
      /// Remove a registered exclusion function.
      /// </summary>
      /// <param name="id">ID of the exclusion function.</param>
      public void RemoveExclusionFunc(string id) => _options.ExclusionFunctions.Remove(id);

      /// <summary>
      /// Remove all exclusion functions.
      /// </summary>
      public void ClearExclusionFunctions() => _options.ExclusionFunctions.Clear();
      #endregion

      #region Async Methods
      public IEnumerable<CSVParseResult<T>> ParseFilesParallel<T>(string[] filePaths, ICSVParserOptions options) where T : class, new()
      {
         _options = options;
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
      public IEnumerable<CSVParseResult<T>> ParseFilesParallel<T>(string[] filePaths) where T : class, new()
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
      public async Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath, ICSVParserOptions options) where T : class, new()
      {
         _options = options;
         return await Task.Run(() => ParseFile<T>(filePath));
      }

      /// <summary>
      /// Parse CSV file asyncronously.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="filePath">File to parse.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      public async Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath) where T : class, new() =>
        await Task.Run(() => ParseFile<T>(filePath));
      #endregion

      public async Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream) where T : class, new()
      {
         var reader = new StreamReader(stream);
         if (reader is null)
            throw new Exception("Unable to convert stream to a reader stream.");
         return await Task.Run(() => ParseFile<T>(reader));
      }

      public async Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream, ICSVParserOptions options) where T : class, new()
      {
         _options = options;
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
      public CSVParseResult<T> ParseFile<T>(Stream stream) where T : class, new()
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
      public CSVParseResult<T> ParseFile<T>(StreamReader stream, ICSVParserOptions options) where T : class, new()
      {
         _options = options;
         return ParseFile<T>(stream);
      }

      /// <summary>
      /// Open and parse the provided CSV file and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="filePath">file path to open.</param>
      /// <param name="options">Options to use instead of the defaults.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      public CSVParseResult<T> ParseFile<T>(string filePath, ICSVParserOptions options) where T : class, new()
      {
         _options = options;
         return ParseFile<T>(filePath);
      }

      /// <summary>
      /// Open and parse the provided CSV file and create a <see cref="CSVParseResult{T}"/>.
      /// </summary>
      /// <typeparam name="T">Model to create. Must be a class.</typeparam>
      /// <param name="filePath">file path to open.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and line errors.</returns>
      /// <exception cref="AggregateException"/>
      public CSVParseResult<T> ParseFile<T>(string filePath) where T : class, new()
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
      public CSVParseResult<T> ParseFile<T>(StreamReader reader) where T : class, new()
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
               if (_options.ExclusionFunctions.Values.Any(func => func(propData)))
               { continue; }
               if (_options.EndOfFileMarker != null)
               {
                  if (propData[0] == _options.EndOfFileMarker)
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
         return errors.Count > 0 && !_options.IgnoreLineParseErrors
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
      private string[] ParseLine(string propsLine)
      {
         if (string.IsNullOrEmpty(propsLine))
            throw new ArgumentNullException(nameof(propsLine));
         var split = propsLine.Split(_options.Delimiters, StringSplitOptions.None);

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
      private CSVPropertyCollection BuildProperties<T>(string firstLine) where T : class, new()
      {
         string[] propStrings = ParseLine(firstLine);
         var output = new List<CSVPropertyModel>();
         var modelProps = new T().GetType().GetProperties();
         for (int i = 0; i < propStrings.Length; i++)
         {
            var foundProp = modelProps.FirstOrDefault(p => p.GetCustomAttributes<CSVProperty>().FirstOrDefault(c => c.CompareProperty(propStrings[i], _options.IgnoreCase)) != null);
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
         else if (type.Name == "UInt32")
         {
            if (uint.TryParse(data.Replace(",", ""), out uint i))
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
}
