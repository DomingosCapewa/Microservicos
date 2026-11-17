# Microservicos

Projeto demonstrativo em .NET com separação entre API, domínio, persistência e serviços de mensageria.

## Arquitetura

- `MS.Api`: entrada HTTP e controllers;
- `MS.Domain`: entidades e modelos;
- `MS.Repository`: interfaces e persistência com MongoDB;
- `MS.Service`: serviços de aplicação e integração com RabbitMQ.

## Requisitos

- .NET SDK 9.0;
- RabbitMQ;
- MongoDB, caso queira executar os fluxos de persistência.

## Executando

Suba o RabbitMQ com o painel de gerenciamento:

```bash
docker run -d --hostname rabbit --name rabbitmq \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management
```

Restaure, compile e execute a API:

```bash
dotnet restore
dotnet build
dotnet run --project MS.Api
```

O painel do RabbitMQ fica disponível em `http://localhost:15672`, usando as credenciais locais padrão da imagem (`guest`/`guest`).

## Mensageria

O serviço `MS.Service/RabbitMqService.cs` publica mensagens na fila `myQueue`, e os endpoints em `MS.Api/Controllers` podem disparar esse fluxo.

Ao declarar uma fila, todos os consumidores precisam usar os mesmos valores de `durable`, `exclusive`, `autoDelete` e `arguments`. Se o broker acusar `PRECONDITION_FAILED`, remova a fila somente em um ambiente local descartável ou alinhe a declaração do código com a fila já existente.

## Desenvolvimento

```bash
dotnet watch --project MS.Api run
```

Evite `async void` em serviços: métodos assíncronos devem retornar `Task` para que erros possam ser aguardados e tratados.
