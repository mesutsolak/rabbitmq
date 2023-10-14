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

}