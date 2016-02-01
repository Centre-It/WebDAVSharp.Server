using System.Security.Principal;
using System.Web;

namespace WebDAVSharp.Server.Adapters.WebContext
{
    /// <summary>
    /// Web context wrapper
    /// </summary>
    public class WebDavHttpContext : IWebHttpContext
    {
        private readonly HttpContext _context;
        private readonly WebHttpContextRequest _request;
        private readonly WebHttpContextResponse _response;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public WebDavHttpContext(HttpContext context)
        {
            _context = context;
            _request = new WebHttpContextRequest(context.Request);
            _response = new WebHttpContextResponse(context.Response);
        }

        /// <summary>
        /// Gets the <see cref="IWebDavRequest" /> request adapter.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        public IWebDavRequest Request
        {
            get { return _request; }
        }

        /// <summary>
        /// Gets the <see cref="IWebDavResponse" /> response adapter.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        public IWebDavResponse Response
        {
            get { return _response; }
        }

        /// <summary>
        /// Gets the <see cref="IPrincipal" /> of the context.
        /// </summary>
        public IPrincipal User
        {
            get { return _context.User; }
        }

        /// <summary>
        /// Any additional operations after main process request logic
        /// </summary>
        public void AfterProcessRequest()
        {
        }

        /// <summary>
        /// Gets the internal instance that was adapted for WebDAV#.
        /// </summary>
        /// <value>
        /// The adapted instance.
        /// </value>
        public HttpContext AdaptedInstance
        {
            get { return _context; }
        }
    }
}