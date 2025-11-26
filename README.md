# MicroServicos
Projeto de exemplo com arquitetura de microserviços (APIs, Domínio, Repositório e Serviço).

**Descrição curta**: solução .NET dividida em projetos pequenos para demonstrar comunicação via RabbitMQ e separação de responsabilidades.

**Estrutura do repositório**
- **`MS.Api`**: API web (Controllers)  ponto de entrada HTTP.
- **`MS.Domain`**: entidades e modelos do domínio.
- **`MS.Repository`**: implementação de persistência (MongoDB) e interfaces.
- **`MS.Service`**: serviços auxiliares, incluindo integração com RabbitMQ.

**Pré-requisitos**
- **.NET SDK 9.0**: instale o SDK correspondente ao target `net9.0`.
- **RabbitMQ**: broker local (recomendado via Docker) com plugin de Management para inspeção/remoção de filas.
- **(Opcional)**: MongoDB se quiser testar persistência.

**Como executar (rápido)**

- Inicie o RabbitMQ (Docker com Management):

```powershell
docker run -d --hostname rabbit --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

- Restaure, compile e execute a API (`MS.Api`):

```powershell
dotnet restore
dotnet build
dotnet run --project MS.Api
```

- Alternativa com `dotnet watch` (desenvolvimento):

```powershell
dotnet watch --project MS.Api run
```

**Acesso ao RabbitMQ Management**
- Abra `http://localhost:15672` e faça login com `guest` / `guest` (por padrão).

**Pontos principais do código**
- `MS.Service/RabbitMqService.cs`: classe que cria conexão e publica mensagens em `myQueue`.
- `MS.Api/Controllers`: endpoints que podem disparar mensagens (ver `RabbitMqController.cs`).

**Erro comum: PRECONDITION_FAILED (durable mismatch)**

Se você vir um erro como:

```
PRECONDITION_FAILED - inequivalent arg 'durable' for queue 'myQueue' ... received 'false' but current is 'true'
```

Significa que já existe uma fila chamada `myQueue` no RabbitMQ com propriedade `durable=true`, e o código está tentando declarar a mesma fila com `durable=false`. O RabbitMQ não permite redeclarar uma fila com propriedades diferentes.

Opções para resolver:
- **Fazer o código igual à fila existente (recomendado)**: declare a fila com `durable: true` e publique mensagens persistentes. Exemplo de alteração em `MS.Service/RabbitMqService.cs`:

```csharp
channel.QueueDeclare(queue: "myQueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var body = Encoding.UTF8.GetBytes(message);
var props = channel.CreateBasicProperties();
props.Persistent = true;

channel.BasicPublish(exchange: "",
                     routingKey: "myQueue",
                     basicProperties: props,
                     body: body);
```

Também é recomendável alterar a assinatura `public async void SendMessageAsync(...)` para `public Task SendMessageAsync(...)` (ou `public async Task`) para que exceções possam ser aguardadas ou observadas corretamente.

- **Remover a fila existente no broker**: se você preferir manter `durable:false` no código, remova a fila existente (atenção: isso pode afetar outros consumidores).

  - Pelo Management UI: `Queues`  selecione `myQueue`  `Delete`.
  - Via HTTP API (exemplo cURL / PowerShell):

```powershell
# cURL
curl -u guest:guest -X DELETE http://localhost:15672/api/queues/%2F/myQueue

# PowerShell (Invoke-RestMethod)
Invoke-RestMethod -Uri "http://localhost:15672/api/queues/%2F/myQueue" -Method Delete -Credential (Get-Credential)
```

**Boas práticas**
- Garanta que todas as aplicações que declaram a mesma fila usem os mesmos argumentos (`durable`, `exclusive`, `autoDelete`, `arguments`).
- Use `props.Persistent = true` para mensagens que precisam sobreviver a restart do broker quando a fila for `durable`.
- Evite `async void` em métodos que não são handlers de eventos; prefira `Task`.

**Comandos úteis**
- Build e run específico do projeto:

```powershell
dotnet run --project MS.Api
```

- Abrir a UI do RabbitMQ: `http://localhost:15672` (usuário `guest` / `guest`).

**Contribuição**
- Pull requests são bem-vindos. Descreva mudanças de arquitetura ou dependências no PR.