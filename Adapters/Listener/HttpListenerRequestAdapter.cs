using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace WebDAVSharp.Server.Adapters.Listener
{
    /// <summary>
    /// This 
    /// <see cref="IWebDavRequest" /> implementation wraps around a
    /// <see cref="HttpListenerRequest" /> instance.
    /// </summary>
    internal sealed class HttpListenerRequestAdapter : IWebDavRequest
    {
        private readonly HttpListenerRequest _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpListenerRequestAdapter" /> class.
        /// </summary>
        /// <param name="request">The <see cref="HttpListenerRequest" /> to adapt for WebDAV#.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <c>null</c>.</exception>
        public HttpListenerRequestAdapter(HttpListenerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            _request = request;
        }

        /// <summary>
        /// Gets the client IP address and port number from which the request originated.
        /// </summary>
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return _request.RemoteEndPoint;
            }
        }

        /// <summary>
        /// Gets the <see cref="Uri" /> object requested by the client.
        /// </summary>
        public Uri Url
        {
            get
            {
                return _request.Url;
            }
        }

        /// <summary>
        /// Gets the HTTP method specified by the client.
        /// </summary>
        public string HttpMethod
        {
            get
            {
                return _request.HttpMethod;
            }
        }

        /// <summary>
        /// Gets the collection of header name/value pairs sent in the request.
        /// </summary>
        public NameValueCollection Headers
        {
            get
            {
                return _request.Headers;
            }
        }

        /// <summary>
        /// Gets a <see cref="Stream" /> that contains the body data sent by the client.
        /// </summary>
        public Stream InputStream
        {
            get
            {
                return _request.InputStream;
            }
        }

        /// <summary>
        /// Gets the content <see cref="Encoding" /> that can be used with data sent with the request.
        /// </summary>
        public Encoding ContentEncoding
        {
            get
            {
                return _request.ContentEncoding;
            }
        }

        /// <summary>
        /// Gets the length of the body data included in the request.
        /// </summary>
        public long ContentLength64
        {
            get
            {
                return _request.ContentLength64;
            }
        }
    }
}