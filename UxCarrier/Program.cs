using CommonLib.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using UxCarrier.Controllers.Filter;
using UxCarrier.Data;
using UxCarrier.Middleware;
using UxCarrier.Models.AppSetting;
using UxCarrier.Repository;
using UxCarrier.Repository.IRepository;
using UxCarrier.Services;
using UxCarrier.Services.Email;
using UxCarrier.Services.IService;

namespace UxCarrier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors();
            #region Nlog
            var env = builder.Environment.EnvironmentName;
            NLogBuilder.ConfigureNLog($"nlog.{env}.config").GetCurrentClassLogger();
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            #endregion
            builder.Services.AddOptions<AppSetting>().BindConfiguration("AppSetting");
            builder.Services.AddSingleton<HtmlEncoder>(
                 HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                                           UnicodeRanges.CjkUnifiedIdeographs }));

            #region DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultSQLConnection"));
                option.EnableSensitiveDataLogging(true);
            });
            builder.Services.AddDbContext<EIVO03DbContext>(option =>
            {
                option.UseSqlServer(
                    builder.Configuration.GetConnectionString("EIVO03SQLConnection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                option.EnableSensitiveDataLogging(true);
            });
            #endregion

            #region repo/services
            builder.Services.AddScoped<IUxBindCardRepository, UxBindCardRepository>();
            builder.Services.AddScoped<IUxCardEmailRepository, UxMemberEmailRepository>();
            builder.Services.AddScoped<IUxCardRepository, UxMemberRepository>();
            builder.Services.AddScoped<UxBindService, UxBindService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            #endregion

            #region IHttpClientFactory
            builder.Services.AddHttpClient();
            #endregion

            #region Controller/JsonSerializer
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers(opt =>
                {
                    opt.Filters.Add<LogActionFilter>();
                })
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            #endregion
            var setting = builder.Configuration.GetSection("AppSetting");

            #region Authentication-jwt
            var key = setting.GetValue<string>("JwtSecret");
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(s: key!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            #region api version
            /*
            int apiVersion = int.Parse($"{setting.GetValue<int>("ApiVersion")}");
            var apiVersioningBuilder = builder.Services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(majorVersion: apiVersion); //if [ApiVersion("1.0", Deprecated = true)], change to new ApiVersion(2.0, 0)
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    //new QueryStringApiVersionReader("api-version"),
                    //new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver"));
            });

            apiVersioningBuilder.AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            builder.Services.AddEndpointsApiExplorer();
            */
            #endregion

            #region swagger
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
            #endregion

            #region Email&Smtp
            builder.Services.AddOptions<MailSettings>().BindConfiguration("MailSettings");
            builder.Services.AddScoped<EmailFactory>();
            builder.Services.AddScoped<EmailBody>();
            builder.Services.AddSingleton<IMailService, MailService>();
            builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
            #endregion

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            //app.UseStatusCodePagesWithReExecute("/Home/ModalMessage", "");
            //app.UseStatusCodePages(async context =>
            //{
            //    var request = context.HttpContext.Request;
            //    var response = context.HttpContext.Response;

            //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
            //    {
            //        response.Redirect("/Home/ModalMessage");
            //    }
            //});

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            } else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
