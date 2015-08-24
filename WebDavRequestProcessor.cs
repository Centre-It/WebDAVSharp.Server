using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Logging;
using WebDAVSharp.Server.Adapters;
using WebDAVSharp.Server.Exceptions;
using WebDAVSharp.Server.MethodHandlers;
using WebDAVSharp.Server.Stores;

namespace WebDAVSharp.Server
{
    /// <summary>
    /// Request processor
    /// </summary>
    public class WebDavRequestProcessor
    {
        private readonly ILog _log;
        private readonly IWebDavStore _store;
        private readonly Dictionary<string, IWebDavMethodHandler> _methodHandlers;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="store">The 
        /// <see cref="IWebDavStore" /> store object that will provide
        /// collections and documents</param>
        /// <exception cref="System.ArgumentNullException">
        /// <para>
        ///   <paramref name="store" /> is <c>null</c>.</para></exception>
        /// <param name="methodHandlers">A collection of HTTP method handlers to use or
        /// <c>null</c> to use the built-in method handlers.</param>
        /// <exception cref="System.ArgumentException"><para>
        ///   <paramref name="methodHandlers" /> is empty.</para>
        /// <para>- or -</para>
        /// <para>
        ///   <paramref name="methodHandlers" /> contains a <c>null</c>-reference.</para></exception>
        public WebDavRequestProcessor(IWebDavStore store, IEnumerable<IWebDavMethodHandler> methodHandlers = null)
        {
            _log = LogManager.GetCurrentClassLogger();

            if (store == null)
                throw new ArgumentNullException("store");

            _store = store;

            methodHandlers = methodHandlers ?? WebDavMethodHandlers.BuiltIn;

            IWebDavMethodHandler[] webDavMethodHandlers = methodHandlers as IWebDavMethodHandler[] ?? methodHandlers.ToArray();

            if (!webDavMethodHandlers.Any())
                throw new ArgumentException("The methodHandlers collection is empty", "methodHandlers");
            if (webDavMethodHandlers.Any(methodHandler => methodHandler == null))
                throw new ArgumentException("The methodHandlers collection contains a null-reference", "methodHandlers");

            var handlersWithNames =
                from methodHandler in webDavMethodHandlers
                from name in methodHandler.Names
                select new
                {
                    name,
                    methodHandler
                };
            _methodHandlers = handlersWithNames.ToDictionary(v => v.name, v => v.methodHandler);
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The request.</param>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavMethodNotAllowedException">If the method to process is not allowed</exception>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavUnauthorizedException">If the user is unauthorized or has no access</exception>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavNotFoundException">If the item was not found</exception>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavNotImplementedException">If a method is not yet implemented</exception>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavInternalServerException">If the server had an internal problem</exception>
        public void ProcessRequest(IWebDavContext context)
        {
            if (context == null)
            {
                _log.Warn("Context is null");
                throw new WebDavInternalServerException();
            }

            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();

            // For authentication
            Thread.SetData(Thread.GetNamedDataSlot(Constants.HttpUser), identity);

            _log.Info(context.Request.HttpMethod + " " + context.Request.RemoteEndPoint + ": " + context.Request.Url);
            try
            {
                try
                {
                    string method = context.Request.HttpMethod;
                    IWebDavMethodHandler methodHandler;
                    if (!_methodHandlers.TryGetValue(method, out methodHandler))
                        throw new WebDavMethodNotAllowedException(string.Format(CultureInfo.InvariantCulture, "%s ({0})", context.Request.HttpMethod));

                    context.Response.AppendHeader("DAV", "1,2,1#extend");

                    methodHandler.ProcessRequest(context, _store);
                    context.AfterProcessRequest();

                }
                catch (WebDavException)
                {
                    throw;
                }
                catch (UnauthorizedAccessException)
                {
                    throw new WebDavUnauthorizedException();
                }
                catch (FileNotFoundException ex)
                {
                    _log.Warn(ex.Message);
                    throw new WebDavNotFoundException(innerException: ex);
                }
                catch (DirectoryNotFoundException ex)
                {
                    _log.Warn(ex.Message);
                    throw new WebDavNotFoundException(innerException: ex);
                }
                catch (NotImplementedException ex)
                {
                    _log.Warn(ex.Message);
                    throw new WebDavNotImplementedException(innerException: ex);
                }
                catch (Exception ex)
                {
                    _log.Warn(ex.Message);
                    throw new WebDavInternalServerException(innerException: ex);
                }
            }
            catch (WebDavException ex)
            {
                _log.Warn(ex.StatusCode + " " + ex.Message);
                context.Response.StatusCode = ex.StatusCode;
                context.Response.StatusDescription = ex.StatusDescription;
                if (ex.Message != context.Response.StatusDescription)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(ex.Message);
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                context.AfterProcessRequest();
            }
            finally
            {
                _log.Info(context.Response.StatusCode + " " + context.Response.StatusDescription + ": " + context.Request.HttpMethod + " " + context.Request.RemoteEndPoint + ": " + context.Request.Url);
            }
        }
    }
}