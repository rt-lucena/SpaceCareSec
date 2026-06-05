namespace SpaceCare.Domain.Comportamentos.Enums
{
    /// <summary>
    /// Especifica os tipos de gestos e padrões físicos mapeados pelo sistema de visão computacional SpaceCare.
    /// </summary>
    public enum GestoDetectado
    {
        /// <summary>
        /// Indica o gesto de polegar para cima. Sinaliza aprovação, conformidade ou estado estável.
        /// </summary>
        PolegarParaCima = 0,

        /// <summary>
        /// Indica o gesto de polegar para baixo. Sinaliza desaprovação, problemas de comunicação ou desconforto leve.
        /// </summary>
        PolegarParaBaixo = 1,

        /// <summary>
        /// Indica que o turista está com a mão na cabeça. Pode sinalizar fadiga, tontura ou desorientação espacial
        /// </summary>
        MaoNaCabeca = 2,

        /// <summary>
        /// Indica que o turista está com a mão no peito. Sinal de alerta crítico relacionado a dores cardíacas, falta de ar ou dor torácica.
        /// </summary>
        MaoNoPeito = 3
    }
}
