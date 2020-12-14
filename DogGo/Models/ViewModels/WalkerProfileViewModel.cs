using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public List<Walk> Walks { get; set; }
        public Walker Walker { get; set; }

        public string TotalWalkTime()
        {
            List<int> durationArray = Walks.Select(w => w.Duration).ToList();
            int totalminutes = durationArray.Sum() / 60;

            int hours = (totalminutes / 60);
            int mins = totalminutes - (hours * 60);

            return $"{hours} hours {mins} mins";
        }
    }
}
