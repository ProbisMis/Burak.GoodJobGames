## .NET Core 3.0 Simple Leaderboard with Redis and AWS Elasticbeanstalk ##

### Installation ###

* type `git clone https://github.com/ProbisMis/Burak.GoodJobGames.git projectname` to clone the repository 
* type `cd projectname`
* Open Solution .sln with Visual Studio 2019
* From this point you should adjust appsettings.json file for database and redis connection strings, for database  either SQL or MySql server can be used by 
for Redis i used AWS Elasticache, that should be in same VPC with EC2 instance and with all tcp ports open. 
* Elasticache nginx config file is located in .platform file, which is needed for reverse proxy port setting. My application runs on port 5001, this can be changed but .platform/nginx/nginx.conf file should be changed as well.

### Include ###

* [NLog] for logging 
* [EntityFramework] for database query
* [Swagger] for documentation and testing purposes
* [StackExchange.Redis] for caching and leaderboard operations
* [Fluent] for database migration
* [AutoMapper] for mapping models

### Features ###

* Add,Get User 
* Submit Score
* Global and Country specific leaderboard
* Bulk Import User (not ready)

### Notes ###

The project was challenging and educatory. Designing a leaderboard that serves millions of users arises some problems. First of all, if the server can handle millions of request at a time? I used loadbalancer and autoscaling for distributing the load equally to servers as well as horizontally scaling number of instances if the current amount is not enough. Then, if the database can handle many operation? I tried to decrease the number of request going to server by caching the data into Redis. That decreased the amount of request is going to database. However, the redis server should have good amount of memory and it should be scaled as well. Thanks to AWS Elasticache service, scaling operations are handled easily by enabling cluster mode. Redis also helped me to achieve fast data search. Its SortedSet class is a perfect fit for leaderboard, automatically sorted after an update. Getting a rank of single user takes O(logn) time. Also, user information is stored in Redis Hash therefore there is no need to ask database. 

Thanks.
