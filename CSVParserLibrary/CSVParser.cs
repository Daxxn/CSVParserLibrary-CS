﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CSVParserLibrary.Models;

namespace CSVParserLibrary
{
   /// <summary>
   /// The object used to parse CSV file.
   /// </summary>
   public class CSVParser : ICSVParser
   {
      #region Local Props
      private ICSVParserOptions _options;
      #endregion

      #region Constructors
      /// <summary>
      /// Create a CSV file parser object with the default options.
      /// </summary>
      public CSVParser() { _options = new CSVParserOptions(); }
      /// <summary>
      /// Create a CSV file parser object with custom parser options.
      /// </summary>
      /// <param name="options">The custom option to use when parsing CSV files.</param>
      public CSVParser(ICSVParserOptions options) => _options = options;
      #endregion

      #region Methods

      #region Setup Methods
      /// <summary>
      /// Change the current parser option.
      /// </summary>
      /// <param name="options">The new parser option to use.</param>
      public void UpdateOptions(ICSVParserOptions options) => _options = options;
      /// <summary>
      /// Reset the parser options.
      /// </summary>
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
      /// <summary>
      /// Parse multiple CSV files in parallel.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="filePaths">The <see cref="Array"/> of CSV files to parse.</param>
      /// <param name="options">The options to use when parsing each file.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      public IEnumerable<CSVParseResult<T>> ParseFilesParallel<T>(string[] filePaths, ICSVParserOptions options) where T : class, new()
      {
         _options = options;
         var results = new ConcurrentBag<CSVParseResult<T>>();
         Parallel.ForEach(filePaths, (filePath, token) => results.Add(ParseFile<T>(filePath)));
         return results;
      }

      /// <summary>
      /// Parse multiple CSV files in parallel.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="filePaths">The <see cref="Array"/> of CSV files to parse.</param>
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
      /// Parse a CSV file asyncronously.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="filePath">File to parse.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      public async Task<CSVParseResult<T>> ParseFileAsync<T>(string filePath) where T : class, new() =>
        await Task.Run(() => ParseFile<T>(filePath));
      #endregion

      /// <summary>
      /// Parse a CSV file asyncronously.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="stream">The <see cref="Stream"/> from the file system.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      /// <exception cref="Exception">Thrown when a stream cannot be converted to a <see cref="StreamReader"/>.</exception>
      public async Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream) where T : class, new()
      {
         var reader = new StreamReader(stream);
         return reader is null
            ? throw new Exception("Unable to convert stream to a reader stream.")
            : await Task.Run(() => ParseFile<T>(reader));
      }

      /// <summary>
      /// Parse a CSV file asyncronously.
      /// </summary>
      /// <typeparam name="T">Model to create from each line in the file.</typeparam>
      /// <param name="stream">The <see cref="Stream"/> from the file system.</param>
      /// <param name="options">Change the options used during parsing.</param>
      /// <returns><see cref="CSVParseResult{T}"/> with all data and errors from parsing.</returns>
      /// <exception cref="Exception">Thrown when a stream cannot be converted to a <see cref="StreamReader"/>.</exception>
      public async Task<CSVParseResult<T>> ParseFileAsync<T>(Stream stream, ICSVParserOptions options) where T : class, new()
      {
         _options = options;
         var reader = new StreamReader(stream);
         return reader is null
            ? throw new Exception("Unable to convert stream to a reader stream.")
            : await Task.Run(() => ParseFile<T>(reader));
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
         return reader is null
            ? throw new Exception("Unable to convert stream to a reader stream.")
            : ParseFile<T>(reader);
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
               //var propData = ParseLine(line);
               var propData = ParseLineNew(line);
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
      private string[] ParseLineNew(string line)
      {
         List<string> output = new();
         StringBuilder sb = new();
         bool ignoreDelimiter = false;
         char prevChar = '\0';
         foreach (var ch in line)
         {
            if (_options.IgnoreDelimiters.Contains(ch))
            {
               if (prevChar != _options.QuoteDelimiter)
               {
                  ignoreDelimiter = !ignoreDelimiter;
               }
               else
               {
                  sb.Append(ch);
               }
               if (!ignoreDelimiter)
               {
                  if (sb.Length == 0)
                  {
                     output.Add("");
                  }
               }
               continue;
            }
            else if (!ignoreDelimiter && _options.Delimiters.Contains(ch))
            {
               if (sb.Length > 0)
               {
                  output.Add(sb.ToString());
                  sb.Clear();
               }
            }
            else
            {
               sb.Append(ch);
            }
            prevChar = ch;
         }
         if (sb.Length > 0)
         {
            output.Add(sb.ToString());
         }

         return output.ToArray();
      }

      /// <summary>
      /// Parse property line and match each property to the corresponding property name OR <see cref="CSVPropertyAttribute"/> attribute.
      /// <para/>
      /// <b>Property Name</b> takes priority.
      /// </summary>
      /// <typeparam name="T">Model to check for properties.</typeparam>
      /// <param name="firstLine">property line to parse.</param>
      /// <returns></returns>
      private CSVPropertyCollection BuildProperties<T>(string firstLine) where T : class, new()
      {
         //string[] propStrings = ParseLine(firstLine);
         string[] propStrings = ParseLineNew(firstLine);
         var output = new List<CSVPropertyModel>();
         var modelProps = new T().GetType().GetProperties();
         for (int i = 0; i < propStrings.Length; i++)
         {
            var foundProp = modelProps.FirstOrDefault(
               p => p.Name == propStrings[i]
                  || p.GetCustomAttributes<CSVPropertyAttribute>().FirstOrDefault(
                     c => c.CompareProperty(propStrings[i]))
                        != null);

            if (foundProp != null && foundProp.GetCustomAttribute<CSVIgnoreAttribute>() is null)
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
         else if (type.Name == "Nullable`1")
         {
            if (type.GenericTypeArguments.Length == 1)
            {
               return string.IsNullOrEmpty(data) ? null : ParseType(data, type.GenericTypeArguments[0]);
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
