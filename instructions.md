# Instructions for Running the Application

## Prerequisites
To run this application, you need the following installed on your machine:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- A modern code editor, such as [Visual Studio Code](https://code.visualstudio.com/) (used one)
- [Postman](https://www.postman.com/) or a similar API testing tool (optional, for testing the API endpoints)

## Cloning the Repository
1. Open a terminal.
2. Clone the repository using the following command:
   ```bash
   git clone <repository_url>
   ```
3. Navigate to the project folder:
   ```bash
   cd <project_folder>
   ```

## Installing Dependencies
The project uses .NET dependencies that must be restored before running the application. Run the following command in the project folder:
```bash
 dotnet restore
```

## Running the Application
1. Navigate to the project folder if you haven’t already:
   ```bash
   cd <project_folder>
   ```
2. Navigate to the src folder:
   ```bash
   cd src
   ```
3. Start the application using the .NET CLI:
   ```bash
   dotnet run
   ```
   By default, the application will run on `https://localhost:7240` (HTTPS) and `http://localhost:5271` (HTTP).
   
3. Optionally, you can use Swagger to explore and test the API by visiting:
   ```
   https://localhost:7240/swagger
   ```

## Endpoints

### Pedido Endpoints

#### Create Pedido
- **URL**: `POST /api/pedido`
- **Payload**:
   ```json
   {
       "codigo": "123456",
       "itens": [
           {
               "descricao": "Item A",
               "precoUnitario": 10,
               "qtd": 1
           },
           {
               "descricao": "Item B",
               "precoUnitario": 5,
               "qtd": 2
           }
       ]
   }
   ```
- **Response**:
   - 201 Created with the created pedido details.

#### Get Pedido
- **URL**: `GET /api/pedido/{codigo}`
- **Response**:
   - 200 OK with the pedido details.
   - 404 Not Found with `{ "status": "CODIGO_PEDIDO_INVALIDO" }`.

#### Update Pedido
- **URL**: `PUT /api/pedido/{codigo}`
- **Payload**:
   ```json
   {
       "codigo": "123456",
       "itens": [
           {
               "descricao": "Item C",
               "precoUnitario": 20,
               "qtd": 3
           }
       ]
   }
   ```
- **Response**:
   - 204 No Content.
   - 404 Not Found with `{ "status": "CODIGO_PEDIDO_INVALIDO" }`.

#### Delete Pedido
- **URL**: `DELETE /api/pedido/{codigo}`
- **Response**:
   - 204 No Content.
   - 404 Not Found with `{ "status": "CODIGO_PEDIDO_INVALIDO" }`.

### Status Endpoints

#### Change Status
- **URL**: `POST /api/status`
- **Payload**:
   ```json
   {
       "status": "APROVADO",
       "itensAprovados": 3,
       "valorAprovado": 20,
       "pedido": "123456"
   }
   ```
- **Response Examples**:
   - 200 OK with:
     ```json
     {
         "pedido": "123456",
         "status": ["APROVADO"]
     }
     ```
   - 404 Not Found with:
     ```json
     {
         "pedido": "123456-N",
         "status": ["CODIGO_PEDIDO_INVALIDO"]
     }
     ```

## Running Tests
The project includes unit and integration tests to validate functionality.

1. Navigate to the test project folder:
   ```bash
   cd <test_project_folder>
   ```
2. Run the tests using the .NET CLI:
   ```bash
   dotnet test
   ```

## Additional Notes
- The project uses an in-memory database, so all data will be reset when the application restarts.