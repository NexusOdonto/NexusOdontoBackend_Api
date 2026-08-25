using Api.Extensions;
using Application.Mappings;
using Application.Validators;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Serilog;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog structured logging
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers();

// FluentValidation: registra los validadores de la capa Application
builder.Services.AddValidatorsFromAssemblyContaining<CrearPacienteRequestDtoValidator>();

// Mapster: carga los IRegister de la capa Application
TypeAdapterConfig.GlobalSettings.Scan(typeof(ClinicalMappingRegister).Assembly);
builder.Services.AddMapster();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddResilientHttpClients();

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
