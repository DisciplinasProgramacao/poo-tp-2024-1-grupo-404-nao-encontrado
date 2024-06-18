using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Mesa
{
    private int _capacidade;
    private bool _estaOcupada;

    public Mesa(int capacidade)
    {
        _capacidade = capacidade;
        _estaOcupada = false;
    }

    public int GetCapacidade()
    {
        return _capacidade;
    }

    public bool GetEstaOcupada()
    {
        return _estaOcupada;
    }

    public void SetEstaOcupada(bool valor)
    {
        _estaOcupada = valor;
    }

    public string Relatorio()
    {
        return $"Mesa com capacidade para {_capacidade} pessoas." +
               $" Está ocupada: {_estaOcupada}.";
    }

    public bool VerificarDisponibilidade(int qntPessoas)
    {
        // Retorna verdadeiro se a mesa não está ocupada e se a quantidade de pessoas não excede a capacidade
        return !_estaOcupada && qntPessoas <= _capacidade;
    }

    public string DescricaoOcupacao()
    {
        return _estaOcupada ? "Ocupada" : "vazia";
    }
}
