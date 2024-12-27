public class Pedido
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public List<Item> Itens { get; set; } = new();
}