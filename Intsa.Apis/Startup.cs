using Intsa.Models.Boards;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Intsa.Apis
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
            services.AddControllers();

            #region CORS 
            // [CORS][1] ����� 
            // [CORS][1][2] �⺻
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", 
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()); 
            });

            // [CORS][1][3] Ư�� �����θ� ���
            services.AddCors(o => o.AddPolicy("AllowSpecific", options =>
                    options.WithOrigins("https://localhost:44356")
                            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                            .WithHeaders("accept", "content-type", "origin", "X-TotalRecordCount"))); 

            #endregion

            AddDependencyInjectionContainerForNoticeApp(services);

        }

        /// <summary>
        /// �������� ���� ������ ���� ���� �ڵ� ���� ���� 
        /// </summary>
        /// <param name="services"></param>
        private void AddDependencyInjectionContainerForNoticeApp(IServiceCollection services)
        {
            // NoticeAppDbContext.cs Inject: New DbContext Add
            services.AddEntityFrameworkSqlServer().AddDbContext<NoticeAppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); 

            // INoticeRepositoryAsync.cs Inject: DI Container�� ����(�������丮) ��� 
            services.AddTransient<INoticeRepository, NoticeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // [CORS][2] ��� ���
            app.UseCors("AllowAnyOrigin"); 

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
