using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walk
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        int Duration { get; set; }
        int WalkerId { get; set; }
        Walker Walker { get; set; }
        int DogId { get; set; }
        Dog Dog { get; set; }
    }
}
