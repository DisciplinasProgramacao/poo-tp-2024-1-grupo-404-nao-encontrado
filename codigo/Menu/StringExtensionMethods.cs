using System;


public static class StringExtensionMethods {


    public static void _(this string text) {

        Console.WriteLine(text + "\n");

    }


    public static string Reais(this float value) {

        return "R$ " + value.ToString("F2");

    }


}