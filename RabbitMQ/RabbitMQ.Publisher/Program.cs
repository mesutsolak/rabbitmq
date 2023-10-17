ConnectionFactory factory = new()
{
    Uri = new Uri("amqps://sgprqblp:LHM2zAJddMkm7fGd0BmG17VtWF1O8nO7@moose.rmq.cloudamqp.com/sgprqblp")
};
//factory.HostName = "localhost"; Localhosta bağlanmak yerine cloud bir ortama bağlanmaktayız.


using (IConnection connection = factory.CreateConnection()) //  Bağlantı sağlanmakta
using (IModel channel = connection.CreateModel()) // Kanal oluşturma
{
    #region Simple Queue

    /*
        QueueDeclare metodunu ve parametlerini açıklayalım.
        QueueDeclare metodu kuyruk oluşturmaya yaramaktadır.
        queue : Kuyruk ismi
        durable : Normal şartlarda oluşturmuş olduğumuz kuyruklar bellek üzerinde tutulmaktadır.Böyle bir durumda
        RabbitMQ sunucuları restart edilirse veriler kaybolacaktır.Durable default olarak false gelmektedir.Verilerimizin
        böyle bir durumda kaybolmamasını ve güvenlik amaçlı fiziksel olarak kaydedilmesi için true verilmesi gerekmektedir.
        exclusive : Bu kuyruğa birden fazla kanal bağlanacak mı ? 
        autoDelete : Tüm kuyruklar bittikten sonra kendisini otomatik olarak imha etsin mi ?
     */

    channel.QueueDeclare("mesajkuyrugu", false, false, false);

    /* 
     *  GetBytes string bir değer aldığı için bir dizi ya da sınıf gönderme işleminde JsonConvert.SerializeObject kullanmalıyız.
    */

    byte[] bytemessage = Encoding.UTF8.GetBytes("sebepsiz boş yere ayrılacaksan");

    /*
        exchange: Kullanmıyorsak boş bırakmalıyız böylelikle sistem tarafından atanan default exchange devreye girecektir.
        routingKey: Eğerki default exchange kullanıyorsanız routing key olarak oluşturmuş olduğunuz kuyruk ismini veriyoruz.
        body: Gönderilecek mesajın ta kendisidir.
     */

    channel.BasicPublish(exchange: "", routingKey: "mesajkuyrugu", body: bytemessage);

    #endregion

    #region Secure Queue

    // durable true dediğimiz zaman kuyruğumuz fiziksel olarak disk üzerinde saklanır ve RabbitMQ sunucusunun yeniden başlatılması durumunda dahi kuyruk korunur.

    channel.QueueDeclare("iskuyrugu", durable: true, false, false, null);

    // Publisher ayağa her kalktığında 100 adet mesaj kuyruğa iletmektedir.

    for (int i = 1; i <= 100; i++)
    {
        byte[] secureMessage = Encoding.UTF8.GetBytes($"is - {i}");

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        /*
         * Durable mantığıyla mantık olarak benzemektedir.Persintent fiziksel olarak mesajları disk üzerinde saklamaktadır.Yeniden başlatılması durumunda kaybolmaz.
         * Mesajların disk güvenliği sağlaması durumda performansı bir miktar etkileyebileceğini unutmamak gerekir.
         * Çünkü mesajlar disk üzerinde depolanacağı için bellek kullanımı ve performans açısından ek yük getirebilir. 
         * Bu nedenle, uygulama gereksinimlerinize göre dengeli bir yaklaşım benimsemek önemlidir.
         * 
        */
        channel.BasicPublish(exchange: "", routingKey: "iskuyrugu", basicProperties: properties, body: secureMessage);
    }

    #endregion

    #region Fanout Exchange 

    /*
     * Artık bir exchange kullanıldığı için publisher tarafında bir kuyruk oluşturmak yerine exchange oluşturmalı ve mesajlar exchange tarafından gitmelidir.
     * Exchange ürettikten sonra kuyruk oluşturmuyoruz.Bunun nedeni exchange üretildikten sonra exchange'e bağlanmak isteyen bir consumer için farklı kuyruk oluşturulacaktır.
     * Yani kısaca bir exchange 5 tane consumer bağlanacaksa 5 consumer için farklı kuyruklar oluşturulacaktır.
     */
    channel.ExchangeDeclare("iskuyrugu", type: ExchangeType.Fanout);
    for (int i = 1; i <= 100; i++)
    {
        byte[] fanoutExchangeMessage = Encoding.UTF8.GetBytes($"is - {i}");

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: "iskuyrugu", routingKey: "", basicProperties: properties, body: fanoutExchangeMessage);
    }

    #endregion

    #region Direct Exchange

    channel.ExchangeDeclare("directexchange", type: ExchangeType.Direct);
    for (int i = 1; i <= 100; i++)
    {
        byte[] directExchangeMessage = Encoding.UTF8.GetBytes($"sayı - {i}");

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        // routingKey ile anahtar belirtiyoruz.

        if (i % 2 != 0)
            channel.BasicPublish(exchange: "directexchange", routingKey: "ciftsayilar", basicProperties: properties, body: directExchangeMessage);
        else
            channel.BasicPublish(exchange: "directexchange", routingKey: "teksayilar", basicProperties: properties, body: directExchangeMessage);
    }

    #endregion

    #region Topic Exchange

    channel.ExchangeDeclare("topicexchange", type: ExchangeType.Topic);
    for (int i = 1; i <= 100; i++)
    {
        byte[] topicMessage = Encoding.UTF8.GetBytes($"{i}. görev verildi.");

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        channel.BasicPublish(exchange: "topicexchange", routingKey: $"Asker.Subay.{(i % 2 == 0 ? "Yuzbasi" : (i % 11 == 0 ? "Binbasi" : "Tegmen"))}", basicProperties: properties, body: topicMessage);
    }

    #endregion

    #region Header Exchange

    channel.ExchangeDeclare("headerexchange", type: ExchangeType.Headers);
    for (int i = 1; i <= 100; i++)
    {
        byte[] headerMessage = Encoding.UTF8.GetBytes($"{i}. mesaj");

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Headers = new Dictionary<string, object>()
        {
            ["no"] = args[0] == "1" ? "123456" : "654321"
        };

        channel.BasicPublish(exchange: "headerexchange", routingKey: string.Empty, basicProperties: properties, body: headerMessage);
    }

    #endregion
}

