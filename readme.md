# Himanshu Pathak api testing stub
## _This application is trying to create a stub for the git api project api -  Get all codes of conduct

URL of the above api - "https://api.github.com/codes_of_conduct/citizen_code_of_conduct",

## Features
- .net core console application (.net core 5.XXX, C#)
- Not using any nunit or xunit. Simply creating tests in the main method.
- WireMock plugin / nuget package is installed to run a dummy server, and mocking the api calls for GET and POST

## How to execute the application
- Download the visual studio code
- Download the application
- Open the folder in VS code where you saved the application
- run the following commands
- dotnet restore Voyager_test.csproj 
- dotnet run Voyager_test.csproj   