# ShoesContainer
.NET 5 microservice


To run the project on docker 


1.) Install certificate for https running on localhost
dotnet dev-certs https --clean
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\DllNameOfApp.pfx -p aspnetcoreapp
dotnet dev-certs https --trust

2.) Setup docker images/containers 
  cd C:\Dev\ShoeContainer\ShoeContainer
  docker-compose build
  docker-compose up mssqlserver
  docker-compose up catalog
  

