var builder = WebApplication.CreateBuilder(args);

// Adicione servi�os ao cont�iner.
// O AddCors() deve ser adicionado aqui, antes do app.Build().
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// JWT Auth
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

// Adiciona os servi�os de CORS e define a pol�tica
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

// Configure o pipeline de requisi��es HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// O UseCors() deve ser chamado aqui, depois do app.Build() e antes do app.MapControllers().
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();