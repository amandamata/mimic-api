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
using MimicAPI.Helper.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace MimicAPI
{
    public class Startup
    {
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
                c.SwaggerDoc("v2.0", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "MimicAPI - v2.0", Version = "v2.0" });
                c.SwaggerDoc("v1.1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "MimicAPI - v1.1", Version = "v1.1" });
                c.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "MimicAPI - v1.0", Version = "v1.0" });
                
                var caminhoProjeto = PlatformServices.Default.Application.ApplicationBasePath;
                var nomeProjeto = $"{PlatformServices.Default.Application.ApplicationName}.xml";
                var caminhoArquivoXMLComentario = Path.Combine(caminhoProjeto, nomeProjeto);
                c.IncludeXmlComments(caminhoArquivoXMLComentario);
                
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    if (actionApiVersionModel == null) return true;
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);

                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v2.0/swagger.json", "MimicAPI - v2.0");
                c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MimicAPI - v1.1");
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MimicAPI - v1.0");
                c.RoutePrefix = String.Empty;
            });
        }
    }
}
