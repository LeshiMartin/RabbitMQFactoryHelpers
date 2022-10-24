# RabbitMQFactoryHelpers

#Helper Containing a Set of factories for constructing a Sender/Receiver Object For RabbitMQ  With Persistent Connection.

#Usage

``` c#
using RabbitMQFactoryHelpers;
var sender = SenderFactory.GenerateSender(queueName:quename,hostName=localhost,port=5672);
sender.BasicPublish(exchangeName: "", routingKey: "",mandatory:true basicProperties: null, body: body);

//Receiver
    