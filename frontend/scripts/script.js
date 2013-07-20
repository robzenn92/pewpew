function mainCtrl($scope) {
	$scope.artist = "Artist";
	$scope.title = "Title";
	$scope.mood = "Relaxed";
	$scope.color = "#9eb7ee";
	$scope.image = "http://farm8.staticflickr.com/7106/7557578150_8249cf6b06_b.jpg";
	$scope.audio = "http://tympanus.net/Development/AudioPlayer/audio/BlueDucks_FourFlossFiveSix.mp3";
}

$(function() {
    $('audio').audioPlayer();
});