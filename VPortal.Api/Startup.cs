using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using VPortal.TokenManager;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace VPortal.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                options.Cookies.ApplicationCookie.AutomaticChallenge = true;
                options.Cookies.ApplicationCookie.AuthenticationScheme ="Bearer";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            Register(app);


        }
         public void Register(IApplicationBuilder app)
        {
            string issuer = TokenHandler.TokenIssuerName;
            string audience = TokenHandler.TokenIssuerName;
            var secret = Encoding.UTF8.GetBytes(TokenHandler.PrivateKey);

            var tokenProviderOptions = new TokenProviderOptions
            {
                Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            };
           app.UseJwtBearerAuthentication(new JwtBearerOptions()
	            {
	                AutomaticAuthenticate = true,
	                AutomaticChallenge = true,
                     AuthenticationScheme = "Bearer",
	                TokenValidationParameters = new TokenValidationParameters()
	                {
	                    ValidIssuer = issuer,
	                    ValidAudience = audience,
	                    ValidateIssuerSigningKey = true,
	                    IssuerSigningKey = new SymmetricSecurityKey(secret),
	                    ValidateLifetime = true
                        
	                }
            });
             app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = "Cookie",
                CookieName = Configuration.GetSection("TokenAuthentication:CookieName").Value,
                TicketDataFormat = new CustomTokenFormat(
                    SecurityAlgorithms.HmacSha256,
                     new TokenValidationParameters()
	                {
	                    ValidIssuer = issuer,
	                    ValidAudience = audience,
	                    ValidateIssuerSigningKey = true,
	                    IssuerSigningKey = new SymmetricSecurityKey(secret),
	                    ValidateLifetime = true
                        
	                })
            });
             app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions));
        }
         private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            // DEMO CODE, DON NOT USE IN PRODUCTION!!!
            Debug.WriteLine(username);
            if (username == "TEST" && password == "TEST123")
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
            }

            // Account doesn't exists
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
