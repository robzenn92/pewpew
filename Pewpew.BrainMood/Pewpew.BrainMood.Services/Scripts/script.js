var app = angular.module('brain-mood', ['ngResource']);

app.factory('mood', function($resource) {
	return $resource('/api/mood/');
});

function mainCtrl($scope, mood) {

	$scope.update = function() {
		$scope.mood = mood.get(function() {
			nextSong = $('<audio preload="auto" />');
			nextSong[0].src = $scope.mood.songUrl;

			$(".audio").html(nextSong);
			$("audio").audioPlayer();
			$("audio")[0].play();

			$("audio").on("ended", function () {
				$scope.update();
			});
		});
	};

	$scope.update();
}

