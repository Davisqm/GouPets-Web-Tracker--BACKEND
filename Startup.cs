using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetsApi.Models;
using Microsoft.EntityFrameworkCore;
using PetsApi.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetsApi.Services;
using PetsApi.Settings;

namespace PetsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureJWT(services);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .WithOrigins(
                                "http://www.app.goupets.com.co",
                                "http://app.goupets.com.co",
                                "http://52.73.59.250:80",
                                "http://localhost:8080",
                                "http://localhost:8083",
                                "http://127.0.0.1:8080",
                                "http://127.0.0.1:8083")
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .WithExposedHeaders("Access-Control-Allow-Origin");
                    });
            });
            services.AddControllers();
            services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));
            services.AddDbContext<SuppliersContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("SuppliersConnectionStr"));
                opt.EnableSensitiveDataLogging(true);
            });
            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddTransient<INotifyService, NotifyService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ISammService, SammService>();
            services.AddTransient<IFlespiService, FlespiService>();
            services.AddTransient<ITraccarHistoryService, TraccarHistoryService>();       
            services.AddTransient<IEmailService, EmailService>();
            services.AddHostedService<SenderTimeService>();    
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddHttpClient();
            services.AddMvc();

            //_ebb
 
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //} else
            app.UseExceptionHandler("/error");

            // app.UseHttpsRedirection();

            app.UseCors();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                using (var context = scope.ServiceProvider.GetService<SuppliersContext>())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error migrando la base de datos: " + ex.Message);
            }
        }

        public void ConfigureJWT(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JWTSecretKey"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        }
    }
}
