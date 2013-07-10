using System;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Factory class for getting instances of the various functional interfaces in this DLL.
    /// </summary>
    public static class KinveyFactory
    {
        /// <returns>an instance of the handshake interface</returns>
        public static IKinveyHandshake GetHandshake()
        {
            return new KinveyHandshake(Factory.Instance);
        }

        /// <summary>
        /// Gets a service interface for a particular type. This type <b>must be</b> annotated with the 
        /// [KinveyCollection] attribute and must extend KinveyObject.
        /// </summary>
        /// <typeparam name="T">the type, extending KinveyObject, for which to create a service</typeparam>
        /// <returns>a service interface for a type T</returns>
        /// <seealso cref="KinveyCollectionAttribute"/>
        /// <seealso cref="KinveyObject"/>
        /// <exception cref="ArgumentException">if the type T is not marked up as necessary</exception>
        public static IKinveyService<T> GetService<T>() where T : KinveyObject, new()
        {
            return new KinveyService<T>(Factory.Instance);
        }

        /// <returns>a service interface for the KinveyUser type.</returns>
        public static IKinveyUserService GetUserService()
        {
            return new KinveyUserService(Factory.Instance);
        }

        // ReSharper disable UnusedMember.Global
        // usage implicit
        /// <typeparam name="T">a type extending KinveyUser</typeparam>
        /// <returns>a service interface for a type extending the KinveyUser type.</returns>
        public static IKinveyUserService<T> GetUserService<T>() where T : KinveyUser
        {
            return new KinveyUserService<T>(Factory.Instance);
        }
        // ReSharper restore UnusedMember.Global
    }
}
