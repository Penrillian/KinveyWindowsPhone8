using System.Threading.Tasks;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// The IKinveyHandshake interface provides access to the handshake function.
    /// </summary>
    public interface IKinveyHandshake
    {
        /// <summary>
        /// Performs a handshake to the app server, authenticated with basic authentication.
        /// </summary>
        /// <returns>the handshake response</returns>
        Task<KinveyHandshakeResponse> Do();
    }
}
