using Customers.Api.Contracts.Data;
using Dapper;
using MySqlConnector;

namespace Customers.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{

    private readonly string connectionString = "Server=127.0.0.1;User ID=root;Password=password;Database=aws";

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        using var connection = await CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Customers (Id, GitHubUsername, FullName, Email, DateOfBirth) 
            VALUES (@Id, @GitHubUsername, @FullName, @Email, @DateOfBirth)",
            customer);
        return result > 0;
    }

    private async Task<MySqlConnection> CreateConnectionAsync()
    {
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<CustomerDto?> GetAsync(string id)
    {
        using var connection = await CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<CustomerDto>(
            "SELECT * FROM Customers WHERE Id = @Id LIMIT 1", new { Id = id });
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        using var connection = await CreateConnectionAsync();
        return await connection.QueryAsync<CustomerDto>("SELECT * FROM Customers");
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        using var connection = await CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"UPDATE Customers SET GitHubUsername = @GitHubUsername, FullName = @FullName, Email = @Email, 
                 DateOfBirth = @DateOfBirth WHERE Id = @Id",
            customer);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = await CreateConnectionAsync();
        var result = await connection.ExecuteAsync(@"DELETE FROM Customers WHERE Id = @Id",
            new {Id = id});
        return result > 0;
    }

    /*
     INSERT INTO Customers (Id, GitHubUsername, FullName, Email, DateOfBirth)
        VALUES ('bd006bc2-151e-42c1-af2d-84927265a012', 'Mzbasel', 'Meryem Sel', 'mzbasel637936@gmail.com', '1998-08-06');
     */

    /*
    CREATE TABLE Customers (
        Id VARCHAR(36) NOT NULL,
        GitHubUsername VARCHAR(256) NOT NULL,
        FullName VARCHAR(256) NOT NULL,
        Email VARCHAR(256) NOT NULL,
        DateOfBirth VARCHAR(10),
        PRIMARY KEY (Id)
    );

     */
}
