// MappingProfiles/PedidoProfile.cs
using AutoMapper;

public class PedidoProfile : Profile
{
    public PedidoProfile()
    {
        CreateMap<Pedido, PedidoResponseDto>();
        CreateMap<Item, ItemDto>();

        CreateMap<CreatePedidoDto, Pedido>();
        CreateMap<UpdatePedidoDto, Pedido>();
        CreateMap<ItemDto, Item>();

        CreateMap<StatusRequestDto, StatusRequest>();

    }
}
