using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DatabaseHelper
{
    private readonly string connectionString;

    public DatabaseHelper()
    {
        connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }

    // Login User
    public User LoginUser(string phoneNumber)
    {
        string query = "SELECT ID, Name, Age, PhoneNumber, CreatedAt FROM Users WHERE PhoneNumber = @Phone";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Phone", phoneNumber);
                conn.Open();
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            ID = reader.GetInt32("ID"),
                            Name = reader.GetString("Name"),
                            Age = reader.GetInt32("Age"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            CreatedAt = reader.GetDateTime("CreatedAt")
                        };
                    }
                }
            }
        }
        return null;
    }

    // Register New User
    public User RegisterUser(string name, int age, string phoneNumber)
    {
        // Check if user exists
        if (UserExists(phoneNumber))
            return null;

        string query = @"INSERT INTO Users (Name, Age, PhoneNumber) 
                        OUTPUT INSERTED.ID, INSERTED.Name, INSERTED.Age, 
                               INSERTED.PhoneNumber, INSERTED.CreatedAt
                        VALUES (@Name, @Age, @Phone)";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Phone", phoneNumber);
                
                conn.Open();
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            ID = reader.GetInt32("ID"),
                            Name = reader.GetString("Name"),
                            Age = reader.GetInt32("Age"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            CreatedAt = reader.GetDateTime("CreatedAt")
                        };
                    }
                }
            }
        }
        return null;
    }

    // Check if User Exists
    public bool UserExists(string phoneNumber)
    {
        string query = "SELECT COUNT(*) FROM Users WHERE PhoneNumber = @Phone";
        
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Phone", phoneNumber);
                conn.Open();
                
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
 
