**Space**

**How to Run the Application?**

• Navigate to the path .\src\Space\Space.Infrastructure and run the command line (Windows) or terminal (Mac OS) to create all the infrastructure (MongoDB & RabbitMQ server) using the following command:
docker compose up -d

• Build the two projects: Space.BrokerService and Space.SensorClientService: 
- Space.SensorClientService is a service that represents the Space Service, which collects data in space
- Space.BrokerService is a service located on Earth that attempts to synchronize data from the Space Service

• To simulate losing and restoring the connection between the services, use the endpoints (in the Swagger interface) "/stop" and "/resume".

• To retrieve the data, use the endpoint "/getData/{amount}".
