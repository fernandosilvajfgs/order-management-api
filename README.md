# Technical Challenge - RESTful API in .NET Core

This project was developed as part of a technical challenge for a recruitment process. The goal was to create a RESTful API for order management, ensuring adherence to **SOLID principles**, **clean code**, and designing a solution that is **extensible** and capable of handling **high concurrency**.

## Features

- **CRUD operations** for managing orders.
- Endpoint to update the status of orders, including comprehensive business logic.
- **In-memory database** for quick setup and testing.
- Automated tests (**unit tests** and **integration tests**) to ensure reliability.
- **Swagger** integration for API documentation.
- Clean and extensible architecture.

## Technologies Used

- **.NET 6** / ASP.NET Core
- **Entity Framework Core** (for database operations)
- **xUnit** (for automated testing)

## How to Run the Project

Follow the instructions provided in the `instructions.md` file to set up and run the project locally.

## Business Logic Rules

### Status Update Rules

The API processes status changes for orders according to the following rules:

1. **Order Not Found**:
   - Response: `{"status": "CODIGO_PEDIDO_INVALIDO"}`

2. **Status `REPROVADO`**:
   - Response: `{"status": "REPROVADO"}`

3. **Status `APROVADO`**:
   - Matches order total value and quantity:
     - Response: `{"status": "APROVADO"}`
   - Value less than total:
     - Response: `{"status": "APROVADO_VALOR_A_MENOR"}`
   - Quantity less than total:
     - Response: `{"status": "APROVADO_QTD_A_MENOR"}`
   - Value greater than total:
     - Response: `{"status": "APROVADO_VALOR_A_MAIOR"}`
   - Quantity greater than total:
     - Response: `{"status": "APROVADO_QTD_A_MAIOR"}`

## Notes

- The project was designed for demonstration purposes as part of a recruitment process.
- Some adjustments, such as authentication and a persistent database, would be necessary for production.
- Feel free to fork and suggest improvements.

## License

This project is open-source and available under the [MIT License](LICENSE).

---

Thank you for exploring this project! If you have any questions or feedback, please feel free to reach out.
