using System;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Can be thrown when expressing query constraints if descriptive expressions are not
    /// given in the correct format
    /// </summary>
    public class KinveyQueryException : Exception
    {
        public KinveyQueryException(string message) : base(message)
        { }
    }
}