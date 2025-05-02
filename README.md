This is an API which can process a payment and retrieve the status of a processed payment, using a simulated acquirer.

The architecture of the project is:
- API to get the payment requests
- The Acquirer simulator, it is a worker that is listening from payment requests
- Kafka is a platform used to publish, subscribe to, store, and process real-time data streams. In this project is used to get the payment requests and store the response from the aquirer

The expected flow for a payment request is:
- API request
- It creates a message in the queue (into a topic with payment requests)
- The Acquirer simulator gets the message with the request and generates a random response
- When it has a response, it creates a message in the queue (into a topic with payment responses)
- The original API request is waiting for the response, so it gets the response message and return it

Requirements to run:
- Docker

Steps to run:
- Clone the repository
- In the root folder, run this in the Command prompt:
  docker-compose up --build
  
- Use Postman or any HTTP request tool to do a request like this:
	
	POST http://localhost:5000/api/payments
 	```
	{
		"cardNumber": "1234567812345678",
		"expiryMonth": "06",
		"expiryYear": "26",
		"cvv": "123",
		"amount": 700,
		"currency": "usd"
	}
	```
	This is the expected result:
	```
	{
		"paymentId": "__payment_id__",
		"cardNumber": "************5678",
		"amount": 700,
		"currency": "usd",
		"status": "Success|Failure" 
	}
	```
	Then you can get the status in this way:

	GET http://localhost:5000/api/payments/__payment_id__
	
	This is the expected result:
	```
	{
		"paymentId": "__payment_id__",
		"status": "Success|Failure" 
	}
 	```

 __NOTE:__ Please consider to wait some time (~1 minute) to be sure that the environment is done and all the Docker containers are working.
