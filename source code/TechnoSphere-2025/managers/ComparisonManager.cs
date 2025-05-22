public static class ComparisonManager
{
    private static List<int> _comparisonList = new List<int>();

    public static void AddToComparison(int productId)
    {
        if (!_comparisonList.Contains(productId))
            _comparisonList.Add(productId);
    }

    public static void RemoveFromComparison(int productId)
    {
        _comparisonList.Remove(productId);
    }

    public static List<int> GetComparisonList()
    {
        return _comparisonList.ToList();
    }
}