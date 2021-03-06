﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Business.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WS.Data;
using WS.Interfaces;
using WS.Business.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using WS.Data.Repositories;
using WS.Business;

namespace WS.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "691453484585-9i6kjel2pmoccfn5qrvbfuilpnu88h8j.apps.googleusercontent.com";
                googleOptions.ClientSecret = "9W-kuFKkfRp8LmuDCSFhKtV1";
            }).AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "281006039204288";
                facebookOptions.AppSecret = "e9fa86f0142053dcf03b19a39a353077";
            });

            // Add application services.
            services.AddTransient<EmailSender>();

            services.AddScoped<DocumentRepository>();
            services.AddScoped<DocumentRepository>();
            services.AddScoped<DocumentLinkRepository>();
            services.AddScoped<UserDocumentRepository>();
            services.AddScoped<UserRepository>();

            services.AddTransient<DocumentService>();
            services.AddTransient<DocumentLinkService>();
            services.AddTransient<UserDocumentService>();
            services.AddTransient<DownloadService>();
            services.AddTransient<UserService>();
            services.AddTransient<PathProvider>();
            services.AddTransient<SearchService>();
            services.AddAutoMapper();
            services.AddSharing();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
               // app.UseBrowserLink();
              //  app.UseDeveloperExceptionPage();
               // app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
            }
            app.UseExceptionHandler("/Error/500");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseStaticFiles();
            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
         
        }
    }
}
