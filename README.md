# Project Library

This project is a library management system that allows users to keep track of authors and books. It consists of a MySQL database, a Console Application, a LibraryAPI, and a Blazor Web Application, all integrated within the same solution in Visual Studio.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Setup](#setup)
- [Usage](#usage)

## Features

- **MySQL Database**: Utilizes a relational database to store information about authors and their books.
- **Console Application**: Provides a command-line interface for interacting with the library system.
- **LibraryAPI**: Offers a RESTful API to perform CRUD operations on authors and books.
- **Blazor Web Application**: Presents a user-friendly interface for managing library data.

## Technologies Used

- **MySQL**: A popular open-source relational database management system.
- **C#**: The primary programming language used for developing the Console Application, LibraryAPI, and Blazor Web Application.
- **Visual Studio**: An integrated development environment (IDE) used for developing and managing the entire solution.
- **Entity Framework Core**: An Object-Relational Mapping (ORM) framework for .NET used to interact with the MySQL database.
- **ASP.NET Core**: A cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications.

## Setup

To run this project locally, follow these steps:

1. **Clone the Repository**:
   (https://github.com/cyber-guy69/Library.git)

2. **Open Solution in Visual Studio**: 
Open Visual Studio and navigate to the cloned repository directory. Open the solution file (`Library.sln`).

3. **Set up MySQL Database**: 
- Create a new MySQL database.
- Update the connection string in the `appsettings.json` file of the LibraryAPI and Blazor Web Application projects to point to your MySQL database.

4. **Build Solution**: 
Build the solution to restore dependencies and compile the projects.

5. **Run Console Application**: 
Run the Console Application project to interact with the library system via the command line.

6. **Run LibraryAPI**: 
Run the LibraryAPI project to start the API server.

7. **Run Blazor Web Application**: 
Run the Blazor Web Application project to start the web server and access the library system through a web browser.

## Usage

- **Console Application**: Follow the prompts to perform actions such as adding authors, adding books, listing authors, listing books, etc.
- **LibraryAPI**: Send HTTP requests to perform CRUD operations on authors and books. Refer to the API documentation for details on available endpoints and payloads.
- **Blazor Web Application**: Use the web interface to add, edit, view, and delete authors and books.

