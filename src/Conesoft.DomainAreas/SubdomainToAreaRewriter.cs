using Microsoft.AspNetCore.Rewrite;

namespace Conesoft.DomainAreas
{
    public class SubdomainToAreaRewriter : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var host = context.HttpContext.Request.Host.Host;
            if (context.HttpContext.Request.Path.Value.StartsWith("/_blazor") == false)
            {
                context.HttpContext.Request.Path = "/" + host.Replace(".localhost", "") + context.HttpContext.Request.Path;
            }
        }
    }
}