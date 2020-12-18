using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walk
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Duration { get; set; }
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        [Display(Prompt = "Dog")]
        [Required]
        public int DogId { get; set; }
        public Dog Dog { get; set; }
    }
}
