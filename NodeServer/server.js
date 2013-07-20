var http = require('http');
var SerialPort = require('serialport').SerialPort;

var options = 
{
	host: '192.168.2.100',
	path: '/azure/api/',
	port: 80,
	method: 'POST'
};

var serialPort = new SerialPort('/dev/ttymxc3', { baudrate: 9600 });

var sRead = '';

serialPort.on('data', function(data)
{	
	sRead += data.toString();
	
	if(sRead.indexOf('\n') === -1 && sRead.indexOf('\r') === -1) return;
	
	sRead = sRead.replace('\n', '').replace('\r', '');
	
	if(sRead.length == 0) return;

	var req = http.request(options, function(resp)
	{	
		console.log('STATUS: ' + resp.statusCode);
		console.log(sRead);
		
		resp.on('data', function(d)
		{
			console.log(d.toString());
		});
	});
	
	req.on('error', function(e)
	{
		console.log('ERROR! ' + e.message);
	});
	
	var vals = sRead.split(';');
	
	console.log("Read: " + sRead);
	console.log("Send: " + 'att=' + vals[0] + '&med=' + vals[1]);
	
	req.write('att=' + vals[0] + '&med=' + vals[1]);
	req.end();
	
	sRead = '';
});

http.createServer(function(req, res)
{
	res.writeHead(200, { 'Content-Type': 'text/plain' });
	res.end('Hello node!');
}).listen(1337, '192.168.2.166');

console.log('Server running on localhost!');
