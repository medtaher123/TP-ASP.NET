namespace MyFristTP.Models
{
    public class MovieCustomerViewModel
    {
        public Movie Movie { get; set; }
        public List<Customer> Customers { get; set; }

        public MovieCustomerViewModel(Movie movie, List<Customer> customers)
        {
            Movie = movie;
            Customers = customers;
        }

        public override string ToString()
        {
            return $"Movie: {Movie.ToString()}, \nCustomers: {string.Join(", ", Customers.Select(c => c.Name))}";
        }
    }
}