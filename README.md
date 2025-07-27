## Introduction


RabbitMQ is a widely used message queue system built on the AMQP protocol, supporting other protocols like MQTT and STOMP, and designed to handle asynchronous tasks in a scalable and high-performance manner by queuing and processing them in order.

This project consists of two main folders: Core and API.
The Core folder provides the infrastructure for communicating with RabbitMQ,
while the API project handles incoming HTTP requests and forwards them to the RabbitMQ queue.

## Start

To get the application up and running, the appropriate .NET 8 SDK version must be installed.

To get a RabbitMQ instance, you need to create one through CloudAMQP.

## Dependencies

| Name                                 | Category           | Version     |
|--------------------------------------|--------------------|-------------|
| RabbitMQ.Client                      | Queue System       | 6.6.0       |
| Swashbuckle.AspNetCore               | API Documentation  | 6.6.2       |
