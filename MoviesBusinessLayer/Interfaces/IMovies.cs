using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesDataAccessLayes.Models;
namespace MoviesBusinessLayer.Interfaces
{
    public interface IMovies
    {
        List<MoviesGetModel> GetAllMovies();

        MovieModel GetMovie(int MovieID = 0);

        bool AddUpdateMovie(MovieModel model);

        List<DropdownList> GetFilmCrewList(DepartmentType DepartmentType);

        bool AddFilmCrew(FilmCrewModel model);

        bool DeleteMovie(int MovieID);
    }
}
