using System;
using System.Net;
using System.Security.Principal;

namespace WebDAVSharp.Server.Adapters.Listener
{
    /// <summary>
    /// This 
    /// <see cref="IHttpListenerContext" /> implementation wraps around a
    /// <see cref="HttpListenerContext" /> instance.
    /// </summary>
    internal sealed class HttpListenerContextAdapter : IHttpListenerContext
    {
        private readonly HttpListenerContext _context;
        private readonly HttpListenerRequestAdapter _request;
        private readonly HttpListenerResponseAdapter _response;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpListenerContextAdapter" /> class.
        /// </summary>
        /// <param name="context">The <see cref="HttpListenerContext" /> to adapt for WebDAV#.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        /// <exception cref="ArgumentNullException"><paramref name="context" /> is <c>null</c>.</exception>
        public HttpListenerContextAdapter(HttpListenerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
            _request = new HttpListenerRequestAdapter(context.Request);
            _response = new HttpListenerResponseAdapter(context.Response);
        }

        /// <summary>
        /// Gets the internal instance that was adapted for WebDAV#.
        /// </summary>
        /// <value>
        /// The adapted instance.
        /// </value>
        public HttpListenerContext AdaptedInstance
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Gets the <see cref="IWebDavRequest" /> request adapter.
        /// </summary>
        public IWebDavRequest Request
        {
            get
            {
                return _request;
            }
        }

        /// <summary>
        /// Gets the <see cref="IWebDavResponse" /> response adapter.
        /// </summary>
        public IWebDavResponse Response
        {
            get
            {
                return _response;
            }
        }

        public IPrincipal User
        {
            get { return _context.User; }
        }

        public void AfterProcessRequest()
        {
            Response.Close();
        }
    }
}