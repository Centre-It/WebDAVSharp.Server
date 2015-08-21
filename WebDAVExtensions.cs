using System;
using System.Linq;
using System.Net;
using System.Web;
using WebDAVSharp.Server.Adapters;
using WebDAVSharp.Server.Adapters.Listener;
using WebDAVSharp.Server.Exceptions;
using WebDAVSharp.Server.Stores;

namespace WebDAVSharp.Server
{
    /// <summary>
    /// This class holds extension methods for various types related to WebDAV#.
    /// </summary>
    internal static class WebDavExtensions
    {
        /// <summary>
        /// Gets the Uri to the parent object.
        /// </summary>
        /// <param name="uri">The <see cref="Uri" /> of a resource, for which the parent Uri should be retrieved.</param>
        /// <returns>
        /// The parent <see cref="Uri" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">uri</exception>
        /// <exception cref="System.InvalidOperationException">Cannot get parent of root</exception>
        /// <exception cref="ArgumentNullException"><paramref name="uri" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="uri" /> has no parent, it refers to a root resource.</exception>
        public static Uri GetParentUri(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            if (uri.Segments.Length == 1)
                throw new InvalidOperationException("Cannot get parent of root");

            string url = uri.ToString();
            int index = url.Length - 1;
            if (url[index] == '/')
                index--;
            while (url[index] != '/')
                index--;
            return new Uri(url.Substring(0, index + 1));
        }

        /// <summary>
        /// Sets status code and description.
        /// </summary>
        /// <param name="context">The <see cref="IHttpListenerContext" /> to send the response through.</param>
        /// <param name="statusCode">The HTTP status code for the response.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        /// <exception cref="ArgumentNullException"><paramref name="context" /> is <c>null</c>.</exception>
        public static void SetStatusCode(this IWebDavContext context, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.Response.StatusCode = statusCode;
            context.Response.StatusDescription = HttpWorkerRequest.GetStatusDescription(statusCode);
        }

        /// <summary>
        /// Sets status code and description.
        /// </summary>
        /// <param name="context">The <see cref="IHttpListenerContext" /> to send the response through.</param>
        /// <param name="statusCode">The HTTP status code for the response.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        /// <exception cref="ArgumentNullException"><paramref name="context" /> is <c>null</c>.</exception>
        public static void SetStatusCode(this IWebDavContext context, int statusCode)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.Response.StatusCode = statusCode;
            context.Response.StatusDescription = HttpWorkerRequest.GetStatusDescription(statusCode);

            string exactPrefix = server.Listener.Prefixes
                .FirstOrDefault(item => url.StartsWith(item, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(exactPrefix))
            {
                return new Uri(exactPrefix);
            }

            string wildcardUrl = new UriBuilder(uri) { Host = "WebDAVSharpSpecialHostTag" }
                .ToString().Replace("WebDAVSharpSpecialHostTag", "*");

            string wildcardPrefix = server.Listener.Prefixes
                .FirstOrDefault(item => wildcardUrl.StartsWith(item, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(wildcardPrefix))
            {
                return new Uri(wildcardPrefix.Replace("://*", string.Format("://{0}", uri.Host)));
            }
           
        }

        ///// <summary>
        ///// Gets the prefix <see cref="Uri" /> that matches the specified <see cref="Uri" />.
        ///// </summary>
        ///// <param name="uri">The <see cref="Uri" /> to find the most specific prefix <see cref="Uri" /> for.</param>
        ///// <param name="context">Context</param>
        ///// <returns>
        ///// The most specific <see cref="Uri" /> for the given <paramref name="uri" />.
        ///// </returns>
        ///// <exception cref="WebDAVSharp.Server.Exceptions.WebDavInternalServerException">Unable to find correct server root</exception>
        ///// <exception cref="WebDavInternalServerException"><paramref name="uri" /> specifies a <see cref="Uri" /> that is not known to the <paramref name="server" />.</exception>
        //public static Uri GetPrefixUri(this Uri uri, IWebDavContext context)
        //{
        //    string.Format("{0}://{1}", context.Request.Url.Scheme, context.Request.Url.Authority);

        //    string url = uri.ToString();
        //    foreach (
        //        string prefix in
        //            server.Listener.Prefixes.Where(
        //                prefix => url.StartsWith(uri.ToString(), StringComparison.OrdinalIgnoreCase)))
        //        return new Uri(prefix);
        //    throw new WebDavInternalServerException("Unable to find correct server root");
        //}

        /// <summary>
        /// Retrieves a store item through the specified
        /// <see cref="Uri" /> from the
        /// specified
        /// <see cref="WebDavServer" /> and
        /// <see cref="IWebDavStore" />.
        /// </summary>
        /// <param name="uri">The <see cref="Uri" /> to retrieve the store item for.</param>
        /// <param name="store">The <see cref="IWebDavStore" /> from which to retrieve the store item.</param>
        /// <param name="context">Context</param>
        /// <returns>
        /// The retrieved store item.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><para>
        ///   <paramref name="uri" /> is <c>null</c>.</para>
        /// <para>
        ///   <paramref name="context" /> is <c>null</c>.</para>
        /// <para>
        ///   <paramref name="store" /> is <c>null</c>.</para></exception>
        /// <exception cref="WebDAVSharp.Server.Exceptions.WebDavNotFoundException">If the item was not found.</exception>
        /// <exception cref="WebDavConflictException"><paramref name="uri" /> refers to a document in a collection, where the collection does not exist.</exception>
        /// <exception cref="WebDavNotFoundException"><paramref name="uri" /> refers to a document that does not exist.</exception>
        public static IWebDavStoreItem GetItem(this Uri uri, IWebDavStore store, IWebDavContext context)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            if (context == null)
                throw new ArgumentNullException("context");
            if (store == null)
                throw new ArgumentNullException("store");

            IWebDavStoreCollection collection = store.Root;

            IWebDavStoreItem item = null;
            if (context.Request.Url.Segments.Length == uri.Segments.Length)
                return collection;

            for (int index = context.Request.Url.Segments.Length; index < uri.Segments.Length; index++)
            {
                string segmentName = Uri.UnescapeDataString(uri.Segments[index]);
                IWebDavStoreItem nextItem = collection.GetItemByName(segmentName.TrimEnd('/', '\\'));
                if (nextItem == null)
                    throw new WebDavNotFoundException(); //throw new WebDavConflictException();

                if (index == uri.Segments.Length - 1)
                    item = nextItem;
                else
                {
                    collection = nextItem as IWebDavStoreCollection;
                    if (collection == null)
                        throw new WebDavNotFoundException();
                }
            }

            if (item == null)
                throw new WebDavNotFoundException();

            return item;
        }
    }
}