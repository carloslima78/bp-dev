using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace BeePlace.API.Swagger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BeePlaceDataBaseConnectionString = configuration.GetConnectionString("BeePlaceDataBaseConnectionString");
            BeePlaceStorageConnectionString = configuration.GetConnectionString("BeePlaceStorageConnectionString");
        }

        public static string BeePlaceDataBaseConnectionString { get; private set; }

        public static string BeePlaceStorageConnectionString { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented; });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BeePlace API", Description = "BeePlace Core API", Version = "v1" });
                //Inclui comentários na documentação do swagger
                var xmlPath = AppDomain.CurrentDomain.BaseDirectory + @"BeePlace.API.Swagger.xml";
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //Habilita o midleware para servir o Swagger gerado como um endpoint JSON
            app.UseSwagger();
            //Habilita o midleware para servir o swagger-ui(HTML,JS,CSS,etc), especificando o endpoint Swagger JSON
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Core API V1");
            });
        }
    }
}
