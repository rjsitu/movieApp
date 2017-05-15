define([], function () {

    var app = angular.module('movieApp', ['ui.router', 'angularjs-dropdown-multiselect', 'ngDialog', 'ui.bootstrap', 'ngSanitize']);

    app.config(['$stateProvider', '$urlRouterProvider', 'ngDialogProvider', '$httpProvider', function ($stateProvider, $urlRouterProvider, ngDialogProvider, $httpProvider) {

        ngDialogProvider.setDefaults({
            className: 'ngdialog-theme-default',
            plain: false,
            showClose: true,
            closeByDocument: true,
            closeByEscape: true,
            appendTo: false,
            preCloseCallback: function () {
                console.log('default pre-close callback');
            }
        });

        $httpProvider.interceptors.push('movieHttpInterceptor');
       
        $urlRouterProvider.otherwise('/movies');

        $stateProvider.state('movies', {
            url: '/movies',
            templateUrl: 'app/movies/views/movieView.html',
            controller: 'movieController'
        }).state('addMovie', {
            url: '/movie/addMovie',
            templateUrl: 'app/movies/views/manageMovie.html',
            controller: 'managemovieController'
        }).state('editmovie', {
            url: '/movie/:id/editMovie',
            templateUrl: 'app/movies/views/manageMovie.html',
            controller: 'managemovieController'
        });
    }]).factory('movieHttpInterceptor', ["$window", "$q", function ($window, $q) {
        return {
            request: function (config) {
                $('#lodingDiv').show();
                config.headers = config.headers || {};
                return config;
            },
            requestError: function (rejection) {
                $('#lodingDiv').hide();
                $q.reject(rejection);
            },
            response: function (response) {
                $('#lodingDiv').hide();
                return response || $q.when(response);
            },
            responseError: function (response) {
                $('#lodingDiv').hide();
                if (response != null && response.status == 401) {

                }
                return response || $q.when(response);
            }
        }
    }]);

    return app;
});