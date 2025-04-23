using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimplyFlyServer.Context;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Mapper;
using SimplyFlyServer.Models;
using SimplyFlyServer.Repository;
using SimplyFlyServer.Service;
using System.Text;

namespace SimplyFlyServer
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
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                             new string[] {}
                     }
            });
            });

            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            #endregion
            #region Context
            builder.Services.AddDbContext<SimplyFlyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            #region Repository
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int,Airline>,AirlineRepository>();
            builder.Services.AddScoped<IRepository<int, Aircraft>, AircraftRepository>();
            builder.Services.AddScoped<IRepository<int, FlightRoute>, RouteRepository>();
            builder.Services.AddScoped<IRepository<int, Flight>, FlightRepository>();
            builder.Services.AddScoped<IRepository<int,Price>, PriceRepository>();
            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<int,Booking>, BookingRepository>();
            builder.Services.AddScoped<IRepository<int,Seat>, SeatRepository>();
            builder.Services.AddScoped<IRepository<int,Cancellation>, CancellationRepository>();
            builder.Services.AddScoped<IRepository<int,Notification>, NotificationRepository>();
            
           
            #endregion

            #region Tocken
            builder.Services.AddScoped<ITokenService, TokenService>();
            #endregion


            #region Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthenticateService, AuthenticationService>();
            builder.Services.AddScoped<IAirlineService, AirlineService>();
            builder.Services.AddScoped<IAircraftService, AircraftService>();
            builder.Services.AddScoped<IRouteservice, RouteService>();
            builder.Services.AddScoped<IFlightService, FlightService>();
            builder.Services.AddScoped<IPriceService, PriceService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<ICancellationService, CancellationService>();
            builder.Services.AddScoped<IUserHistoryService, UserHistoryService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IAnalysisService, AnalysiService>();
            #endregion

            #region Mapper
            builder.Services.AddAutoMapper(typeof(FlightMapper));
            builder.Services.AddAutoMapper(typeof(CancellationMapper));

            #endregion

            #region Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtToken"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"Unauthorized: Access denied due to your role.\"}");
                        }
                    };
                });
            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
