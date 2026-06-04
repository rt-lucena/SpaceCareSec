namespace SpaceCare.Domain.Telemetrias.Dtos
{
    /// <summary>
    /// DTO estruturado como imutável (<see langword="record"/>),
    /// utilizado para detalhar as informações de uma telemetria nas respostas da API.
    /// </summary>
    /// <param name="Id">O identificador único da telemetria no banco de dados.</param>
    /// <param name="TuristaId">O identificador único do turista associado a esta leitura.</param>
    /// <param name="BatimentosCardiacos">A frequência cardíaca registrada em batimentos por minuto (BPM).</param>
    /// <param name="TemperaturaCorporal">A temperatura corporal do turista aferida em graus Celsius (°C).</param>
    /// <param name="PressaoArterial">Os dados textuais da aferição de pressão arterial.</param>
    /// <param name="DataLeitura">A data e hora em que a leitura foi realizada.</param>
    public record DetalharTelemetria(
        int Id,
        int TuristaId,
        int BatimentosCardiacos,
        decimal TemperaturaCorporal,
        string PressaoArterial,
        DateTime DataLeitura
    );
}
