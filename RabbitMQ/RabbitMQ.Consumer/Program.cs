ConnectionFactory factory = new()
{
    Uri = new("amqps://sgprqblp:LHM2zAJddMkm7fGd0BmG17VtWF1O8nO7@moose.rmq.cloudamqp.com/sgprqblp")
};

using (IConnection connection = factory.CreateConnection())
using (IModel channel = connection.CreateModel())
{
    #region Simple Queue

    //  Consumer her ne kadar tüketici olsa bile publisher'ın tanımladığı şekilde kuyruk tanımlamalıdır.
    channel.QueueDeclare("mesajkuyrugu", false, false, false);

    // Tanımladığımız kuyruktaki mesajları yakalayacak bir event oluşturuyoruz.
    EventingBasicConsumer consumer = new(channel);

    /*
     * Basic Consume ile mesajları tüketiyoruz.Parametreleri tanımlayalım.
       queue : Kuyruk ismi
       autoAck / autoAcknowledge: Kuyruktan alınan mesajın silinip silinmemesini sağlıyor.Silinmesi pek önerilmiyor bunun yerine
       ilgili işlem tamamlandıktan sonra kendi kontrolümüzde silinmesini öneriliyor.
       
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

    #endregion

    #region Secure Queue

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
    secureConsumer.Received += (sender, e) =>
    {
        /*
             Console uygulamaları parametre olarak dışardan args adı altında parametre alabilirler.  
             Consumer  ayağa kaldırılırken gönderilen argümandaki sayısal değer kadar süreci durdurarak işlemektedir.
             Bu parametreye dizinin 0 indexini alarak ulaşabiliriz. | args[0]
             Console uygulamasını powershell'de ayağa kaldırırken klasör yolunu seçip  dotnet run 1000 | dotnet run 2000 | dotnet run 3000
             diyerek consume işleminin ne kadar süreyle bir olacağını belirtmiş oluyoruz.
         */
        Thread.Sleep(int.Parse(args[0]));

        // Bir deSerialize işlemi yapılacaksa e.body.toArray() denilecek sonrasnda JsonConvert.DeSerialize() kullanılacaktır.

        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span) + " alındı");

        // Sunucuya haber vermek için kuyruktaki mesaj alındıktan sonra silinmek için kullanılan mesajdır.
        channel.BasicAck(e.DeliveryTag, false);
    };

    #endregion

    #region Fanout Exchange

    channel.ExchangeDeclare("iskuyrugu", type: ExchangeType.Fanout);

    // Her Consumer İçin Oluşturulacak Kuyruklara Random İsim Oluşturma
    var queueName = channel.QueueDeclare().QueueName; //Random isim oluşturuyoruz.
    channel.QueueBind(queue: queueName, exchange: "iskuyrugu", routingKey: ""); //Oluşturulan Random ismi ilgili exchange'e bind ediyoruz.

    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    EventingBasicConsumer fanoutExchangeConsumer = new EventingBasicConsumer(channel);
    channel.BasicConsume(queueName, false, fanoutExchangeConsumer);
    fanoutExchangeConsumer.Received += (sender, e) =>
    {
        Thread.Sleep(int.Parse(args[0]));
        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span) + " alındı");
        channel.BasicAck(e.DeliveryTag, false);
    };

    #endregion

    #region Direct Exchange

    channel.ExchangeDeclare("directexchange", type: ExchangeType.Direct);

    string directQueueName = channel.QueueDeclare().QueueName;
    if (int.Parse(args[0]) == 1)
        channel.QueueBind(queue: directQueueName, exchange: "directexchange", routingKey: "teksayilar");
    else
        channel.QueueBind(queue: directQueueName, exchange: "directexchange", routingKey: "ciftsayilar");

    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    EventingBasicConsumer directConsumer = new(channel);
    channel.BasicConsume(directQueueName, false, directConsumer);
    directConsumer.Received += (sender, e) =>
    {
        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span) + " sayısı alındı.");
        channel.BasicAck(e.DeliveryTag, false);
    };

    #endregion

    #region Topic Exchange

    channel.ExchangeDeclare("topicexchange", type: ExchangeType.Topic);

    var topicQueueName = channel.QueueDeclare().QueueName;
    var routingKey = "";

    // routing key kurallarını belirtiyoruz. '.' ile kural setlerini belirtiyoruz.

    routingKey = args[0] switch
    {
        "1" => $"*.*.Tegmen",
        "2" => $"*.#.Yuzbasi",
        "3" => $"#.Binbasi.#",
        "4" => $"Asker.Subay.Tegmen",
        _ => throw new NotImplementedException(),
    };

    channel.QueueBind(queue: topicQueueName, exchange: "topicexchange", routingKey: routingKey);

    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    EventingBasicConsumer topicConsumer = new(channel);
    channel.BasicConsume(topicQueueName, false, topicConsumer);
    topicConsumer.Received += (sender, e) =>
    {
        Console.WriteLine($"{routingKey} {Encoding.UTF8.GetString(e.Body.Span)} görevi aldı.");
        channel.BasicAck(e.DeliveryTag, false);
    };


    #endregion

    #region Header Exchange

    channel.ExchangeDeclare("headerexchange", type: ExchangeType.Headers);

    //Kuyruk ismi diğer exchange değerlerine göre manuel oluşturulmaktadır.

    channel.QueueDeclare($"kuyruk-{args[0]}", false, false, false, null);

    channel.QueueBind(queue: $"kuyruk-{args[0]}", exchange: "headerexchange", routingKey: string.Empty, new Dictionary<string, object>
    {
        /*
           x-match alanı all veya any değeri almaktadır.İlgili kuyrukta herhangi bir tane başlığın eşleşmesinin yeterli olduğunu 
           vurgulamak istiyorsan any , hepsinin uymasını istiyorsan o zaman all dememiz gerekmektedir.
         */

        ["x-match"] = "all",
        ["no"] = args[0] == "1" ? "123456" : "654321",
    });

    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    EventingBasicConsumer headerConsumer = new(channel);
    channel.BasicConsume($"kuyruk-{args[0]}", false, headerConsumer);
    headerConsumer.Received += (sender, e) =>
    {
        Console.WriteLine($"{Encoding.UTF8.GetString(e.Body.Span)}. mesaj");
        channel.BasicAck(e.DeliveryTag, false);
    };

    #endregion
}
Console.Read();