var app = angular.module('brainMood', ['ngResource']);

app.factory('mood', function($resource) {
	return $resource('/api/mood/');
});

function mainCtrl($scope, mood) {

	$scope.update = function() {
		$scope.mood = mood.get(function() {
			nextSong = $('#nextSong');
			nextCall = nextSong.duration * 1000;
			currentSong = $('#currentSong');
			currentSong.attr("src", nextSong.attr("src"));
			currentSong.audioPlayer();

			sleep(nextCall);

			$scope.update();
		});
	};

	$scope.update();
}

