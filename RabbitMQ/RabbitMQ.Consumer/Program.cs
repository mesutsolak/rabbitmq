ConnectionFactory factory = new()
{
    Uri = new("amqps://sgprqblp:LHM2zAJddMkm7fGd0BmG17VtWF1O8nO7@moose.rmq.cloudamqp.com/sgprqblp")
};

using (IConnection connection = factory.CreateConnection())
using (IModel channel = connection.CreateModel())
{

    //  Consumer her ne kadar tüketici olsa bile publisher'ın tanımladığı şekilde kuyruk tanımlamalıdır.
    channel.QueueDeclare("mesajkuyrugu", false, false, false);

    // Tanımladığımız kuyruktaki mesajları yakalayacak bir event oluşturuyoruz.
    EventingBasicConsumer consumer = new(channel);

    /*
     * Basic Consume ile mesajları tüketiyoruz.Parametreleri tanımlayalım.
       queue : Kuyruk ismi
       autoAck :Kuruktan alınan mesajın silinip silinmemesini sağlıyor.Silinmesi pek önerilmiyor.
       consumer : Tüketici
    */
    channel.BasicConsume("mesajkuyrugu", false, consumer);


    /*
      Consumer'ın Received değeri bize kuyruktaki mesajları getirecektir.Mesaj byte dizisi olarak gönderildiğinden dolayı
      mesaj byte dizisi olarak elde edilecektir.
     */
    consumer.Received += (sender, e) =>
    {
        //e.Body : Kuyruktaki mesajı verir.
        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    };
}
Console.Read();