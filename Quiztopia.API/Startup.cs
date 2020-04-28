using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Quiztopia.Models.Data;
using Quiztopia.Models.Repositories;

namespace Quiztopia.API
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
            //1. Setup
            services.AddControllers(); //Enkel API controllers nodig

            //1.1 loophandling verhinderen
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.RespectBrowserAcceptHeader = true;
            }).AddNewtonsoftJson(options =>
            {
                //circulaire referenties verhinderen door navigatie props
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //2. Registraties (van context, Identity) 
            //2.1. Context
            services.AddDbContext<QuiztopiaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //2.2 Identity ( NIET de AddDefaultIdentity())
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                     options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<QuiztopiaDbContext>();

            //3 Registraties van Repos
            services.AddScoped<IAnswerRepo, AnswerRepo>();
            services.AddScoped<IDifficultyRepo, DifficultyRepo>();
            services.AddScoped<IQuestionRepo, QuestionRepo>();
            services.AddScoped<IQuizRepo, QuizRepo>();
            services.AddScoped<ITopicRepo, TopicRepo>();

            //4. open API documentatie
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Quiztopia", Version = "v1.0" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleMgr, UserManager<IdentityUser> userMgr)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(); //enable swagger
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger"; //path naar de UI pagina: /swagger/index.html
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Quiztopia API v1.0");
            });

            QuiztopiaDbContextExtensions.SeedRoles(roleMgr).Wait();
            QuiztopiaDbContextExtensions.SeedUsers(userMgr).Wait();
        }
    }
}
