using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using static Cardapio;

public class MenuRestaurante : MenuBase
{
    Restaurante _restaurante;

    public MenuRestaurante(Restaurante restaurante)
    {
        _restaurante = restaurante;
    }

    //________________________________________

    public override void Menu_Inicio()
    {
        string NomeRestaurante = "Restaurante - Comidinhas Veganas";
        int option = -1;

        while (option != 0)
        {
            ResetConsole(NomeRestaurante);

            "Escolha uma opção:"._();
            _();
            "1 - Cadastrar Cliente"._();
            "2 - Encontrar Cliente"._();
            "3 - Consultar ocupações das mesas"._();
            "4 - Relatório Restaurante"._();
            "0 - Encerrar"._();
            _();

            option = ReadInt();

            switch (option)
            {
                case 0:
                    continue; // Encerra o loop

                case 1:
                    Menu_CadastrarCliente(NomeRestaurante);
                    break;

                case 2:
                    Menu_EncontrarCliente(NomeRestaurante);
                    break;

                case 3:
                    Menu_Ocupacoes(NomeRestaurante);
                    break;

                case 4:
                    _restaurante.RelatorioRestaurantes(true, true, true)._();
                    WaitForUserInput();
                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;
            }
        }
    }

    //________________________________________

    private void Menu_CadastrarCliente(string path)
    {
        path += "/ Cadastro de cliente";

        ResetConsole(path);

        "Digite o nome do cliente:"._();

        string nome = Read();

        Cliente cliente = _restaurante.GerarNovoCliente(nome);

        $"Cliente {nome} cadastrado."._();

        Menu_OpcoesCliente(path, ref cliente);
    }

    //________________________________________

    private void Menu_EncontrarCliente(string path)
    {
        path += "/ Cliente";

        if (_restaurante.GetTotalClientes() == 0)
        {
            "Nenhum cliente cadastrado."._();

            WaitForUserInput();

            return;
        }

        int IndiceVoltar = 0;
        int min = 0;
        int max = _restaurante.GetTotalClientes();
        string[] arrayDescricao = _restaurante.ArrayDescricaoClientes();

        ResetConsole(path);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < arrayDescricao.Length; i++)
        {
            if (i != 0)
                sb.Append("\n");
            sb.Append($"{(i + 1).ToString("D2")} - {arrayDescricao[i]}");
        }

        "Lista de Clientes:"._();
        sb.ToString()._();
        $"0 - Voltar"._();
        $"Escolha um cliente pelo índice, ou 0 para voltar: (min {min}, max {max})"._();

        int indiceOpcao = ReadIntRange(min, max);
        if (indiceOpcao == IndiceVoltar)
        {
            return;
        }

        Cliente cliente = _restaurante.GetClientePorIndice(indiceOpcao - 1);
        Menu_OpcoesCliente(path, ref cliente);
    }

    private void Menu_OpcoesCliente(string path, ref Cliente cliente)
    {
        int option = -1;

        while (option != 0)
        {
            ResetConsole(path);

            ("Cliente: " + cliente.GetNome())._();

            "Escolha uma opção:"._();
            _();
            "1 - Requisitar mesa"._();
            "2 - Relatório cliente"._();
            "0 - Voltar"._();
            _();

            option = ReadInt();

            switch (option)
            {

                case 0:
                    continue; // Encerra o loop

                case 1:
                    Menu_NovaRequisicao(path, ref cliente);
                    break;

                case 2:
                    _restaurante.RelatorioCliente(cliente, false)._();
                    WaitForUserInput();
                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;
            }
        }
    }

    //________________________________________

    private void Menu_NovaRequisicao(string path, ref Cliente cliente)
    {
        path += "/ Nova requisição";

        ResetConsole(path);

        int min = _restaurante.GetMinPessoasRequisicao();
        int max = _restaurante.GetMaiorCapacidadeMesas();

        $"Quantas pessoas estarão na mesa? (min {min}, Máx {max})"._();
        int qntPessoas = ReadIntRange(min, max);

        Requisicao requisicao = _restaurante.GerarNovaRequisicao(cliente, qntPessoas);

        "Requisição criada. Consulte a fila das mesas no menu principal para ocupar uma mesa."._();

        WaitForUserInput();
    }

    //________________________________________

    private void Menu_Ocupacoes(string path)
    {
        path += " / Mesas";

        int indexVoltar = 0;
        int indiceMax = _restaurante.GetTotalMesas();
        int indiceOpcao = -1;

        while (indiceOpcao != indexVoltar)
        {
            bool indiceMesaEValido = false;

            while (!indiceMesaEValido)
            {
                ResetConsole(path);

                _restaurante.TentarOcuparMesasVazias();
                $"0 - Lista de Mesas"._();
                _restaurante.ListaMesas()._();
                $"0 - Voltar"._();
                $"Escolha uma mesa ocupada pelo índice, ou 0 para voltar:"._();
                indiceOpcao = ReadIntRange(0, indiceMax);

                if (indiceOpcao == indexVoltar)
                {
                    break;
                }

                Mesa mesa = _restaurante.GetMesaPorIndice(indiceOpcao - 1);

                if (mesa.GetEstaOcupada())
                {
                    _restaurante.TryGetUltimaRequisicaoPorMesa(mesa, out Requisicao? requisicao);

                    if (requisicao != null)
                    {
                        Menu_OcupacaoAberta(path, ref requisicao);
                        indiceMesaEValido = true;
                    }
                }
                else
                {
                    "Mesa não está ocupada."._();
                    WaitForUserInput();
                }
            }
        }
    }

    private void Menu_OcupacaoAberta(string path, ref Requisicao requisicao)
    {
        path += "";

        int option = -1;

        while (option != 0)
        {
            ResetConsole(path);

            "Escolha uma opção:"._();
            _();
            "1 - Fazer pedido"._();
            "2 - Pedir conta e fechar ocupação"._();
            "3 - Relatório ocupação da mesa"._();
            "0 - Voltar"._();
            _();

            option = ReadInt();

            switch (option)
            {
                case 0:
                    continue; // Encerra o loop

                case 1:
                    Menu_OpcoesCardapio(path, ref requisicao);
                    break;

                case 3:
                    "// ----------"._();
                    _restaurante.RelatorioOcupacao(requisicao)._();
                    WaitForUserInput();
                    break;

                case 2:
                    Menu_FecharOcupacao(path, ref requisicao);
                    option = 0; // Encerra o loop
                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;
            }
        }
    }

    private void Menu_FecharOcupacao(string path, ref Requisicao requisicao)
    {
        requisicao.EncerrarOcupacao();
        "Ocupação encerrada"._();
        requisicao.getRelatorioConta()._();
        WaitForUserInput();
    }

    //________________________________________

    private void Menu_OpcoesCardapio(string path, ref Requisicao requisicao)
    {
        path += "/ Pedido";

        if (_restaurante.GetTotalItensCardapio() == 0)
        {
            "Nenhum item cadastrado no cardápio."._();

            WaitForUserInput();

            return;
        }

        int option = -1;

        while (option != 0)
        {

            ResetConsole(path);

            "Escolha uma opção:"._();
            _();
            "1 - Pedir prato"._();
            "2 - Pedir bebida"._();
            "0 - Voltar"._();
            _();

            option = ReadInt();

            switch (option)
            {

                case 0:
                    continue; // Encerra o loop

                case 1:
                    Menu_PedirPrato(path, ref requisicao);
                    break;

                case 2:
                    Menu_PedirBebida(path, ref requisicao);
                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;
            }
        }
    }

    private void Menu_PedirPrato(string path, ref Requisicao requisicao)
    {
        path += " / Pratos";

        if (_restaurante.GetTotalComidasCardapio() == 0)
        {
            "Nenhum item cadastrado no cardápio."._();

            WaitForUserInput();

            return;
        }

        int IndiceVoltar = 0;

        int min = 0;
        int max = _restaurante.GetTotalComidasCardapio();

        ResetConsole(path);

        "Lista de pratos:"._();
        _restaurante.RelatorioListaComidas()._();

        $"Escolha um prato pelo índice, ou {IndiceVoltar} para voltar: (min {min}, max {max})"._();
        int indiceOpcao = ReadIntRange(min, max);

        if (indiceOpcao == IndiceVoltar)
        {
            return;
        }

        Cardapio.Comida comida = _restaurante.GetComidaPorIndice(indiceOpcao - 1);

        $"Escolha quantos pratos {comida.GetNome()} (R$ {comida.GetPreco().ToString("F2")}) adicionar ao pedido"._();
        int quantidadeOpcao = ReadIntRange(0);

        requisicao.AddPedidoAConta($"Prato {comida.GetNome()} ({comida.GetPreco().Reais()}) x {quantidadeOpcao} = {(comida.GetPreco() * quantidadeOpcao).Reais()}", comida.GetPreco() * quantidadeOpcao);

        $"{quantidadeOpcao} prato(s) {comida.GetNome()} adicionado(s) ao pedido."._();

        WaitForUserInput();
    }

    private void Menu_PedirBebida(string path, ref Requisicao requisicao)
    {
        path += " / Bebidas";

        if (_restaurante.GetTotalBebidasCardapio() == 0)
        {
            "Nenhum item cadastrado no cardápio."._();

            WaitForUserInput();

            return;
        }

        int IndiceVoltar = 0;

        int min = 0;
        int max = _restaurante.GetTotalBebidasCardapio();

        ResetConsole(path);

        "Lista de bebidas:"._();
        _restaurante.RelatorioListaBebidas()._();

        $"Escolha um prato pelo índice, ou {IndiceVoltar} para voltar: (min {min}, max {max})"._();
        int indiceOpcao = ReadIntRange(min, max);


        if (indiceOpcao == IndiceVoltar)
        {
            return;
        }

        Cardapio.Bebida bebida = _restaurante.GetBebidaPorIndice(indiceOpcao - 1);

        $"Escolha quantas bebidas {bebida.GetNome()} ({bebida.GetPreco().Reais()}) adicionar ao pedido"._();
        int quantidadeOpcao = ReadIntRange(0);

        requisicao.AddPedidoAConta($"Bebida {bebida.GetNome()} ({bebida.GetPreco().Reais()}) x {quantidadeOpcao} = {(bebida.GetPreco() * quantidadeOpcao).Reais()}", bebida.GetPreco() * quantidadeOpcao);

        $"{quantidadeOpcao} bebida(s) {bebida.GetNome()} adicionada(s) ao pedido."._();

        WaitForUserInput();
    }

    //________________________________________

    #region Modelo de exemplo

    private void Menu_Model(string path)
    {
        path += "";
        int option = -1;

        while (option != 0)
        {
            ResetConsole(path);

            "Escolha uma opção:"._();
            _();
            "1 - "._();
            "2 - "._();
            "0 - Encerrar"._();
            _();

            option = ReadInt();

            switch (option)
            {

                case 0:
                    continue; // Encerra o loop

                case 1:

                    break;

                case 2:

                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;
            }
        }
    }

    #endregion

}
