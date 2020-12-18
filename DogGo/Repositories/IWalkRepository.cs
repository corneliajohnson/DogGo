using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetWalksByWalkerId(int walkerId);
        void AddWalk(Walk walk);
        List<Walk> GetWalksByOwnerId(int ownerId);
    }
}
