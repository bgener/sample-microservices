/**
 * Generic command handler that receives the command via message bus 
 * and resolves certain command handler
 */
const orderQueue = 'order';

const amqp = require('amqplib'),
    async = require('async'),
    log4js = require('log4js'),
    dispatcher = require('./dispatcher');

const logger = log4js.getLogger('messageReceiver');


/**
 * Message receiver
 */
const createChannel = (connection) => {
    logger.info('Successfully connected to message broker');

    process.once('SIGINT', () => {
        logger.fatal('[AMQP] SIGINT, closing the message broker connection...');

        //release connection on NodeJs unconditional termination, e.g. crash
        connection.close();

        logger.fatal('error', '[AMQP] SIGINT, closed the message broker connection');
    });

    return connection.createChannel();
};

const processMessage = (message) => {
    var body = message.content.toString();
    logger.info(" [x] Received message '%s'", body);

    var messageBody = JSON.parse(body);

    try {
        dispatcher.dispatch(messageBody);
    }
    catch (error) {
        logger.error(" [!] Message dispatching %s", error.message);
        return;
    }

    logger.info(' [x] Done');
};

const consumeMessages = (channel) => {
    logger.debug('Created new channel');

    //connect to the existing queue
    channel.checkQueue(orderQueue)
        .then(() => channel.prefetch(1))
        .then(() => {
            logger.info(" [*] Waiting for messages...");
            var options = { noAck: false };

            channel.consume(orderQueue, (message) => {
                try {
                    processMessage(message);
                    //acknowledge success
                    channel.ack(message);
                } catch (error) {
                    logger.error(" [!] Failed to process message %s", error.message);

                    //TODO: implement retry with configurable number of attempts

                    //reject the message amd redrive it to the dead-letter queue
                    channel.nack(message, false, false);
                }
            }, options);
        });
};

const receiver = {

    start: () => {
        logger.info('Connecting to the message broker: %s', process.env.MESSAGE_BROKER_URL);

        //retry 10 times with exponential backoff
        // (i.e. intervals of 1000, 2000, 4000, 8000, 16000, ... milliseconds)
        const retryOptions = {
            times: 10,
            interval: (retryCount) => 500 * Math.pow(2, retryCount)
        };

        //reconnect on error
        async.retry(retryOptions, callback => {
            //retryable callback
            amqp.connect(process.env.MESSAGE_BROKER_URL)
                .then(connection => createChannel(connection))
                .then(channel => consumeMessages(channel))
                .catch(error => {
                    logger.warn('[AMQP] Connection error, retrying... ' + error);
                    callback(error);
                });
        },
            (error, result) => {
                //final callback, either success or error
                if (error) {
                    logger.error('[AMQP] Failed to connect to the message broker: %s', error);
                    return;
                }
            });
    }
};

module.exports = receiver;