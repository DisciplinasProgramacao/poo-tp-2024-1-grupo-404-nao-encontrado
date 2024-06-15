﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cafe
{
    List<Cliente> _listaClientes = new List<Cliente>();

    Cardapio _cardapio = new Cardapio();

    Dictionary<Cliente, List<Ocupacao>> OcupacaoPorCliente = new Dictionary<Cliente, List<Ocupacao>>();

    //________________________________________

    public Cliente GerarNovoCliente(string nome)
    {
        Cliente cliente = new Cliente(nome);
        _listaClientes.Add(cliente);

        OcupacaoPorCliente.Add(cliente, new List<Ocupacao>());

        return cliente;
    }

    public IEnumerable<Cliente> EnctorarClientesComNome(string nome)
    {
        return  _listaClientes.Where(c => (c.GetNome() == nome));
    }
    
    public bool TentarApagarCliente(Cliente cliente)
    {
        if (!_listaClientes.Contains(cliente))
        {
            // Client doesn't exist on system
            
            return false;
        }

        _listaClientes.Remove(cliente);
        OcupacaoPorCliente.Remove(cliente);

        return true;
    }

    public int GetTotalClientes()
    {
        return _listaClientes.Count();
    }

    public Cliente GetClientePorIndice(int indice)
    {
        return _listaClientes[indice];
    }

    public string RelatorioCliente(Cliente cliente, bool imprimirContas)
    {
        StringBuilder sb = new StringBuilder();

        string spacing_1 = "   ";
        string spacing_2 = spacing_1 + spacing_1;

        sb.AppendLine($"Cliente {cliente.GetNome()}");

        if (imprimirContas)
        {
            List<Ocupacao> ocupacao = OcupacaoPorCliente[cliente];

            for (int i = 0; i < ocupacao.Count; i++)
            {
                string estado = (ocupacao[i].Fechado) ? "Fechada" : "Aberta";

                sb.AppendLine(spacing_1 + $"Conta {i + 1} ({estado})");

                sb.AppendLine(ocupacao[i].Conta.Relatorio(spacing_2));
            }
        }

        return sb.ToString();
    }

    public string[] ArrayDescricaoClientes()
    {
        string[] arrayDescricao = new string[_listaClientes.Count];

        for (int i = 0; i < _listaClientes.Count; i++)
        {
            arrayDescricao[i] += _listaClientes[i].GetNome();
        }

        return arrayDescricao;
    }

    //________________________________________

    internal void NovoCardapio(Cardapio cardapio)
    {
        _cardapio = cardapio;
    }

    public int GetTotalItensCardapio()
    {
        return _cardapio.getTotalItens();
    }

    public int GetTotalComidasCardapio()
    {
        return _cardapio.getTotalComidas();
    }

    public int GetTotalBebidasCardapio()
    {
        return _cardapio.getTotalBebidas();
    }

    public string RelatorioListaComidas()
    {
        return _cardapio.RelatorioListaComidas();
    }

    public string RelatorioListaBebidas()
    {
        return _cardapio.RelatorioListaBebidas();
    }

    public Cardapio.Comida GetComidaPorIndice(int indice)
    {
        return _cardapio.GetComidaPorIndice(indice);
    }

    public Cardapio.Bebida GetBebidaPorIndice(int indice)
    {
        return _cardapio.GetBebidaPorIndice(indice);
    }

    //________________________________________

    public bool HaOcupacoesPara(ref Cliente cliente)
    {
        return OcupacaoPorCliente[cliente].LastOrDefault() != null;
    }

    public void AddPedidoAConta(Cliente cliente, string PedidoDescr, float valor)
    {
        Ocupacao ultimaOcupacao = OcupacaoPorCliente[cliente].LastOrDefault();

        if (ultimaOcupacao ==  null || ultimaOcupacao.Fechado)
        {
            ultimaOcupacao = new Ocupacao();

            OcupacaoPorCliente[cliente].Add(ultimaOcupacao);
        }

        ultimaOcupacao.Conta.IncluirPedido(PedidoDescr, valor);
    }

    public void EncerrarOcupacao(Cliente cliente)
    {
        OcupacaoPorCliente[cliente].LastOrDefault().Fechado = true;
    }

    public string getRelatorioConta(Cliente cliente)
    {
        return OcupacaoPorCliente[cliente].Last().Conta.Relatorio();
    }

}
