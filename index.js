// Require libs and set up port
var http = require('http'), fs = require('fs'), gruntfile = require('./gruntfile'), grunt = require('grunt');
const PORT = process.env.PORT || 8000;

// Handle requests
function handleRequest(req, res) {
	if (req.url === '/api/colors') {
		res.setHeader('Access-Control-Allow-Origin', '*');
		res.setHeader('Access-Control-Request-Method', '*');
		res.setHeader('Access-Control-Allow-Methods', '*');
		res.setHeader('Access-Control-Allow-Headers', '*');
		fs.readdir('./css/colors', (err, files) => {
			if (err) {
				res.end('{}')
			} else {
				let f = files.filter(x => x !== '_color-scheme.less').map(x => x.replace('.less', ''))
				res.end(JSON.stringify(f))
			}
		})
    } else if (req.url === '/js') {
        res.setHeader('Content-Type', 'text/javascript');
        res.end(fs.readFileSync('js/script.js'));
    } else if (req.url === '/test/styrdok')
        res.end(fs.readFileSync('test/styrdok.html'));
    else if (req.url === '/test/tech')
        res.end(fs.readFileSync('test/tech.html'));
    else {
        res.setHeader('Content-Type', 'text/css');
        res.end(fs.readFileSync('css/compiled.css'));
    }
}

// Start server
gruntfile(grunt);
grunt.tasks(['default'], {}, () =>
    http.createServer(handleRequest).listen(PORT, () => console.log("Server listening on port %s", PORT))
);