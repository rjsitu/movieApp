define(['app'], function (app) {
    
    var injectParams = ['$q', '$http']
    var movieService = function ($q, $http) {

        var baseURL = ""; //'http://localhost/Movie';

        this.GetMovies = function () {
            var $deferred = $q.defer();
            $http({
                url: baseURL + '/api/movie/getMovie',
                method: "GET",
                data: {},
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                $deferred.resolve(response);
            }, function (err, status) {
                $deferred.reject(err);
            });
            return $deferred.promise;
        }
        this.DeleteMovie = function (movieId) {

            var $deferred = $q.defer();
            $http({
                url: baseURL + '/api/movie/delete',
                method: "DELETE",
                data: {},
                params: { MovieID: movieId },
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                $deferred.resolve(response);
            }, function (err, status) {
                $deferred.reject(err);
            });
            return $deferred.promise;
        }
        this.GetMovieModel = function (movieId) {
            var $deferred = $q.defer();
            $http({
                url: baseURL + '/api/movie/new',
                method: "GET",
                params: { MovieID: movieId },
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                $deferred.resolve(response);
            }, function (err, status) {
                $deferred.reject(err);
            });
            return $deferred.promise;
        }

        this.AddUpdateMovie = function (data) {
            var $deferred = $q.defer();
            $http({
                url: baseURL + '/api/movie/addUpdateMovie',
                method: "POST",
                data: data,
                headers: { 'Content-Type': undefined }
            }).then(function (response) {
                $deferred.resolve(response);
            }, function (err, status) {
                $deferred.reject(err);
            });
            return $deferred.promise;
        };

        this.AddfilmCrew = function (data) {
            var $deferred = $q.defer();
            $http({
                url: baseURL + '/api/movie/filmCrew',
                method: "POST",
                data: JSON.stringify(data) || {},
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                $deferred.resolve(response);
            }, function (err, status) {
                $deferred.reject(err);
            });
            return $deferred.promise;
        };

        this.getActors = function () {
            var $deferred = $q.defer();
            $http({
                url: baseURL + '/api/movie/getActors',
                method: "GET",
                data: {},
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                $deferred.resolve(response);
            }, function (err, status) {
                $deferred.reject(err);
            });
            return $deferred.promise;
        }

        this.getProducers = function () {
            var $deferred = $q.defer();
            $http({
                url: baseURL + '/api/movie/getProducers',
                method: "GET",
                data: {},
                headers: { 'Content-Type': 'application/json' }
            }).then(function (response) {
                $deferred.resolve(response);
            }, function (err, status) {
                $deferred.reject(err);
            });
            return $deferred.promise;
        }
    }
    movieService.$inject = injectParams;
    app.service('movieService', movieService);
});
