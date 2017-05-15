using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataAccessLayes.Models
{
    public class MovieActors
    {
        public int ID { get; set; }
        public int MovieID { get; set; }
        public int ActorID { get; set; }
    }
}
