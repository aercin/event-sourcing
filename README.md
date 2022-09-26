# event-sourcing
These repo include three solutions and u think that each of solution are also micro service sample. One of them is event sourcing implimentation and others hold current state of data at their storage.  And these service interectation is made via Kafka Topics also. 

We have a simple e-commerce domain which include Order, Stock and Payment service. 
Stock has a few quantity of different three products for a seed data. 
## Use Cases Of Micro Services: 

### 1- Order Service:
   * Customer place an order which include products in stock
   * Customer can add a new products to order if order status is suspend yet
   * Customer can remove products from order if order status is suspend yet and also stock has enough quantity
 
### 2- Stock Service:
   * Change order status to fail and move flow to order service if stock dont have an enough quantity of products in order
   * Decrease quantity of products in stock based on order and move flow to payment service if stock have an enough quantity of products in order  

### 3- Payment Service:
   * Move flow to order service if creation of payment dont have any problem.

## Summary
We use a choreography based saga implimentation between service interactions. All service interactions is made by Kafka Topics. 
* Order service includes read model and write model apis. Write side is an event sourcing thats why it stores domain events to PostgreSql and when store a domain event to storage, we also use outbox pattern implimentation for sending projection and integration events to Kafka to protect ourselves from interruption of network or Kafka broker down scenarios.
Our read models are looking for an answers of two questions. 
One of them is "how many of a product was ordered at which unit prices" and other one is "How many of the total order has been successful and how many has been unsuccessful".
We use nosql databases for storage in Read model side. One of read model data storage is Redis and other one is MongoDb.
We use administration application for both which are Another Redis Desktop Manager and Mongo Express.
For Kafka also we use Conduktor for looking our topics and records in specific topic.
* Integration between services in this system is made by Kafka without directly sending integration or projection events to Kafka, firstly all integration and projection events store to outbox table of related service storage and message relay service listen these tables continuously and responsible for sending these events to Kafka.

## Tech Stack:
* Redis as Primary Database - StackExchangeRedis
* Another Redis Desktop Manager
* MongoDb
* MongoExpress for mongo management tool
* .Net6 web api-worker service
* Generic Repository Pattern-Unit Of Work Pattern-Strategy Design Pattern-Outbox Pattern-Choreography Based Saga Pattern
* Dapper
* EF Core - Postgre Database Provider
* Confluent Kafka for produce and consume kafka topics
* Conduktor for Kafka management tool 
* Onion Architecture for all service implimentation
* MediatR for cqrs 
* Automapper for domain object to application object mappings
* Docker Compose for all side dependencies of system 
