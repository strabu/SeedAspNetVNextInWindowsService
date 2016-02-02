This is a seed-project for ASP.NET vNext-Applications that are self-hosted in a WindowsService.

![alt tag](https://raw.githubusercontent.com/strabu/SeedAspNetVNextInWindowsService/master/assets/ServiceScreenshot.png)

Usage
=====
1) Clone the repository

2) Configure hostname and port in WWWService/appsettings.json
```
"server.urls": "http://localhost:5000"
```

3) Run a Command Prompt as Administrator and execute the following commands to register the Service
3a) List the installed dot.net runtimes and make sure you are using this version of the runtime:
  1.0.0-rc1-update1 clr     x86          win             default
```
dnvm list
```

3a) get the path to your dnx.exe
```
where dnx.exe
```

3b) install the service
```
sc.exe create "My Self-hosted Website" binPath= "C:\Users\YOUR_USER_NAME\.dnx\runtimes\dnx-clr-win-x86.1.0.0-rc2-16357\bin\dnx.exe -p C:\YOUR_DIRECTORY\WWWService\ run --windows-service"
```

4) Open the Windows-Services-Window, search for your service (you might need to refresh by hitting F5),
double-click it to change these settings:
-on tab "General": set "Startup type" to "Automatic"
-on tab "Log On": select "This account" and enter the credentials of your user.
The service needs the user-credentials to be able to download dot.net-Assemblies via nuget.

If you want to run your service with the "Local System account" 
- you have to publish your service with "dnu publish" after every change to your CSharp-Code
- in this case you would register the service with this path 
```
sc.exe create "My Self-hosted Website" binPath= "C:\Users\YOUR_USER_NAME\.dnx\runtimes\dnx-clr-win-x86.1.0.0-rc2-16357\bin\dnx.exe -p C:\YOUR_DIRECTORY\WWWService\bin\output\approot\src\WWWService run --windows-service"
```

6) Start the service

7) Look into the windows Application Event-Log to see what the service is doing

8) Open a browser and connect to http://localhost:5000


Developing
----------
Stop the Windows-service and run it on the commandline
```
cd WWWService
dnx restore
dnx run
```
- If you make Changes to the static html-files these will be reflected immediately (just refresh your browser).
- After changing your CSharp-files restart the Application and then the browser.
