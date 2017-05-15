using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataAccessLayes.Models
{
    public class FilmCrewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime DOB { get; set; }
        public string Bio { get; set; }
        public int DepartmentType { get; set; }
    }
}
