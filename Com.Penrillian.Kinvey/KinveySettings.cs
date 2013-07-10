using System;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// <para>Singleton class containing global, app-wide settings.</para>
    /// <para>Use this class to set the AppKey and the AppAuthToken before attempting to get
    /// service interfaces from the Factory class.</para>
    /// <para>Attempting to generate service instances before setting these fields will
    /// result in exceptions.</para>
    /// </summary>
    public sealed class KinveySettings
    {
        private static KinveySettings _kinveySettings;

        private static string StaticUserAuthToken { get; set; }
        private static string StaticAppAuthToken { get; set; }

        private static string StaticAppKey { get; set; }

        private static KinveyUser StaticCurrentUser { get; set; }

        internal static void Reset()
        {
            StaticUserAuthToken = null;
            StaticAppAuthToken = null;
            StaticAppKey = null;
        }

        /// <summary>
        /// Gets the singleton instance of the KinveySettings class
        /// </summary>
        /// <returns></returns>
        public static KinveySettings Get()
        {
            return _kinveySettings ?? (_kinveySettings = new KinveySettings());
        }

        /// <summary>
        /// The authorization token associated with the current user, or null if there is no
        /// active user session.
        /// </summary>
        public string UserAuthToken
        {
            get { return StaticUserAuthToken; }
            internal set
            {
                StaticUserAuthToken = value;
            }
        }

        /// <summary>
        /// The basic authorization token associated with this app's key. This can only be set
        /// once, and will generate exceptions if attempts to set more than once are made. This
        /// must be set before attempting to get service interface instances from the KinveyFactory.
        /// </summary>
        public string AppAuthToken
        {
            get
            {
                return StaticAppAuthToken;
            }
            set
            {
                if(null != StaticAppAuthToken) 
                    throw new Exception("AppAuthToken already set");
                StaticAppAuthToken = value;
            }
        }

        /// <summary>
        /// The key associated with this app. This can only be set once, and will generate exceptions 
        /// if attempts to set more than once are made. This must be set before attempting to get 
        /// service interface instances from the KinveyFactory.
        /// </summary>
        public string AppKey
        {
            get
            { 
                return StaticAppKey;
            }
            set
            {
                if (null != StaticAppKey)
                    throw new Exception("AppAuthToken already set");
                StaticAppKey = value;
            }
        }

        /// <summary>
        /// The user associated with the current user session, or null if there is no active user session.
        /// </summary>
        public KinveyUser CurrentUser
        {
            get { return StaticCurrentUser; }
            internal set
            {
                StaticCurrentUser = value;
            }
        }
    }
}