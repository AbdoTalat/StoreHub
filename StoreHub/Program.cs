using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreHub.Core.Repository.Interfaces;
using StoreHub.Core.Repository.Implementations;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using StoreHub.UnitOfWork;
using StoreHub.Models.Entities;
using StoreHub.Data.DbContext;
using StoreHub.Data.SeedData;
using StoreHub.Core.Services.Implementation;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Core.SharedRepository;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        //Configure The ConnectionString
        builder.Services.AddDbContext<StoreHubContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString"));
        });

        //Including The Identity Stores
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<StoreHubContext>();

        //To convert Enum numeric values to it's names
        builder.Services.AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyPolicy", options =>
            {
                options.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });


        //Injection
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IPaymentService1, PaymentService1>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();

        

        builder.Services.AddLogging();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


        //Authentication
        

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false; //HTTPs
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "No Key"))
            };
        });



        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
        });



        var app = builder.Build();

        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseCors("MyPolicy");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                await AppSeedData.Initialize(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred seeding the database: {ex.Message}");
            }
        }
        app.Run();
    }
}
