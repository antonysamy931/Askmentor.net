var myApp = angular.module('myApp', ["ratings"]);
myApp.controller('feedController', ['$scope', 'feedService', function ($scope, feedService) {
    $scope.emailPattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    $scope.rating = 1;
    $scope.InsertFeed = function () {
        if ($scope.feedForm.$valid) {
            var date = Date();
            function FeedForm(Id, Name, Email, Rating, Comments, Date) {
                this.FeedId = Id;
                this.Name = Name;
                this.Email = Email;
                this.Rating = Rating;
                this.Comments = Comments;
                this.CurrentDate = Date;
            }
            var feedData = new FeedForm(0, $scope.name, $scope.email, $scope.rating, $scope.comments, date);
            feedService.feedInsert(feedData).success(function (result) {
                if (result.Result === "Success") {
                    window.location.replace('/Home/Feedback');
                }
                else {
                    alert(result.Result);
                }
            });
        }
    }
}]);

myApp.service('feedService', function ($http) {
    this.feedInsert = function (data) {
        return $http.post('/Home/InsertFeedback' , data);
    }
});
