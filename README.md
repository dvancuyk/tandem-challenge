# Running Integration Tests

To run the integration tests, run the following command in the root folder:
``` docker-compose up```

This will set up a mongo db that is used as the backend store. It also stands up a Mongo Express on the local machine which allows you to view the database and the added records. To access Mongo Express, simply open a browser and navigate to http://localhost:5025/. 

The Integration tests that test the repository are configured to point to a database called *userstestdb* and the
web integration tests are stored in the *UsersChallenge*