namespace BackendShop.Core.Dto.Order
{
    public class CreateOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
