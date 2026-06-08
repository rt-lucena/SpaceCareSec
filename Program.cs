using Microsoft.EntityFrameworkCore;
using SpaceCare.Domain.Comportamentos.Interfaces;
using SpaceCare.Domain.Telemetrias.Interfaces;
using SpaceCare.Domain.Turistas.Interfaces;
using SpaceCare.Infra.MIddleware;
using SpaceCare.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// CONFIGURAÇÃO ENTITY FRAMEWORK COM ORACLE
var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

builder.Services.AddDbContext<SpaceCare.Infra.Data.AppDbContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddScoped<ITuristaService, TuristaService>();
builder.Services.AddScoped<ITelemetriaService, TelemetriaService>();
builder.Services.AddScoped<IComportamentoService, ComportamentoService>();
builder.Services.AddScoped<MedicalCryptoService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<TratadorGlobalErros>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapGet("/teste-crypto", (MedicalCryptoService crypto) =>
{
    var textoOriginal = "PASSAPORTE123";

    var textoCriptografado = crypto.Encrypt(textoOriginal);

    var textoDescriptografado = crypto.Decrypt(textoCriptografado);

    return Results.Ok(new
    {
        Original = textoOriginal,
        Criptografado = textoCriptografado,
        Descriptografado = textoDescriptografado
    });
});

app.Run();
