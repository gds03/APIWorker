# APIWorker

The idea of this project is to define 2 processes that interact with each other through a broker.
API will be restful using HATEOAS and will contain the endpoints to be invoked by users.

The state change is done through event sourcing from the API to the Worker, and worker will eventually pick up the events and mutate the data state.

The example here to be shown is that a Order is placed, and once the order is placed, the Payment is processed async.


# Docker:

docker rm -v -f $(docker ps -qa)

docker run -d --name rabbitmq_unit \
-p 5672:5672 \
-p 15672:15672 \
-e RABBITMQ_DEFAULT_USER=user \
-e RABBITMQ_DEFAULT_PASS=password \
rabbitmq:3-management


docker run -d --name mysqldb_unit \
--restart always \
-p 3306:3306 \
-e MYSQL_ROOT_PASSWORD=rootpassword \
-e MYSQL_DATABASE=APIWorker \
-e MYSQL_USER=myuser \
-e MYSQL_PASSWORD=mypassword \
-v mysql_data:/var/lib/mysql \
mysql:8

# EF Core /MYSQL


dotnet ef migrations add InitialCreate --project Domain --startup-project API

dotnet ef database update --project Domain --startup-project API


