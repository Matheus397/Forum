using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Swashbuckle.AspNetCore.Swagger;
using ViewModels;

namespace APIForum
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2); 

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "dddd, dd/MM/yyyy";
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "MatheusForum", Version = "v1.0.0" }));

            var config = new MapperConfiguration(w =>
            {
                w.CreateMap<PublicacaoAtualizacaoView, Publicacao>().ForMember(Destino => Destino.Titulo, y => y.Condition(origem => origem.Titulo != null)).ForMember(Destino => Destino.Status, y => y.Condition(origem => origem.Status != null)).ForMember(Destino => Destino.Texto, y => y.Condition(origem => origem.Texto != null));

                w.CreateMap<Usuario, Autor>();

                w.CreateMap<UsuarioView, Usuario>();

                w.CreateMap<Usuario, LoginViewRetorno>().ForMember(Destino => Destino.tokenAcesso, y => y.MapFrom(src => src.ID)).ForMember(Destino => Destino.Nome, y => y.MapFrom(src => src.Nome));

                w.CreateMap<PublicacaoView, Publicacao>();         
              
                w.CreateMap<ComentarioView, Comentario>().ForMember(Destino => Destino.ID, y => y.Ignore()).ForMember(Destino => Destino.Data, y => y.Ignore());

                w.CreateMap<ComentarioAtualizacaoView, Comentario>().ForMember(Destino => Destino.CitacaoId, y => y.Condition(origem => origem.CitacaoId != null)).ForMember(Destino => Destino.mensagem, y => y.Condition(origem => origem.Msg != null));

                w.CreateMap<Publicacao, Publicacao>()
                  .ForMember(Destino => Destino.ID, y => y.Ignore())
                  .ForMember(Destino => Destino.Data, y => y.Ignore())
                  .ForMember(Destino => Destino.Tipo, y => y.Ignore())
                  .ForMember(Destino => Destino.lstComentarios, y => y.Ignore())
                  .ForMember(Destino => Destino.Autor, y => y.Ignore())
                  .ForMember(Destino => Destino.MediaDeVotos, y => y.Ignore())
                  .ForMember(Destino => Destino.Titulo, y => y.Condition(origem => origem.Titulo != null))
                  .ForMember(Destino => Destino.Status, y => y.Condition(origem => origem.Status != null))
                  .ForMember(Destino => Destino.Texto, y => y.Condition(origem => origem.Texto != null));

            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            app.UseSwagger();

            // Ativa o Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiMatheusForum");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}