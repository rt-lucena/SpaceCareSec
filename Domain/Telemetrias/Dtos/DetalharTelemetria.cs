namespace SpaceCare.Domain.Telemetrias.Dtos
{
    public record DetalharTelemetria(
        int Id,
        int TuristaId,
        int BatimentosCardiacos,
        decimal TemperaturaCorporal,
        string PressaoArterial,
        DateTime DataLeitura
    );
}
