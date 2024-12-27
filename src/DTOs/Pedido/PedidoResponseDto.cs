public class PedidoResponseDto
{
    public string Codigo { get; set; } = string.Empty;
    public List<ItemDto> Itens { get; set; } = new();
}