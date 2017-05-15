using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataAccessLayes.Models
{

    public class DropdownList
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
    public class MovieModel
    {
        public int MovieID { get; set; }
        public string Name { get; set; }
        public Int16 YearOfRelease { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string ContentType { get; set; }
        public int ProducerID { get; set; }
        public int[] Actors { get; set; }
        public List<DropdownList> ProducersList { get; set; }
        public List<DropdownList> ActorsList { get; set; }
    }
}
