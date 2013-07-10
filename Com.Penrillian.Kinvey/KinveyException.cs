using System;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Thrown when an error occured using a service interface
    /// </summary>
    public class KinveyException : Exception
    {
        // ReSharper disable MemberCanBePrivate.Global
        /// <summary>
        /// The error received from the app server
        /// </summary>
        public KinveyError KinveyError {get; private set; }
        // ReSharper restore MemberCanBePrivate.Global

        internal KinveyException(KinveyError kinveyError) : base(kinveyError.Description)
        {
            KinveyError = kinveyError;
        }
    }
}
