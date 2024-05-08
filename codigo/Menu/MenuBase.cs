using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public abstract class MenuBase {

    protected Path _path = new Path("Sistema teste");

    // private NumberStyles styles = NumberStyles.Number; // any
    private CultureInfo culture = CultureInfo.CurrentCulture; // invariant



    #region Path_Class

    protected class Path {

        string _title;

        List<string> _pathList = new List<string>();


        public Path(string title) {
            this._title = title;
        }


        public void Include(string path) {
            _pathList.Add(path);
        }


        public void Remove(string path) {

            for (int i = _pathList.Count - 1; i > -1; i--) {
                if (_pathList[i] == path) {
                    _pathList.RemoveRange(i, _pathList.Count - i);
                    break;
                }

            }

        }


        public void Restart(string firstPath = "") {

            _pathList.Clear();

            _pathList.Add(firstPath);

        }


        public void Print() {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(_title).AppendLine(":\n");

            if (_pathList.Count == 0) {
                Console.WriteLine(stringBuilder.ToString());
                return;
            }

            for (int i = 0; i < _pathList.Count; i++) {
                stringBuilder.Append(_pathList[i]);

                if (i < _pathList.Count - 1) {
                    stringBuilder.Append(" / ");
                }
            }

            Console.WriteLine(stringBuilder.Append("\n").ToString());

        }


        internal void SetTitle(string newTitle) {
            _title = newTitle;
        }


    }

    #endregion



    #region Console_Helpers

    protected void Clear() {
        Console.Clear();
    }


    /// <summary>
    /// interrompe a execução do método para aguardar um input do usuário.
    /// </summary>
    protected void WaitForUserInput() {
        "Tecle para continuar..."._();
        ReadKey();
    }


    /// <summary>
    /// Limpa a tela de console e imprime o caminho dos menus, a título de organização e testes.
    /// </summary>
    /// <param name="path">Caminho dos menus</param>
    protected void ResetConsole(string path) {
        Clear();
        path._();
    }

    #endregion



    #region Read_Helpers

    protected void _(string s = "") {
        Console.WriteLine(s);
    }


    protected string Read() {
        
        Console.Write("=> ");

        string line = Console.ReadLine() ?? "";

        Console.WriteLine();

        return line;

    }


    protected int ReadInt() {

        string line = Read();

        int number;

        while (!int.TryParse(line, out number)) {
            "Por favor, insira um número inteiro válido: "._();
            line = Read();
        }

        return number;

    }


    protected int ReadIntRange(int minInclusive, int maxInclusive = int.MaxValue) {

        int inteiroLido = ReadInt();

        while (inteiroLido < minInclusive || inteiroLido > maxInclusive) {

            $"valor deve estar entre {minInclusive} e {maxInclusive}"._();

            inteiroLido = ReadInt();

        }

        return inteiroLido;

    }


    protected decimal ReadDecimal() {

        string line = Read();

        decimal number;

        while (!Decimal.TryParse(line, NumberStyles.Number, CultureInfo.CurrentCulture, out number)) {
            _("\nPor favor, insira um número decimal válido: ");
            line = Read();
        }

        return number;

    }


    protected ConsoleKeyInfo ReadKey() {
        return Console.ReadKey();
    }


    protected virtual void AvisoImplementar() {
        "Método a ser implementado."._();
        WaitForUserInput();
    }

    #endregion



    #region Exemplos_Menus

    /// <summary>
    /// Exemplo de início do Menu.
    /// </summary>
    public virtual void Menu_Inicio() {

        string path = "[Título]";

        Menu_Model(path);

        ResetConsole(path += "0 - Encerrar");

        "Fim - Obrigado."._();
        WaitForUserInput();

    }


    /// <summary>
    /// Exemplo de menu após início.
    /// </summary>
    private void Menu_Model(string path) {

        path += "";

        int option = -1;

        while (option != 0) {

            ResetConsole(path);

            "Escolha uma opção:"._();
            _();
            "1 - "._();
            "2 - "._();
            "0 - Encerrar"._();
            _();

            option = ReadInt();

            switch (option) {

                case 0:
                    continue;

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
