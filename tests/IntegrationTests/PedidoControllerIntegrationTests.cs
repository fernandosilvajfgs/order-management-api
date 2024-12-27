using Xunit;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class PedidoControllerIntegrationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PedidoControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllPedidos_ReturnsListOfPedidos()
    {
        // Arrange
        var pedido1 = new CreatePedidoDto
        {
            Codigo = "PED-001",
            Itens = new List<ItemDto>
        {
            new ItemDto { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 }
        }
        };

        var pedido2 = new CreatePedidoDto
        {
            Codigo = "PED-002",
            Itens = new List<ItemDto>
        {
            new ItemDto { Descricao = "Item B", PrecoUnitario = 20, Qtd = 2 }
        }
        };

        var jsonPedido1 = JsonSerializer.Serialize(pedido1);
        var contentPedido1 = new StringContent(jsonPedido1, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/pedido", contentPedido1);

        var jsonPedido2 = JsonSerializer.Serialize(pedido2);
        var contentPedido2 = new StringContent(jsonPedido2, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/pedido", contentPedido2);

        // Act
        var response = await _client.GetAsync("/api/pedido");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        var pedidos = JsonSerializer.Deserialize<List<PedidoResponseDto>>(responseBody);

        Assert.NotNull(pedidos);
        Assert.Equal(2, pedidos.Count);
        Assert.Contains(pedidos, p => p.Codigo == "PED-001");
        Assert.Contains(pedidos, p => p.Codigo == "PED-002");
    }

    [Fact]
    public async Task PostPedido_ComPayloadValido_RetornaCreated()
    {
        // Arrange
        var novoPedido = new CreatePedidoDto
        {
            Codigo = "INT-001",
            Itens = new List<ItemDto>
            {
                new ItemDto { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 },
                new ItemDto { Descricao = "Item B", PrecoUnitario = 5,  Qtd = 2 }
            }
        };

        var jsonBody = JsonSerializer.Serialize(novoPedido);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/pedido", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Contains("INT-001", responseBody);
    }

    [Fact]
    public async Task GetPedido_QuandoNaoExiste_RetornaNotFound()
    {
        // Arrange
        var codigoInexistente = "NAO_EXISTE";

        // Act
        var response = await _client.GetAsync($"/api/pedido/{codigoInexistente}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("CODIGO_PEDIDO_INVALIDO", responseBody);
    }

    [Fact]
    public async Task PostPedido_QuandoDuplicado_RetornaBadRequest()
    {
        // Arrange
        var pedido = new CreatePedidoDto
        {
            Codigo = "DUP-001",
            Itens = new List<ItemDto>
            {
                new ItemDto { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 }
            }
        };

        var jsonPedido = JsonSerializer.Serialize(pedido);
        var contentPedido = new StringContent(jsonPedido, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/pedido", contentPedido);

        // Act
        var response = await _client.PostAsync("/api/pedido", contentPedido);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("CODIGO_PEDIDO_DUPLICADO", responseBody);
    }
}
