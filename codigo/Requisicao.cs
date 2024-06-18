using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

public class Requisicao
{
    public class OcupacaoInvalidaException : InvalidOperationException
    {
        public OcupacaoInvalidaException(string message) : base(message) { }
    }

    //________________________________________

    Cliente _cliente;
    int _qntPessoas;
    Mesa? _mesaOcupada;
    bool _estaOcupandoMesa = false;
    Conta _conta = new Conta();

    //________________________________________

    public Requisicao(Cliente cliente, int qntPessoas)
    {
        _cliente = cliente;
        _qntPessoas = qntPessoas;
    }

    public bool AssociadoAoCliente(in Cliente conta)
    {
        return _cliente == conta;
    }

    public int getQntPessoas()
    {
        return _qntPessoas;
    }

    //________________________________________

    public void OcuparMesa(Mesa mesa)
    {
        if (_mesaOcupada != null)
        {
            throw new OcupacaoInvalidaException("Requisição já ocupou mesa.");
        }

        if (mesa.GetEstaOcupada())
        {
            throw new OcupacaoInvalidaException("Mesa encontra-se ocupada.");
        }

        _mesaOcupada = mesa;
        mesa.SetEstaOcupada(true);
        _estaOcupandoMesa = true;
    }

    public void DesocuparMesa(Mesa mesa)
    {
        if (_mesaOcupada != null)
        {
            throw new OcupacaoInvalidaException("Requisição já ocupou mesa.");
        }

        if (mesa.GetEstaOcupada())
        {
            throw new OcupacaoInvalidaException("Mesa encontra-se ocupada.");
        }

        _mesaOcupada = mesa;
        mesa.SetEstaOcupada(true);
        _estaOcupandoMesa = false;
    }

    public bool EstaOcupandoMesa()
    {
        return _estaOcupandoMesa;
    }

    public bool OcupouUmaMesa()
    {
        return _mesaOcupada != null;
    }

    public bool OcupouAMesa(Mesa mesa)
    {
        return _mesaOcupada == mesa;
    }

    public bool EstaAguardandoMesa()
    {
        return _mesaOcupada == null;
    }

    public Mesa GetMesa()
    {
        return _mesaOcupada;
    }

    //________________________________________

    public void AddPedidoAConta(string PedidoDescr, float valor)
    {
        _conta.IncluirPedido(PedidoDescr, valor);
    }

    public string getRelatorioConta()
    {
        return _conta.Relatorio();
    }

    public void EncerrarOcupacao()
    {
        if (!_estaOcupandoMesa)
        {
            throw new OcupacaoInvalidaException("Não ocupa mesa.");
        }

        _mesaOcupada?.SetEstaOcupada(false);
        _estaOcupandoMesa = false;
    }

    //________________________________________

    public string Relatorio()
    {
        return $"Requisição de {_cliente.GetNome()} para {_qntPessoas}.";
    }

}
