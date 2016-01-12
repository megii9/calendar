/**
 * Main AngularJS Web Application
 */
var app = angular.module('calendar', [
  'ngRoute',
  'ngCookies',
  'pascalprecht.translate'
]);

/**
 * Configure translations
 */
app.config(['$translateProvider', function ($translateProvider) {
    $translateProvider
        .useStaticFilesLoader({
            prefix: '/public/langs/',
            suffix: '.json'
        });
}]);


/**
 * Configure the Routes
 */
app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        // Calendar
        .when("/", { templateUrl: "templates/demo.html", controller: "CalendarCtrl" })
        .otherwise({
            redirectTo: "/"
        });
}]);



app.run(['$rootScope', '$cookies', '$translate', function ($rootScope, $cookies, $translate) {

    var lang = $cookies.get('lang') || 'pl';
    $translate.use(lang);

    $rootScope.lang = lang;
    $rootScope.theme = 'default';
}
]);


/**
 * Services
 */
app.factory('eventService', [
    '$http', function ($http) {

        return {
            getEvents: function (period) {
                var config = {
                    params: {
                        date: new Date(),
                        period: period
                    }
                };
                return $http.get("/api/event", config)
                    .then(function (response) {
                        return response.data;
                    });;
            },
            remove: function (eventId) {
                var config = {
                    params: {
                        eventId: eventId
                    }
                };
                $http.delete("/api/event", config);
            }
        }
    }
]);

/**
 * Controllers
 */
app.controller("LangCtrl", [
    "$scope", "$translate", "$cookies",
    function ($scope, $translate, $cookies) {
        $scope.setLanguage = function (lang) {
            $cookies.put('lang', lang);
            $translate.use(lang);
        }
    }
]);

app.controller('CalendarCtrl', [
    "$scope", "eventService",
    function ($scope, eventService) {
        var addEventsToScope = function (events) {
            $scope.events = events;
        };

        $scope.last = function () { };

        $scope.getDaysEvents = function () {
            $scope.last = $scope.getDaysEvents;
            eventService.getEvents("Day").then(addEventsToScope);
        };

        $scope.getWeeksEvents = function () {
            $scope.last = $scope.getWeeksEvents;
            eventService.getEvents("WorkingWeek").then(addEventsToScope);
        };

        $scope.getMonthsEvents = function () {
            $scope.last = $scope.getMonthsEvents;
            eventService.getEvents("Month").then(addEventsToScope);
        }

        $scope.remove = function (eventId) {
            eventService.remove(eventId);
            $scope.last();
        }

    }
]);


