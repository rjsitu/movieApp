using MoviesBusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesDataAccessLayes.Models;
using MoviesDataAccessLayes.Entity;
namespace MoviesBusinessLayer.Repositories
{

    public class Base64Image
    {
        public Base64Image()
        {

        }

        public Base64Image(string contentType, byte[] fileContents)
        {
            this.ContentType = contentType;
            this.FileContents = fileContents;

        }
        public static Base64Image Parse(string base64Content)
        {
            if (string.IsNullOrEmpty(base64Content))
            {
                throw new ArgumentNullException("");
            }

            int indexOfSemiColon = base64Content.IndexOf(";", StringComparison.OrdinalIgnoreCase);

            string dataLabel = base64Content.Substring(0, indexOfSemiColon);

            string contentType = dataLabel.Split(':').Last();

            var startIndex = base64Content.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7;

            var fileContents = base64Content.Substring(startIndex);

            var bytes = Convert.FromBase64String(fileContents);

            return new Base64Image
            {
                ContentType = contentType,
                FileContents = bytes
            };
        }

        public string ContentType { get; set; }

        public byte[] FileContents { get; set; }

        public override string ToString()
        {
            return string.Format("data:{0};base64,{1}", ContentType, Convert.ToBase64String(FileContents));
        }
    }
    public class MoviesRepository : IMovies
    {
        #region Private variables & constructor
        private MoviesDBEntities _MoviesDBEntitiesContext;

        /// <summary>
        /// CommonRepository constructor
        /// </summary>
        /// <param name="ratingConfigcontext"></param>
        public MoviesRepository(MoviesDBEntities moviesDBEntitiesContext)
        {
            this._MoviesDBEntitiesContext = moviesDBEntitiesContext;
        }

        /// <summary>
        /// CommonRepository constructor
        /// </summary>
        public MoviesRepository()
        {
            this._MoviesDBEntitiesContext = new MoviesDBEntities();
        }

        #endregion

        /// <summary>
        /// Get List Of Movies
        /// </summary>
        /// <returns></returns>
        public List<MoviesGetModel> GetAllMovies()
        {
            var _query = (from m in _MoviesDBEntitiesContext.tblMovies
                          join ma in _MoviesDBEntitiesContext.tblMovieActors on m.MovieID equals ma.MovieID
                          join fc in _MoviesDBEntitiesContext.tblFilmCrews on ma.ActorID equals fc.ID
                          join p in _MoviesDBEntitiesContext.tblFilmCrews on m.ProducerID equals p.ID
                          select new { Movie = m, ProducerName = p.Name, ActorName = fc.Name, ActorID = fc.ID })
                          .AsEnumerable();

            var data = _query.GroupBy(g => new { g.Movie, g.ProducerName }).Select(s => new MoviesGetModel
            {
                MovieID = s.Key.Movie.MovieID,
                Name = s.Key.Movie.Name,
                Plot = s.Key.Movie.Plot,
                Poster = new Base64Image(s.Key.Movie.PosterContentType, s.Key.Movie.Poster).ToString(),
                YearOfRelease = s.Key.Movie.YearOfRelease,
                ProducerName = s.Key.ProducerName,
                ActorsName = s.Select(a => a.ActorName).ToArray()

            }).ToList();
            return data;

        }

        /// <summary>
        /// Get All Film Crew List List
        /// </summary>
        /// <returns></returns>
        public List<DropdownList> GetFilmCrewList(DepartmentType DepartmentType)
        {
            int _departmentType = (int)DepartmentType;
            return _MoviesDBEntitiesContext.tblFilmCrews.Where(w => w.DepartmentType == _departmentType).Select(x => new DropdownList { Key = x.ID, Value = x.Name }).ToList();
        }

        /// <summary>
        /// TO Manage Movie
        /// </summary>
        /// <param name="MovieID"></param>
        /// <returns></returns>
        public MovieModel GetMovie(int MovieID = 0)
        {
            MovieModel model = new MovieModel();
            if (MovieID > 0)
            {
                var movieEntity = _MoviesDBEntitiesContext.tblMovies.FirstOrDefault(f => f.MovieID == MovieID);
                if (movieEntity != null)
                {
                    model.MovieID = movieEntity.MovieID;
                    model.Name = movieEntity.Name;
                    model.Plot = movieEntity.Plot;
                    model.Poster = new Base64Image(movieEntity.PosterContentType, movieEntity.Poster).ToString();
                    model.ProducerID = movieEntity.ProducerID;
                    model.YearOfRelease = movieEntity.YearOfRelease;
                    model.Actors = movieEntity.tblMovieActors.Select(s => s.ActorID).ToArray();
                }
            }
            
            return model;
        }

        public bool AddUpdateMovie(MovieModel model)
        {
            try
            {
                tblMovy _movieModel = model.MovieID > 0 ? _MoviesDBEntitiesContext.tblMovies.FirstOrDefault(f => f.MovieID == model.MovieID) : new tblMovy();
                _movieModel.Name = model.Name;
                _movieModel.Plot = model.Plot;

                if (!string.IsNullOrEmpty(model.Poster))
                {
                    _movieModel.Poster = Base64Image.Parse(model.Poster).FileContents;
                    _movieModel.PosterContentType = model.ContentType;
                }

                _movieModel.ProducerID = model.ProducerID;
                _movieModel.YearOfRelease = model.YearOfRelease;
                _MoviesDBEntitiesContext.Entry(_movieModel).State = model.MovieID > 0 ? System.Data.Entity.EntityState.Modified : System.Data.Entity.EntityState.Added;
                _MoviesDBEntitiesContext.SaveChanges();
                if (model.MovieID > 0)
                {
                    _MoviesDBEntitiesContext.tblMovieActors.RemoveRange(_MoviesDBEntitiesContext.tblMovieActors.Where(w => w.MovieID == model.MovieID));
                    _MoviesDBEntitiesContext.SaveChanges();
                }
                foreach (int actorID in model.Actors)
                {
                    if (!_MoviesDBEntitiesContext.tblMovieActors.Any(fk => fk.ActorID == actorID && fk.MovieID == _movieModel.MovieID))
                    {
                        tblMovieActor _movieActor = new tblMovieActor();
                        _movieActor.ActorID = actorID;
                        _movieActor.MovieID = _movieModel.MovieID;
                        _MoviesDBEntitiesContext.tblMovieActors.Add(_movieActor);
                        _MoviesDBEntitiesContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool DeleteMovie(int MovieID)
        {
            if (MovieID > 0)
            {
                var movie = _MoviesDBEntitiesContext.tblMovies.FirstOrDefault(f => f.MovieID == MovieID);
                if (movie != null)
                {
                    _MoviesDBEntitiesContext.tblMovies.Remove(movie);
                    _MoviesDBEntitiesContext.SaveChanges();
                    return true;
                }


            }

            return false;
        }
        public bool AddFilmCrew(FilmCrewModel model)
        {
            try
            {
                tblFilmCrew _tblFilmCrew = new tblFilmCrew();
                _tblFilmCrew.ID = model.ID;
                _tblFilmCrew.Name = model.Name;
                _tblFilmCrew.Sex = model.Sex.ToString();
                _tblFilmCrew.DOB = model.DOB;
                _tblFilmCrew.Bio = model.Bio;
                _tblFilmCrew.DepartmentType = model.DepartmentType;
                _MoviesDBEntitiesContext.Entry(_tblFilmCrew).State = System.Data.Entity.EntityState.Added;
                _MoviesDBEntitiesContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
