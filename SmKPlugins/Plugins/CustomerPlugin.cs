using Microsoft.Data.SqlClient;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SmKPlugins.Plugins
{
    public class CustomerPlugin
    {
        private readonly string _connectionString;

        public CustomerPlugin(string connectionString)
        {
            _connectionString = connectionString;
        }

        //[KernelFunction, Description("lissta de clientes y sus email")]
        [KernelFunction, Description("Retrieves a list of clients along with their email addresses.")]

        public async Task<List<Customer>> GetClientesAsync()
        {
            var clientes = new List<Customer>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT Nombres as Nombre, UserName as Email FROM AspNetUsers", connection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    clientes.Add(new Customer
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }

            return clientes;
        }

        public class Customer
        {
            public string Nombre { get; set; }
            public string Email { get; set; }
        }
    }
}
