using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    interface IWalkRepository
    {
        List<Walk> GetWalksByWalkerId(int walkerId);
    }
}
