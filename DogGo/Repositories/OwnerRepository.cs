using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        public OwnerRepository(IConfiguration config)
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

        public List<Owner> GetAllOwners()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, Address, Phone, NeighborhoodId
                                        FROM Owner";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();

                    while(reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };
                        owners.Add(owner);
                    }
                    reader.Close();
                    return owners;
                }

            }
        }

        public Owner GetOwnerById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    //cmd.CommandText = @"SELECT Id, Name, Address, Phone, NeighborhoodId
                    //                    FROM Owner
                    //                    WHERE Id = @id";

                    cmd.CommandText = @"SELECT d.Name as DogName, Breed, o.Name as OwnerName, o.Id as OwnerId, Address, Phone, NeighborhoodId
                                               FROM Owner o
                                               JOIN Dog d ON d.OwnerId = o.Id AND o.Id = @id
                                               GROUP BY d.Name, Breed, o.Name, o.Id,  Address, Phone, NeighborhoodId";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        List<Dog> dogs = new List<Dog>();
                        while(reader.Read())
                        {
                            Dog dog = new Dog
                            {
                                Name = reader.GetString(reader.GetOrdinal("DogName")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            };
                            dogs.Add(dog);
                        }

                        owner.Dogs = dogs;

                        reader.Close();
                        return owner;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }

        }
    }
}
