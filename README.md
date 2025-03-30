# APIWorker

The idea of this project is to define 2 processes that interact with each other through a broker.
API will be restful using HATEOAS and will contain the endpoints to be invoked by users.

The state change is done through event sourcing from the API to the Worker, and worker will eventually pick up the events and mutate the data state.

The example here to be shown is that a Order is placed, and once the order is placed, the Payment is processed async.
