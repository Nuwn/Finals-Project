# Finals project...

... for a shorter course in .NET.

In this project I wanted to build a API using .net, and a frontend with a JS framework.
I chose Svelte as the frontend, as I've had my hands on react before.

The idea is to present a minigame inspired by wordle, but with math.

The gameplay is simple. 
In a 3x3 grid you get 3 randomly assigned numbers. Your goal is to calculate each row and each column,
to equal the numbers on the side.

A new "quiz" is generated daily based on server time.
With highscore and personal score.

### Tech
- ASP .net web core API, with models and controllers.
- Svelte, Sveltkit.
- MS SQL

### Features
- Login, Register, JWT tokens.
- Gameplay, Scores, Highscores.

### How to run
#### Database
1. Using Microsoft SQL server management studio.
2. Import Database.sql.

#### API
1. Open "/Finals-Api/Finals-Api.sln"
2. Take note of URL. http://localhost:5200
3. Change URL in "/properties/launchSettings.json" if needed,
4. Make sure connection to database is correct in, "appsettings.json" 
5. Build project.

#### Website
1. Open "Finals-UI" in terminal of choice.
2. npm install
3. If API URL was changed, go to "src/routes/api.js" to change endpoint.
3. npm run dev.
4. Open website in preferred browser.
