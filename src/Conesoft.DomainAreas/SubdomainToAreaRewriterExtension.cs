using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Conesoft.DomainAreas
{

    public static class SubdomainToAreaRewriterExtension
    {
        public static void UseSubdomainToAreaRewriter(this IApplicationBuilder app, string globalPrefix = "", bool withCascadingStaticFiles = false)
        {
            if(withCascadingStaticFiles)
            {
                app.UseStaticFilesForSubdomains();
            }

            var rewriter = new RewriteOptions();
            rewriter.Add(new SubdomainToAreaRewriter(globalPrefix));
            app.UseRewriter(rewriter);
        }

        private static void UseStaticFilesForSubdomains(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;
            if (env != null)
            {
                foreach (var directory in (Files.Directory.From(env.ContentRootPath) / "Areas").Directories.Where(d => d.Directories.Any(sd => sd.Name == "wwwroot")))
                {
                    app.UseMiddleware<AspNetCore.StaticFiles.StaticFileMiddleware>(Options.Create(new AspNetCore.Builder.StaticFileOptions
                    {
                        ShouldSkip = ctx => ctx.Context.Request.Host.Host.Split(':').First() != directory.Name && ctx.Context.Request.Host.Host.Split(':').First() != directory.Name + ".localhost",
                        FileProvider = new PhysicalFileProvider((directory / "wwwroot").Path)
                    }));
                }
            }

            app.UseStaticFiles();
        }
    }
}