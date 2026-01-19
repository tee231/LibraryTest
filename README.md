# LibraryTest
This is an Api that performs crude operations for a library. The Api was designed using the Clean Architecture, CodeFirst Approach.
Migration has been added as well as db Update.
This Api has two controllers, the Authentication controller that handles registration and login
and the book controller that handles all crud oerations for the books model

All methods have a try catch block to enable proper trcking of error and prevent the 
app from breaking. Null checks was also implemented. Dependency Inject was implemented on the Books controller and cors was enabled in the program.cs
Data seeding was also done in the context class.
security measures included below:
- In some method, Query paramters was avoided for security purposes, this is to prevent sql injection.
- model validation was also implemented on all controller methods.
- use of DTO's
- jwt Authentication was also implemented
- All controllers methods were also secured using the Jwt Authentication.
  -logging was added but no logging was done throughout the code implementation

  Payload to generate Token is - {
  "username": "TestorExam",
  "password": "ElectronicPass"
}    
