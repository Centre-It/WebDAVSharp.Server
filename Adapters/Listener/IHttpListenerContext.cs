using System.Net;

namespace WebDAVSharp.Server.Adapters.Listener
{
    /// <summary>
    /// This is an interface-version of the parts of 
    /// <see cref="HttpListenerContext" /> that
    /// the 
    /// <see cref="WebDavServer" /> requires to operator.
    /// </summary>
    /// <remarks>
    /// The main purpose of this interface is to facilitate unit-testing.
    /// </remarks>
    public interface IHttpListenerContext : IWebDavContext, IAdapter<HttpListenerContext>
    {
        
    }
}