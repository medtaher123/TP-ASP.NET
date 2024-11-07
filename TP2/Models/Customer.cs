namespace TP2.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string toString()
        {
            return $"{Id} - {Name} - {Email}";
        }
    }
}
