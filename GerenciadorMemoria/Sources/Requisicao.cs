using System.Text;


namespace GerenciadorMemoria.Sources
{
    public class Requisition
    {
        /// <summary>
        /// Numero da requisição
        /// </summary>
        public int numeroRequisicao { get; set; }

        /// <summary>
        /// Tipo de requisição: S para (S)olicitacao, L para (L)iberacao, I para bloco de memória l(I)vre
        /// </summary>
        public char tipoRequisicao { get; set; }

        /// <summary>
        /// Número da requisição que vai ser liberada da memória.
        /// </summary>
        public int numRequisicaoLiberar { get; set; }

        /// <summary>
        /// Quantidade de memória requisitada pelo processo
        /// </summary>
        public int qtdeMemoriaRequisitada { get; set; }

        /// <summary>
        /// Quantidade de memória disponível
        /// </summary>
        private int qtdeMemoriaDisponivel { get; set; }

        /// <summary>
        /// Endereço inicial do bloco de memória para a requisição (Exclusivo para bloco de memória)
        /// </summary>
        public int enderecoInicialBlocoMemoria { get; set; }

        /// <summary>
        /// Endereço final do bloco de memória para a requisição (Exclusivo para bloco de memória)
        /// </summary>
        public int enderecoFinalBlocoMemoria { get; set; }


        public Requisition(int numeroRequisicao, char tipoRequisicao, int numRequisicaoLiberar, int qtdeMemoriaRequisitada, int endInicialBlocoMem, int endFinalBlocoMem)
        {
            this.numeroRequisicao = numeroRequisicao;
            this.tipoRequisicao = tipoRequisicao;
            this.numRequisicaoLiberar = numRequisicaoLiberar;
            this.qtdeMemoriaRequisitada = qtdeMemoriaRequisitada;
            this.enderecoInicialBlocoMemoria = endInicialBlocoMem;
            this.enderecoFinalBlocoMemoria = endFinalBlocoMem;
            setQtdeMemoriaDisponivel();
        }

        /// <summary>
        /// Retorna a quantidade de memória disponível.
        /// </summary>
        /// <returns>Qtde Memoria Disponível</returns>
        public int getQtdeMemoriaDisponivel()
        {
            return enderecoFinalBlocoMemoria - enderecoInicialBlocoMemoria;
        }

        /// <summary>
        /// Seta Qtde Memoria disponível como enderecoFinalBlocoMemoria - enderecoInicialBlocoMemoria;
        /// </summary>
        public void setQtdeMemoriaDisponivel()
        {
            qtdeMemoriaDisponivel = enderecoFinalBlocoMemoria - enderecoInicialBlocoMemoria;
        }

        /// <summary>
        /// Seta um valor para a Qtde memoria disponivel
        /// </summary>
        /// <param name="valor">Qtde Memoria Disponível</param>
        public void setQtdeMemoriaDisponivel(int valor)
        {
            qtdeMemoriaDisponivel = valor;
        }

        /// <summary>
        /// Método para impressão da requisição
        /// </summary>
        /// <returns>Impressão de acordo com o tipo de Requisição: S, L ou I</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            switch (tipoRequisicao)
            {
                case 'S':
                    sb.Append("Número da Requisição: " + numeroRequisicao + "\t");
                    sb.Append("Tipo de Requisição: " + tipoRequisicao + "\t");
                    sb.Append("Qtde de Memória Requisitada: " + qtdeMemoriaRequisitada);
                    break;
                case 'L':
                    sb.Append("Tipo de Requisição: " + tipoRequisicao + "\t");
                    sb.Append("Número da Requisição Liberada: " + numeroRequisicao + "\t");
                    sb.Append("End. Inicial Bloco Memória: " + enderecoInicialBlocoMemoria + "\t");
                    sb.Append("End. Final Bloco Memória: " + enderecoFinalBlocoMemoria);
                    break;
                case 'I':
                    sb.Append("Qtde de Memória Disponível: " + getQtdeMemoriaDisponivel() + "\t");
                    sb.Append("End. Inicial Bloco Memória: " + enderecoInicialBlocoMemoria + "\t");
                    sb.Append("End. Final Bloco Memória: " + enderecoFinalBlocoMemoria);
                    break;
            }
            sb.Append(" }");
            return sb.ToString();
        }

        /// <summary>
        /// Imprime o estado da memória quando ocorre fragmentação externa
        /// </summary>
        /// <returns>Visualização do estado da memória</returns>
        public string ImprimeRequisicaoFormatoMemoria()
        {
            StringBuilder sb = new StringBuilder();
            return string.Format("{0} - {1}\t\t{2}\t\t{3}", enderecoInicialBlocoMemoria, enderecoFinalBlocoMemoria, GetBlocoLivreOuNumero(), getQtdeMemoriaDisponivel());
        }

        /// <summary>
        /// Imprime se o bloco está livre ou o número do bloco
        /// </summary>
        /// <returns>"Livre" se o bloco está livre, "Bloco x" caso contrário (onde x é o número do bloco)</returns>
        private string GetBlocoLivreOuNumero()
        {
            if(numeroRequisicao == 0)
            {
                return "Livre";
            }

            return $@"Bloco {numeroRequisicao}";
        }
    }
}
