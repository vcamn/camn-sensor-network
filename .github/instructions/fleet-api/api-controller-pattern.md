# API Controller Pattern

Controllers are responsible only for HTTP concerns.

### Controllers:

- never access FleetDbContext
- never contain business logic
- depend only on service interfaces
- return ActionResult<T>
- return DTOs only
- use constructor injection
- use async methods

#### Routing

- ApiController
- ApiVersion("1.0")
- Route("api/v{version:apiVersion}/[controller]")

#### Responses

- Ok()
- CreatedAtAction()
- NoContent()
- BadRequest()
- NotFound()

Controllers follow existing project naming and folder conventions. 