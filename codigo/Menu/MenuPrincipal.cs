

#define DadosTeste

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MenuPrincipal : MenuBase
{
    public override void Menu_Inicio()
    {
        Restaurante _restaurante = new Restaurante();
        // 10 mesas adicionadas;
        _restaurante
            .NovasMesas(4, 4)
            .NovasMesas(4, 6)
            .NovasMesas(2, 8);

        _restaurante.NovoCardapio(
            new Cardapio()
                .AddComida("Moqueca de Palmito", 32.00f)
                .AddComida("Falafel Assado", 20.00f)
                .AddComida("Salada Primavera com Macarrão Konjac", 25.00f)
                .AddComida("Escondidinho de Inhame", 18.00f)
                .AddComida("Strogonoff de Cogumelos", 35.00f)
                .AddComida("Caçarola de legumes", 22.00f)

                .AddBebida("Água", 3.00f)
                .AddBebida("Copo de suco", 7.00f)
                .AddBebida("Refrigerante orgânico", 7.00f)
                .AddBebida("Cerveja vegana", 9.00f)
                .AddBebida("Taça de vinho vegano", 18.00f)
        );


#if DadosTeste // Comentar ou descomentar o #define na linha 1 do código
        DadosTesteRestaurante(_restaurante);
#endif


        MenuRestaurante menuRestaurante = new MenuRestaurante(_restaurante);

        //________________________________________

        Cafe cafe = new Cafe();

        cafe.NovoCardapio(
            new Cardapio()
                .AddComida("Não de queijo", 5.00f) // Tava não de queijo no doc do trabalho
                .AddComida("Bolinha de cogumelo", 7.00f)
                .AddComida("Rissole de palmito", 7.00f)
                .AddComida("Coxinha de carne de jaca", 8.00f)
                .AddComida("Fatia de queijo de caju", 9.00f)
                .AddComida("Biscoito amanteigado", 3.00f)
                .AddComida("Cheesecake de frutas vermelhas", 15.00f)

                .AddBebida("Água", 3.00f)
                .AddBebida("Copo de suco", 7.00f)
                .AddBebida("Café espresso orgânico", 6.00f)
        );


#if DadosTeste // Comentar ou descomentar o #define na linha 1 do código
        DadosTesteCafe(cafe);
#endif


        MenuCafe menuCafe = new MenuCafe(cafe);

        //________________________________________

        string path = "";

        int option = -1;

        while (option != 0)
        {

            ResetConsole(path);

            "Escolha uma opção:"._();
            _();
            "1 - Restaurante"._();
            "2 - Café"._();
            "0 - Encerrar"._();
            _();

            option = ReadInt();

            switch (option)
            {

                case 0:
                    continue;

                case 1:
                    menuRestaurante.Menu_Inicio();
                    break;

                case 2:
                    menuCafe.Menu_Inicio();
                    break;

                default:
                    "Opção inválida."._();
                    WaitForUserInput();
                    break;

            }
        }

        //________________________________________

        // Fim do programa

        ResetConsole("");

        "Fim - Obrigado."._();
        WaitForUserInput();
    }

    static void DadosTesteRestaurante(Restaurante restaurante)
    {
        void NovosDados(string nome, int pessoasRequisicao)
        {
            Cliente novoCliente = restaurante.GerarNovoCliente(nome);
            var requisicao = restaurante.GerarNovaRequisicao(novoCliente, pessoasRequisicao);

            Cardapio.Comida comida_1 = restaurante.GetComidaPorIndice(1);
            Cardapio.Comida comida_2 = restaurante.GetComidaPorIndice(2);
            Cardapio.Comida comida_3 = restaurante.GetComidaPorIndice(3);

            void AddComida(Cardapio.Comida comida, int qnt)
            {
                requisicao.AddPedidoAConta($"Prato {comida.GetNome()} ({comida.GetPreco().Reais()}) x {qnt} = {(comida.GetPreco() * qnt).Reais()}", comida.GetPreco() * qnt);
            }

            AddComida(comida_1, 1);
            AddComida(comida_2, 2);
            AddComida(comida_3, 2);

            Cardapio.Bebida bebida_1 = restaurante.GetBebidaPorIndice(1);
            Cardapio.Bebida bebida_2 = restaurante.GetBebidaPorIndice(2);
            Cardapio.Bebida bebida_3 = restaurante.GetBebidaPorIndice(3);

            void AddBebida(Cardapio.Bebida bebida, int qnt)
            {
                requisicao.AddPedidoAConta($"Bebida {bebida.GetNome()} ({bebida.GetPreco().Reais()}) x {qnt} = {(bebida.GetPreco() * qnt).Reais()}", bebida.GetPreco() * qnt);
            }

            AddBebida(bebida_1, 1);
            AddBebida(bebida_2, 2);
            AddBebida(bebida_3, 3);
        }

        NovosDados("Zeca Pagodinho", 3);
        NovosDados("Beth Carvalho", 4);
        NovosDados("Arlindo Cruz", 5);
        NovosDados("Alcione", 6);
        NovosDados("Diogo Nogueira", 7);

        void DesocuparMesa(int indexmesa)
        {
            var mesa = restaurante.GetMesaPorIndice(indexmesa);

            if (mesa.GetEstaOcupada() && restaurante.TryGetUltimaRequisicaoPorMesa(mesa, out Requisicao? requisicao))
            {
                requisicao?.EncerrarOcupacao();
            }
        }

        DesocuparMesa(1);
        DesocuparMesa(3);
    }

    static void DadosTesteCafe(Cafe cafe)
    {
        void NovosDados(string nome)
        {
            Cliente novoCliente = cafe.GerarNovoCliente(nome);

            Cardapio.Comida comida_1 = cafe.GetComidaPorIndice(1);
            Cardapio.Comida comida_2 = cafe.GetComidaPorIndice(2);
            Cardapio.Comida comida_3 = cafe.GetComidaPorIndice(3);

            void AddComida(Cardapio.Comida comida, int qnt)
            {
                cafe.AddPedidoAConta(novoCliente, $"Prato {comida.GetNome()} ({comida.GetPreco().Reais()}) x {qnt} = {(comida.GetPreco() * qnt).Reais()}", comida.GetPreco() * qnt);
            }

            AddComida(comida_1, 0);
            AddComida(comida_2, 1);
            AddComida(comida_3, 2);

            Cardapio.Bebida bebida_1 = cafe.GetBebidaPorIndice(0);
            Cardapio.Bebida bebida_2 = cafe.GetBebidaPorIndice(1);
            Cardapio.Bebida bebida_3 = cafe.GetBebidaPorIndice(2);

            void AddBebida(Cardapio.Bebida bebida, int qnt)
            {
                cafe.AddPedidoAConta(novoCliente, $"Bebida {bebida.GetNome()} ({bebida.GetPreco().Reais()}) x {qnt} = {(bebida.GetPreco() * qnt).Reais()}", bebida.GetPreco() * qnt);
            }

            AddBebida(bebida_1, 1);
            AddBebida(bebida_2, 2);
            AddBebida(bebida_3, 3);
        }

        NovosDados("Zeca Pagodinho");
        NovosDados("Beth Carvalho");
        NovosDados("Arlindo Cruz");
        NovosDados("Alcione");
        NovosDados("Diogo Nogueira");
    }
}
