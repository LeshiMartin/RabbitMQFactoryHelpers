// See https://aka.ms/new-console-template for more information
using RabbitMQFactoryHelpers.Factories;
using System.Text;

Console.WriteLine ("Hello, World!");
var sender = SenderFactory.GenerateSender ("base");
Console.WriteLine ("Type what you want to send");
var data =  Console.ReadLine ();
//                channel.BasicPublish("", routingKey: RabbitMqQueueNames.UserDeleted, basicProperties: null, body: body);

var dataArr = Encoding.UTF8.GetBytes (data!);
sender.BasicPublish ("", "base", true, null, dataArr);
Console.ReadLine ();
