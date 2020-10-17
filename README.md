## .NET Core Simple Leaderboard with Redis and AWS Elasticbeanstalk ##

### Installation ###

* type `git clone https://github.com/ProbisMis/Burak.GoodJobGames.git projectname` to clone the repository 
* type `cd projectname`
* Open Solution .sln with Visual Studio 2019
* From this point you should adjust appsettings.json file for database and redis connection strings, for database  either SQL or MySql server can be used by 
for Redis i used AWS Elasticache, that should be in same VPC with EC2 instance and with all tcp ports open. 
* Elasticache nginx config file is located in .platform file, which is needed for reverse proxy port setting. My application runs on port 5001, this can be changed but .platform/nginx/nginx.conf file should be changed as well.

### Include ###

* [NLog] for logging 
* [XUnit] for unit test and coverage.
* [EntityFramework] for database query
* [Swagger] for documentation and testing purposes
* [StackExchange.Redis] for caching and leaderboard operations
* [Fluent] for database migration
* [AutoMapper] for mapping models

### Features ###

* Add,Get User 
* Submit Score
* Global and Country specific leaderboard
* Bulk Import User

### Notes ###



Thanks.

### Presentation ###
API Routes List
![routes](/images/routes1.png)

#### GET Methods ####
Replace port, user_id, friend_id

GET All Friends, friend list.
http://localhost:port/api/user/friends/user_id

GET Chat is used for reading messages. Order of ID's matters in order to update read status. 
http://localhost:port/api/chat/user_id/friend_id

GET Get All Chats, doesnt update read status. It will get all open chats of user.
http://localhost:port/api/chat/user_id

#### POST Methods ####

Register,Login can be used by filling Username and Password are in Postman. 
```json
{
	"Username" :"TEST",
	"Password" :"123"
}
```
A sample response for login will return user data
![user data](/images/loginresult.png)

A sample response for login error
![user data](/images/loginerror1.png)

Add Friend, Block Friend, Start Chat are using below parameters.
* user_id is sender, friend_id is reciever. 
```json
{
	"user_id" :1,
	"friend_id" :1002
}
```

Send Message 
```json
{
	"message_body" : "Hello 1!",
	"user_id" :1002,
	"friend_id" :1
}
```

Send Message - swap users
![Chat3](/images/message2.png)


Get Message - If reciever recieves messages via GetChat function. Read status is updated. According to whose reading.
![Chat4](/images/message3.png)
