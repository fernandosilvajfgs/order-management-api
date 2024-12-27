using Xunit;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class StatusControllerIntegrationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public StatusControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ChangeStatus_QuandoNaoExiste_RetornaNotFound()
    {
        // Arrange
        var request = new StatusRequestDto
        {
            Status = "APROVADO",
            ItensAprovados = 3,
            ValorAprovado = 20,
            Pedido = "NAO_EXISTE"
        };

        var jsonBody = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/status", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("CODIGO_PEDIDO_INVALIDO", responseBody);
    }

    [Fact]
    public async Task ChangeStatus_QuandoReprovado_DeveRetornarReprovado()
    {
        // Arrange
        var pedido = new CreatePedidoDto
        {
            Codigo = "TESTE-REPROVADO",
            Itens = new List<ItemDto>
            {
                new ItemDto { Descricao = "Item A", PrecoUnitario = 10, Qtd = 1 },
            }
        };

        var jsonPedido = JsonSerializer.Serialize(pedido);
        var contentPedido = new StringContent(jsonPedido, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/pedido", contentPedido);

        var request = new StatusRequestDto
        {
            Status = "REPROVADO",
            ItensAprovados = 0,
            ValorAprovado = 0,
            Pedido = "TESTE-REPROVADO"
        };

        var jsonStatus = JsonSerializer.Serialize(request);
        var contentStatus = new StringContent(jsonStatus, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/status", contentStatus);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("REPROVADO", responseBody);
    }

    [Fact]
    public async Task ChangeStatus_QuandoAprovadoValorMenor_DeveRetornarAprovadoValorAMenor()
    {
        // Arrange
        var pedido = new CreatePedidoDto
        {
            Codigo = "TESTE-APROVADO",
            Itens = new List<ItemDto>
            {
                new ItemDto { Descricao = "Item A", PrecoUnitario = 10, Qtd = 2 }
            }
        };

        var jsonPedido = JsonSerializer.Serialize(pedido);
        var contentPedido = new StringContent(jsonPedido, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/pedido", contentPedido);

        var request = new StatusRequestDto
        {
            Status = "APROVADO",
            ItensAprovados = 2,
            ValorAprovado = 10,
            Pedido = "TESTE-APROVADO"
        };

        var jsonStatus = JsonSerializer.Serialize(request);
        var contentStatus = new StringContent(jsonStatus, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/status", contentStatus);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("APROVADO_VALOR_A_MENOR", responseBody);
    }
}
