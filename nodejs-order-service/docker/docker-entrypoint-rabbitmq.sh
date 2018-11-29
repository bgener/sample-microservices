#!/bin/sh

HOSTNAME=`env hostname`

echo $HOSTNAME

# Wait while Rabbitmq server is starting
( 	rabbitmqctl wait /var/lib/rabbitmq/mnesia/rabbit\@$HOSTNAME.pid ; \

# Create Rabbitmq user
rabbitmqctl add_user $RABBITMQ_USER $RABBITMQ_PASSWORD 2>/dev/null ; \
rabbitmqctl set_user_tags $RABBITMQ_USER administrator ; \
rabbitmqctl set_permissions -p / $RABBITMQ_USER  ".*" ".*" ".*" ; \
echo "*** User '$RABBITMQ_USER' with password '$RABBITMQ_PASSWORD' completed. ***" ; \
echo "*** Log in the WebUI at port 15672 (example: http:/localhost:15672) ***" ; \

# Setup messaging topology (queues, exchanges and bindings)
echo "*** Importing broker definition ***" ; \
rabbitmqadmin import /etc/rabbitmq/definitions.json ; \

## Set a dead letter policy on all queues
# rabbitmqctl set_policy DLX ".*" '{"dead-letter-exchange":"global.dead-letter"}' --apply-to queues ; \

##Enable shovel plugin to move messages between queues
#rabbitmq-plugins enable rabbitmq_shovel rabbitmq_shovel_management ; \
) &

# $@ is used to pass arguments to the rabbitmq-server command.
# For example if you use it like this: docker run -d rabbitmq arg1 arg2,
# it will be as you run in the container rabbitmq-server arg1 arg2
rabbitmq-server $@