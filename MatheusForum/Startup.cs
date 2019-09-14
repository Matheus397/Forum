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

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "APIForum", Version = "v1.0.0" }));

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Usuario, Autor>();
                cfg.CreateMap<UsuarioView, Usuario>();

                cfg.CreateMap<Usuario, LoginViewRetorno>()
                    .ForMember(dest => dest.tokenAcesso, opt => opt.MapFrom(src => src.ID))
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome));

                cfg.CreateMap<PublicacaoView, Publicacao>();

                cfg.CreateMap<PublicacaoAtualizacaoView, Publicacao>()
                    .ForMember(dest => dest.Titulo, opt => opt.Condition(ori => ori.Titulo != null))
                    .ForMember(dest => dest.Status, opt => opt.Condition(ori => ori.Status != null))
                    .ForMember(dest => dest.Texto, opt => opt.Condition(ori => ori.Texto != null));

                cfg.CreateMap<Publicacao, Publicacao>()
                    .ForMember(dest => dest.ID, opt => opt.Ignore())
                    .ForMember(dest => dest.Data, opt => opt.Ignore())
                    .ForMember(dest => dest.Tipo, opt => opt.Ignore())
                    .ForMember(dest => dest.lstComentarios, opt => opt.Ignore())
                    .ForMember(dest => dest.Autor, opt => opt.Ignore())
                    .ForMember(dest => dest.MediaDeVotos, opt => opt.Ignore())
                    .ForMember(dest => dest.Titulo, opt => opt.Condition(ori => ori.Titulo != null))
                    .ForMember(dest => dest.Status, opt => opt.Condition(ori => ori.Status != null))
                    .ForMember(dest => dest.Texto, opt => opt.Condition(ori => ori.Texto != null));

                cfg.CreateMap<ComentarioView, Comentario>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.Data, opt => opt.Ignore());

                cfg.CreateMap<ComentarioAtualizacaoView, Comentario>()
                .ForMember(dest => dest.CitacaoId, opt => opt.Condition(ori => ori.CitacaoId != null))
                .ForMember(dest => dest.Msg, opt => opt.Condition(ori => ori.Msg != null));


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