using Figgle;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedModels;
using WalletAPI.Handlers;
using WalletAPI.Services;
using TokenHandler = WalletAPI.Handlers.TokenHandler;

Console.ForegroundColor = ConsoleColor.Blue;
Console.Write(FiggleFonts.Standard.Render("DEVNULL"));
Console.ForegroundColor = ConsoleColor.White;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddTransient<TokenHandler>();

builder.Services.AddHttpClient<IOpenApiService, OpenApiService>()
    .ConfigurePrimaryHttpMessageHandler(() => new UnsafeHttpClientHandler())
    .AddHttpMessageHandler<TokenHandler>();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "WalletAPI", Version = "v1" }); });

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

app.UseHttpsRedirection();

app.MapControllers();

app.Run();