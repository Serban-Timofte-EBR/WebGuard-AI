# WebGuard AI - Backend Features Overview

## Project Overview
The **WebGuard AI** backend is built using **C# with .NET Core** and serves as the core service layer that handles requests from the frontend and communicates with the AI model service for website security analysis. The backend provides a set of RESTful APIs for submitting URLs, processing analysis requests, fetching results, and managing other core functionalities of the application.

## Key Features

### 1. URL Submission and Validation
- **Description**: An API endpoint to accept URLs submitted by users from the frontend. The endpoint will perform basic validation to ensure the URL is well-formed and possibly check against a list of known malicious or safe URLs for initial filtering.
- **Endpoint**: `POST /api/url/submit`
- **Request Body**: `{ "url": "https://example.com" }`
- **Response**: `{ "id": "12345", "status": "submitted" }`

### 2. URL Security Analysis Request
- **Description**: This feature involves sending a request to the AI model service to analyze the submitted URL for potential security threats (e.g., phishing, malware, vulnerabilities). The backend will handle asynchronous communication with the Python AI service and manage the state of the analysis request.
- **Endpoint**: `POST /api/url/analyze`
- **Request Body**: `{ "id": "12345" }`
- **Response**: `{ "id": "12345", "status": "processing" }`

### 3. Fetch Analysis Results
- **Description**: An API endpoint that allows the frontend to fetch the results of a security analysis for a specific URL. The endpoint will query the database or cache for the status and details of the analysis.
- **Endpoint**: `GET /api/url/results/{id}`
- **Response**: 
  ```json
  {
    "id": "12345",
    "url": "https://example.com",
    "status": "completed",
    "results": {
      "phishing": false,
      "malware": false,
      "vulnerabilities": ["SQL Injection", "XSS"]
    }
  }

### 4. Health Check and Monitoring
- **Description**: A health check endpoint to ensure the backend service is running correctly. This endpoint will be useful for monitoring purposes and integration with DevOps pipelines.
- **Endpoint**: GET /api/health
- **Response**: { "status": "healthy" }

### 5. Integration with AI Model Service (Python API)
- **Description**: The backend will integrate with the Python-based AI model service, sending requests and receiving responses for URL analysis. This integration will be handled using the `HttpClient` class in .NET Core, managing timeouts, retries, and error handling.
- **Internal Communication**: `HttpClient` calls to Python service endpoints.

### 6. User Authentication and Authorization
- **Description**: Implement a robust authentication and authorization system using **JWT (JSON Web Tokens)** to secure API endpoints. This ensures that only authenticated users can submit URLs, request analysis, and fetch results. Authorization rules will control access to specific resources based on user roles (e.g., admin, regular user).
- **Endpoints Affected**: 
  - All secure endpoints (e.g., `/api/url/submit`, `/api/url/results/{id}`).
  - Authentication endpoints: `/api/auth/login`, `/api/auth/register`.
- **Authentication Flow**:
  1. **User Registration**: New users can register via `/api/auth/register`, providing necessary details (e.g., email, password).
  2. **User Login**: Registered users can log in via `/api/auth/login`, receiving a JWT token upon successful authentication.
  3. **Token-Based Access**: The JWT token must be included in the `Authorization` header (`Bearer <token>`) for all subsequent API requests to access secure endpoints.
  4. **Token Validation**: Middleware will verify the JWT token on each request to ensure validity, expiration, and user roles.
- **Endpoint Examples**:
  - `POST /api/auth/register`: Registers a new user.
  - `POST /api/auth/login`: Authenticates a user and returns a JWT token.
  - `GET /api/url/results/{id}`: Requires `Authorization` header with a valid JWT token.

## Technical Stack
- **Framework**: ASP.NET Core Web API
- **Language**: C#
- **Database**: In-memory database for initial development; extendable to SQL Server or NoSQL (e.g., Cosmos DB)
- **API Communication**: REST APIs using JSON
- **External Service Integration**: AI Model Service (Python Flask/FastAPI)
- **Security**: JWT-based authentication and role-based authorization