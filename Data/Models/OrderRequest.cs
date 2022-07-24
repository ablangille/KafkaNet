namespace KafkaDocker.Data.Models
{
    public class OrderRequest
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
    }
}
