# Coltium-Test MVC Application

This is an MVC application built with .NET, featuring user account management pages and backend logic.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Cloning the Repository](#cloning-the-repository)
3. [Running the Application](#running-the-application)
4. [Database Migrations](#database-migrations)
5. [Email Service Configuration](#email-service-configuration)

## Prerequisites

- .NET SDK (version 8.0 or higher)
- A database (SQLite, as specified with `app.db`)
- Visual Studio, Rider or Visual Studio Code (optional, but recommended)

## Cloning the Repository

1. Open a terminal/command prompt and run the following command to clone the repository:

    ```bash
    git clone https://github.com/IamEnoch/Coltium-Test.git
    ```

2. Navigate to the project folder:

    ```bash
    cd Coltium-Test
    ```

## Running the Application

1. Open the solution file `Coltium-Test.sln` in Visual Studio or use the terminal for a .NET CLI.

2. Run the application using the following command:

    ```bash
    dotnet run
    ```

   This will start the MVC application, and you should be able to access it via `https://localhost:xxxx` in your browser.

## Database Migrations

This project uses SQLite for database management, with the database file `app.db`. To apply migrations to set up the database schema, follow these steps:

1. Open the terminal or command prompt in the project directory.

2. Run the following command to create the initial migrations (if not already created):

    ```bash
    dotnet ef migrations add InitialCreate
    ```

3. Apply the migrations to your SQLite database by running:

    ```bash
    dotnet ef database update
    ```

4. This will create the necessary database schema in `app.db`.

## Email Service Configuration

To configure the email service in your application, you need to set up the necessary settings in either the `appsettings.json` or `secrets.json` file.

### Configuration

#### Using `appsettings.json`

Add the following section to your `appsettings.json` file:

```json
{
  "Mailgun": {
    "ApiKey": "your-mailgun-api-key",
    "Domain": "your-mailgun-domain"
  }
}
```

#### Using `secrets.json`

Add the following section to your `secrets.json` file:

```json
{
  "Mailgun": {
    "ApiKey": "your-mailgun-api-key",
    "Domain": "your-mailgun-domain"
  }
}
```
> **Note:** It's recommended to use `secrets.json` for storing sensitive data like API keys during development to keep them secure and out of version control.
