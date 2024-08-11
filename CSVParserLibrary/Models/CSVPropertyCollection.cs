using System.Collections;

namespace CSVParserLibrary.Models;

internal class CSVPropertyCollection : IEnumerable<CSVPropertyModel>
{
   #region Local Props
   private readonly List<CSVPropertyModel> _items = new();

   public CSVPropertyModel this[int index] { get => _items[index]; set => _items[index] = value; }
   public int Count => _items.Count;
   public bool IsReadOnly { get; } = false;
   public int HighestPropertyIndex { get; init; }
   public int TotalPropertyCount { get; init; }
   #endregion

   #region Constructors
   public CSVPropertyCollection(IEnumerable<CSVPropertyModel> properties, int totalPropCount)
   {
      _items = properties.ToList();
      HighestPropertyIndex = _items.Max(i => i.Index);
      TotalPropertyCount = totalPropCount;
   }
   #endregion

   #region Methods
   public bool Contains(CSVPropertyModel item) => _items.Contains(item);
   public int IndexOf(CSVPropertyModel item) => _items.IndexOf(item);
   public IEnumerator<CSVPropertyModel> GetEnumerator() => _items.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   #endregion

   #region Full Props

   #endregion
}
