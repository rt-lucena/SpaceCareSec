namespace SpaceCare.Domain.Exceptions
{
    /// <summary>
    /// Exceção lançada para indicar que um turista não foi encontrado no SpaceCare.
    /// </summary>
    public class TuristaNotFoundException : Exception
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="TuristaNotFoundException"/> com uma mensagem padrão.
        /// </summary>
        public TuristaNotFoundException() : base ("Turista não encontrado no sistema.") {}

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="TuristaNotFoundException"/> com uma mensagem personalizada.
        /// </summary>
        /// <param name="message">A mensagem que descreve a exceção.</param>
        public TuristaNotFoundException(string message) : base(message) {}
    }
}
