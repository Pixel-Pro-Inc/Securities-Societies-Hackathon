# Securities-Societies-Hackathon
BIH Sponsored Hackathon for Ministry of Defense and Ministry of Immigration


We use a Angular + Dotnet tech stack and a Firebase database with Firebase hosting. 
The reason this fits the problem at hand is that its highly scalable, with each deparment or board that can act like a microservice and be attached on need base.
The fact that its modular also reduces risk and gives chance to be more unique per service, so as to fit the needs of what ever public sector needed to be addressed.	
Firebase database hosting also helps. We use azure web hosting.They have good and affordable rates.

Angular uses architeture that has every component as a self-sufficient piece of UI, screen, or route, and contains elements with related functionality. The fact that 
the code base is decoupled is proof that the microservice capability is pragmatic and relevent to the problem. And the .NET side is easy to maintain with robust, open-source 
documentation.

==Briefing
The solution has an API which feeds data to the client named as such.
You need to have both hosted for you to be able to observe the solution

--Instructions
-First navigate to where the API folder is and within it, in your command-line fire dotnet run. NOTE: you have to have dotnet installed on the device
-Second , in another command-line window navigate to the client folder, and within it fire ng serve. NOTE: You need to have npm and angular/cli installed on your device

**Follow these link for more clarification https://angular.io/cli, https://dotnet.microsoft.com/en-us/download/dotnet

-For other dependencies and libraries, you will need to run dotnet install @microsoft/signalr and run alot of npm install commands for ngx-spinner, ngx-toaster, ngx-bootstrap 
amougst others. The rule of thumb is to check the app.module.ts code and if something is missing run npm i <depencency> here. If that doesnt work then its a dotnet install issue

!!Descriptions
-All the front-end of the work is in the Angular client named client, as well as some necessary middleware. Nothing critically important is stored here as its only
concerned with the UX/UI and serving the information called from the API. It is worth noting that alot of the user flow  is done here only.
-All the back-end is in the API folder, that includes all the entities and the business logic, which will be found in the controllers. Its here that calles to the database 
are made and recieved. You will find in the Dto folder all the expected types that will run between the API and database
-On the server side it will be noted that the security database rules are NOT YET updated to ensure, perfect security (still in test mode), but rest assured measures will 
be set in place quite soon. The node nature of the database makes it perfect to fetch necessary data without having to sift too deeply into the database. As it stands though
we have basically surface level nodes. The plan is, nodes will be automatically made and structured per department(Securities/Societies)=>Account/variousServices=> and a 
final major node on time, as in quarterly incoming new data; Not too deep and not to surface.
-For security we use JsWebTokens so that no matter what its impossible for others to intercept and or inject into the system. All public methods are called with variables 
privately or protectedly accessed by respective objects. Note: Review will be necessary to ensure that this stays the case in all matters

^^^Unique Features
-There is SMS and email functionality

****************************
Future functionality-()-()-()-

****
-**In-platform messaging** was also in the works. This one much more further than SMS functionality. The code did not prove finished as it was not perfectly stable, so we decided
it was safe to push that in for later. You can look through MessageController, MessageRepository and Message datatypes and judge for yourself. Constructive feedback will
be gratefully recieved.
****
