using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GerenciadorMemoria.Sources;

namespace GerenciadorMemoria
{
    class Program
    {
        static void Main(string[] args)
        {

            //for(var i = 1; i <= 5; i++)
            //{
            //    string directory = Environment.CurrentDirectory;
            //    string fileName = $@"EntradaTeste{i}.txt";
            //    string fullPath = $@"{directory}\Files\{fileName}";
            //    string[] linhasArquivo;

            //    ReadFile(out linhasArquivo, fullPath);

            //    FilaEncadeada FilaRequisicoes = new FilaEncadeada();
            //    FilaEncadeada FilaPendencias = new FilaEncadeada();
            //    FilaEncadeada FilaPendenciasAuxiliar = new FilaEncadeada();
            //    int enderecoInicialBlocoMemoria = 0;
            //    int enderecoFinalBlocoMemoria = 0;

            //    ObterParametrosGerenciadorMemoria(linhasArquivo, out FilaRequisicoes, out enderecoInicialBlocoMemoria, out enderecoFinalBlocoMemoria);


            //    Sources.GerenciadorMemoria gerMem = new Sources.GerenciadorMemoria(enderecoInicialBlocoMemoria, enderecoFinalBlocoMemoria, FilaRequisicoes, FilaPendencias, FilaPendenciasAuxiliar);

            //    Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% " + fileName + " %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            //    gerMem.GerenciarMemoria();

            //    Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Fim do arquivo " + fileName + " %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%\n\n\n");
            //}

            string directory = Environment.CurrentDirectory;
            string fileName = $@"EntradaTeste6.txt";
            string fullPath = $@"{directory}\Files\{fileName}";
            string[] linhasArquivo;

            ReadFile(out linhasArquivo, fullPath);

            FilaEncadeada FilaRequisicoes = new FilaEncadeada();
            FilaEncadeada FilaPendencias = new FilaEncadeada();
            FilaEncadeada FilaPendenciasAuxiliar = new FilaEncadeada();
            int enderecoInicialBlocoMemoria = 0;
            int enderecoFinalBlocoMemoria = 0;

            ObterParametrosGerenciadorMemoria(linhasArquivo, out FilaRequisicoes, out enderecoInicialBlocoMemoria, out enderecoFinalBlocoMemoria);


            Sources.GerenciadorMemoria gerMem = new Sources.GerenciadorMemoria(enderecoInicialBlocoMemoria, enderecoFinalBlocoMemoria, FilaRequisicoes, FilaPendencias, FilaPendenciasAuxiliar);

            gerMem.GerenciarMemoria();

            Console.ReadLine();
        }

        /// <summary>
        /// Obtém a fila de Requisições o endereço inicial e o endereço final do bloco de memória
        /// </summary>
        /// <param name="linhasArquivo">arquivo lido</param>
        /// <param name="FilaRequisicoes">saida => Fila de Requisições</param>
        /// <param name="enderecoInicialMemoria">saida => Endereco inicial do bloco de memória</param>
        /// <param name="enderecoFinalMemoria">saida => Endereco final do bloco de memória</param>
        private static void ObterParametrosGerenciadorMemoria(string[] linhasArquivo, out FilaEncadeada FilaRequisicoes, out int enderecoInicialMemoria, out int enderecoFinalMemoria)
        {
            FilaEncadeada lFilaReq = new FilaEncadeada();
            int endInicialBlocoMem = int.MinValue;
            int endFinalBlocoMem = int.MinValue;

            int contadorRequisicoes = 1;
            for (int i = 1; i < linhasArquivo.Count(); i++)
            {
                if (i == 1)
                {
                    int.TryParse(linhasArquivo[i].ToString(), out endInicialBlocoMem);
                }
                else if (i == 2)
                {
                    int.TryParse(linhasArquivo[i].ToString(), out endFinalBlocoMem);

                }
                else
                {
                    char tipoRequisicao = char.MinValue;
                    int qtdeMemoriaOuRequisicaoLiberar = int.MinValue;
                    Requisition req;

                    string[] parametrosRequisicoes = linhasArquivo[i].Split(' ');

                    char.TryParse(parametrosRequisicoes[0].ToString(), out tipoRequisicao);

                    int.TryParse(parametrosRequisicoes[1].ToString(), out qtdeMemoriaOuRequisicaoLiberar);

                    if (tipoRequisicao == 'L')
                    {
                        req = new Requisition(0, tipoRequisicao, qtdeMemoriaOuRequisicaoLiberar, 0, int.MinValue, int.MinValue);
                        lFilaReq.Add(req);
                    }
                    else
                    {
                        req = new Requisition(contadorRequisicoes, tipoRequisicao, 0, qtdeMemoriaOuRequisicaoLiberar, int.MinValue, int.MinValue);
                        lFilaReq.Add(req);
                        contadorRequisicoes++;
                    }
                }
            }
            FilaRequisicoes = lFilaReq;
            enderecoInicialMemoria = endInicialBlocoMem;
            enderecoFinalMemoria = endFinalBlocoMem;
        }

        /// <summary>
        /// Faz a leitura do arquivo, e carrega para um array cada uma das linhas do arquivo
        /// </summary>
        /// <param name="lines">array contendo as linhas do arquivo</param>
        /// <param name="fullPath">caminho do arquivo</param>
        private static void ReadFile(out string[] lines, string fullPath)
        {
            try
            {
                lines = File.ReadAllLines(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Falha ao abrir arquivo!");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
