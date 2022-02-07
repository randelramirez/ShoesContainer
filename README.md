# ShoesContainer
.NET 5 microservice (Web Api + MVC for front-end)


To run the project on docker 


1.) Install certificate for https running on localhost <br>

dotnet dev-certs https --clean <br>

dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetcoreapp.pfx -p aspnetcoreapp <br>

dotnet dev-certs https --trust <br>

<br>

2.) Setup docker images/containers 
  cd C:\Dev\ShoeContainer\ShoeContainer <br>
  
  docker-compose build <br>
  
  docker-compose up mssqlserver <br>
  
  docker-compose up catalog webmvc <br>
  
  
  
 ### Creating the database for running locally <br>
  
  docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=ProductApi(!)' -e 'MSSQL_PID=Developer' -p 1448:1433 --name productdatabase -h productdatabase -d mcr.microsoft.com/mssql/server:2019-latest <br>

  docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=IdentityApi(!)' -e 'MSSQL_PID=Developer' -p 1446:1433 --name identitydatabase -h identitydatabase -d mcr.microsoft.com/mssql/server:2019-latest <br>
  

  
  

