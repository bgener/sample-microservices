# Order service

This service is used to manage orders. It provides the full information of a order, such as reservation dates, pricing, order status etc.

## Running with npm

* To build and run the service, run the following command:

```bash
npm install
npm start
```

* Direct your browser to

```bash
http://localhost:3000/ping
```

* Login to RabbitMQ management portal and send the message to the `command.fanout` exchange

```json
{
    //NOTE: the name property will be removed, AMQP routing_key header will be used instead
    "name":"createOrder",
    "propertyCode":"HILTONAMS-01"
}
```

## Running with docker

* To run the service in docker container, execute the following command:

```bash
 docker-compose -f docker-compose.dev.yml up -d --build
```

## Running unit tests

* To run unit tests, execute the following command:

```bash
npm run test-unit
```

* To run unit tests with code coverage, execute the following command:

```bash
npm run test-coverage
```

* To run integration tests, execute the following command:

```bash
npm run test-integration
```

## Messaging topology

The rabbitmq broker is configured as demonstrated below. Key points to note:

* `commands.fanout` exchange is used for routing a command to single queue
* `events.fanout` exchange is used for broadcasting the events to all subscribed queues
* `error.fanout` exchange collects all unhandled errors, i.e. when the service does not use a dedicated error queue. This is specified on a queue level (x-dead-letter-exchange: error.fanout) or globally via policies
* `audit` queue is used to keep track of all the messages sent throught the rabbitmq broker

```apache
  |_commands.fanout
    |_audit.queue  (routing_key=*)
    |_order.queue  (routing_key=createOrder)

  |_events.fanout
    |_audit.queue  (routing_key=*)
    |_order.orderCreated.fanout (routing_key=orderCreated)
      |_apigateway.queue
    |_order.orderConfirmed.fanout (routing_key=orderConfirmed)
      |_apigateway.queue
      |_notification.queue

  |_error.fanout
    |_error.queue
```

## Troubleshoot

* Ensure that both the service and rabbitmq docker containers are up & running

```bash
docker ps -a
```

You should see the result similar to the following (pay an attention to the STATUS column):

```bash
CONTAINER ID        IMAGE                    CREATED             STATUS              PORTS
0573a88d365b        order_order-service      4 weeks ago         Up 5 minutes        0.0.0.0:3000->3000/tcp
10b2ec446844        order_rabbitmq           4 weeks ago         Up 5 minutes        0.0.0.0:5672->5672/tcp, 0.0.0.0:15672->15672/tcp
```


* To see the RabbitMQ management portal, navigate to the url and use default credentials test_user/test_user

```bash
http://server-name:15672/
```

* To ensure that the service is up & running, direct your browser to

```bash
http://localhost:3000/ping
```

* To see the logs, find the order_service.log file in the service's root directory
