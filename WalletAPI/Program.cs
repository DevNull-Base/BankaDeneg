using System.Text;
using Figgle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WalletAPI.Factories;
using WalletAPI.Handlers;
using WalletAPI.Services;

Console.ForegroundColor = ConsoleColor.Blue;
Console.Write(FiggleFonts.Standard.Render("DEVNULL"));
Console.ForegroundColor = ConsoleColor.White;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]));
var issuer = builder.Configuration["ISSUER"];
var audience = builder.Configuration["AUDIENCE"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            IssuerSigningKey = secretKey,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<UserAccountCredentialsFactory>();
builder.Services.AddSingleton<IUserAccountService, UserAccountService>();

builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddHttpClient<IOpenApiService, OpenApiService>()
    .ConfigurePrimaryHttpMessageHandler(() => new UnsafeHttpClientHandler());

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WalletAPI", Version = "v1" }); 
    
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    
    c.AddSecurityDefinition("Bearer", securityScheme);
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    });
});

var app = builder.Build();


var settings = new JsonSerializerSettings
{
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new CamelCaseNamingStrategy()
    }
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Использование Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankAppAPI V1"); });
    app.UseDeveloperExceptionPage();
}
else
{
    var port = builder.Configuration["Port"];
    app.Urls.Add($"http://localhost:{port}");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();