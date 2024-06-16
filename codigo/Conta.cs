using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Conta
{
    List<string> _pedidos = new List<string>();
    float _valorTotal = 0;
    const float _TaxaServico = 0.1f;

    public void IncluirPedido(string PedidoDescr, float valor)
    {
        _pedidos.Add(PedidoDescr);
        _valorTotal += valor;
    }

    public string Relatorio(string padding = "")
    {
        StringBuilder strBuilder = new StringBuilder();

        strBuilder.AppendLine(padding + "Pedidos: {");

        string spacing = "   ";

        foreach (string pedido in _pedidos)
        {
            strBuilder.AppendLine(padding + spacing + pedido);
        }

        strBuilder.AppendLine(padding + "}");
        strBuilder.AppendLine(padding + "Valores:  {");
        strBuilder.AppendLine(padding + spacing + "Custo de pedidos = " + _valorTotal.Reais());
        strBuilder.AppendLine(padding + spacing + "Taxa de serviços = " + (_valorTotal * _TaxaServico).Reais());
        strBuilder.AppendLine(padding + spacing + "Custo total = " + (_valorTotal * (1 + _TaxaServico)).Reais());
        strBuilder.AppendLine(padding + "}");

        return strBuilder.ToString();
    }
}