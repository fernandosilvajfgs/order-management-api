public class Pedido
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public List<Item> Itens { get; set; }
}