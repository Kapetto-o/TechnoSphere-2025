namespace TechnoSphere_2025.models
{
    public class ComparisonChangedEventArgs : EventArgs
    {
        public int UserID { get; }
        public int ProductID { get; }
        public bool IsAdded { get; }
        public ComparisonChangedEventArgs(int u, int p, bool isAdded)
        {
            UserID = u;
            ProductID = p;
            IsAdded = isAdded;
        }
    }
}
