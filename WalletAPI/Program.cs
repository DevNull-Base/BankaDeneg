using System.Text;
using Figgle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
<<<<<<< Updated upstream
=======
using WalletAPI.Extensions;
using WalletAPI.Factories;
using WalletAPI.Handlers;
using WalletAPI.Services;
>>>>>>> Stashed changes

Console.ForegroundColor = ConsoleColor.Blue;
Console.Write(FiggleFonts.Standard.Render("DEVNULL"));
Console.ForegroundColor = ConsoleColor.White;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
<<<<<<< Updated upstream
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WalletAPI", Version = "v1" });
=======
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WalletAPI", Version = "v1" }); 
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
    
    c.AddServer(new OpenApiServer
    {
        Url = "http://localhost:5041",
        Description = "Dev"
    });
    
    c.AddServer(new OpenApiServer
    {
        Url = "https://api.devnullteam.ru",
        Description = "Production DevNull server"
    });
    
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
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
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    
    c.DescribeAllParametersInCamelCase();
    c.EnableAnnotations();
    c.SchemaFilter<EnumSchemaFilter>();
>>>>>>> Stashed changes
});


var secretKey = builder.Configuration["SecretKey"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        };
    });

var app = builder.Build();

var port = builder.Configuration["Port"];
app.Urls.Add($"http://localhost:{port}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Использование Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankAppAPI V1");
    });
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();