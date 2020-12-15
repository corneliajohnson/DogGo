using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class DogFormViewModel
    {
        public List<Owner> Owners { get; set; }
        public Dog Dog { get; set; }
    }
}
