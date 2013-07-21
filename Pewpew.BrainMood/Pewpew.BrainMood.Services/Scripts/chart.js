  $(function() {

		// We use an inline data source in the example, usually data would
		// be fetched from a server

		var data = [],
			maxValori = 5,
			res = [];

		function readNewValue() {

			if (data.length === maxValori + 1) {
				data = data.slice(1);
			}

			// Leggo il dato
			var values = {
				meditazione: 0,
				attenzione: 0
			};
			res = [];
			$.getJSON("/api/graph", function(asd) {
				data.push(asd.meditation);
				j = 0;
				for (var i = data.length - 1; i >= 0 ; i--) {
					res.push([maxValori - j++, data[i]]);
				}
				update();
			});
		}

		var updateInterval = 5000;
		readNewValue();
		var plot = $.plot("#chart", [ res ], {
			series: {
				shadowSize: 0	// Drawing is faster without shadows
			},
			yaxis: {
				min: 0,
				max: 50000
			},
			xaxis: {
				min: 0,
				max: maxValori - 1
			}
		});

		function update() {
			plot.setData([res]);
			plot.draw();
			setTimeout(readNewValue, updateInterval);
		}
	});
