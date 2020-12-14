using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace DogGo.Repositories 
{
    public class NeighborhoodRepository : INeighborhoodRepository
    {
    private readonly IConfiguration _config;

    // The constructor accepts an IConfiguration object as a parameter. 
    // This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
    public NeighborhoodRepository(IConfiguration config)
    {
        _config = config;
    }

    public SqlConnection Connection
    {
        get
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }

    public  Neighborhood GetNeighborhoodById(int Id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT Id, Name
                                    FROM Neighborhood";

                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    Neighborhood neighborhood = new Neighborhood
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name"))
                    };
                    reader.Close();
                    return neighborhood;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
        }
    }
        public List<Neighborhood> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name FROM Neighborhood";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Neighborhood> neighborhoods = new List<Neighborhood>();

                    while (reader.Read())
                    {
                        Neighborhood neighborhood = new Neighborhood()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        neighborhoods.Add(neighborhood);
                    }

                    reader.Close();

                    return neighborhoods;
                }
            }
        }
    }
}
