using System.Security.Principal;

namespace WebDAVSharp.Server.Adapters
{
    /// <summary>
    /// Base interface for context
    /// </summary>
    public interface IWebDavContext
    {
        /// <summary>
        /// Gets the <see cref="IWebDavRequest" /> request adapter.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        IWebDavRequest Request
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IWebDavResponse" /> response adapter.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        IWebDavResponse Response
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IPrincipal" /> of the context.
        /// </summary>
        IPrincipal User
        {
            get;
        }

        /// <summary>
        /// Any additional operations after main process request logic
        /// </summary>
        void AfterProcessRequest();
    }
}