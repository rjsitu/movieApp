define(['app', 'movies/services/movieService'], function (app, movieService) {
    var injectParams = ['$scope', '$location', '$stateParams', 'movieService', 'ngDialog']
    var managemovieController = function ($scope, $location, $stateParams, movieService, ngDialog) {
        $scope.title = "Manage Movie";
        $scope.selectedActors = [];
        $scope.selectedProducer = "";
        $scope.Actors = [];
        $scope.Producers = [];
        $scope.YearOfRelease = new Date();
        $scope.opened = false;

        $scope.open = function ($event) {
            $scope.opened = true;
        };

        $scope.dateOptions = {
            formatYear: 'yyyy',
            startingDay: 1,
            minMode: 'year'
        };

        $scope.formats = ['yyyy'];
        $scope.format = $scope.formats[0];

        $scope.actorOptions = {
            displayProp: 'Value',
            idProperty: 'Key',
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true,
            smartButtonMaxItems: 5,
            showCheckAll: false,
            showUncheckAll: false
        };

        $scope.MovieID = $stateParams && $stateParams.id ? $stateParams.id : 0;

        $scope.LoadDefaultMovieData = function () {
            movieService.GetMovieModel($scope.MovieID).then(function (response) {
                $scope.GetProducers();
                $scope.GetActors();
                $scope.MoviesData = response.data;
                $scope.YearOfRelease = new Date(parseInt($scope.MoviesData.YearOfRelease), 0, 1);
            });
        }

        $scope.GetProducers = function () {
            movieService.getProducers().then(function (response) {
                $scope.Producers = response.data;
                $scope.selectedProducer = $scope.MoviesData.ProducerID.toString();
            });
        }

        $scope.GetActors = function () {
            movieService.getActors().then(function (response) {
                $scope.Actors = response.data;
                $scope.selectedActors = $.grep($scope.Actors, function (ele, idx) {
                    return $.inArray(ele.Key, $scope.MoviesData.Actors) >= 0;
                });
            });
        }

        $scope.LoadDefaultMovieData();

        $scope.addMovie = function () {
            var arrayActors = [];
            var payload = new FormData();

            if ($scope.addUpdateMovieForm.$valid) {

                angular.forEach($scope.selectedActors, function (value, key) {
                    this.push(value.Key);
                }, arrayActors);

                $scope.MoviesData.Actors = arrayActors;
                $scope.MoviesData.ProducerID = $scope.selectedProducer;
                $scope.MoviesData.YearOfRelease = new Date($scope.YearOfRelease).getFullYear();

                payload.append("MovieID", $scope.MoviesData.MovieID);
                payload.append("Name", $scope.MoviesData.Name);
                payload.append("YearOfRelease", $scope.MoviesData.YearOfRelease);
                payload.append("Plot", $scope.MoviesData.Plot);
                payload.append("ProducerID", $scope.MoviesData.ProducerID);
                payload.append("Actors", $scope.MoviesData.Actors);

                var fileInput = document.getElementById('newPoster');

                if (fileInput.files.length === 0 && $scope.MoviesData.MovieID === 0) return;

                var file = fileInput && (fileInput.files.length > 0) ? fileInput.files[0] : $scope.MoviesData.Poster;
                payload.append("file", file);

                movieService.AddUpdateMovie(payload).then(function (response) {
                    $scope.cancelMovie();
                });
            }
        };

        $scope.AddProducer = function () {
            ngDialog.open({
                template: 'addProducerDialogId',
                scope: $scope,
                controller: ['$scope', '$timeout', function ($scope, $timeout) {
                    $scope.ProducerName = "";
                    $scope.Sex = "M";
                    $scope.DOB = new Date();
                    $scope.Bio = "";
                    $scope.opened = false;

                    $scope.open = function ($event) {
                        $scope.opened = true;
                    };

                    $scope.dateOptions = {
                        datepickerMode: 'day',
                        minMode: 'day',
                        maxMode: 'year',
                        showWeeks: true,
                        startingDay: 0,
                        yearRange: 20,
                        minDate: null,
                        maxDate: null
                    };

                    $scope.formats = ['dd/MM/yyyy'];
                    $scope.format = $scope.formats[0];

                    $scope.SaveProducer = function () {
                        var producerViewModel = {
                            ID: 0,
                            Name: $scope.ProducerName,
                            Sex: $scope.Sex,
                            DOB: $scope.DOB,
                            Bio: $scope.Bio,
                            DepartmentType: 1
                        }

                        movieService.AddfilmCrew(producerViewModel).then(function (response) {
                            $scope.$parent.GetProducers();
                            $scope.close();
                        });

                    }

                    $scope.close = function () {
                        ngDialog.close();
                    };

                }],
                className: 'ngdialog-theme-default'
            });
        }

        $scope.AddActor = function () {
            ngDialog.open({
                template: 'addActorDialogId',
                scope: $scope,
                controller: ['$scope', '$timeout', function ($scope, $timeout) {
                    $scope.ActorName = "";
                    $scope.Sex = "M";
                    $scope.DOB = new Date();
                    $scope.Bio = "";
                    $scope.opened = false;

                    $scope.open = function ($event) {
                        $scope.opened = true;
                    };

                    $scope.dateOptions = {
                        datepickerMode: 'day',
                        minMode: 'day',
                        maxMode: 'year',
                        showWeeks: true,
                        startingDay: 0,
                        yearRange: 20,
                        minDate: null,
                        maxDate: null
                    };

                    $scope.formats = ['dd/MM/yyyy'];
                    $scope.format = $scope.formats[0];

                    $scope.SaveActor = function () {
                        var actorViewModel = {
                            ID: 0,
                            Name: $scope.ActorName,
                            Sex: $scope.Sex,
                            DOB: $scope.DOB,
                            Bio: $scope.Bio,
                            DepartmentType: 2
                        }

                        movieService.AddfilmCrew(actorViewModel).then(function (response) {
                            $scope.$parent.GetActors();
                            $scope.close();
                        });

                    }

                    $scope.close = function () {
                        ngDialog.close();
                    };

                }],
                className: 'ngdialog-theme-default'
            });
        }

        $scope.cancelMovie = function () {
            $location.path("/movie");
        }

        $scope.setFile = function (element) {
            $scope.currentFile = element.files[0];
            var reader = new FileReader();

            reader.onload = function (event) {
                $scope.MoviesData.Poster = event.target.result;
                $scope.$apply();

            }

            reader.readAsDataURL(element.files[0]);
        }
    }

    managemovieController.$inject = injectParams;
    app.controller('managemovieController', managemovieController);
});

