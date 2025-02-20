# CSVParserLibrary

A library that parses CSV (Comma separated values) file.

This library is meant to be flexable. Its not designed to be *ultra performant*. I designed it to handle the different kinds of csv files I need to work with and not complain.

## Usage

```cs
using CSVParserLibrary;

static void Main(string[] args)
{
	// Define custom options to use during parsing:
	CSVParserOptions options = new()
	{
		// Add functions that will be called on each line. If the function returns true, the line will be skipped.
		ExclusionFunctions =
		{
			{ "subtotal-exlusion", (string[] values) => values.Any(val => val.Contains("Subtotal"))}
		}
	};
	
	// Create the parser:
    CSVParser parser = new CSVParser(options);

	var results = parser.ParseFile<LineModel>(@"Path\To\The\File.csv");

	// Check for any errors:
	if (results.Errors != null)
	{
		// Handle errors...
	}

	// Use data:
	foreach(var data in results)
	{
		// Do what u want with the data...
	}
}
```