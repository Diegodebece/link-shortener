# Link Shortener API

**Technologies used:** ASP.NET Core, C#, .NET 10, Entity Framework Core, PostgreSQL, Neon, Docker, Render, Clean Architecture, Minimal APIs.

Link Shortener API is a backend project for creating shortened URLs, redirecting users to the original URL, and tracking basic click statistics.

The project was built with a Clean Architecture approach, separating domain rules, application use cases, infrastructure concerns, and the HTTP API.

## Live API

The API is deployed on Render:

https://link-shortener-efvn.onrender.com

Health check:

https://link-shortener-efvn.onrender.com/health

## Features

- Create short links from long URLs
- Redirect short URLs to their original destination
- Track click count and last click date
- Store data in PostgreSQL
- Validate URLs and short codes through domain rules
- Centralized error handling middleware
- Health check endpoint
- Docker support
- Deployed to Render

## Architecture

The solution is organized into four main layers:

```text
LinkShortener.Domain
```

Contains the core business entities, value objects, domain exceptions, repository contracts, and domain services.

```text
LinkShortener.Application
```

Contains use cases, DTOs, application-level exceptions, and interfaces used by the application flow.

```text
LinkShortener.Infrastructure
```

Contains EF Core persistence, PostgreSQL configuration, repository implementations, migrations, Unit of Work, clock service, and short code generation.

```text
LinkShortener.Api
```

Exposes the HTTP endpoints using ASP.NET Core Minimal APIs.

## Main Endpoints

```http
GET /health
```

Checks if the API is running.

```http
POST /api/links
```

Creates a new short link.

Example body:

```json
{
  "originalUrl": "https://example.com",
  "expiresAt": null
}
```

```http
GET /{shortCode}
```

Redirects to the original URL and records a click.

```http
GET /api/links/{shortCode}/stats
```

Returns statistics for a short link.

## Docker

Build the API image:

```bash
docker build -t link-shortener-api .
```

Run the container:

```bash
docker run --name link-shortener-api -p 8080:8080 -e "ConnectionStrings__DefaultConnection=YOUR_CONNECTION_STRING" link-shortener-api
```

Then test:

```http
GET http://localhost:8080/health
```

## Deployment

The API is deployed with Docker on Render and uses Neon PostgreSQL as the cloud database.

Environment variable required:

```text
ConnectionStrings__DefaultConnection
```

## Notes

This project was built as a backend portfolio project focused on ASP.NET Core, PostgreSQL, Clean Architecture, Docker, and cloud deployment.
