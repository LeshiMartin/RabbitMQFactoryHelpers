// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RabbitMQFactoryHelpers;
using RabbitMQFactoryHelpers.Factories;
using System.Text;

Console.WriteLine ("Hello, World!");

var app = Host
  .CreateDefaultBuilder ().ConfigureServices (services =>
  {
    services.TryAddSingleton<IApplicationBuilder, ApplicationBuilder> ();
    services.RegisterConsumer ();
  }).Build ();


app.UseRabbitListener (ConsumerFactory.CreateConsumer ("base", x =>
{
  var data = Encoding.UTF8.GetString (x.Body.ToArray ());
  Console.WriteLine ($"From Consumer {data}");
}));

Console.ReadLine ();
