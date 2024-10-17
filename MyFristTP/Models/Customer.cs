namespace MyFristTP.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }              // Nouvel attribut pour l'email
        public string PhoneNumber { get; set; }        // Nouvel attribut pour le numéro de téléphone
        public string Address { get; set; }             // Nouvel attribut pour l'adresse
        public DateTime RegistrationDate { get; set; }  // Nouvel attribut pour la date d'inscription

        public static readonly List<Customer> CUSTOMERS = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "Alice",
                Email = "alice@example.com",
                PhoneNumber = "123-456-7890",
                Address = "123 Main St, Anytown, USA",
                RegistrationDate = new DateTime(2023, 1, 15)
            },
            new Customer
            {
                Id = 2,
                Name = "Bob",
                Email = "bob@example.com",
                PhoneNumber = "987-654-3210",
                Address = "456 Elm St, Anytown, USA",
                RegistrationDate = new DateTime(2023, 2, 20)
            },
            new Customer
            {
                Id = 3,
                Name = "Charlie",
                Email = "charlie@example.com",
                PhoneNumber = "555-555-5555",
                Address = "789 Oak St, Anytown, USA",
                RegistrationDate = new DateTime(2023, 3, 10)
            }
        };


        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber}, Address: {Address}, Registered on: {RegistrationDate.ToShortDateString()}";
        }
    }
}
