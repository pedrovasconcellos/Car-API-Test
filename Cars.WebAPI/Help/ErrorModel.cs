using System.Collections.Generic;

namespace Cars.WebAPI.Help
{
    /// <summary>
    /// Error
    /// </summary>
    /// <remarks>
    /// Entidade de resposta de todos os erros de solicitações realizadas na WebAPI.
    /// </remarks>
    public class ErrorModel
    {
        /// <summary>
        /// Error
        /// </summary>
        /// <remarks>
        /// Construtor da Entidade Error que descreve os erros gerados na WebAPI.
        /// </remarks>
        public ErrorModel(short errorCode, string parameter, string message)
        {
            this.ErrorCode = errorCode;
            this.Parameter = parameter;
            this.Message = message;
        }

        /// <summary>
        /// ErrorCode
        /// </summary>
        /// <remarks>
        /// Código ou ordem do erro gerado.
        /// </remarks>
        public short ErrorCode { get; private set; }

        /// <summary>
        /// Parameter
        /// </summary>
        /// <remarks>
        /// Parâmetro ou campo causador do erro.
        /// </remarks>
        public string Parameter { get; private set; }

        /// <summary>
        /// Message
        /// </summary>
        /// <remarks>
        /// Mensagem do erro.
        /// </remarks>
        public string Message { get; private set; }

        /// <summary>
        /// ToList
        /// </summary>
        /// <remarks>
        /// Cria a lista de Erros usando o própio objeto.
        /// </remarks>
        public IList<ErrorModel> ToList() => new List<ErrorModel>() { this };
    }
}
