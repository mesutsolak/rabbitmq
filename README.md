# RabbitMQ
Open Source bir mesaj kuyruk sistemidir.Bundan dolayı oldukça ün ve yaygınlık kazanmıştır.<br>
Çok iyi bir dokümantasyonu mevcuttur.<br>
https://www.rabbitmq.com/getstarted.html

## Özellikleri

<ul>
  <li>Erlang diliyle geliştirilmiştir.</li>
  <li>Cross Platform desteklenmesinden dolayı farklı işletim sistemlerinde kurulabilir ve kullanılabilir.</li>  
  <li>Cloud ortamda web arayüzü sunarak kullanım kolaylığı sağlamaktadır.</li>
</ul>

## Mesaj kuyruk sistemi nedir ?
Bir uygulamadan bir mesajı alıp bir başka uygulamaya sırası geldiğinde ileten sistemdir.
Örnek olarak bir kargo firmasını düşünebiliriz.Bir kargo firmasına bir ürün satıcı tarafından iletilir.Bu kargo zamanı geldiğinde alıcıya teslim edilir.
Bu örnekte olduğu gibi RabbitMQ'da böyle çalışmaktadır.Kendisine verilen mesajı doğru zamanda ilgili tüketiciye göndermektedir.

## Message broker nedir ?
Mesaj kuyruk sistemlerine verilen genel bir isimdir.RabbitMQ ise sadece bunlardan biridir.Kafka , MSMQ vs. diğer mesaj kuyruk sistemlerindendir.

## Mesaj kuyruk sistemi ile Message broker arasındaki fark nedir ?

Message broker, mesajları kaynaktan alıp birden fazla hedefe ileten aracı bir sistemdir, mesaj kuyruk sistemi ise mesajları sırayla işleyen ve doğrudan hedeflere ileten bir yapıdır.

## Neden kullanmalıyız ?

Bir uygulama kullanıcıdan gelen istekleri belirli bir sürede cevap veremeyip onu bekletmek yerine yapacağı işi asenkron bir şekilde başka bir uygulamaya devrederek kullanıcıya daha hızlı cevap vermek için kullanabiliriz.Böylelikle uygulamanın yoğunluğunu düşürmüş oluyoruz.Aksi taktirde kullanıcı gereksiz bir response süresi yaşanacak ve uygulamanın aleyhine durumlar yaşanabilir.Burdaki asenkron süreci yönetecek olan yapımız RabbitMQ'dur.

Örnek olarak randevu oluşturma servisimiz var.Bu randevu oluşturma servisi hem randevu oluşturuyor hem de oluşturulan randevuyu bilgi amaçlı kullanıcıya mail atıyor.Kullanıcıya mail atmak diğer işlemlere göre uzun süren bir işlem.İşte tam burda mail gönderme işlemini kuyruğa göndererek kullanıcıya mail gönderimini beklemeden hızlı bir şekilde cevap dönebiliriz.

## Nasıl işler ?

RabbitMQ'nun işleyiş mantığı aşağıdaki resimde görüldüğü gibidir.

![RabbitMQ-Nedir](https://github.com/mesutsolak/rabbitmq/assets/56551511/2450a703-5103-4859-8815-c8389c878d60)
Rabbitmq servislerine gönderilen mesajlar exchange tarafından karşılanmakta ve exchange türüne göre ilgili kuyruğa iletirek ilgili consumerlar tarafından tüketilmektedir.<br>
Buradaki akışta mesajı yayınlayan (publisher) ve bu yayınlara abone / subscribe olan consumerların olmasından dolayı biz bu süreci Publish / Subscribe (Pub/Sub) pattern olarak ifade edebilmekteyiz. 

Yukarıda görülen öğeleri sırasıyla açıklayalım :<p>
Publisher: Mesaj kuyruk sistemine mesajı gönderen ya da başka bir deyişle mesajı üreten uygulamadır / kişidir.Publisher mesajı ürettikten sonra publish edecektir ve Exchange karşılayacaktır.<p>
Exchange: Kendisine belirtilen route ile ilgili mesajı kuyruğa yönlendirir.İlgili mesajın nasıl kuyruğa gideceği Exchange içerisindeki routedan öğrenilir.Basit bir mesaj gönderme işleminde kullanılmasına gerek olmayan bir yapıdır.Exchange belirtmeden mesajları kuyruğa direk olarak gönderirsek default exchange kullanmış oluruz. <p>
Queue: Ardından kuyruğa gelen mesajlar Queue'da sıralanır.Queue'de ilk giren ilk çıkar mantığı bulunmaktadır.Buna First in First Out (FIFO) olarak adlandırabiliriz. Bir mesaj kuyruk sistemine ilk giren ilk çıkacaktır.İlk giren ilk işlenir.<p>
Consumer: Kuyruktaki mesajları alan kişi/tüketen (Consumes) /uygulama ise Consumer'dır.Consumer yazılım dilinden bağımsız olarak istenilen dille yazılabilen bir uygulamadır.Bunu şöyle örnek vermek gerekirse , bir .net web api uygulaması geliştirip bu servislere java uygulamasında istek atabiliriz.<p>

RabbitMQ gerçekleştirilen tüm süreçte AMQP(Advanced Message Queuing Protocol) protokolünü kullanır ve ilgili protokol üzerinden faaliyetlerini gerçekleştirir.

## Nasıl kurulur ?

RabbitMQ erlang diliyle yazıldığı için öncelikle erlang compiler ve onun ardından RabbitMQ kurmamız gerekmektedir.<br>
Erlang Compiler : https://www.erlang.org/downloads<br>
RabbitMQ : https://www.rabbitmq.com/download.html

Setupları kurabilmek için klasik yöntem olan next next yöntemini kullanabiliriz.<br>
Setupları kurduktan sonra web arayüzünü aktifleştirebilmek için command prompt açarak "cd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.12.6\sbin" seçmemiz gerekir. <br>
Dosya yolunu seçtikten sonra "rabbitmq-plugins enable rabbitmq_management" diyerek RabbitMQ'yu aktif hale getirebiliriz.RabbitMQ'yu ilk defa aktifleştirdiğimiz için bilgisayarı yeniden başlatmamız gerekebilir.<br>

Web arayüzünü açabilmek için "http://localhost:15672" siteyi açıyoruz.Siteye girebilmek için kullanıcı adı ve parola bilgisine guest yazarak giriş yapabiliriz.Açılan web arayüzdeki sekmelerin anlamları aşağıdaki gibidir : <br>

Connections : RabbitMQ sunucusuna yapılan bağlantıları<br>
Channels : Kanalları<br>
Exchanges : Exchangeleri<br>
Queues : Tüm kuyrukları<br>
Admin : Sisteme giriş yapan tüm yöneticileri görebiliriz.<p>

## Cloud Ortamda Kurmak (CloudAMQP)

RabbitMQ genellikle kendi sunucularımızda barındırmaktan ziyade RabbitMQ servislerini barındıran CloudAMQP servislerini tercih etmek daha doğru olacaktır.Bu servis üzerinde yapılandırmalarımızı az maliyetli , bakımı kolay , basit , erişimi pratik ve belirli bir limite kadar her türlü mesaj gönderimini ücretsiz yapabiliriz.Devasa ve daha kapsamlı projelerde docker ile ayağa kaldırmamız gerekebilir.

Cloud ortam genellikle;

<ul>
  <li>Düşük donanım maliyeti</li>
  <li>Gelişmiş performans</li>
  <li>Anında güncelleme ve bakım</li>
  <li>Arttırılmış veri güvenliği</li>
</ul>

CloudAMQP customer.cloudamqp.com sitesi üzerinden kayıt olabilir ve giriş yapabiliriz.Giriş yapıktan sonra her uygulama için farklı olucak şekilde instance oluşturmamız gerekir.

## Akıllı Kuyruk Sistemi nedir ?
Bir kuyruk sistemini akıllı hale getirmek demek genellikle performansı arttırmak , kaynak kullanımını optimize etmek , hata toleransını sağlamak ve verimliliğini arttırmak anlamına gelir.


1.Elde ettiği mesajları güvenli / dayanıklı bir şekilde tutmak<br>
2.Eşit bir dağılımla tüketilmesi <br>
3.Sunucuya mesajlar iletildiğine dair haberdar edilmesi<p>

## Exchange ve Türleri

Bir rabbitmq kendisine gönderilen mesajları direk oluşturulan kuyruklara eklemekte ve consumerlar oluşturulan bu mesajları sırasıyla tükemektedir.Süreçlar bu şekilde ilerleyebilirken birden fazla consumerın olduğu durumlarda mesajın nasıl tüketileceği ve hangi consumerın ne kadar mesaj işleyeceği bildirmemiz gerekmektedir.Böyle bir durumda devreye exchangeler girmektedir.Exchangelerin türleri vardır ve her exchange farklı bir değere ve anlama sahiptir.Gönderilen mesajlar exchange türüne göre consumerlara gönderilerek
işlenmektedir.

Cloud ortamdaki exchangelere erişmek için hornet.rmq.cloudamqp.com url adresini kullanabiliriz.

### Fanout Exchange

Fanout exchange kural gözetmeksizin aldığı mesajı tüm kuyruklara ileterek ilgili consumerlar tarafından tüketilmektedir.Fanout exchange hangi durumlarda tercih edilebilir ?

<ul>
  <li>Güncel skorların tüm oyunculara bildirilmesi</li>
  <li>Hava durumu bilgisinin tüm kanallara iletilmesi</li>
  <li>Tüm birimlere %5'lik zam yapılması</li>
</ul>

Yani kısaca genel bir işlem söz konusu ise fanout exchange kullanılmaktadır.

### Direct Exchange

Direct Exchange , ilgili mesajı verilen route anahtarına göre ilgili kuyruğa iletmektedir. <br>
Örnek olarak: Tek sayılar ve çift sayıların farklı kuyruklara gönderilmesi gibi.

### Topic Exchange

Topic Exchange ,  mesajı verilen route anahtarının formatına göre ilgili kuyruğa iletmektedir.Zorunlu olarak bir format belirlememize gerek yoktur.<p>

‘usa.news’ -> routing key değeri ‘usa.news’ olan mesajlar bu kuyruğa girecektir. <br>
‘*.weather’ -> ‘*<önemli değil>.weather’ olan mesajlar bu kuyruğa girecektir.<br>
‘#.news’ -> ‘#<başı önemli değil>.news’ olan mesajlar bu kuyruğa girecektir.<br>
‘usa.#’ -> ‘usa.#<sonu önemli değil>’ olan mesajlar bu kuyruğa girecektir. <br>
‘europe.*’ -> ‘europe.*<önemli değil>’ olan mesajlar bu kuyruğa girecektir. <br>
‘#’ -> ‘#< hiçbiri önemli değil>’ olan mesajlar bu kuyruğa girecektir. 

### Header Exchange

Topic Exchange ile aynı mantıkda çalışmaktadır farkı ise birisi routingkey ile belirlerken bir tanesi header üzrinden hangi kuyruğa gideceğini belirtmektedir.