using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorMemoria.Sources
{
    public class FilaEncadeada
    {
        private Node Head;

        public FilaEncadeada()
        {
            Head = null;
        }

        /// <summary>
        /// Inserção das requisições na memória seguindo o algoritmo First Fit.
        /// </summary>
        /// <param name="requisicao">Requisição</param>
        /// <returns>Boolean True se a requisição foi inserida com sucesso, False caso falhe.</returns>
        public bool AddFirstFit(Requisition requisicao)
        {
            Node node = new Node(requisicao);

            if(Count() == 1)
            {
                Node atual = Head;
                if(atual.Req.tipoRequisicao == 'I' && atual.Req.getQtdeMemoriaDisponivel() > requisicao.qtdeMemoriaRequisitada)
                {
                    node.Req.enderecoInicialBlocoMemoria = atual.Req.enderecoInicialBlocoMemoria;
                    node.Req.enderecoFinalBlocoMemoria = node.Req.enderecoInicialBlocoMemoria + node.Req.qtdeMemoriaRequisitada;
                    atual.Req.enderecoInicialBlocoMemoria = node.Req.enderecoFinalBlocoMemoria;
                    atual.Req.setQtdeMemoriaDisponivel();
                    node.Next = atual;
                    Head = node;
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                Node atual = Head;
                atual = Head.Next;
                Node anterior = Head;
                while (atual != null)
                {
                    if (anterior.Req.tipoRequisicao == 'I' && anterior.Req.getQtdeMemoriaDisponivel() >= requisicao.qtdeMemoriaRequisitada)
                    {
                        Node nodeAux = CriarNovoNo(anterior.Req);
                        SetRequisicaoUtilizada(anterior.Req, node.Req);
                        Node novoNode = CriarNovoNo(nodeAux.Req);
                        novoNode.Req.enderecoInicialBlocoMemoria = anterior.Req.enderecoFinalBlocoMemoria;
                        novoNode.Req.setQtdeMemoriaDisponivel();

                        novoNode.Next = anterior.Next;
                        anterior.Next = novoNode;
                        return true;
                    }
                    else if (atual.Next == null)
                    {
                        if (atual.Req.tipoRequisicao == 'I' && atual.Req.getQtdeMemoriaDisponivel() >= requisicao.qtdeMemoriaRequisitada)
                        {
                            Node nodeAux = CriarNovoNo(atual.Req);
                            SetRequisicaoUtilizada(atual.Req, node.Req);

                            node = CriarNovoNo(nodeAux.Req);
                            node.Req.enderecoInicialBlocoMemoria = atual.Req.enderecoFinalBlocoMemoria;
                            node.Req.setQtdeMemoriaDisponivel();
                            node.Next = null;

                            atual.Next = node;
                            return true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        atual = atual.Next;
                        anterior = anterior.Next;
                    }
                }
                return false;
            }

        }

        /// <summary>
        /// Remove uma requisição da memória, alocando os espaços vazios de acordo com a necessidade
        /// </summary>
        /// <param name="numeroRequisicao">Numero da requisição</param>
        public void Remove(int numeroRequisicao)
        {
            if (IsEmpty())
            {
            }
            else if (Count() == 1)
            {
                SetRequisicaoLivre(Head.Req);
            }
            else
            {
                Node atual = Head;
                if (atual.Next != null)
                {
                    atual = atual.Next;
                }
                Node anterior = Head;
                bool isAtual = false;
                bool isAnterior = false;
                bool isNextNull = false;
                while (true)
                {
                    if (atual.Req.numeroRequisicao == numeroRequisicao)
                    {
                        isAtual = true;
                        break;
                    }
                    else if (anterior.Req.numeroRequisicao == numeroRequisicao)
                    {
                        isAnterior = true;
                        break;
                    }
                    else if (atual.Next == null)
                    {
                        isNextNull = true;
                        break;
                    }
                    atual = atual.Next;
                    anterior = anterior.Next;
                }
                if (isNextNull)
                {
                    return;
                }

                if (isAnterior)
                {
                    anterior.Req.enderecoFinalBlocoMemoria = atual.Req.enderecoInicialBlocoMemoria;
                    SetRequisicaoLivre(anterior.Req);
                    UnirEspacosContiguos();
                }
                else
                {
                    if (atual.Next == null)
                    {
                        SetRequisicaoLivre(atual.Req);
                    }
                    else
                    {
                        if (anterior.Req.tipoRequisicao == 'S' && atual.Next.Req.tipoRequisicao == 'S')
                        {
                            SetRequisicaoLivre(atual.Req);
                        }
                        else if (anterior.Req.tipoRequisicao == 'S' && atual.Next.Req.tipoRequisicao == 'I')
                        {
                            atual.Next.Req.enderecoInicialBlocoMemoria = atual.Req.enderecoInicialBlocoMemoria;
                            atual.Req.numeroRequisicao = 0;
                            atual.Req.qtdeMemoriaRequisitada = 0;
                            atual.Req.setQtdeMemoriaDisponivel();
                            anterior.Next = atual.Next;
                            atual.Next = null;
                        }
                        else if (anterior.Req.tipoRequisicao == 'I' && atual.Next.Req.tipoRequisicao == 'S')
                        {
                            anterior.Req.enderecoFinalBlocoMemoria = atual.Req.enderecoFinalBlocoMemoria;
                            SetRequisicaoLivre(anterior.Req);
                            anterior.Next = atual.Next;
                            atual.Next = null;
                        }
                        else
                        {
                            anterior.Req.enderecoFinalBlocoMemoria = atual.Req.enderecoFinalBlocoMemoria;
                            SetRequisicaoLivre(anterior.Req);
                            anterior.Next = atual.Next;
                            atual.Next = null;
                            UnirEspacosContiguos();
                        }
                    }
                    RemoverBlocosZerados();
                }
            }
        }

        /// <summary>
        /// Percorre a fila e une os espaços vazios contíguos da memória 
        /// </summary>
        private void UnirEspacosContiguos()
        {
            Node atual = Head;
            if (atual.Next != null)
            {
                atual = atual.Next;
            }
            Node anterior = Head;


            while (atual != null)
            {
                if (anterior.Req.tipoRequisicao == 'I' && atual.Req.tipoRequisicao == 'I')
                {
                    while (anterior.Req.tipoRequisicao == 'I' && atual.Req.tipoRequisicao == 'I')
                    {
                        anterior.Req.enderecoFinalBlocoMemoria = atual.Req.enderecoFinalBlocoMemoria;
                        anterior.Req.setQtdeMemoriaDisponivel();
                        Node aux = new Node(atual.Req);
                        aux.Next = atual.Next;
                        anterior.Next = aux.Next;
                        atual.Next = null;
                        if (aux.Next == null)
                        {
                            break;
                        }
                        atual = aux.Next;
                        aux.Next = null;
                    }
                }
                atual = atual.Next;
                anterior = anterior.Next;
            }
        }

        /// <summary>
        /// Percorre a fila e remove os blocos com 0 de qtde de memória disponível; 
        /// </summary>
        private void RemoverBlocosZerados()
        {
            Node atual = Head;
            if (atual.Next != null)
            {
                atual = atual.Next;
            }
            Node anterior = Head;


            while (atual != null)
            {
                if (atual.Req.enderecoInicialBlocoMemoria == atual.Req.enderecoFinalBlocoMemoria && atual.Req.getQtdeMemoriaDisponivel() == 0)
                {
                    if (atual.Next != null)
                    {
                        Node aux = new Node(atual.Req);
                        aux.Next = atual.Next;
                        anterior.Next = atual.Next;
                        atual.Next = null;
                        if (aux.Next == null)
                        {
                            break;
                        }
                        atual = aux.Next;
                        aux.Next = null;
                    }
                    else
                    {
                        anterior.Next = atual.Next;
                        break;

                    }
                }
                atual = atual.Next;
                anterior = anterior.Next;
            }
        }

        /// <summary>
        /// Seta um determinado bloco de memória como utilizado
        /// </summary>
        /// <param name="reqAtual">Bloco de memória livre</param>
        /// <param name="reqNova">Requisição com os novos dados</param>
        private void SetRequisicaoUtilizada(Requisition reqAtual, Requisition reqNova)
        {
            reqAtual.enderecoFinalBlocoMemoria = reqAtual.enderecoInicialBlocoMemoria + reqNova.qtdeMemoriaRequisitada;
            reqAtual.numeroRequisicao = reqNova.numeroRequisicao;
            reqAtual.tipoRequisicao = 'S';
            reqAtual.setQtdeMemoriaDisponivel(0);
            reqAtual.qtdeMemoriaRequisitada = reqNova.qtdeMemoriaRequisitada;
        }

        /// <summary>
        /// Seta um determinado bloco de memória como livre
        /// </summary>
        /// <param name="req">Requisição</param>
        private void SetRequisicaoLivre(Requisition req)
        {
            req.tipoRequisicao = 'I';
            req.numeroRequisicao = 0;
            req.qtdeMemoriaRequisitada = 0;
            req.setQtdeMemoriaDisponivel();
        }

        /// <summary>
        /// Cria um novo nó a partir de uma requisição
        /// </summary>
        /// <param name="req">Requisição</param>
        /// <returns>Novo nó da fila encadeada</returns>
        private Node CriarNovoNo(Requisition req)
        {
            Requisition novaReq = new Requisition(req.numeroRequisicao, req.tipoRequisicao, req.numRequisicaoLiberar, req.qtdeMemoriaRequisitada, req.enderecoInicialBlocoMemoria, req.enderecoFinalBlocoMemoria);
            Node node = new Node(novaReq);
            return node;
        }

        /// <summary>
        /// Inserção das requisições no final da fila.
        /// </summary>
        /// <param name="requisicao">requisicao</param>
        public void Add(Requisition requisicao)
        {
            Node no = new Node(requisicao);
            if (Head == null)
            {
                Head = no;
                return;
            }

            Node atual = Head;
            while (atual.Next != null)
            {
                atual = atual.Next;
            }
            atual.Next = no;
        }

        /// <summary>
        /// Retorna a requisicao na posição index da fila.
        /// </summary>
        /// <param name="index">Posição da requisicao na fila.</param>
        /// <returns>Retorna a requisicao na posição index</returns>
        public Requisition GetIndex(int index)
        {
            if (IsEmpty())
            {
                return null;
            }
            else
            {
                Node atual = Head;
                for (int i = 0; i < Count(); i++)
                {
                    if (i == index)
                    {
                        return atual.Req;
                    }
                    else
                    {
                        atual = atual.Next;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Remove a requisição da fila que esteja na posição Index.
        /// </summary>
        /// <param name="index">Posição para remoção da requisição</param>
        public void RemoveIndex(int index)
        {
            if (IsEmpty())
            {

            }
            else if (Count() == 1)
            {
                Head = null;
            }
            else
            {
                Node atual = Head;
                Node anterior = Head;
                for (int i = 0; i < Count(); i++)
                {
                    if (i != index && atual.Next == null)
                    {
                        return;
                    }
                    else if (i != index && atual.Next != null)
                    {
                        anterior = atual;
                        atual = atual.Next;
                    }
                    else
                    {
                        if (atual.Next == null)
                        {
                            anterior.Next = null;
                        } else if (index == 0)
                        {
                            Head = anterior.Next;
                        }
                        else
                        {
                            anterior.Next = atual.Next;
                            atual.Next = null;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Retorna se a fila está vazia ou não.
        /// </summary>
        /// <returns>true, se a fila está vazia. false caso contrário/returns>
        public bool IsEmpty()
        {
            return Head == null;
        }

        /// <summary>
        /// Retorna o tamanho da fila.
        /// </summary>
        /// <returns>tamanho da fila</returns>
        public int Count()
        {
            return Size(Head);
        }

        /// <summary>
        /// Método recursivo para calcular o tamanho da fila.
        /// </summary>
        /// <param name="aux">requisicao</param>
        /// <returns>retorna o tamanho da fila.</returns>
        private int Size(Node aux)
        {
            if (aux == null)
                return 0;
            return 1 + Size(aux.Next);
        }

        /// <summary>
        /// Instancia uma nova fila e copia o conteúdo da fila original para a nova fila
        /// </summary>
        /// <returns>Nova fila com o conteúdo da fila original</returns>
        public FilaEncadeada Clone()
        {
            FilaEncadeada filaRetorno = new FilaEncadeada();
            for (int i = 0; i < Count(); i++)
            {
                filaRetorno.Add(GetIndex(i));
            }
            return filaRetorno;
        }

        /// <summary>
        /// Limpa a fila.
        /// </summary>
        public void Clear()
        {
            this.Head = null;
        }

        /// <summary>
        /// Imprime o estado da memória
        /// </summary>
        public void ImprimeMemoria()
        {
            Console.WriteLine("\n");
            Node no = Head;
            while (no != null)
            {
                Console.WriteLine("\t" + no.Req.ImprimeRequisicaoFormatoMemoria());
                no = no.Next;
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Imprime a fila
        /// </summary>
        public void PrintFila()
        {
            Console.WriteLine("\n");
            Node no = Head;
            while (no != null)
            {
                Console.WriteLine("\t" + no.Req.ToString());
                no = no.Next;
            }
            Console.WriteLine("\n");
        }

    }
}
