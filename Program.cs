using System;
using System.Collections.Generic;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Desafio Target Sistemas - C#");
        Console.WriteLine("================================");

        Console.WriteLine("\n1) Verificacao de Fibonacci");
        VerificarFibonacci();

        Console.WriteLine("\n2) Contagem de letra 'a'");
        ContarLetraA();

        Console.WriteLine("\n3) Soma com while");
        CalcularSomaIndice();

        Console.WriteLine("\n4) Sequencias logicas");
        MostrarSequencias();

        Console.WriteLine("\n5) Desafio das lampadas");
        ResolverLampadas();
    }

    private static void VerificarFibonacci()
    {
        int numero = 1597;
        Console.WriteLine($"Numero de teste: {numero}");

        if (PertenceAFibonacci(numero))
            Console.WriteLine($"{numero} pertence a sequencia de Fibonacci.");
        else
            Console.WriteLine($"{numero} nao pertence a sequencia de Fibonacci.");

        int[] seq = GerarSequenciaFibonacci(10);
        Console.WriteLine("Primeiros 10 termos: " + string.Join(", ", seq));
    }

    private static bool PertenceAFibonacci(int numero)
    {
        if (numero < 0) return false;
        if (numero == 0 || numero == 1) return true;

        int anterior = 0;
        int atual = 1;

        while (atual < numero)
        {
            int proximo = anterior + atual;
            anterior = atual;
            atual = proximo;
        }

        return atual == numero;
    }

    private static int[] GerarSequenciaFibonacci(int quantidade)
    {
        var sequencia = new List<int>();

        if (quantidade <= 0)
            return sequencia.ToArray();

        int anterior = 0;
        int atual = 1;

        for (int i = 0; i < quantidade; i++)
        {
            sequencia.Add(anterior);
            int proximo = anterior + atual;
            anterior = atual;
            atual = proximo;
        }

        return sequencia.ToArray();
    }

    private static void ContarLetraA()
    {
        string texto = "Target Sistemas";
        int quantidade = 0;

        foreach (char c in texto)
        {
            if (char.ToLowerInvariant(c) == 'a')
                quantidade++;
        }

        Console.WriteLine($"Texto: {texto}");
        Console.WriteLine(quantidade > 0
            ? $"Ha {quantidade} letra(s) 'a' no texto."
            : "Nao ha letras 'a' no texto.");
    }

    private static void CalcularSomaIndice()
    {
        int indice = 13;
        int numero = 1;
        int soma = 0;

        while (numero < indice)
        {
            soma += numero;
            numero++;
        }

        Console.WriteLine($"Indice: {indice}");
        Console.WriteLine($"Soma: {soma}");
    }

    private static void MostrarSequencias()
    {
        var sequencias = new Dictionary<string, int>
        {
            ["a)"] = 9,
            ["b)"] = 128,
            ["c)"] = 49,
            ["d)"] = 100,
            ["e)"] = 13,
            ["f)"] = 20
        };

        foreach (var item in sequencias)
        {
            Console.WriteLine($"{item.Key} Proximo numero: {item.Value}");
        }
    }

    private static void ResolverLampadas()
    {
        Console.WriteLine("1. Ligue o primeiro interruptor e espere alguns minutos.");
        Console.WriteLine("2. Desligue o primeiro interruptor e ligue o segundo.");
        Console.WriteLine("3. Va ate a sala das lampadas.");
        Console.WriteLine("4. A lampada acesa = interruptor 2.");
        Console.WriteLine("5. A lampada apagada e quente = interruptor 1.");
        Console.WriteLine("6. A lampada apagada e fria = interruptor 3.");
    }
}
