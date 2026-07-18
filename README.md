# the-fitness-assistant

This project will be a fitness assistant for users looking for a better free-to-use experience than popular fitness app options.

(Tyson's Quote)
"May the force be with you." - Star Wars

(Holly's Quote)
Some people can read War and Peace and come away thinking it's a simple adventure story. Others can read the ingredients on a chewing gum wrapper and unlock the secrets of the universe. - Lex Luthor, Superman Comics

(Jaime's Quote)
"Success starts with every challenge, not from the conform zone"

(Zimondi's Quote)
"Small, consistent steps taken every day lead to lasting strength in fitness, in learning, and in life."

## 

## Running the App Locally

1. Clone the repository
2. Ensure you have .NET 9 SDK installed
3. Configure Google OAuth secrets:

dotnet user-secrets set "Authentication:Google:ClientId" "your-client-id"

dotnet user-secrets set "Authentication:Google:ClientSecret" "your-client-secret"

4. Run:

dotnet watch --launch-profile https

---

XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

# 🏋️ The Fitness Assistant - Local Development Setup

## Updating Your Local Project

After changes are merged into `main`, update your local repository:

```bash
git checkout main
git pull
```

If you are working on a feature branch:

```bash
git checkout your-feature-branch
git merge main
```

---

# 1. Install Required Tools

Make sure you have:

- .NET 9 SDK
- PostgreSQL
- Visual Studio Code (recommended)

Verify your .NET installation:

```bash
dotnet --version
```

---

# 2. Create Your Local PostgreSQL Database

Each developer needs their own local database.

Open PostgreSQL (psql or pgAdmin) and create:

```sql
CREATE DATABASE fitnessassistant;
```

Your database name should match your connection string.

---

# 3. Configure Your Local Connection String

Create or update:

```
appsettings.Development.json
```

Add your local PostgreSQL connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fitnessassistant;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

Replace:

```
YOUR_PASSWORD
```

with your local PostgreSQL password.

---

# 4. Apply Entity Framework Migrations

From the project folder (the folder containing the `.csproj` file), run:

```bash
dotnet ef database update
```

This creates all required tables:

- Users
- Foods
- FoodLogEntries
- CalorieGoals

If Entity Framework tools are not installed:

```bash
dotnet tool install --global dotnet-ef
```

Then run:

```bash
dotnet ef database update
```

---

# 5. Configure Google Login (For Authentication Testing)

Google OAuth credentials are stored locally using .NET User Secrets.

Initialize user secrets:

```bash
dotnet user-secrets init
```

Add your Google OAuth credentials:

```bash
dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_CLIENT_ID"

dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_CLIENT_SECRET"
```

Your local Google OAuth redirect URI must be:

```
https://localhost:7087/signin-google
```

---

# 6. Run the Application

Always run the application using HTTPS:

```bash
dotnet watch --launch-profile https
```

Open:

```
https://localhost:7087
```

For Google authentication testing, do **not** use:

```bash
dotnet watch
```

because it runs the app using HTTP and will cause:

```
Error 400: redirect_uri_mismatch
```

---

# 7. Testing Authentication

1. Click **Login with Google**
2. Complete Google authentication
3. After successful login:
   - A login cookie is created
   - The application checks the `Users` table
   - A user record is automatically created if one does not already exist

To verify:

```sql
SELECT * FROM "Users";
```

---

# Database Troubleshooting

## Tables Are Missing

Run:

```bash
dotnet ef database update
```

---

## Need a Fresh Local Database

Drop your local database:

```sql
DROP DATABASE fitnessassistant;
```

Create it again:

```sql
CREATE DATABASE fitnessassistant;
```

Then apply migrations:

```bash
dotnet ef database update
```

---

# Production Notes

Local development:

- Local PostgreSQL database
- Local User Secrets
- HTTPS launch profile

Render production:

- Render PostgreSQL database
- Environment variables for secrets
- Production Google OAuth redirect URI

Local secrets are never committed to GitHub and are not used by Render.
