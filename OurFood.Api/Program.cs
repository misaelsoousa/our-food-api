using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Server.Kestrel.Core;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using OurFood.Api.Infrastructure;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.Configure<KestrelServerOptions>(options =>

{

    options.Limits.MaxRequestBodySize = int.MaxValue;

});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "OurFood API", Version = "v1" });
    
    // Adicionar suporte à autenticação JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<OurFoodDbContext>(o => o.UseMySql(

builder.Configuration.GetConnectionString("OurFoodDb"),

ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("OurFoodDb"))

));



builder.Services.AddUseCases();



builder.Services.AddAuthentication(options =>

{

    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

})

.AddJwtBearer(options =>

{

    var config = builder.Configuration;

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters

    {

        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateIssuerSigningKey = true,

        ValidateLifetime = true,

        ValidIssuer = config["Jwt:Issuer"],

        ValidAudience = config["Jwt:Audience"],

        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? string.Empty))

    };

});



builder.Services.AddCors(options =>

{

    options.AddPolicy("AllowAllOrigins",

      policy =>

      {

          policy.AllowAnyOrigin()

           .AllowAnyMethod()

           .AllowAnyHeader();

      });

});



var app = builder.Build();



app.UseSwagger();

app.UseSwaggerUI();



app.UseCors("AllowAllOrigins");



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();