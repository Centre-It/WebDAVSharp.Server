using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace WebDAVSharp.Server.Adapters.WebContext
{
    /// <summary>
    /// This is an interface-version of the parts of 
    /// <see cref="HttpRequestBase" /> that
    /// the 
    /// <see cref="WebDavServer" /> requires to operator.
    /// </summary>
    /// /// <remarks>
    /// The main purpose of this interface is to facilitate unit-testing.
    /// </remarks>
    public class WebHttpContextRequest : IWebDavRequest
    {
        private readonly HttpRequest _request;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="request"></param>
        public WebHttpContextRequest(HttpRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Gets the client IP address and port number from which the request originated.
        /// </summary>
        /// <value>
        /// The remote end point.
        /// </value>
        public IPEndPoint RemoteEndPoint
        {
            get { return new IPEndPoint(IPAddress.Parse(_request.UserHostAddress), 80); }
        }

        /// <summary>
        /// Gets the <see cref="Uri" /> object requested by the client.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public Uri Url
        {
            get { return _request.Url; }
        }

        /// <summary>
        /// Gets the HTTP method specified by the client.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public string HttpMethod
        {
            get { return _request.HttpMethod; }
        }

        /// <summary>
        /// Gets the collection of header name/value pairs sent in the request.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public NameValueCollection Headers
        {
            get { return _request.Headers; }
        }

        /// <summary>
        /// Gets a <see cref="Stream" /> that contains the body data sent by the client.
        /// </summary>
        /// <value>
        /// The input stream.
        /// </value>
        public Stream InputStream
        {
            get { return _request.InputStream; }
        }

        /// <summary>
        /// Gets the content <see cref="Encoding" /> that can be used with data sent with the request.
        /// </summary>
        /// <value>
        /// The content encoding.
        /// </value>
        public Encoding ContentEncoding
        {
            get { return _request.ContentEncoding; }
        }

        /// <summary>
        /// Gets the length of the body data included in the request.
        /// </summary>
        /// <value>
        /// The content length64.
        /// </value>
        public long ContentLength64
        {
            get { return _request.ContentLength; }
        }
    }
}