using System;

public class Cliente
{
    private string _nome;

    public Cliente(string nome)
    {
        this._nome = nome;
    }

    public string GetNome()
    {
        return _nome;
    }
}