// Require libs and set up port
var http = require('http'), fs = require('fs'), gruntfile = require('./gruntfile'), grunt = require('grunt');
const PORT = process.env.PORT || 8000;

// Handle requests
function handleRequest(req, res) {
    if (req.url === '/test/styrdok')
        res.end(fs.readFileSync('test/styrdok.html'));
    else if (req.url === '/test/tech')
        res.end(fs.readFileSync('test/tech.html'));
    else {
        res.set('Content-Type', 'text/css');
        res.header('Content-Type', 'text/css');
        res.end(fs.readFileSync('css/compiled.css'));
    }
}

// Start server
gruntfile(grunt);
grunt.tasks(['default'], {}, () =>
    http.createServer(handleRequest).listen(PORT, () => console.log("Server listening on port %s", PORT))
);