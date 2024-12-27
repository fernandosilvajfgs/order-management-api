using Xunit;
using Moq;

public class PedidoServiceTests
{
    private readonly PedidoService _service;
    private readonly Mock<IPedidoRepository> _repoMock;

    public PedidoServiceTests()
    {
        _repoMock = new Mock<IPedidoRepository>();
        _service = new PedidoService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllPedidosAsync_ReturnsAllPedidos()
    {
        // Arrange
        var pedidos = new List<Pedido>
    {
        new Pedido { Codigo = "PED-001", Itens = new List<Item>() },
        new Pedido { Codigo = "PED-002", Itens = new List<Item>() }
    };

        _repoMock
            .Setup(r => r.GetAllPedidosAsync())
            .ReturnsAsync(pedidos);

        // Act
        var result = await _service.GetAllPedidosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Codigo == "PED-001");
        Assert.Contains(result, p => p.Codigo == "PED-002");
    }

    [Fact]
    public async Task CreatePedidoAsync_WhenCodigoDuplicado_ThrowsInvalidOperationException()
    {
        // Arrange
        var existingPedido = new Pedido { Codigo = "123" };
        _repoMock
            .Setup(r => r.GetByCodigoAsync("123"))
            .ReturnsAsync(existingPedido);

        var newPedido = new Pedido { Codigo = "123", Itens = new List<Item>() };

        // Act/Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreatePedidoAsync(newPedido));
    }

    [Fact]
    public async Task CreatePedidoAsync_WhenCodigoIsUnique_AddsToRepository()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByCodigoAsync("XYZ"))
            .ReturnsAsync((Pedido?)null);

        var newPedido = new Pedido
        {
            Codigo = "XYZ",
            Itens = new List<Item>
            {
                new Item { Descricao = "Item 1", PrecoUnitario = 5, Qtd = 2 }
            }
        };

        // Act
        var result = await _service.CreatePedidoAsync(newPedido);

        // Assert
        _repoMock.Verify(r => r.AddAsync(newPedido), Times.Once);
        Assert.Equal("XYZ", result.Codigo);
    }

    [Fact]
    public async Task UpdatePedidoAsync_WhenCodigoInvalido_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByCodigoAsync("NAO_EXISTE"))
            .ReturnsAsync((Pedido?)null);

        var updatedPedido = new Pedido { Codigo = "NAO_EXISTE" };

        // Act/Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdatePedidoAsync("NAO_EXISTE", updatedPedido));
    }

    [Fact]
    public async Task UpdatePedidoAsync_WhenCodigoExiste_UpdatesItens()
    {
        // Arrange
        var existingPedido = new Pedido
        {
            Codigo = "ABC",
            Itens = new List<Item>
            {
                new Item { Descricao = "ItemVelho", PrecoUnitario = 10, Qtd = 1 }
            }
        };

        _repoMock
            .Setup(r => r.GetByCodigoAsync("ABC"))
            .ReturnsAsync(existingPedido);

        var updatedPedido = new Pedido
        {
            Codigo = "ABC",
            Itens = new List<Item>
            {
                new Item { Descricao = "ItemNovo", PrecoUnitario = 5, Qtd = 2 }
            }
        };

        // Act
        await _service.UpdatePedidoAsync("ABC", updatedPedido);

        // Assert
        Assert.Single(existingPedido.Itens);
        Assert.Equal("ItemNovo", existingPedido.Itens[0].Descricao);

        _repoMock.Verify(r => r.UpdateAsync(existingPedido), Times.Once);
    }

    [Fact]
    public async Task DeletePedidoAsync_WhenCodigoInvalido_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByCodigoAsync("NAO_EXISTE"))
            .ReturnsAsync((Pedido?)null);

        // Act/Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.DeletePedidoAsync("NAO_EXISTE"));
    }

    [Fact]
    public async Task DeletePedidoAsync_WhenCodigoExiste_CallsDelete()
    {
        // Arrange
        var existingPedido = new Pedido { Codigo = "EXISTE" };
        _repoMock
            .Setup(r => r.GetByCodigoAsync("EXISTE"))
            .ReturnsAsync(existingPedido);

        // Act
        await _service.DeletePedidoAsync("EXISTE");

        // Assert
        _repoMock.Verify(r => r.DeleteAsync(existingPedido), Times.Once);
    }
}
