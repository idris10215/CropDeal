### **Step 1: Project Scaffolding & Environment Setup**

* **Action:** Initialized the Web API project using the .NET CLI and performed a clean-up of boilerplate code (removing default WeatherForecast files).
* **Definition:** Initializing the **ASP.NET Core Web API** framework.
* **Purpose:** To establish the foundational request-response pipeline. This provides the infrastructure for hosting our microservices, handling HTTP routing, and managing the server lifecycle. It creates the "container" where all our business logic will live.
* **Technical Commands:**
    ```bash
    # Create a new Web API project named 'CropDeal'
    dotnet new webapi -n CropDeal

    # Navigate into the project directory
    cd CropDeal
    ```

### **Step 2: Domain Modeling (The Identity Entity)**

* **Action:** Created the `User.cs` class within a new `Models` directory.
* **Definition:** Defining a **POCO (Plain Old CLR Object)** Entity.
* **Purpose:** To define the schema for the `USERS` table as specified in the **LLD (Page 4)**. Since .NET is a statically-typed language, this model acts as a strict contract. It ensures that every user object handled by the system contains the required properties (Email, Password, Role) before any processing occurs.
* **Key Components:**
    * **Properties:** Defined fields like `Id`, `Username`, `Email`, `Password`, and `Role`.
    * **Data Integrity:** Used specific C# types (e.g., `int` for ID, `string` for text) to prevent data corruption.

### **Step 3: Data Access Layer Integration (EF Core)**

* **Action:** Installed the Microsoft Entity Framework Core (EF Core) library and its supporting tools via the NuGet Package Manager.
* **Definition:** Integrating an **ORM (Object-Relational Mapper)**.
* **Purpose:** To bridge the gap between our C# Objects and the SQL Server Relational Database. Without this, we would have to write manual SQL strings. EF Core allows us to use C# syntax to perform database operations, making the code cleaner and easier to maintain.
* **Technical Commands:**
    ```bash
    # The Main Engine for SQL Server
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer

    # Tools for creating the database schema from code
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.Tools

    # Global CLI tool to run migration commands
    dotnet tool install --global dotnet-ef
    ```

### **Step 4: The Database Context (Gatekeeper)**

* **Action:** Created the `AppDbContext.cs` file within a new `Data` directory.
* **Definition:** Implementing the **DbContext**.
* **Purpose:** This class acts as the central coordinator for Entity Framework. It maps our C# `User` model to a physical table named `Users` in the database. It is responsible for opening connections, tracking changes to data, and saving information to the physical SQL Server.
* **Key Components:**
    * **DbSet<User>:** Tells the system to create and manage a table based on the User model.
    * **Constructor:** Allows the database connection settings to be passed into the context at runtime.

### **Step 5: Dependency Injection & Externalized Configuration**

* **Action:** Configured the database connection string in `appsettings.json` and registered the `AppDbContext` in `Program.cs`.
* **Definition:** Implementing the **Inversion of Control (IoC)** pattern and **Externalized Configuration**.
* **Purpose:** To follow the **High-Level Design (HLD)** principles. By storing the connection string in a JSON file rather than hardcoding it, we make the app more secure and easier to deploy to different environments. Registering the service in `Program.cs` ensures that the database connection is automatically shared with any part of the app that needs it.
* **Key Components:**
    * **appsettings.json:** Stores the database "Address" (Connection String).
    * **Program.cs:** Acts as the "Brain" where the database service is officially registered into the application's service container.

### **Step 6: Database Schema Evolution (Migrations)**

* **Action:** Generated a migration script and applied it to the local SQL Server instance.
* **Definition:** **Code-First Migrations**.
* **Purpose:** To physically build the database and tables in SQL Server based on the C# models. Instead of writing manual SQL `CREATE TABLE` scripts, we use Migrations to keep the database version-controlled. This ensures that the database structure always stays in sync with the source code.
* **Technical Commands:**
    ```bash
    # Step A: Create the migration (The "Plan")
    dotnet ef migrations add InitialCreate

    # Step B: Apply the migration (The "Build")
    dotnet ef database update
    ```
* **Validation:** Verified the creation of the `CropDealDB` and the `Users` table using the SQL Server Object Explorer.

### **Step 7: The Presentation Layer (Auth Controller)**
* **Action:** Created `AuthController.cs` and implemented the `Register` endpoint.
* **Definition:** Implementing **RESTful API Endpoints**.
* **Purpose:** To provide a public interface for the "Register" functionality. The controller receives user data via an HTTP POST request, utilizes the injected `AppDbContext` to persist the data to SQL Server, and returns a success status code.
* **Key Learning:** Used **Dependency Injection** to access the database and **Asynchronous Programming** (`async/await`) to ensure high-performance data processing.

### **Step 8: End-to-End Verification**
* **Action:** Performed a live registration test using Swagger UI.
* **Process:** 1. Sent an HTTP POST request to `/api/auth/register` with a JSON payload.
    2. Verified the response returned a `200 OK` status and the echoed User object.
    3. Confirmed the data persistence by querying the `Users` table in SQL Server.
* **Result:** Successfully confirmed that the Identity Microservice can receive, process, and store new user accounts.

### **Step 9: Security Layer (Password Hashing)**
* **Action:** Integrated `BCrypt.Net-Next` library.
* **Definition:** **One-way Cryptographic Hashing**.
* **Purpose:** To ensure that user passwords are never stored in plain text, protecting user privacy even in the event of a database breach.
* **Key Learning:** Learned how to use `BCrypt.HashPassword` to scramble data before saving.

### **Step 10: Authentication Logic (Login DTO & Verification)**
* **Action:** Created `LoginDto.cs` and implemented the `Login` endpoint.
* **Definition:** **Credential Verification**.
* **Purpose:** To validate incoming user credentials against the stored hashes in the database.
* **Key Learning:** Used `BCrypt.Verify` to compare plain-text input with hashed database records safely.

### **Step 11: JWT Implementation (Identity Tokens)**
* **Action:** Added `CreateToken` helper method and injected `IConfiguration` into the `AuthController`.
* **Definition:** **Bearer Token Generation**.
* **Purpose:** To issue a signed digital "badge" (JWT) containing user claims (Name, Email, Role) that can be verified by other microservices.
* **Key Learning:** Understood the structure of a JWT (Header, Payload, Signature) and how to sign it using a Symmetric Security Key.

### **Step 12: JWT Middleware and Token Generation**
* **Action:** Implemented `CreateToken` helper and configured JWT Authentication in `Program.cs`.
* **Definition:** **Stateless Authentication**.
* **Purpose:** To allow the system to verify users without constant database lookups. 
* **Key Learning:** Learned how to encode Claims (User info) into a signed token and configure the Middleware Pipeline to validate incoming requests.


## **Phase 2: Crop Microservice**

### **Step 13: Project Initialization**
* **Action:** Created a new Web API project named `CropService`.
* **Purpose:** To decouple the marketplace inventory logic from the authentication logic.
* **Key Learning:** Applying **Single Responsibility Principle (SRP)** at the architectural level by separating business domains into different projects.

### **Step 14: Data Modeling**
* **Action:** Defined the `Crop.cs` model and `CropDbContext`.
* **Purpose:** To manage crop-specific data (name, price, quantity) independently of user profiles.
* **Relationship:** Linking to the Identity Service via `FarmerEmail` as a logical foreign key.

### **Step 15: Crop Service Infrastructure**
* **Action:** Configured `Program.cs` with SQL Server and JWT Bearer authentication.
* **Purpose:** To prepare the service for secure data persistence.
* **Key Learning:** Learned how to resolve "DbContext" instantiation errors by correctly registering services in the Dependency Injection container.