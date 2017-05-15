require.config({
    baseUrl: 'app',
    urlArgs: "v=" + (new Date()).getTime()
});

require([
'app',
'movies/controllers/movieController',
'movies/controllers/managemovieController'
],
function () {
    angular.bootstrap(document, ['movieApp']);
});