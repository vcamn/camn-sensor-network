# Recipe: Fleet CRUD Endpoint

## Purpose

Implement a complete CRUD feature for a Fleet domain entity.

### A completed feature consists of

- Controller
- Service Interface
- Service
- DTOs
- Dependency Injection
- HTTP Test File
- Mapping
- Validation
- API Versioning

### Steps

1.
- Create DTOs rather than exposing Entity Framework entities.
- Generate DTO contracts
	- EntityDto
	- CreateEntityDto
	- UpdateEntityDto (when required)

2.
- Generate Service Interface
	- I`<Entity>`Service

3.
- Generate Service Implementation
	- `<Entity>`Service

4.
- Move all database access into the service layer.
- Database logic belongs only inside services.
- Use Include() only where required.
- Use async methods.

5.
- Generate mapping methods.
- Use private mapping methods inside the service.
- e.g., ToDto(Entity entity) and ToEntity(CreateEntityDto dto)

6.
- Register DI.
	- Register the interface for DI in Program.cs.

7.
- Apply controller conventions.

8.
- Generate HTTP test file.
- Using the existing project template and following existing project conventions.

9.
- Review generated code.

10.
- Verify the feature meets the CRUD Feature Complete Definition.