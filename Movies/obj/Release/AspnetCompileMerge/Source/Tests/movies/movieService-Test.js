window.assert = chai.assert;

describe('movieService', function () {
    // define variables for the services we want to access in tests
    var movieService, $q, $http;


    beforeEach(function () {
        // load the module we want to test
        module('movieApp');

        // inject the services we want to test
        inject(function (_movieService_, _$q_, _$http_) {
            movieService = _movieService_;
            $q = _$q_;
            $http = _$http_;
        })
    });

    describe('#GetMovies', function () {
        it('should log the message "something done!"', function () {

            // Act
            movieService.GetMovies();

        });
    });
});