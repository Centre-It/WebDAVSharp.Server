using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace WebDAVSharp.Server.Adapters.WebContext
{
    /// <summary>
    /// This is an implementation of 
    /// <see cref="IWebDavResponse" />
    /// </summary>
    /// <remarks>
    /// The main purpose of this interface is to facilitate unit-testing.
    /// </remarks>
    public class WebHttpContextResponse : IWebDavResponse
    {
        private readonly HttpResponse _response;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="response"></param>
        public WebHttpContextResponse(HttpResponse response)
        {
            _response = response;
        }

        /// <summary>
        /// Gets or sets the HTTP status code to be returned to the client.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode
        {
            get { return _response.StatusCode; }
            set { _response.StatusCode = value; }
        }

        /// <summary>
        /// Gets or sets a text description of the HTTP <see cref="IWebDavResponse.StatusCode">status code</see> returned to the client.
        /// </summary>
        /// <value>
        /// The status description.
        /// </value>
        public string StatusDescription
        {
            get { return _response.StatusDescription; }
            set { _response.StatusDescription = value; }
        }

        /// <summary>
        /// Gets a <see cref="Stream" /> object to which a response can be written.
        /// </summary>
        /// <value>
        /// The output stream.
        /// </value>
        public Stream OutputStream
        {
            get { return _response.OutputStream; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Encoding" /> for this response's <see cref="IWebDavResponse.OutputStream" />.
        /// </summary>
        /// <value>
        /// The content encoding.
        /// </value>
        public Encoding ContentEncoding
        {
            get { return _response.ContentEncoding; }
            set { _response.ContentEncoding = value; }
        }

        /// <summary>
        /// Gets or sets the number of bytes in the body data included in the response.
        /// </summary>
        /// <value>
        /// The content length64.
        /// </value>
        public long ContentLength64
        {
            get
            {
                var contentLength = _response.Headers.Get("Content-length");

                if (contentLength != null)
                    return long.Parse(contentLength);

                return 0;
            }
            set { _response.Headers["Content-length"] = value.ToString(); }
        }

        /// <summary>
        /// Sends the response to the client and releases the resources held by the adapted
        /// <see cref="HttpListenerResponse" /> instance.
        /// </summary>
        public void Close()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Appends a value to the specified HTTP header to be sent with the response.
        /// </summary>
        /// <param name="name">The name of the HTTP header to append the <paramref name="value" /> to.</param>
        /// <param name="value">The value to append to the <paramref name="name" /> header.</param>
        public void AppendHeader(string name, string value)
        {
            _response.AddHeader(name, value);
        }
    }
}