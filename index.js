// Require libs and set up port
var http = require('http'), fs = require('fs');
const PORT = process.env.PORT || 8000;

// Handle requests
function handleRequest(req, res) {
    if (req.url === '/test/styrdok')
        res.end(fs.readFileSync('test/styrdok.html'));
    else if (req.url === '/test/tech')
        res.end(fs.readFileSync('test/tech.html'));
    else
        res.end(fs.readFileSync('css/compiled.css'));
}

// Start server
http.createServer(handleRequest).listen(PORT, () => console.log("Server listening on: http://localhost:%s", PORT));