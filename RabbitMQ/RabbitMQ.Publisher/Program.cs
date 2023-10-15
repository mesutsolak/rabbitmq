ConnectionFactory factory = new()
{
    Uri = new Uri("amqps://sgprqblp:LHM2zAJddMkm7fGd0BmG17VtWF1O8nO7@moose.rmq.cloudamqp.com/sgprqblp")
};
//factory.HostName = "localhost"; Localhosta bağlanmak yerine cloud bir ortama bağlanmaktayız.


using (IConnection connection = factory.CreateConnection()) //  Bağlantı sağlanmakta
using (IModel channel = connection.CreateModel()) // Kanal oluşturma
{
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

    byte[] bytemessage = Encoding.UTF8.GetBytes("sebepsiz boş yere ayrılacaksan");

    /*
        exchange: Kullanmıyorsak boş bırakmalıyız böylelikle sistem tarafından atanan default exchange devreye girecektir.
        routingKey: Eğerki default exchange kullanıyorsanız routing key olarak oluşturmuş olduğunuz kuyruk ismini veriyoruz.
        body: Gönderilecek mesajın ta kendisidir.
     */

    channel.BasicPublish(exchange: "", routingKey: "mesajkuyrugu", body: bytemessage);

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

}

