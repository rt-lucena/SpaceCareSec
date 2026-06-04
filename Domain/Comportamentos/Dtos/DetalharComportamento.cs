using SpaceCare.Domain.Comportamentos.Enums;

namespace SpaceCare.Domain.Comportamentos.Dtos
{
    public record DetalharComportamento(
        int Id,
        int TuristaId,
        int DuracaoGestoSegundos,
        GestoDetectado GestoDetectado,
        NivelAlerta NivelAlerta,
        DateTime DataLeitura
    );
}
