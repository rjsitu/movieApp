using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataAccessLayes.Models
{
    public class MoviesGetModel
    {
        public int MovieID { get; set; }
        public string Name { get; set; }
        public Int16 YearOfRelease { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string ProducerName { get; set; }
        public string[] ActorsName { get; set; }
    }
}
