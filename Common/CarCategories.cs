namespace Common
{
    public static class CarCategories
    {
        //Not an enum due to readability in db
        public const string Compact = "Compact";
        public const string Premium = "Premium";
        public const string Minivan = "Minivan";

        public static bool IsOfThisType(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return false;
            return category == Compact || category == Premium || category == Minivan;
        }
    }
}
