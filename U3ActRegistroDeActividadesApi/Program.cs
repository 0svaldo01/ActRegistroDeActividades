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

#region Conexion a la base de datos
var dbConnectionString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<ItesrcneActividadesContext>(options =>
{
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString));
});
#endregion
#region Services
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
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
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwt =>
    {
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            ValidIssuer = jwtoken.Issuer,
            ValidAudience = jwtoken.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoken.Key))
        };
    });
}
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

var app = builder.Build();

#region Uso de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

app.UseCors(x => x
.AllowAnyHeader()
.AllowAnyMethod()
.AllowAnyOrigin());

#region Configuracion General de app
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion