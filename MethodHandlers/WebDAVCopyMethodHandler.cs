using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using WebDAVSharp.Server.Adapters;
using WebDAVSharp.Server.Adapters.Listener;
using WebDAVSharp.Server.Exceptions;
using WebDAVSharp.Server.Stores;

namespace WebDAVSharp.Server.MethodHandlers
{
    /// <summary>
    /// This class implements the <c>COPY</c> HTTP method for WebDAV#.
    /// </summary>
    internal class WebDavCopyMethodHandler : WebDavMethodHandlerBase, IWebDavMethodHandler
    {
        /// <summary>
        /// Gets the collection of the names of the HTTP methods handled by this instance.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        public IEnumerable<string> Names
        {
            get
            {
                return new[]
                {
                    "COPY"
                };
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The 
        /// <see cref="IHttpListenerContext" /> object containing both the request and response
        /// objects to use.</param>
        /// <param name="store">The <see cref="IWebDavStore" /> that the <see cref="WebDavServer" /> is hosting.</param>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavMethodNotAllowedException"></exception>
        public void ProcessRequest(IWebDavContext context, IWebDavStore store)
        {            
            IWebDavStoreItem source = context.Request.Url.GetItem(store, context);
            if (source is IWebDavStoreDocument || source is IWebDavStoreCollection)
                CopyItem(context, store, source);
            else
                throw new WebDavMethodNotAllowedException();
        }

        /// <summary>
        /// Copies the item.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="store">The store.</param>
        /// <param name="source">The source.</param>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavForbiddenException"></exception>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavPreconditionFailedException"></exception>
        private static void CopyItem(IWebDavContext context, IWebDavStore store, IWebDavStoreItem source)
        {
            Uri destinationUri = GetDestinationHeader(context.Request);
            IWebDavStoreCollection destinationParentCollection = GetParentCollection(store, context, destinationUri);

            bool copyContent = (GetDepthHeader(context.Request) != 0);
            bool isNew = true;

            string destinationName = Uri.UnescapeDataString(destinationUri.Segments.Last().TrimEnd('/', '\\'));
            IWebDavStoreItem destination = destinationParentCollection.GetItemByName(destinationName);
            
            if (destination != null)
            {
                if (source.ItemPath == destination.ItemPath)
                    throw new WebDavForbiddenException();
                if (!GetOverwriteHeader(context.Request))
                    throw new WebDavPreconditionFailedException();
                if (destination is IWebDavStoreCollection)
                    destinationParentCollection.Delete(destination);
                isNew = false;
            }

            destinationParentCollection.CopyItemHere(source, destinationName, copyContent);

            var statusCode = isNew ? HttpStatusCode.Created : HttpStatusCode.NoContent;
            context.SetStatusCode(statusCode);
        }
    }
}