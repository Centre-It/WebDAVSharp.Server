using System.Collections.Generic;
using WebDAVSharp.Server.Adapters;
using WebDAVSharp.Server.Stores;

namespace WebDAVSharp.Server.MethodHandlers
{
    /// <summary>
    /// This class implements the <c>OPTIONS</c> HTTP method for WebDAV#.
    /// </summary>
    internal class WebDavOptionsMethodHandler : WebDavMethodHandlerBase, IWebDavMethodHandler
    {
        /// <summary>
        /// Gets the collection of the names of the HTTP methods handled by this instance.
        /// </summary>
        public IEnumerable<string> Names
        {
            get
            {
                return new[]
                {
                    "OPTIONS"
                };
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The 
        /// <see cref="IWebDavContext" /> object containing both the request and response
        /// objects to use.</param>
        /// <param name="store">The <see cref="IWebDavStore" /> that the <see cref="WebDavServer" /> is hosting.</param>
        public void ProcessRequest(IWebDavContext context, IWebDavStore store)
        {
            List<string> verbsAllowed = new List<string> { "OPTIONS", "TRACE", "GET", "HEAD", "POST", "COPY", "PROPFIND", "LOCK", "UNLOCK" };

            List<string> verbsPublic = new List<string> { "OPTIONS", "GET", "HEAD", "PROPFIND", "PROPPATCH", "MKCOL", "PUT", "DELETE", "COPY", "MOVE", "LOCK", "UNLOCK" };

            foreach (string verb in verbsAllowed)
                context.Response.AppendHeader("Allow", verb);

            foreach (string verb in verbsPublic)
                context.Response.AppendHeader("Public", verb);

            // Sends 200 OK
            context.SetStatusCode();
        }
    }
}