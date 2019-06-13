using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorMemoria.Sources
{
    public class GerenciadorMemoria
    {
        /// <summary>
        /// Endereço inicial do bloco de memória
        /// </summary>
        public int endInicialBlocoMemoria { get; set; }

        /// <summary>
        /// Endereço final do bloco de memória
        /// </summary>
        public int endFinalBlocoMemoria { get; set; }

        /// <summary>
        /// Fila de novas requisições
        /// </summary>
        public FilaEncadeada FilaRequisicoes { get; set; }

        /// <summary>
        /// Fila de controle de requisições pendentes
        /// </summary>
        public FilaEncadeada FilaPendencias { get; set; }

        /// <summary>
        /// Fila auxiliar de controle de requisições pendentes
        /// </summary>
        public FilaEncadeada FilaPendenciasAuxiliar { get; set; }

        /// <summary>
        /// Memória
        /// </summary>
        public FilaEncadeada Memoria { get; set; }

        /// <summary>
        /// Instancia o gerenciador de memória
        /// </summary>
        /// <param name="endIniBlocoMem">Endereço inicial do bloco de memória</param>
        /// <param name="endFimBlocoMem">Endereço final do bloco de memória</param>
        /// <param name="filaReq">Fila com requisições</param>
        /// <param name="filaPend">Fila de pendências</param>
        /// <param name="filaPendAux">Fila auxiliar para controle de pendências</param>
        public GerenciadorMemoria(int endIniBlocoMem, int endFimBlocoMem, FilaEncadeada filaReq, FilaEncadeada filaPend, FilaEncadeada filaPendAux)
        {
            endInicialBlocoMemoria = endIniBlocoMem;
            endFinalBlocoMemoria = endFimBlocoMem;
            FilaRequisicoes = filaReq;
            FilaPendencias = filaPend;
            FilaPendenciasAuxiliar = filaPendAux;
            Memoria = InicializarMemoria(endInicialBlocoMemoria, endFinalBlocoMemoria);

        }

        /// <summary>
        /// Inicializa a Memória do sistema.
        /// </summary>
        /// <param name="pEndInicialBlocoMemoria">O endereço inicial do bloco de memória</param>
        /// <param name="pEndFinalBlocoMemoria">O endereço final do bloco de memória</param>
        /// <returns>A fila que representa a utilização da memória</returns>
        private FilaEncadeada InicializarMemoria(int pEndInicialBlocoMemoria, int pEndFinalBlocoMemoria)
        {
            Requisition req = new Requisition(0, 'I', 0, 0, pEndInicialBlocoMemoria, pEndFinalBlocoMemoria);
            FilaEncadeada lMemoria = new FilaEncadeada();
            lMemoria.Add(req);
            return lMemoria;
        }

        /// <summary>
        /// Método que inicializa o processo de gerenciar a memória a partir das filas fornecidas.
        /// </summary>
        public void GerenciarMemoria()
        {
            bool hasSuccess = true;
            for (int i = 0; i < FilaRequisicoes.Count(); i++)
            {
                if (FilaRequisicoes.GetIndex(i).tipoRequisicao == 'S')
                {
                    hasSuccess = Memoria.AddFirstFit(FilaRequisicoes.GetIndex(i));
                    if (!hasSuccess)
                    {
                        FilaPendencias.Add(FilaRequisicoes.GetIndex(i));
                        int tamanhoMemoria = 0;
                        for(int m = 0; m < Memoria.Count(); m++)
                        {
                            if(Memoria.GetIndex(m).tipoRequisicao == 'I')
                            {
                                tamanhoMemoria += Memoria.GetIndex(m).getQtdeMemoriaDisponivel();
                            }
                        }

                        if (tamanhoMemoria < FilaRequisicoes.GetIndex(i).qtdeMemoriaRequisitada)
                        {
                            ImprimeFaltaMemoria(FilaRequisicoes.GetIndex(i));
                        } else
                        {
                            ImprimeFragmentacaoExterna(FilaRequisicoes.GetIndex(i));
                        }
                        
                    }
                }
                else
                {
                    Memoria.Remove(FilaRequisicoes.GetIndex(i).numRequisicaoLiberar);
                    FilaEncadeada temp = new FilaEncadeada();
                    for (int l = 0; l < FilaPendencias.Count(); l++)
                    {
                        hasSuccess = Memoria.AddFirstFit(FilaPendencias.GetIndex(l));
                        if (!hasSuccess)
                        {
                            FilaPendenciasAuxiliar.Add(FilaPendencias.GetIndex(l));
                        } else
                        {
                            ImprimeEstadoFinalGerenciadorMemoria();
                        }
                    }
                    FilaPendencias = FilaPendenciasAuxiliar.Clone();
                    FilaPendenciasAuxiliar.Clear(); 
                }
            }
            ImprimeEstadoFinalGerenciadorMemoria();
        }


        /// <summary>
        /// Imprime o estado da memória e a requisição quando ocorre fragmentação externa
        /// </summary>
        /// <param name="requisicao">Requisição realizada à memória</param>
        private void ImprimeFragmentacaoExterna(Requisition requisicao)
        {
            Console.WriteLine("\n**************** Fragmentação externa **************\n");
            Memoria.ImprimeMemoria();
            Console.WriteLine("");
            FilaPendencias.PrintFila();
            Console.WriteLine("\n**************** Fragmentação externa **************\n");
        }

        /// <summary>
        /// Imprime o estado da memória e a requisição quando ocorre falta de memória
        /// </summary>
        /// <param name="requisicao">Requisição realizada à memória</param>
        private void ImprimeFaltaMemoria(Requisition requisicao)
        {
            Console.WriteLine("\n**************** Falta de memória **************\n");
            Memoria.ImprimeMemoria();
            Console.WriteLine("");
            FilaPendencias.PrintFila();
            Console.WriteLine("\n**************** Falta de memória **************\n");
        }

        /// <summary>
        /// Imprime o estado final do gerenciador de memória, bem como as requisições aguardando liberação
        /// </summary>
        private void ImprimeEstadoFinalGerenciadorMemoria()
        {
            Console.WriteLine("\n**************** Memória Atual **************\n");
            Memoria.ImprimeMemoria();
            Console.WriteLine("\n**************** Memória Atual **************\n");

            Console.WriteLine("\n**************** Aguardando Liberação **************\n");
            List<Requisition> listMemoria = new List<Requisition>();
            List<Requisition> listPendencia = new List<Requisition>();
            
            for(var i = 0; i < Memoria.Count(); i++)
            {
                listMemoria.Add(Memoria.GetIndex(i));
            }

            for (var i = 0; i < FilaPendencias.Count(); i++)
            {
                listPendencia.Add(FilaPendencias.GetIndex(i));
            }

            listPendencia.RemoveAll(itemP => listMemoria.Exists(itemM => itemM.numeroRequisicao == itemP.numeroRequisicao));

            FilaEncadeada filaPrint = new FilaEncadeada();
            
            foreach(var item in listPendencia)
            {
                filaPrint.Add(item);
            }
            filaPrint.PrintFila();
                
            Console.WriteLine("\n**************** Aguardando Liberação **************\n");
        }
    }
}
