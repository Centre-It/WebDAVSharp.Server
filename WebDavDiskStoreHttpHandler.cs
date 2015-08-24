using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using Common.Logging;
using WebDAVSharp.Server.Adapters;
using WebDAVSharp.Server.Adapters.WebContext;
using WebDAVSharp.Server.Exceptions;
using WebDAVSharp.Server.Stores.DiskStore;

namespace WebDAVSharp.Server
{
    /// <summary>
    /// Http handler for web projects
    /// </summary>
    public class WebDavDiskStoreHttpHandler : IHttpHandler, IRequiresSessionState
    {
        private static bool _isConfigured;
        private static string _rootPath;
        private static WebDavDiskStore _store;
        private readonly ILog _log;
        private static WebDavRequestProcessor _requestProcessor;

        /// <summary>
        /// Sets the base path on the disk to the files
        /// </summary>
        /// <param name="rootPath"></param>
        public static void Configure(string rootPath)
        {
            _rootPath = rootPath;
            _store = new WebDavDiskStore(rootPath);
            _requestProcessor = new WebDavRequestProcessor(_store);
            _isConfigured = true;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WebDavDiskStoreHttpHandler()
        {
            _log = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            if (!_isConfigured)
            {
                _log.Error("Call WebDavDiskStoreHttpHandler.Configure(string rootPath) to configure the handler");
                throw new WebDavInternalServerException();
            }

            var webDavHttpContext = new WebDavHttpContext(context);

            _requestProcessor.ProcessRequest(webDavHttpContext);
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { return true; }
        }
    }
}
