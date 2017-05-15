using MoviesBusinessLayer.Interfaces;
using MoviesBusinessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MoviesDataAccessLayes.Models;
using System.Web;
using System.IO;

namespace Movies.Controllers
{

    [RoutePrefix("api/movie")]
    public class MovieController : ApiController
    {

        #region Constructor & Variables

        private IMovies _MoviesReposotory;
        public MovieController()
        {
            this._MoviesReposotory = new MoviesRepository();
        }

        #endregion

        [HttpGet]
        [Route("getMovie")]
        public IHttpActionResult Get()
        {
            return Ok<List<MoviesDataAccessLayes.Models.MoviesGetModel>>(_MoviesReposotory.GetAllMovies());
        }

        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult Delete(int MovieID)
        {
            return Ok<bool>(_MoviesReposotory.DeleteMovie(MovieID));
        }


        [HttpGet]
        [Route("new")]
        public IHttpActionResult New(int MovieID = 0)
        {
            return Ok<MovieModel>(_MoviesReposotory.GetMovie(MovieID));
        }

        [HttpPost]
        [Route("addUpdateMovie")]
        public HttpResponseMessage AddUpdateMovie()
        {
            try
            {
                var movieID = HttpContext.Current.Request.Form["MovieID"];
                var name = HttpContext.Current.Request.Form["Name"];
                var yearOfRelease = HttpContext.Current.Request.Form["YearOfRelease"];
                var plot = HttpContext.Current.Request.Form["Plot"];
                var producerID = HttpContext.Current.Request.Form["ProducerID"];
                var actors = HttpContext.Current.Request.Form["Actors"].Split(',').Select(s => Convert.ToInt32(s)).ToArray();

                var fileData = HttpContext.Current.Request.Files["file"];
                byte[] data = null;
                string ContentType = string.Empty;
                if (fileData != null && fileData.ContentLength > 0)
                {
                    MemoryStream target = new MemoryStream();
                    fileData.InputStream.CopyTo(target);
                    data = target.ToArray();
                    ContentType = fileData.ContentType;
                }


                MovieModel movieModel = new MovieModel();

                movieModel.MovieID = Convert.ToInt32(movieID);
                movieModel.Name = name;
                movieModel.YearOfRelease = Convert.ToInt16(yearOfRelease);
                movieModel.Plot = plot;
                movieModel.ProducerID = Convert.ToInt32(producerID);
                movieModel.Actors = actors;

                if (!string.IsNullOrEmpty(ContentType) && data.Length > 0)
                {
                    movieModel.Poster = new Base64Image(ContentType, data).ToString();
                    movieModel.ContentType = ContentType;
                }

                return Request.CreateResponse<bool>(HttpStatusCode.OK, _MoviesReposotory.AddUpdateMovie(movieModel));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString() + "Error occured while executing api method");
            }
        }

        [HttpGet]
        [Route("getActors")]
        public IHttpActionResult getActors()
        {
            return Ok<List<DropdownList>>(_MoviesReposotory.GetFilmCrewList(DepartmentType.Actor));
        }

        [HttpGet]
        [Route("getProducers")]
        public IHttpActionResult getProducers()
        {
            return Ok<List<DropdownList>>(_MoviesReposotory.GetFilmCrewList(DepartmentType.Producer));
        }

        [HttpPost]
        [Route("filmCrew")]
        public IHttpActionResult AddFileCrew(FilmCrewModel model)
        {
            return Ok<bool>(_MoviesReposotory.AddFilmCrew(model));
        }

    }
}
