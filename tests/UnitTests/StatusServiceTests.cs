using Xunit;
using Moq;

public class StatusServiceTests
{
    private readonly StatusService _service;
    private readonly Mock<IPedidoRepository> _repoMock;

    public StatusServiceTests()
    {
        _repoMock = new Mock<IPedidoRepository>();
        _service = new StatusService(_repoMock.Object);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenPedidoNaoExiste_ReturnsCODIGO_PEDIDO_INVALIDO()
    {
        // Arrange
        var request = new StatusRequest
        {
            Pedido = "NAO_EXISTE",
            Status = "APROVADO",
            ItensAprovados = 2,
            ValorAprovado = 10
        };

        _repoMock
            .Setup(r => r.GetByCodigoAsync("NAO_EXISTE"))
            .ReturnsAsync((Pedido?)null);

        // Act
        var (success, message, statusList) = await _service.ChangeStatusAsync(request);

        // Assert
        Assert.False(success);
        Assert.Equal("CODIGO_PEDIDO_INVALIDO", message);
        Assert.Empty(statusList);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenStatusReprovado_ReturnsReprovado()
    {
        // Arrange
        var pedidoDb = new Pedido
        {
            Codigo = "123",
            Itens = new List<Item>
            {
                new Item { Descricao = "Item1", PrecoUnitario = 5, Qtd = 2 }
            }
        };
        _repoMock
            .Setup(r => r.GetByCodigoAsync("123"))
            .ReturnsAsync(pedidoDb);

        var request = new StatusRequest
        {
            Pedido = "123",
            Status = "REPROVADO"
        };

        // Act
        var (success, message, statusList) = await _service.ChangeStatusAsync(request);

        // Assert
        Assert.True(success);
        Assert.Null(message);
        Assert.Single(statusList);
        Assert.Contains("REPROVADO", statusList);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenStatusAprovado_EverythingMatches_ReturnsAPROVADO()
    {
        // Arrange
        var pedidoDb = new Pedido
        {
            Codigo = "123",
            Itens = new List<Item>
            {
                new Item { Descricao = "Item1", PrecoUnitario = 10, Qtd = 2 }
            }
        };
        _repoMock
            .Setup(r => r.GetByCodigoAsync("123"))
            .ReturnsAsync(pedidoDb);

        var request = new StatusRequest
        {
            Pedido = "123",
            Status = "APROVADO",
            ItensAprovados = 2,
            ValorAprovado = 20
        };

        // Act
        var (success, message, statusList) = await _service.ChangeStatusAsync(request);

        // Assert
        Assert.True(success);
        Assert.Null(message);
        Assert.Contains("APROVADO", statusList);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenValorAprovado_MenorQueTotalValue_ReturnsAPROVADO_VALOR_A_MENOR()
    {
        // Arrange
        var pedidoDb = new Pedido
        {
            Codigo = "123",
            Itens = new List<Item>
            {
                new Item { Descricao = "Item1", PrecoUnitario = 10, Qtd = 2 }
            }
        };
        _repoMock
            .Setup(r => r.GetByCodigoAsync("123"))
            .ReturnsAsync(pedidoDb);

        var request = new StatusRequest
        {
            Pedido = "123",
            Status = "APROVADO",
            ItensAprovados = 2,
            ValorAprovado = 15
        };

        // Act
        var (success, message, statusList) = await _service.ChangeStatusAsync(request);

        // Assert
        Assert.True(success);
        Assert.Contains("APROVADO_VALOR_A_MENOR", statusList);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenItensAprovados_MaiorQueTotal_ReturnsAPROVADO_QTD_A_MAIOR()
    {
        // Arrange
        var pedidoDb = new Pedido
        {
            Codigo = "123",
            Itens = new List<Item>
            {
                new Item { Descricao = "Item1", PrecoUnitario = 10, Qtd = 2 }
            }
        };
        _repoMock
            .Setup(r => r.GetByCodigoAsync("123"))
            .ReturnsAsync(pedidoDb);

        var request = new StatusRequest
        {
            Pedido = "123",
            Status = "APROVADO",
            ItensAprovados = 3,
            ValorAprovado = 20
        };

        // Act
        var (success, message, statusList) = await _service.ChangeStatusAsync(request);

        // Assert
        Assert.True(success);
        Assert.Contains("APROVADO_QTD_A_MAIOR", statusList);
    }
}
