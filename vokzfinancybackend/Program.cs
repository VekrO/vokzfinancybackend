using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VokzFinancy.Data;
using VokzFinancy.DTOs.Mapper;
using VokzFinancy.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var environmentName = configuration["Local"];

var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
    EnvironmentName = environmentName
});

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidAudience = builder.Configuration["Jwt:Audience"],
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key)
});

// Conexão com Sql Server
builder.Services.AddDbContext<BancoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// D.I
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers().AddNewtonsoftJson(o => {
    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {

    options.AddPolicy("AllowVokzFinancy", builder =>
    {
        builder.WithOrigins("https://vokzfinancy.app", "https://hom-vokzfinancy.vercel.app", "http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Permitir credenciais
    });

});

// Auto Mapper
var mappingProfile = new MapperConfiguration(mc => mc.AddProfile(new EntityMappingProfile()));
IMapper mapper = mappingProfile.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();


var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Adicione o middleware de tratamento de exceções aqui
app.UseExceptionHandler(app =>
{

    app.Run(async context =>
    {

        var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandler.Error;

        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/html";

        var response = "Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde.";

        await context.Response.WriteAsync(exception.Message);

    });

});

app.UseRouting();
app.UseCors("AllowVokzFinancy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
