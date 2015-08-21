using System.Web;

namespace WebDAVSharp.Server.Adapters
{
    /// <summary>
    /// This is an interface-version of the parts of 
    /// <see cref="HttpContextBase" /> that
    /// the 
    /// <see cref="WebDavServer" /> requires to operator.
    /// </summary>
    /// <remarks>
    /// The main purpose of this interface is to facilitate unit-testing.
    /// </remarks>
    public interface IHttpContext : IWebDavContext, IAdapter<HttpContextBase>
    {
        
    }
}