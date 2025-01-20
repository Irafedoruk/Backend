namespace BackendShop.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Зв'язок з товаром
        public int Quantity { get; set; }
        public Cart Cart { get; set; } // Зв'язок з кошиком
        public int CartId { get; set; }
    }
}
