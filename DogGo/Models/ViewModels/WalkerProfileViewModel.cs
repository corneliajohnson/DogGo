using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        List<Walk> Walks { get; set; }
        Walker Walker { get; set; }
        string TotalWalkTime { get; set; }
        
    }
}
