<h1 align="center">📞 CDRManager - Call Detail Records Management</h1>

<p align="center">
  <strong>A robust and scalable system for managing Call Detail Records (CDRs)</strong>
</p>

<hr />

<h2>🚀 Technology Choices</h2>
<p>
This project uses modern, scalable technology to ensure robustness, ease of maintenance and ease of development. Below are the main technologies and standards we used:
</p>

<ul>
  <li><strong>ASP.NET Core:</strong> Chosen for its high performance, cross-platform capabilities, and comprehensive support for REST APIs. ASP.NET Core also integrates well with dependency injection and middleware components, essential for building scalable web services.</li>

  <li><strong>Entity Framework Core (EF Core):</strong> Used as the Object-Relational Mapper (ORM) to handle database interactions. EF Core supports easy database migrations and enables the use of LINQ, ensuring readable and maintainable code when working with data.</li>

  <li><strong>xUnit:</strong> Selected for unit testing to validate the core functionalities of the application. xUnit is a popular choice within the .NET ecosystem due to its simplicity and seamless integration with Visual Studio.</li>
</ul>

<h3>🛠️ Design Patterns Implemented</h3>
<p>
Several key design patterns were implemented to ensure a modular and testable architecture:
</p>

<ul>
  <li><strong>Repository Pattern:</strong> Abstracts data access from business logic.</li>
  <li><strong>Dependency Injection:</strong> Facilitates decoupling of service instantiation and improves testability.</li>
  <li><strong>Service Layer Pattern:</strong> Encapsulates business logic in services, making controllers simpler and improving overall maintainability.</li>
</ul>

<hr />
<h2>📋Project Overview: CDRManager</h2>
<p>
<strong>Purpose:</strong> The project manages and processes Call Detail Records (CDRs), which log details of phone calls such as the caller, call duration, cost, and more. It provides services for storing, querying, and summarizing this call data.
</p>
<h3>Key Components:</h3>

<h4>CDRManager Application:</h4>
<ul>
  <li><strong>ASP.NET Core Application:</strong> It provides a web API for managing and querying CDR records.</li>
  <li><strong>Entity Framework Core:</strong> Used for interacting with the database where CDRs are stored.</li>
</ul>

<h4>Database Interaction (DbService):</h4>
<ul>
  <li><strong>Add CDR Records:</strong> Inserts new CDR records into the database.</li>
  <li><strong>Search by Reference:</strong> Finds a specific call record using a unique reference string.</li>
  <li><strong>Call Summary:</strong> Retrieves a summary of calls (total number and duration) for a specific date range, typically within a month.</li>
  <li><strong>Filter by Caller:</strong> Gets all CDRs for a specific caller within a given date range.</li>
  <li><strong>Most Expensive Calls:</strong> Retrieves the most expensive calls for a specific caller within a date range, filtered by currency (GBP).</li>
</ul>

<h4>Data Models:</h4>
<ul>
  <li><strong>CallDetailRecord:</strong> Represents the detailed information for each phone call.</li>
  <li><strong>CallType:</strong> Categorizes calls (e.g., international, local).</li>
  <li><strong>ResultPerMonth:</strong> Provides a summary of calls, including the total number of calls and total duration.</li>
</ul>

<h4>Controllers:</h4>
<p>
The CDRController handles HTTP requests and routes them to the appropriate service methods to interact with CDRs.
</p>

<h4>Testing:</h4>
<p>
There are unit tests to ensure the DbService methods (like getting call summaries or searching by reference) work as expected, enhancing the reliability of the application.
</p>

<hr />

<h2>⚙️ How to Run the Application and Tests Locally</h2>

<h3>🔧 Prerequisites:</h3>
<ul>
  <li>.NET 8.0 SDK</li>
  <li>SQL Server or another supported relational database</li>
  <li>Entity Framework Core CLI (optional, for database migrations)</li>
</ul>

<h3>📋 Steps to Run the Application:</h3>

<ol>
  <li>
    <strong>Clone the repository:</strong>
    <pre><code>git clone https://github.com/loliveir4/CDRManager.git</code></pre>
    <pre><code>cd CDRManager</code></pre>
  </li>

  <li>
    <strong>Configure the database:</strong>
    <p>
      Update the <code>appsettings.json</code> file in the CDRManager project with your database connection string. Example for SQL Server:
    </p>
    <pre><code>
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=CDRManagerDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
    </code></pre>
  </li>

  <li>
    <strong>Apply migrations:</strong>
    <p>Ensure Entity Framework Core is set up and apply the database migrations using:</p>
    <pre><code>dotnet ef database update</code></pre>
  </li>

  <li>
    <strong>Run the application:</strong>
    <p>To start the CDRManager project, run the following command:</p>
    <pre><code>dotnet run --project CDRManager</code></pre>
  </li>
</ol>

<hr />

<h2>✅ Running Tests</h2>

<p>
To run unit tests using xUnit, simply execute the command:
</p>
<pre><code>dotnet test</code></pre>

<hr />

<h2>🔮 Future Enhancements and Considerations</h2>

Given more time, the following improvements could be made:

<ul>
  <li><strong>Real-Time Currency Conversion:</strong> Currently, the currency conversion is static or provided as a parameter. We could integrate a third-party API to fetch real-time exchange rates for dynamic conversion.</li>

  <li><strong>Caching:</strong> Implement caching mechanisms to improve performance, especially for frequently accessed data or expensive database queries.</li>

  <li><strong>Authentication and Authorization:</strong> Implement authentication (e.g., JWT, OAuth) to secure the endpoints and ensure only authorized users can access sensitive data.</li>

  <li><strong>Error Handling:</strong> Improve error handling by providing more detailed responses with appropriate HTTP status codes and error messages.</li>

  <li><strong>Pagination:</strong> Depending on the size of the data returned, especially for lists of CDRs, it may be interesting to implement pagination for large volumes of data (using parameters such as page and pageSize).</li>
</ul>


<p align="center">Developed by Lucas Oliveira</p>
<p align="center">Email: lucasa.dev21@hotmal.com</p>
