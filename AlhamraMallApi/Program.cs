
using AlhamraMall.Data;
using AlhamraMall.Domains.Helper;
using AlhamraMallApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using AlhamraMall.Domains.Services;

namespace AlhamraMallApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // «÷«›… «·”Ê«€— „⁄ «÷«›… ŒÌ«—«   „ﬂÌ‰ «÷«›… «· ÊﬂÌ‰ „‰ Œ·«·Â
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("AlhamraMallApiAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Please enter the authentication token "
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "AlhamraMallApiAuth"
                            }
                        },new List<string>()
                    }
                });


            });


            //  ”ÃÌ· «·ﬂÊ‰ Ìﬂ”  
            builder.Services.AddDbContext<AlhamraMallDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("AlhamraMallContext"))
                );


            // Register FileUploadService
            builder.Services.AddTransient<FileUploadService>();

            //// ≈⁄œ«œ «·ÂÊÌ… «·«› —«÷Ì… „⁄ «·√œÊ«— Ê Œ“Ì‰Â« ›Ì ﬁ«⁄œ… »Ì«‰« 
            //builder.Services.AddDefaultIdentity<IdentityUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<AlhamraMallDbContext>();

            //  ”ÃÌ· «·—Ì»Ê“Ì Ê—Ì «·⁄«„…
            builder.Services.AddScoped(typeof(IGenericRepository<,,>), typeof(GenericRepository<,,>));
            builder.Services.AddScoped<IStoredProcedureRepository, StoredProcedureRepository>();
            builder.Services.AddScoped<ICommercialStoreRepository, CommercialStoreRepository>();


            // Authentication
            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Athentication:Issuer"],
                    ValidAudience = builder.Configuration["Athentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Athentication:secretKey"]))
                };
            });

            // AutoMapper that i will created it
            builder.Services.AddTransient(typeof(HelperMapper<,,>));

            // Register autoMapper that exists in asp.net core 
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Pagination");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // «” Œœ«„ ”Ì«”… CORS «·„÷«›… „”»ﬁ«
            app.UseCors();

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        }
    }
