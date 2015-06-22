# Mail Phishing App

A WPF desktop app that simulates email-phishing attacks against Outlook distribution list(s).

#### INSTALLATION:

1. Dependencies/Packages:
 * Make sure to install all the packages in the Technologies section below via NuGet.

2. Database:
 * The database for this app was hosted on an MS SQL Server instance.
 * The database schema was generated via MS SQL Server Management Studio.
 * Make sure your database is identical to the database schema specified in the project's **[Database Schema File](DATABASE_SCHEMA.sql)**.

3. Edit the [Application Config file](Mail-Phishing/App.config):
 * Write a valid connection string to your database. **Don't change the key name "MailPhishingContext"!!**

4. Compile:
 * Compiling the project will give you one executable, located under "Mail-Phishing/bin/Release/".

#### TECHNOLOGIES

 * C# 4.5
 * Entity Framework 6.1.1
 * Extended WPF Toolkit 2.0.0
 * WPF Rich Text 1.0.16.2

#### DEVELOPERS

 * [Saddam Abu Ghaida](https://github.com/sghaida)
 * [Ahmad Alhour](https://github.com/aalhour)

#### LICENSE:

This project is licensed under the MIT License.
