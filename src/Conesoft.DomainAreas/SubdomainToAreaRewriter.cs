using Microsoft.AspNetCore.Rewrite;

namespace Conesoft.DomainAreas
{
    public class SubdomainToAreaRewriter : IRule
    {
        readonly string globalPrefix = "";

        public SubdomainToAreaRewriter(string globalPrefix)
        {
            this.globalPrefix = globalPrefix;
        }

        public void ApplyRule(RewriteContext context)
        {
            var host = context.HttpContext.Request.Host.Host;
            var path = context.HttpContext.Request.Path.Value;

            if (globalPrefix == "" || host.StartsWith(globalPrefix + ".") == false)
            {
                if (path.StartsWith("/_blazor") == false)
                {
                    context.HttpContext.Request.Path = "/" + host.Replace(".localhost", "") + path;
                }
            }
        }
    }
}