namespace Prism.Logging
{
    public enum Filter
    {
        None = -1,
        All = 99,
        Emergency = 0,
        Alert = 1,
        Critical = 2,
        Error = 3,
        Warning = 4,
        Notice = 5,
        Information = 6,
        Debug = 7,
    }
}
