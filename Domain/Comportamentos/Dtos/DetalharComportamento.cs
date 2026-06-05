using SpaceCare.Domain.Comportamentos.Enums;

namespace SpaceCare.Domain.Comportamentos.Dtos
{
    /// <summary>
    /// DTO estruturado como imutável (<see langword="record"/>),
    /// utilizado para detalhar as informações completas de um comportamento detectado, incluindo o nível de alerta e a data da leitura.
    /// </summary>
    /// <param name="Id">O identificador único do comportamento no banco de dados.</param>
    /// <param name="TuristaId">O identificador único do turista ao qual o comportamento pertence.</param>
    /// <param name="DuracaoGestoSegundos">A duração total do gesto detectado em segundos.</param>
    /// <param name="GestoDetectado">O tipo de gesto detectado (mapeado pelo enumerador <see cref="Enums.GestoDetectado"/>).</param>
    /// <param name="NivelAlerta">O nível de alerta gerado automaticamente para o gesto (mapeado pelo enumerador <see cref="Enums.NivelAlerta"/>).</param>
    /// <param name="DataLeitura">A data e hora em que o comportamento foi registrado.</param>
    public record DetalharComportamento(
        int Id,
        int TuristaId,
        int DuracaoGestoSegundos,
        GestoDetectado GestoDetectado,
        NivelAlerta NivelAlerta,
        DateTime DataLeitura
    );
}
