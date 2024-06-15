using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MenuCafe : MenuBase
{
    Cafe _cafe;
    
    public MenuCafe(Cafe cafe)
    {
        _cafe = cafe;
    }

    public override void Menu_Inicio()
    {

        string path = "Café";

        int option = -1;

        while (option != 0)
        {

            ResetConsole(path);

            "Escolha uma opção:"._();
            _();
            "1 - Cadastrar Cliente"._();
            "2 - Encontrar Cliente"._();
            "0 - Encerrar"._();
            _();

            option = ReadInt();

            switch (option)
            {

                case 0:
                    continue;

                case 1:
                    Menu_CadastrarCliente(path);
                    break;

                case 2:
                    Menu_EncontrarCliente(path);
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

        Cliente cliente = _cafe.GerarNovoCliente(nome);

        $"Cliente {nome} cadastrado."._();

        Menu_Opcoes(path, ref cliente);

    }

    private void Menu_EncontrarCliente(string path)
    {
        path += "/ Cliente";

        if (_cafe.GetTotalClientes() == 0)
        {
            "Nenhum cliente cadastrado."._();

            WaitForUserInput();

            return;
        }

        int IndiceVoltar = 0;

        int min = 0;
        int max = _cafe.GetTotalClientes();

        string[] arrayDescricao = _cafe.ArrayDescricaoClientes();

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

        Cliente cliente = _cafe.GetClientePorIndice(indiceOpcao - 1);
        Menu_Opcoes(path, ref cliente);
    }


    //________________________________________

    private void Menu_Opcoes(string path, ref Cliente cliente)
    {
        path += " / Opções";

        int option = -1;

        while (option != 0)
        {
            ResetConsole(path);

            "Escolha uma opção:"._();
            _();
            "1 - Fazer pedido"._();
            "2 - Pedir conta e fechar pedidos"._();
            "3 - Relatório do cliente"._();
            "0 - Voltar"._();
            _();

            option = ReadInt();

            switch (option)
            {
                case 0:
                    continue; // Encerra o loop

                case 1:
                    Menu_OpcoesCardapio(path, ref cliente);
                    break;

                case 2:
                    Menu_FecharPedidos(path, ref cliente);
                    option = 0; // Encerra o loop
                    break;

                case 3:
                    "// ----------"._();
                    _cafe.RelatorioCliente(cliente, imprimirContas: true)._();
                    WaitForUserInput();
                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;
            }
        }
    }

    private void Menu_FecharPedidos(string path, ref Cliente cliente)
    {
        if (_cafe.HaOcupacoesPara(ref cliente))
        {
            _cafe.EncerrarOcupacao(cliente);
            "Conta (e ocupação) encerrada"._();
            _cafe.getRelatorioConta(cliente)._();
        }
        else
        {
            "Não houveram pedidos deste cliente"._();
        }

        WaitForUserInput();
    }

    //________________________________________

    private void Menu_OpcoesCardapio(string path, ref Cliente cliente)
    {
        path += "/ Pedido";

        if (_cafe.GetTotalItensCardapio() == 0)
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
                    Menu_PedirPrato(path, ref cliente);
                    break;

                case 2:
                    Menu_PedirBebida(path, ref cliente);
                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;
            }
        }
    }

    private void Menu_PedirPrato(string path, ref Cliente cliente)
    {
        path += " / Pratos";

        if (_cafe.GetTotalComidasCardapio() == 0)
        {
            "Nenhum item cadastrado no cardápio."._();

            WaitForUserInput();

            return;
        }

        int IndiceVoltar = _cafe.GetTotalComidasCardapio();

        int min = 0;
        int max = _cafe.GetTotalComidasCardapio();

        ResetConsole(path);

        "Lista de pratos:"._();
        _cafe.RelatorioListaComidas()._();

        $"Escolha um prato pelo índice, ou {IndiceVoltar} para voltar: (min {min}, max {max})"._();
        int indiceOpcao = ReadIntRange(min, max);

        if (indiceOpcao == IndiceVoltar)
        {
            return;
        }

        Cardapio.Comida comida = _cafe.GetComidaPorIndice(indiceOpcao - 1);

        $"Escolha quantos pratos {comida.GetNome()} (R$ {comida.GetPreco().ToString("F2")}) adicionar ao pedido"._();
        int quantidadeOpcao = ReadIntRange(0);

        _cafe.AddPedidoAConta(cliente, $"Prato {comida.GetNome()} ({comida.GetPreco().Reais()}) x {quantidadeOpcao} = {(comida.GetPreco() * quantidadeOpcao).Reais()}", comida.GetPreco() * quantidadeOpcao);

        $"{quantidadeOpcao} prato(s) {comida.GetNome()} adicionado(s) ao pedido."._();

        WaitForUserInput();
    }

    private void Menu_PedirBebida(string path, ref Cliente cliente)
    {
        path += " / Bebidas";

        if (_cafe.GetTotalBebidasCardapio() == 0)
        {
            "Nenhum item cadastrado no cardápio."._();

            WaitForUserInput();

            return;
        }

        int IndiceVoltar = _cafe.GetTotalBebidasCardapio();

        int min = 0;
        int max = _cafe.GetTotalBebidasCardapio();

        ResetConsole(path);

        "Lista de bebidas:"._();
        _cafe.RelatorioListaBebidas()._();

        $"Escolha um prato pelo índice, ou {IndiceVoltar} para voltar: (min {min}, max {max})"._();
        int indiceOpcao = ReadIntRange(min, max);


        if (indiceOpcao == IndiceVoltar)
        {
            return;
        }

        Cardapio.Bebida bebida = _cafe.GetBebidaPorIndice(indiceOpcao - 1);

        $"Escolha quantas bebidas {bebida.GetNome()} ({bebida.GetPreco().Reais()}) adicionar ao pedido"._();
        int quantidadeOpcao = ReadIntRange(0);

        _cafe.AddPedidoAConta(cliente, $"Bebida {bebida.GetNome()} ({bebida.GetPreco().Reais()}) x {quantidadeOpcao} = {(bebida.GetPreco() * quantidadeOpcao).Reais()}", bebida.GetPreco() * quantidadeOpcao);

        $"{quantidadeOpcao} bebida(s) {bebida.GetNome()} adicionada(s) ao pedido."._();

        WaitForUserInput();
    }

    //________________________________________
}
