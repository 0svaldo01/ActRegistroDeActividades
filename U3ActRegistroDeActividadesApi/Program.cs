using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using U3ActRegistroDeActividadesApi.Helpers;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Models.Security;
using U3ActRegistroDeActividadesApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
#region Services
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
#region Agregar Swagger con JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "U3ActRegistroDeActividadesAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
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
            Array.Empty<string>()
        }
    });
});
#endregion
#region Autenticacion JWT
var jwtoken = builder.Configuration.GetSection("JWT").Get<JWT>();
if (jwtoken != null)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
    (
        jwt =>
        {
            jwt.TokenValidationParameters = new()
            {
                ValidIssuer = jwtoken.Issuer,
                ValidAudience = jwtoken.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoken.Key ?? "")),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };

            jwt.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine($"Token validated: {context.SecurityToken}");
                    return Task.CompletedTask;
                }
            };
        }
    );
};
#endregion
#region Singleton
builder.Services.AddSingleton<JWTHelper>();
builder.Services.AddSingleton<Encriptacion>();
#endregion
#region Transient`s
builder.Services.AddTransient<DepartamentosRepository>();
builder.Services.AddTransient<ActividadesRepository>();
#endregion
#endregion
#region Conexion a la base de datos
var Db = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<ItesrcneActividadesContext>(x =>
{
    x.UseMySql(Db, ServerVersion.AutoDetect(Db));
});
#endregion
var app = builder.Build();
#region Uso de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion
#region Configuracion General de app
app.UseCors(
    x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    );
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion