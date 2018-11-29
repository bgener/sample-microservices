/**
 *  Dependencies.
 */
const dotenv = require('dotenv'),
    express = require('express'),
    log4js = require('log4js'),
    messageReceiver = require('./messageReceiver');

/**
 * Load global configuration.
 */
dotenv.config();


/**
* Configure logger
*/
log4js.configure({
    appenders: {
        console: { type: 'console', layout: { type: 'coloured' } },
        // out: { type: 'stdout' },
        app: { type: 'file', filename: 'order.log', maxLogSize: 10485760, backups: 3, compress: true }
    },
    categories: {
        default: { appenders: ['console', 'app'], level: 'debug' }
    }
});
const logger = log4js.getLogger('server');


/**
 * Create Express server.
 */
const app = express();
app.use(log4js.connectLogger(log4js.getLogger("http"), { level: 'auto' }));
app.set('port', process.env.PORT || 3000);

const port = app.get('port');

/**
 * Start Express server.
 */
app.listen(port, () => {
    logger.info('Order service is running at http://localhost:%d \n Press CTRL-C to stop\n', port);
});

app.get('/ping', (req, res) => {
    res.send("OK, order service is up and running.");
});

/**
 * Connect to message broker and receive messages (async)
 */
messageReceiver.start();

module.exports = app;