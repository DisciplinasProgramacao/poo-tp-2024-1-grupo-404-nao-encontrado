using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Requisicao;

public class Restaurante {

    List<Mesa> _listaMesas = new List<Mesa>();

    int _maiorCapacidadeMesas = 0;


    List<Cliente> _listaClientes = new List<Cliente>();

    const int _MinPessoasRequisicao = 1;


    List<Requisicao> _listaRequisicoesSemMesa = new List<Requisicao>();

    List<Requisicao> _listaRequisicoesComMesa = new List<Requisicao>();


    Cardapio _cardapio = new Cardapio();



    #region Mesas

    public Restaurante NovasMesas(int qntNovasMesas, int capacidadePessoas) {

        if (capacidadePessoas < _MinPessoasRequisicao) {
            throw new ArgumentOutOfRangeException($"Capacidade deve ser ao menos {_MinPessoasRequisicao}");
        }

        Mesa[] novasMesas = new Mesa[qntNovasMesas];

        for (int i = 0; i < qntNovasMesas; i++) {

            Mesa mesa = new Mesa(capacidadePessoas);

            novasMesas[i] = mesa;
            _listaMesas.Add(mesa);

        }

        if (_maiorCapacidadeMesas < capacidadePessoas) {
            _maiorCapacidadeMesas = capacidadePessoas;
        }

        return this;

    }


    public int GetTotalMesas() {

        return _listaMesas.Count;

    }


    public int GetMaiorCapacidadeMesas() {

        return _maiorCapacidadeMesas;

    }


    public string DescricaoMesa(Mesa mesa) {

        string descricao = "";

        descricao += $"(cap: {mesa.GetCapacidade()}) ";

        descricao += mesa.DescricaoOcupacao();

        if (mesa.GetEstaOcupada()) {
            TryGetUltimaRequisicaoPorMesa(mesa, out Requisicao? requisicao);

            descricao += " por " + requisicao?.Relatorio();
        }

        return descricao;

    }


    public string[] arrayDescricaoMesas() {

        string[] arrayDescricao = new string[_listaMesas.Count];

        for (int i = 0; i < _listaMesas.Count; i++) {

            arrayDescricao[i] += DescricaoMesa(_listaMesas[i]);

        }

        return arrayDescricao;

    }


    public Mesa GetMesaPorIndice(int index) {

        return _listaMesas[index];

    }

    #endregion



    #region Clientes

    public Cliente GerarNovoCliente(string nome) {

        Cliente cliente = new Cliente(nome);
        _listaClientes.Add(cliente);
        return cliente;

    }


    public int GetTotalClientes() {
        return _listaClientes.Count;
    }


    public Cliente GetClientePorIndice(int indice) {

        return _listaClientes[indice];

    }


    public string[] ArrayDescricaoClientes() {

        string[] arrayDescricao = new string[_listaClientes.Count];

        for (int i = 0; i < _listaClientes.Count; i++) {

            arrayDescricao[i] += _listaClientes[i].GetNome();
        }

        return arrayDescricao;

    }


    public string RelatorioListaClientes() {

        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < _listaClientes.Count; i++) {

            sBuilder.Append($"{i} - {_listaClientes[i].GetNome()}\n");

        }

        return sBuilder.ToString();

    }


    #endregion



    #region Requisições

    public int GetMinPessoasRequisicao() {
        return _MinPessoasRequisicao;
    }


    public Requisicao GerarNovaRequisicao(Cliente cliente, int qntPessoas) {

        if (qntPessoas < _MinPessoasRequisicao) {
            throw new ArgumentException($"Requisições devem ter ao menos {_MinPessoasRequisicao} pessoas.");
        }

        if (qntPessoas > _maiorCapacidadeMesas) {
            throw new ArgumentException($"Requisições devem ter no máximo {_maiorCapacidadeMesas} pessoas.");
        }

        Requisicao requisicao = new Requisicao(cliente, qntPessoas);

        TentarOcuparMesa(requisicao);

        return requisicao;

    }


    public bool TryGetUltimaRequisicaoPorMesa(Mesa mesa, out Requisicao? requisicao) {
    
        requisicao = null;

        for (int i = (_listaRequisicoesComMesa.Count - 1); i >= 0; i--) {

            if (_listaRequisicoesComMesa[i].OcupouAMesa(mesa)) {
                requisicao = _listaRequisicoesComMesa[i];
                break;
            }

        }

        return requisicao != null;

    }


    public void TentarOcuparMesaVazia(Mesa mesa) {

        if (mesa.GetEstaOcupada()) {

            throw new ArgumentException("Mesa está ocupada.");
        
        }

        Requisicao[] reqValidas = _listaRequisicoesSemMesa.Where((req) => (req.getQntPessoas() <= mesa.GetCapacidade())).ToArray();

        if (reqValidas.Length > 0) {
            var selecionada = reqValidas[0];

            selecionada.OcuparMesa(mesa);

            _listaRequisicoesSemMesa.Remove(selecionada);
            _listaRequisicoesComMesa.Add(selecionada);
        }

    }


    public void TentarOcuparMesasVazias() {

        Mesa[] mesasVazias = _listaMesas.Where((mesa) => (!mesa.GetEstaOcupada())).ToArray();

        foreach (Mesa mesa in mesasVazias) {

            TentarOcuparMesaVazia(mesa);

        }

    }


    public void TentarOcuparMesa(Requisicao requisicao) {

        Mesa[] mesasVazias = _listaMesas.Where((mesa) => (!mesa.GetEstaOcupada() && (requisicao.getQntPessoas() <= mesa.GetCapacidade()))).ToArray();

        if (mesasVazias.Length > 0) {

            requisicao.OcuparMesa(mesasVazias[0]);

            _listaRequisicoesComMesa.Add(requisicao);

        }
        else {

            _listaRequisicoesSemMesa.Add(requisicao);

        }

    }

    #endregion



    #region Cardapio

    public void NovoCardapio(in Cardapio cardapio) {

        _cardapio = cardapio;

    }


    public int GetTotalItensCardapio() {

        return _cardapio.getTotalItens();

    }


    public int GetTotalComidasCardapio() {

        return _cardapio.getTotalComidas();

    }


    public int GetTotalBebidasCardapio() {

        return _cardapio.getTotalBebidas();

    }


    public string RelatorioListaComidas() {

        return _cardapio.RelatorioListaComidas();

    }

    
    public string RelatorioListaBebidas() {

        return _cardapio.RelatorioListaBebidas();

    }


    public Cardapio.Comida GetComidaPorIndice(int indice) {

        return _cardapio.GetComidaPorIndice(indice);

    }


    public Cardapio.Bebida GetBebidaPorIndice(int indice) {

        return _cardapio.GetBebidaPorIndice(indice);

    }


    #endregion



    #region Conta

    public void AddPedidoAConta(Requisicao requisicao,string PedidoDescr, float valor) {

        requisicao.AddPedidoAConta(PedidoDescr, valor);

    }


    public string getRelatorioConta(Requisicao requisicao) {

        return requisicao.getRelatorioConta();

    }

    #endregion



    #region Relatorios


    public string ListaMesas() {

        string[] arrayDescricao = arrayDescricaoMesas();

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < arrayDescricao.Length; i++) {
            if (i != 0)
                sb.Append("\n");
            sb.Append($"{(i + 1).ToString("D2")} - {arrayDescricao[i]}");
        }

        return sb.ToString();
    }


    public string RelatorioCliente(Cliente cliente, bool imprimirContas) {
        
        StringBuilder sb = new StringBuilder();

        string spacing_1 = "   ";
        string spacing_2 = spacing_1 + spacing_1;

        Requisicao[] reqs;

        sb.AppendLine($"Cliente {cliente.GetNome()}:");

        reqs = _listaRequisicoesComMesa.Where(r => (r.AssociadoAoCliente(cliente))).ToArray();

        foreach (Requisicao req in reqs) {

            string estado = req.EstaOcupandoMesa() ? "(ocupando mesa)" : "(fechada)";

            sb.AppendLine(spacing_1 + $"Requisição para {req.getQntPessoas()}  {estado}.\n");

        }

        reqs = _listaRequisicoesSemMesa.Where(r => (r.AssociadoAoCliente(cliente))).ToArray();

        foreach (Requisicao req in reqs) {

            sb.AppendLine(spacing_2 + $"Requisição para {req.getQntPessoas()} (aguardando mesa).\n");

        }


        return sb.ToString();

    }


    public string RelatorioOcupacao(Requisicao req) {

        StringBuilder strBuilder = new StringBuilder();

        strBuilder.AppendLine("Relatório de ocupação:\n");

        strBuilder.AppendLine("Mesa " + DescricaoMesa(req.GetMesa()));

        strBuilder.AppendLine("\n" + "Conta em aberto:\n");

        strBuilder.AppendLine(req.getRelatorioConta());

        return strBuilder.ToString();

    }


    public string RelatorioRestaurantes(bool imprimirMesas, bool imprimirClientes, bool imprimirContas) {


        StringBuilder strBuilder = new StringBuilder($"Relatório restaurante:\n");

        strBuilder.AppendLine();

        if (imprimirMesas) {

            strBuilder.AppendLine("// --------\n");
            strBuilder.AppendLine("Relatório mesas:");
            strBuilder.AppendLine();
            strBuilder.AppendLine(ListaMesas());

        }

        if (imprimirMesas && imprimirClientes) {

            strBuilder.AppendLine();
            strBuilder.AppendLine();

        }

        if (imprimirClientes) {

            strBuilder.AppendLine("// --------\n");
            strBuilder.AppendLine("Relatório Clientes:");
            strBuilder.AppendLine();

            foreach (var cliente in _listaClientes) {

                strBuilder.AppendLine(RelatorioCliente(cliente, imprimirContas));

            }

        }

        strBuilder.AppendLine("-- Fim do relatório.");

        return strBuilder.ToString();


    }

    #endregion



}