namespace Com.Penrillian.Kinvey
{
    internal interface IFactory
    {
        T Get<T>(params object[] args);
    }
}