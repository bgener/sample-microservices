"use strict";

const _ = require('lodash'),
    path = require("path"),
    fs = require("fs"),
    log4js = require('log4js');

const logger = log4js.getLogger('dispatcher');

/**
 * Resolve the handlers during startup
 */
const normalizedPath = path.join(__dirname, "../domain/handlers");
const handlers = fs.readdirSync(normalizedPath);

/**
 * Dispatches the message to concrete message handler following the naming convention:
 * handler is named after message, e.g. createOrder
 */
var dispatcher = {
    dispatch: function (message) {
        var handlerName = _.find(handlers, function (fileName) {
            return _.trimEnd(fileName, '.js') === message.name;
        });
        if (!handlerName) {
            logger.error("Could not find handler for message: '%s'", message);
        }

        //load and execute the handler
        var handlerPath = path.join("../domain/handlers", handlerName);
        var handler = require(handlerPath);
        handler.handle(message);
    }
};

module.exports = dispatcher;