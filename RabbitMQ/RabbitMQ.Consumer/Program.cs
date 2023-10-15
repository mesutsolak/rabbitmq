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


    channel.QueueDeclare("iskuyrugu", durable: true, false, false, null);

    /* 
     * Port önceliği anlamına gelen QoS servisi ile consumera öncellik verilmektedir ve böylece eşit dağılım sağlanmaktadır.
     * prefetchSize : Mesaj boyutunu ifade etmektedir.0 diyerek ilgilenmediğimizi iletiyoruz.
     * prefetchCount : Dağıtım adetini belli eder.
     * global : true : tüm consumerların aynı anda prefetchCount kadar mesaj tüketmesini ifader. false : her bir consumerın tüm consumerlardan
     * bağımsız bir şekilde kaç mesaj alıp işleyeceğini belirtir.
     */
    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    // 
    EventingBasicConsumer secureConsumer = new(channel);
    channel.BasicConsume("iskuyrugu", false, secureConsumer);
    consumer.Received += (sender, e) =>
    {
        /*
             Console uygulamaları parametre olarak dışardan args adı altında parametre alabilirler.  
             Consumer  ayağa kaldırılırken gönderilen argümandaki sayısal değer kadar süreci durdurarak işlemektedir.
             Bu parametreye dizinin 0 indexini alarak ulaşabiliriz. | args[0]
             Console uygulamasını powershell'de ayağa kaldırırken klasör yolunu seçip  dotnet run 1000 | dotnet run 2000 | dotnet run 3000
             diyerek consume işleminin ne kadar süreyle bir olacağını belirtmiş oluyoruz.
         */
        Thread.Sleep(int.Parse(args[0]));
        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span) + " alındı");

        // Sunucuya haber vermek için kuyruktaki mesaj alındıktan sonra silinmek için kullanılan mesajdır.
        channel.BasicAck(e.DeliveryTag, false);
    };

}
Console.Read();