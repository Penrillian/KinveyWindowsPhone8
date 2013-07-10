using System.Threading.Tasks;
using Com.Penrillian.Kinvey.OAuth;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// <para>The IKinveyUserService interface provides access to CRUD and authentication functionality for 
    /// objects of type KinveyUser.</para>
    /// <para>An IKinveyUserService instance can operate in two modes, User Authentication mode and
    /// Basic Authentication mode. By default the instance is working in Basic Authentication mode, which 
    /// requires an AppAuthToken to be specified in the KinveySettings singleton. After a successful LogIn 
    /// or SignUp then the instance will automatically switch to User Auth mode, and the current 
    /// UserAuthToken will be used to Authenticate requests. After a Logout request the instance will return 
    /// to working in Basic Auth mode.</para>
    /// </summary>
    public interface IKinveyUserService : IKinveyUserService<KinveyUser>
    {
    }

    /// <summary>
    /// <para>The IKinveyUserService interface provides access to CRUD and authentication functionality for 
    /// objects which extend KinveyUser.</para>
    /// <para>An IKinveyUserService instance can operate in two modes, User Authentication mode and
    /// Basic Authentication mode. By default the instance is working in Basic Authentication mode, which 
    /// requires an AppAuthToken to be specified in the KinveySettings singleton. After a successful LogIn 
    /// or SignUp then the instance will automatically switch to User Auth mode, and the current 
    /// UserAuthToken will be used to Authenticate requests. After a Logout request the instance will return 
    /// to working in Basic Auth mode.</para>
    /// </summary>
    /// <typeparam name="T">The type of object this service is associated with</typeparam>
    /// <seealso cref="KinveyUser"/>
    /// <seealso cref="KinveySettings.AppAuthToken"/>
    /// <seealso cref="KinveySettings.CurrentUser"/>
    public interface IKinveyUserService<T> where T : KinveyUser
    {
        /// <summary>
        /// <para>Makes a POST request to log in with the provided username and password.</para>
        /// <para><b>If successful</b> the user authentication token will be cached in KinveySettings and the IKinveyUserService instance will switch to User Authentication mode.</para>
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>The current KinveyUser after a successful log in</returns>
        /// <exception cref="KinveyException">Can be thrown if the AppAuthToken specified in KinveySettings is not
        /// valid, or has not been set.</exception>
        /// <seealso cref="KinveyUser"/>
        /// <seealso cref="KinveySettings.AppAuthToken"/>
        /// <seealso cref="KinveySettings.CurrentUser"/>
        Task<KinveyUser> LogIn(string username, string password);

        /// <summary>
        /// <para>Makes a POST request to sign up with the provided username and password.</para>
        /// <para><b>If successful</b> the user authentication token will be cached in KinveySettings and the IKinveyUserService instance will switch to User Authentication mode.</para>
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>The current KinveyUser after a successful log in</returns>
        /// <exception cref="KinveyException">Can be thrown if the AppAuthToken specified in KinveySettings is not
        /// valid, or has not been set, or if the supplied credentials are already in use.</exception>
        /// <seealso cref="KinveyUser"/>
        /// <seealso cref="KinveySettings.AppAuthToken"/>
        /// <seealso cref="KinveySettings.CurrentUser"/>
        Task<KinveyUser> SignUp(string username, string password);

        // TODO documentation
        Task<KinveyFacebookUser> SignUp(FacebookIdentityToken socialIdentity);

        // TODO documentation
        Task<KinveyTwitterUser> SignUp(TwitterIdentityToken socialIdentity);

        // TODO documentation
        Task<KinveyGooglePlusUser> SignUp(GooglePlusIdentityToken socialIdentity);

        // TODO documentation
        Task<KinveyLinkedInUser> SignUp(LinkedInIdentityToken socialIdentity);

        /// <summary>
        /// <para>Makes a POST request to log out the current session.</para>
        /// </summary>
        Task LogOut();

        /// <summary>
        /// Makes a GET request to read a record.
        /// </summary>
        /// <param name="id">The ID of the record to retrieve</param>
        /// <returns>The retrieved record</returns>
        Task<T> Read(string id);

        /// <summary>
        /// Makes a POST request to update an existing record.
        /// </summary>
        /// <param name="user">The record to update</param>
        /// <returns>The updated record</returns>
        Task<T> Update(T user);

        /// <summary>
        /// Makes a DELETE request to soft delete an existing record. This delete can be
        /// reversed using Restore
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        /// <seealso cref="Restore"/>
        Task SoftDelete(string id);

        /// <summary>
        /// Makes a DELETE request to soft delete an existing record. This delete can be
        /// reversed using Restore
        /// </summary>
        /// <param name="user">The user to delete</param>
        /// <seealso cref="Restore"/>
        Task SoftDelete(T user);

        /// <summary>
        /// Makes a DELETE request to soft delete an existing record. This cannot be undone
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        Task HardDelete(string id);

        /// <summary>
        /// Makes a DELETE request to soft delete an existing record. This cannot be undone
        /// </summary>
        /// <param name="user">The user to delete</param>
        Task HardDelete(T user);

        /// <summary>
        /// Makes a POST request to restore a soft-deleted user
        /// </summary>
        /// <param name="id">The id of the user to restore</param>
        /// <returns></returns>
        Task Restore(string id);

        /// <summary>
        /// Makes a POST request to verify an email
        /// </summary>
        /// <param name="username">The username of the user to verify</param>
        Task VerifyEmail(string username);

        /// <summary>
        /// Makes a POST request to reset a password
        /// </summary>
        /// <param name="email">The email of the user to reset</param>
        Task PasswordResetEmail(string email);

        /// <summary>
        /// Makes a POST request to reset a password
        /// </summary>
        /// <param name="username">The username of the user to reset</param>
        Task PasswordResetUsername(string username);
    }
}