define(['app', 'movies/services/movieService'], function (app, movieService) {
    var injectParams = ['$scope', '$location', 'movieService', '$window']
    var movieController = function ($scope, $location, movieService, $window) {
        $scope.title = "Movie List";

        movieService.GetMovies().then(function (response) {
            $scope.Movies = response.data;
        });

        $scope.newmovie = function () {
            $location.path("/movie/addMovie");
        };

        $scope.editMovie = function (model) {
            var path = "/movie/" + model.id + "/editMovie";
            $location.path(path);
        }
        $scope.deleteMovie = function (model) {
            movieService.DeleteMovie(model.id).then(function (response) {
                $window.location.reload();
            });
        }
    }
    movieController.$inject = injectParams;
    app.controller('movieController', movieController);
});
