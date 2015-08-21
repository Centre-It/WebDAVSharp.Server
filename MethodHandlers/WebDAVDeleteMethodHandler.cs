using System.Collections.Generic;
using WebDAVSharp.Server.Adapters;
using WebDAVSharp.Server.Stores;

namespace WebDAVSharp.Server.MethodHandlers
{
    /// <summary>
    /// This class implements the <c>DELETE</c> HTTP method for WebDAV#.
    /// </summary>
    internal class WebDavDeleteMethodHandler : WebDavMethodHandlerBase, IWebDavMethodHandler
    {

        #region Properties

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
                    "DELETE"
                };
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The
        /// <see cref="IWebDavContext" /> object containing both the request and response
        /// objects to use.</param>
        /// <param name="store">The <see cref="IWebDavStore" /> that the <see cref="WebDavServer" /> is hosting.</param>
        public void ProcessRequest(IWebDavContext context, IWebDavStore store)
        {
            // Get the parent collection of the item
            IWebDavStoreCollection collection = GetParentCollection(store, context, context.Request.Url);

            // Get the item from the collection
            IWebDavStoreItem item = GetItemFromCollection(collection, context.Request.Url);

            // Deletes the item
            collection.Delete(item);
        }

        #endregion
    }
}