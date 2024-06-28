<h2>Prerequisites</h2>

You have to have [Docker](https://www.docker.com/) installed on your machine to run this app.

<h2>Setup and Configuration</h2>

Generate HTTPS Certificate

Windows: </br>
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\linkapi.pfx -p 1234`<br>
`dotnet dev-certs https --trust`

Mac/Linux </br>
`dotnet dev-certs https -ep ${HOME}\.aspnet\https\linkapi.pfx -p 1234` <br>
`dotnet dev-certs https --trust`

Git clone this repo and cd to the root directory<br>
`git clone <copy link>` <br>
`cd linkAppDev/` <br>

Build and start entire app by running:<br>
`docker compose up` <br>

visit
http://localhost:3000
