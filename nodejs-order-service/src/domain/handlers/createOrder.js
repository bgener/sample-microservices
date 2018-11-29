
/**
 *  Create order command handler.
 */

const log4js = require('log4js');
const logger = log4js.getLogger('createOrderHandler');

var handler = {

    handle: (message) => {
        logger.info("Handling the message: '%s'", JSON.stringify(message));

        if (!message.propertyCode) {
            throw new Error("propertyCode may not be null!");
        }

        //TODO: persist the order into database
        //TODO: publsh an event orderCreated
    }
};

module.exports = handler;