namespace SpaceCare.Domain.Comportamentos.Enums
{
    /// <summary>
    /// Define os graus de severidade, criticidade e triagem médica para os alertas 
    /// gerados com base no comportamento ou sinais vitais dos turistas espaciais.
    /// </summary>
    public enum NivelAlerta
    {
        /// <summary>
        /// Alerta Verde: Estado normal. O tripulante encontra-se estável e dentro dos parâmetros ideais de saúde de voo.
        /// </summary>
        Verde = 0,

        /// <summary>
        /// Alerta Amarelo: Atenção. Sinaliza anomalias leves ou gestos recorrentes de desconforto. Requer monitoramento preventivo.
        /// </summary>
        Amarelo = 1,

        /// <summary>
        /// Alerta Vermelho: Emergência médica. Identifica riscos críticos à integridade do passageiro. Requer intervenção imediata da equipe de saúde.
        /// </summary>
        Vermelho = 2
    }
}
