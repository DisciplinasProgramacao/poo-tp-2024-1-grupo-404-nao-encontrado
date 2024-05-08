using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cardapio {


    #region Classes

    public interface IItem {

        public string GetNome();

        public float GetPreco();


    }


    public class Comida : IItem {

        string _nome;

        float _preco;


        public Comida(string nome, float preco) {
            
            _nome = nome;
            _preco = preco;

        }


        public string GetNome() {

            return _nome;

        }


        public float GetPreco() {

            return _preco;

        }


    }


    public class Bebida : IItem {

        string _nome;

        float _preco;


        public Bebida(string nome, float preco) {

            _nome = nome;
            _preco = preco;

        }


        public string GetNome() {

            return _nome;

        }


        public float GetPreco() {

            return _preco;

        }


    }


    #endregion



    List<Comida> _listaComidas = new List<Comida>();

    List<Bebida> _listaBebidas = new List<Bebida>();



    public Cardapio() { }



    #region Add and Get

    public Cardapio AddComida(in string nome, in float preco) {

        _listaComidas.Add(new Comida(nome, preco));

        return this;

    }


    public Cardapio AddBebida(in string nome, in float preco) {

        _listaBebidas.Add(new Bebida(nome, preco));

        return this;

    }


    public Comida GetComidaPorIndice(int indice) {

        return _listaComidas[indice];

    }


    public Bebida GetBebidaPorIndice(int indice) {

        return _listaBebidas[indice];

    }


    #endregion



    #region Relatorios


    string RelatorioListaItens(in List<IItem> lista) {

        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < _listaBebidas.Count; i++) {

            sBuilder.Append($"{i + 1} - {lista[i].GetNome()}\n");

        }

        return sBuilder.ToString();

    }


    public string RelatorioListaComidas() {

        return RelatorioListaItens(_listaComidas.ToList<IItem>());

    }


    public string RelatorioListaBebidas() {

        return RelatorioListaItens(_listaBebidas.ToList<IItem>());

    }


    public int getTotalComidas() {

        return _listaComidas.Count;

    }


    public int getTotalBebidas() {

        return _listaBebidas.Count;

    }


    public int getTotalItens() {

        return getTotalComidas() + getTotalBebidas();

    }


    #endregion



}
