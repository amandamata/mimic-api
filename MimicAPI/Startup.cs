using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimicAPI.Database;
using MimicAPI.V1.Repository;
using MimicAPI.V1.Repository.Interface;
using AutoMapper;
using MimicAPI.Helper;
using System.Linq;
using System;

namespace MimicAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region AutoMapper-Config
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.AddDbContext<MimicContext>(o => {o.UseSqlite("Data Source=Database\\Mimic.db");});
            services.AddMvc(o => o.EnableEndpointRouting = false);
            services.AddOptions();
            services.AddScoped<IPalavraRepository,PalavraRepository>();
            services.AddApiVersioning(c => {
                c.ReportApiVersions = true;
                c.AssumeDefaultVersionWhenUnspecified = true;
                c.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescription => apiDescription.First());
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "MimicAPI - V1", Version = "V1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json","MimicAPI");
                c.RoutePrefix = String.Empty;
            });
        }
    }
}
