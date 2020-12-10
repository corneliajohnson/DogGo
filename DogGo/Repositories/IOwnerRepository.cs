using System.Collections.Generic;
using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);
    }
}
