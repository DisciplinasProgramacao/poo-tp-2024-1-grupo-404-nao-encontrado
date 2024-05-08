using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


using System.Runtime.Serialization;

public class Requisicao {


    #region Classes internas

    public class OcupacaoInvalidaException : InvalidOperationException {

        public OcupacaoInvalidaException(string message) : base(message) { }

    }


    public class Conta {

        List<string> _pedidos = new List<string>();

        float _valorTotal = 0;

        const float _TaxaServico = 0.1f;


        public void IncluirPedido(string PedidoDescr, float valor) {
            
            _pedidos.Add(PedidoDescr);

            _valorTotal += valor;

        }


        public string Relatorio() {
        
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.AppendLine("Pedidos: {");

            string spacing = "   ";

            foreach (string pedido in _pedidos) {

                strBuilder.AppendLine(spacing + pedido);
            }

            strBuilder.AppendLine("}");

            strBuilder.AppendLine("Valores:  {");

            strBuilder.AppendLine(spacing + "Custo de pedidos = " + _valorTotal.Reais());

            strBuilder.AppendLine(spacing + "Taxa de serviços = " + (_valorTotal * _TaxaServico).Reais());

            strBuilder.AppendLine(spacing + "Custo total = " + (_valorTotal * (1 + _TaxaServico)).Reais());

            strBuilder.AppendLine("}");

            return strBuilder.ToString();

        }


    }

    #endregion



    Cliente _cliente;
    
    int _qntPessoas;
    
    Mesa? _mesaOcupada;

    bool _estaOcupandoMesa = false;

    Conta _conta = new Conta();



    #region Requisicao

    public Requisicao(Cliente cliente, int qntPessoas) {
    
        _cliente = cliente;

        _qntPessoas = qntPessoas;

    }


    public bool AssociadoAoCliente(in Cliente conta) {

        return _cliente == conta;

    }


    public int getQntPessoas() {

        return _qntPessoas;

    }
    
    #endregion



    #region Mesa

    public void OcuparMesa(Mesa mesa) {

        if (_mesaOcupada != null) {
            throw new OcupacaoInvalidaException("Requisição já ocupou mesa.");
        }

        if (mesa.GetEstaOcupada()) {
            throw new OcupacaoInvalidaException("Mesa encontra-se ocupada.");
        }

        _mesaOcupada = mesa;

        mesa.SetEstaOcupada(true);

        _estaOcupandoMesa = true;

    }


    public void DesocuparMesa(Mesa mesa) {

        if (_mesaOcupada != null) {
            throw new OcupacaoInvalidaException("Requisição já ocupou mesa.");
        }

        if (mesa.GetEstaOcupada()) {
            throw new OcupacaoInvalidaException("Mesa encontra-se ocupada.");
        }

        _mesaOcupada = mesa;

        mesa.SetEstaOcupada(true);

        _estaOcupandoMesa = false;

    }


    public bool EstaOcupandoMesa() {

        return _estaOcupandoMesa;

    }


    public bool OcupouUmaMesa() {

        return _mesaOcupada != null;

    }


    public bool OcupouAMesa(Mesa mesa) {

        return _mesaOcupada == mesa;

    }


    public bool EstaAguardandoMesa() {
        return _mesaOcupada == null;
    }

    public Mesa GetMesa() {

        return _mesaOcupada;


    }

    #endregion



    #region Ocupação

    public void AddPedidoAConta(string PedidoDescr, float valor) {

        _conta.IncluirPedido(PedidoDescr, valor);

    }


    public string getRelatorioConta() {

        return _conta.Relatorio();

    }


    public void EncerrarOcupacao() {

        if (!_estaOcupandoMesa) {
            throw new OcupacaoInvalidaException("Não ocupa mesa.");
        }

        _mesaOcupada?.SetEstaOcupada(false);

        _estaOcupandoMesa = false;

    }

    #endregion


    public string Relatorio() {

        return $"Requisição de {_cliente.GetNome()} para {_qntPessoas}.";

    }


}
