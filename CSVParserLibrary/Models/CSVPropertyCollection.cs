using System.Collections;

namespace CSVParserLibrary.Models;

public class CSVPropertyCollection : IEnumerable<CSVPropertyModel>
{
   #region Local Props
   private readonly List<CSVPropertyModel> _items = new();
   private readonly int _highestIndex;
   private readonly int _totalPropCount;
   public CSVPropertyModel this[int index] { get => _items[index]; set => _items[index] = value; }
   public int Count => _items.Count;
   public bool IsReadOnly { get; } = false;
   public int HighestPropertyIndex => _highestIndex;
   public int TotalPropertyCount => _totalPropCount;
   #endregion

   #region Constructors
   public CSVPropertyCollection(IEnumerable<CSVPropertyModel> properties, int totalPropCount)
   {
      _items = properties.ToList();
      _highestIndex = _items.Max(i => i.Index);
      _totalPropCount = totalPropCount;
   }
   #endregion

   #region Methods
   public bool Contains(CSVPropertyModel item) => throw new NotImplementedException();
   public int IndexOf(CSVPropertyModel item) => throw new NotImplementedException();
   public IEnumerator<CSVPropertyModel> GetEnumerator() => _items.GetEnumerator();
   IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   #endregion

   #region Full Props

   #endregion
}
