# ShoesContainer
.NET 5 microservice (Web Api + MVC for front-end)


To run the project on docker 


1.) Install certificate for https running on localhost <br>

dotnet dev-certs https --clean <br>

dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetcoreapp.pfx -p aspnetcoreapp <br>

dotnet dev-certs https --trust <br>

<br>
<br>

2.) Setup docker images/containers 
  cd C:\Dev\ShoeContainer\ShoeContainer <br>
  
  docker-compose build <br>
  
  docker-compose up mssqlserver <br>
  
  docker-compose up catalog webmvc <br>
  

