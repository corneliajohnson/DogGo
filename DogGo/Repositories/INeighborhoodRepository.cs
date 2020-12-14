using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
   public interface INeighborhoodRepository
    {
        Neighborhood GetNeighborhoodById(int Id);
        List<Neighborhood> GetAll();
    }
}
