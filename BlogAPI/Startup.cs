using BlogAPI.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BlogAPI {

    /// <summary>
    /// Initializes the Web API.
    /// </summary>
    public class Startup {
        /// <summary>
        /// Contains the applications configuration from appsettings.json 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes the Web API.
        /// </summary>
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }


        /// <summary>
        /// Adds and configures services for the Web API.
        /// </summary>
        public void ConfigureServices(IServiceCollection services){

            //Add the DB to the application context.
            services.AddDbContext<BlogContext>();

            //Allow for the Controllers to send JSON with null values that are hidden.
            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            //Allow for the Controllers to use JWT Tokens to prevent unauthorized access.
            services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<BlogContext>()
            .AddDefaultTokenProviders();

            //Defines the schemas for Authentication. This is using JWT.
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            //Setup the JWT Bearer to match our needs.
            .AddJwtBearer(options =>{
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
                };
            });


            //Use a custom format for invalid model state requests.
            services.PostConfigure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = (context) => {
                    return new BadRequestObjectResult(new
                    {
                        Code = 400,
                        Request_Id = Activity.Current.Id,
                        Messages = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
                    });
                };
            });
        }

        /// <summary>
        /// Adds and configures the HTTP Pipeline for the Web API.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
