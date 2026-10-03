using System.Globalization;
using System.Text.Json;

internal static class Program
{
    private static readonly CultureInfo CulturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");

    private const string JsonVendas = """
    {
      "vendas": [
        { "vendedor": "João Silva", "valor": 1200.50 },
        { "vendedor": "João Silva", "valor": 950.75 },
        { "vendedor": "João Silva", "valor": 1800.00 },
        { "vendedor": "João Silva", "valor": 1400.30 },
        { "vendedor": "João Silva", "valor": 1100.90 },
        { "vendedor": "João Silva", "valor": 1550.00 },
        { "vendedor": "João Silva", "valor": 1700.80 },
        { "vendedor": "João Silva", "valor": 250.30 },
        { "vendedor": "João Silva", "valor": 480.75 },
        { "vendedor": "João Silva", "valor": 320.40 },
        { "vendedor": "Maria Souza", "valor": 2100.40 },
        { "vendedor": "Maria Souza", "valor": 1350.60 },
        { "vendedor": "Maria Souza", "valor": 950.20 },
        { "vendedor": "Maria Souza", "valor": 1600.75 },
        { "vendedor": "Maria Souza", "valor": 1750.00 },
        { "vendedor": "Maria Souza", "valor": 1450.90 },
        { "vendedor": "Maria Souza", "valor": 400.50 },
        { "vendedor": "Maria Souza", "valor": 180.20 },
        { "vendedor": "Maria Souza", "valor": 90.75 },
        { "vendedor": "Carlos Oliveira", "valor": 800.50 },
        { "vendedor": "Carlos Oliveira", "valor": 1200.00 },
        { "vendedor": "Carlos Oliveira", "valor": 1950.30 },
        { "vendedor": "Carlos Oliveira", "valor": 1750.80 },
        { "vendedor": "Carlos Oliveira", "valor": 1300.60 },
        { "vendedor": "Carlos Oliveira", "valor": 300.40 },
        { "vendedor": "Carlos Oliveira", "valor": 500.00 },
        { "vendedor": "Carlos Oliveira", "valor": 125.75 },
        { "vendedor": "Ana Lima", "valor": 1000.00 },
        { "vendedor": "Ana Lima", "valor": 1100.50 },
        { "vendedor": "Ana Lima", "valor": 1250.75 },
        { "vendedor": "Ana Lima", "valor": 1400.20 },
        { "vendedor": "Ana Lima", "valor": 1550.90 },
        { "vendedor": "Ana Lima", "valor": 1650.00 },
        { "vendedor": "Ana Lima", "valor": 75.30 },
        { "vendedor": "Ana Lima", "valor": 420.90 },
        { "vendedor": "Ana Lima", "valor": 315.40 }
      ]
    }
    """;

    private const string JsonEstoque = """
    {
      "estoque": [
        { "codigoProduto": 101, "descricaoProduto": "Caneta Azul", "estoque": 150 },
        { "codigoProduto": 102, "descricaoProduto": "Caderno Universitário", "estoque": 75 },
        { "codigoProduto": 103, "descricaoProduto": "Borracha Branca", "estoque": 200 },
        { "codigoProduto": 104, "descricaoProduto": "Lápis Preto HB", "estoque": 320 },
        { "codigoProduto": 105, "descricaoProduto": "Marcador de Texto Amarelo", "estoque": 90 }
      ]
    }
    """;

    private static readonly List<Produto> Produtos =
        JsonSerializer.Deserialize<EstoquePayload>(JsonEstoque,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!.Estoque;

    private static readonly List<Movimentacao> Movimentacoes = [];
    private static int proximoId = 1;

    private static void Main()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1 - Calcular comissões");
            Console.WriteLine("2 - Movimentar estoque");
            Console.WriteLine("3 - Calcular juros");
            Console.WriteLine("0 - Sair");
            Console.Write("Opção: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CalcularComissoes();
                    break;
                case "2":
                    MovimentarEstoque();
                    break;
                case "3":
                    CalcularJuros();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    private static void CalcularComissoes()
    {
        var vendas = JsonSerializer.Deserialize<VendasPayload>(JsonVendas,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!.Vendas;
        var totais = new Dictionary<string, decimal>();

        foreach (var venda in vendas)
        {
            var comissao = venda.Valor switch
            {
                < 100m => 0m,
                < 500m => venda.Valor * 0.01m,
                _ => venda.Valor * 0.05m
            };

            totais[venda.Vendedor] = totais.GetValueOrDefault(venda.Vendedor) + comissao;
        }

        foreach (var (vendedor, total) in totais)
            Console.WriteLine($"{vendedor}: {total.ToString("C2", CulturaBrasileira)}");
    }

    private static void MovimentarEstoque()
    {
        Console.WriteLine("Produtos:");
        foreach (var produto in Produtos)
            Console.WriteLine($"{produto.CodigoProduto} - {produto.DescricaoProduto}: {produto.Estoque}");

        if (!LerInteiro("Código do produto: ", out var codigo))
            return;
        var produtoEscolhido = Produtos.FirstOrDefault(p => p.CodigoProduto == codigo);
        if (produtoEscolhido is null)
        {
            Console.WriteLine("Produto não encontrado.");
            return;
        }

        Console.Write("Tipo (entrada/saida): ");
        var tipo = Console.ReadLine()?.Trim().ToLowerInvariant();
        if (tipo is not ("entrada" or "saida"))
        {
            Console.WriteLine("Tipo inválido.");
            return;
        }

        if (!LerInteiro("Quantidade: ", out var quantidade) || quantidade <= 0)
        {
            Console.WriteLine("Informe uma quantidade inteira maior que zero.");
            return;
        }

        if (tipo == "saida" && quantidade > produtoEscolhido.Estoque)
        {
            Console.WriteLine("Estoque insuficiente.");
            return;
        }

        Console.Write("Descrição da movimentação: ");
        var descricao = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(descricao))
        {
            Console.WriteLine("A descrição é obrigatória.");
            return;
        }

        produtoEscolhido.Estoque += tipo == "entrada" ? quantidade : -quantidade;
        var movimentacao = new Movimentacao(
            proximoId++,
            produtoEscolhido.CodigoProduto,
            descricao,
            tipo,
            quantidade);
        Movimentacoes.Add(movimentacao);

        Console.WriteLine($"Identificador: {movimentacao.Id}");
        Console.WriteLine($"Descrição: {movimentacao.Descricao}");
        Console.WriteLine($"Estoque final de {produtoEscolhido.DescricaoProduto}: {produtoEscolhido.Estoque}");
    }

    private static void CalcularJuros()
    {
        if (!LerDecimal("Valor: R$ ", out var valor) || valor < 0m)
        {
            Console.WriteLine("Informe um valor válido e não negativo.");
            return;
        }

        Console.Write("Data de vencimento (dd/MM/aaaa): ");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CulturaBrasileira,
                DateTimeStyles.None, out var vencimento))
        {
            Console.WriteLine("Data inválida.");
            return;
        }

        var diasAtraso = DateOnly.FromDateTime(DateTime.Today).DayNumber - vencimento.DayNumber;
        var juros = diasAtraso > 0 ? valor * 0.025m * diasAtraso : 0m;
        Console.WriteLine($"Dias de atraso: {Math.Max(diasAtraso, 0)}");
        Console.WriteLine($"Juros: {juros.ToString("C2", CulturaBrasileira)}");
        Console.WriteLine($"Total: {(valor + juros).ToString("C2", CulturaBrasileira)}");
    }

    private static bool LerInteiro(string prompt, out int valor)
    {
        Console.Write(prompt);
        return int.TryParse(Console.ReadLine(), out valor);
    }

    private static bool LerDecimal(string prompt, out decimal valor)
    {
        Console.Write(prompt);
        var texto = Console.ReadLine();
        return decimal.TryParse(texto, NumberStyles.Number, CulturaBrasileira, out valor)
            || decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
    }

    private sealed class VendasPayload
    {
        public List<Venda> Vendas { get; set; } = [];
    }

    private sealed class Venda
    {
        public string Vendedor { get; set; } = "";
        public decimal Valor { get; set; }
    }

    private sealed class EstoquePayload
    {
        public List<Produto> Estoque { get; set; } = [];
    }

    private sealed class Produto
    {
        public int CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; } = "";
        public int Estoque { get; set; }
    }

    private sealed record Movimentacao(
        int Id,
        int CodigoProduto,
        string Descricao,
        string Tipo,
        int Quantidade);
}
