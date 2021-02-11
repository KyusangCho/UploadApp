using BlazorDemos.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;
using System.Linq;
using UploadApp.Areas.Identity;
using UploadApp.Data;
using UploadApp.Hubs;
using UploadApp.Models.BNotices;
using UploadApp.Models.BUploads;

namespace UploadApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

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

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddScoped<SampleService>();

            AddDependencyInjectionContainerForBoards(services);
            AddDependencyInjectionContainerForUploads(services);

        }

        /// <summary>
        /// DI for Board Repository Classes & Interfaces
        /// </summary>
        /// <param name="services"></param>
        private void AddDependencyInjectionContainerForBoards(IServiceCollection services)
        {
            // Board > NoticeAppDbContext.cs Inject: New Dbcontext Add
            services.AddEntityFrameworkSqlServer().AddDbContext<NoticeAppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);


            // INoticeRepositoryAsync Inject
            services.AddTransient<INoticeRepository, NoticeRepository>();
        }

        /// <summary>
        /// DI for Board Repository Classes & Interfaces
        /// </summary>
        /// <param name="services"></param>
        private void AddDependencyInjectionContainerForUploads(IServiceCollection services)
        {
            // Board > NoticeAppDbContext.cs Inject: New Dbcontext Add
            services.AddEntityFrameworkSqlServer().AddDbContext<UploadAppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);


            // IUploadRepository Inject
            services.AddTransient<IUploadRepository, UploadRepository>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzk0OTIxQDMxMzgyZTM0MmUzMGswR0VyQVBtOGRKSmNqcGM2TFkwM2RFNU1ua1FlZlJGK2FUbXJMU20xTmM9");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("AllowAnyOrigin");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<NoticeHub>("/noticehub");      // ��� �������� �ǽð� �˸�
                endpoints.MapFallbackToPage("/_Host");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
