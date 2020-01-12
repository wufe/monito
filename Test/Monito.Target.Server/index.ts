import Express from 'express';
import { createWriteStream } from 'fs';

const WebServer = Express();

setTimeout(() => {
	console.log('Creating 100k test links..')
	const writeStream = createWriteStream('./test-links.txt');
	const genUrl = () => 
		'http://localhost:8009/' + Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
	for (let i = 0; i < 100000; i++)
		writeStream.write(genUrl() + '\n', 'utf8');
	writeStream.on('finish', () => {
		console.log('Done');
	})
});

WebServer.all("*", (req, res) => {
	// const statusCodes = [
	// 	200, 201, 202, 203, 204, 205, 206, 207, 208,
	// 	300, 301, 302, 303, 304, 305, 306, 308, 307,
	// 	400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 420, 422, 426, 429, 449, 451,
	// 	500, 501, 502, 503, 504, 505, 509];
	const statusCodes = [
		200,
		403, 404,
		500, 502];
	console.log(req.headers)
	res.sendStatus(statusCodes[Math.floor(Math.random() * statusCodes.length)])
})

const port = 8009;
console.log(`Server listening on port ${port}`);
WebServer.listen(port);