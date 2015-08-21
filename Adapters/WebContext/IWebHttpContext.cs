using System.Web;

namespace WebDAVSharp.Server.Adapters.WebContext
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWebHttpContext : IWebDavContext, IAdapter<HttpContext>
    {
    }
}