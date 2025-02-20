using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParserLibrary.Models;

/// <summary>
/// Set a property to be ignored by the <see cref="CSVParser"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class CSVIgnoreAttribute : Attribute { }
