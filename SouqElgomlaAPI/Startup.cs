using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace SouqElgomlaAPI
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

            services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<SouqElgomlaContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SouqElGomla"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            /**Identity Configuration of User*/

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<SouqElgomlaContext>()
                .AddDefaultTokenProviders();


            /**Configuration for UserRepository
             * One instance per Request with AddScopped
             */

            services.AddScoped<IUserRepository, UserRepository>();

            #region check Token validation
            /**configuration to check if the token is valid or not when user send it in Bearer Authorizatio
             * in check validation of Token we used the same way of creating signinkey in UserRepository
             * (the way used to Encryption used again to Decyription)
             */

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            #endregion


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SouqElgomlaAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SouqElgomlaAPI v1"));
            }

            app.UseRouting();

            /*Allow cors cross-origin requests*/
            app.UseCors(options =>
                           options.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());

            /**
             * After apply identity configuration add UseAuthentication and UseAuthorization
             * after UseRouting and before controolers mapping to check if the user is regestred or not
             */

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
