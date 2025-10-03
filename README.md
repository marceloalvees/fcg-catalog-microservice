# FCG - FIAP Cloud Catalog

**Microsserviço para gerenciamento de Catálogos de Jogos na FGC**

---

## Resumo

Este repositório contém um microsserviço desenvolvido em .NET 9 para o gerenciamento de catálogos de jogos na FGC (FIAP Cloud Games). O projeto adota a Arquitetura Hexagonal (Ports & Adapters), promovendo desacoplamento entre regras de negócio e infraestrutura, facilitando testes, manutenção e integração com diferentes tecnologias. Utiliza ElasticSearch como mecanismo de persistência e expõe uma API REST para operações relacionadas a catálogos e jogos.

## Índice

- [Resumo](#resumo)
- [Visão Geral](#visão-geral)
- [Camadas do Projeto](#camadas-do-projeto)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Principais Componentes](#principais-componentes)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Testes](#testes)
- [Como Executar](#como-executar)
- [Endpoints da API](#endpoints-da-api)
- [Observações](#observações)

## Visão Geral

Este projeto é um microsserviço responsável pelo gerenciamento de catálogos de jogos na FGC, desenvolvido em .NET 9. Ele segue os princípios da Arquitetura Hexagonal (Ports & Adapters), promovendo separação de responsabilidades, testabilidade e flexibilidade para integração com diferentes tecnologias.

O sistema permite a criação, manutenção e consulta de catálogos, onde cada catálogo pode conter múltiplos jogos, fornecendo uma organização hierárquica dos conteúdos da plataforma.

### Camadas do Projeto

- **Domínio ([`src/Domain`](src/Domain))**: Contém as entidades de negócio (`Catalog`), regras de domínio e interfaces (ports) que definem contratos para operações essenciais, como repositórios.
- **Aplicação ([`src/Core/Application`](src/Core/Application))**: Implementa os casos de uso (use cases) do sistema através do padrão CQRS com MediatR, orquestrando as operações do domínio e validando dados de entrada com FluentValidation.
- **Infraestrutura ([`src/Driven/Infrastructure.ElasticSearch`](src/Driven/Infrastructure.ElasticSearch))**: Implementa os adaptadores (adapters) para tecnologias externas, como persistência em ElasticSearch, seguindo os contratos definidos no domínio.
- **API ([`src/Driving/Api`](src/Driving/Api))**: Camada de apresentação, expõe endpoints HTTP RESTful, recebe requisições, faz a orquestração dos casos de uso e retorna respostas padronizadas.

### Estrutura de Pastas

| Pasta                                               | Papel na Arquitetura Hexagonal                  | Tipo         |
|-----------------------------------------------------|--------------------------------------------------|--------------|
| [`src/Domain/`](src/Domain)                         | Núcleo do domínio, entidades, regras, ports     | Domínio/Port |
| [`src/Core/Application/`](src/Core/Application)     | Casos de uso, validações, handlers              | Aplicação    |
| [`src/Driven/Infrastructure.ElasticSearch/`](src/Driven/Infrastructure.ElasticSearch) | Integrações externas, persistência, adapters | Adapter      |
| [`src/Driving/Api/`](src/Driving/Api)               | API REST, controllers, entrada do sistema       | Adapter      |
| [`tests/UnitTests/`](tests/UnitTests)               | Testes unitários das regras de negócio          | Testes       |
| [`tests/IntegrationTests/`](tests/IntegrationTests) | Testes de integração da API e infraestrutura    | Testes       |

> As pastas de infraestrutura e API representam os "adapters" da arquitetura hexagonal, conectando o núcleo do domínio a tecnologias externas (bancos, frameworks, protocolos, etc) e à interface de entrada (HTTP/API).

> O projeto segue a organização de pastas baseada na Arquitetura Hexagonal com separação clara entre `Driving` (adaptadores de entrada) e `Driven` (adaptadores de saída).

## Principais Componentes

### Entidades de Domínio
- **Catalog**: Representa um catálogo que pode conter múltiplos jogos
- **EntityBase**: Classe base para entidades com propriedades comuns

### Casos de Uso (Application)
- **Commands**: Operações de escrita (Create, Update, Delete)
- **Queries**: Operações de leitura (Get, List, Search)
- **Validators**: Validações usando FluentValidation
- **DTOs**: Objetos de transferência de dados para contratos da API

### Adaptadores
- **Controllers**: Endpoints REST para catálogos
- **Repositories**: Implementações concretas para persistência em ElasticSearch
- **Health Checks**: Monitoramento da saúde da aplicação

## Tecnologias Utilizadas

- **.NET 9**: Framework principal
- **MediatR**: Implementação do padrão Mediator para CQRS
- **FluentValidation**: Validação de dados de entrada
- **ElasticSearch/OpenSearch**: Mecanismo de persistência e busca
- **Swagger/OpenAPI**: Documentação da API
- **Prometheus**: Métricas e observabilidade
- **Health Checks**: Monitoramento da aplicação
- **Docker**: Containerização da aplicação

## Testes

O projeto possui testes unitários e de integração localizados na pasta `tests/`, cobrindo:
- Casos de uso e regras de negócio
- Validações de entrada
- Endpoints da API
- Integração com ElasticSearch

### Executar Testes

```powershell
# Executar todos os testes
dotnet test

# Executar apenas testes unitários
dotnet test tests/UnitTests/UnitTests.csproj

# Executar apenas testes de integração
dotnet test tests/IntegrationTests/IntegrationTests.csproj
```

## Como Executar

1. **Clone o repositório:**
   ```powershell
   git clone https://github.com/marceloalvees/fcg-catalog-microservice
   cd fcg-catalog-microservice
   ```

2. **Restaure as dependências:**
   ```powershell
   dotnet restore
   ```

3. **Configure o ElasticSearch:**
   - Ajuste as configurações em `src/Driving/Api/appsettings.json`
   - Certifique-se de que o ElasticSearch esteja rodando

4. **Execute a aplicação:**
   ```powershell
   dotnet run --project src/Driving/Api/Api.csproj
   ```

5. **Acesse a documentação da API:**
   - Swagger UI: [http://localhost:5000/swagger](http://localhost:5000/swagger)
   - Health Checks: [http://localhost:5000/health](http://localhost:5000/health)
   - Métricas: [http://localhost:5000/metrics](http://localhost:5000/metrics)

### Executar com Docker

```powershell
# Build da imagem
docker build -t fcg-catalog .

# Executar container
docker run -p 5000:5000 fcg-catalog
```

## Endpoints da API

### Catálogos
   - `GET /api/catalogs` - Listar catálogos
   - `GET /api/catalogs/{id}` - Obter catálogo por ID
   - `POST /api/catalogs` - Criar novo catálogo
   - `PUT /api/catalogs/{id}` - Atualizar catálogo
   - `DELETE /api/catalogs/{id}` - Remover catálogo
   - `POST /api/catalogs/Add` - Adicionar jogos ao catálogo
   - `POST /api/catalogs/Remove` - Remover jogos do catálogo

### Monitoramento
- `GET /health` - Status da aplicação
- `GET /metrics` - Métricas Prometheus

## Observações

- O projeto utiliza ElasticSearch/OpenSearch como mecanismo de persistência
- Configurações específicas podem ser ajustadas nos arquivos `appsettings.*.json`
- A aplicação suporta Health Checks para monitoramento
- Métricas Prometheus estão disponíveis para observabilidade
- A API é documentada automaticamente via Swagger/OpenAPI

### Configuração do ElasticSearch

```json
{
   "ElasticSearchSettings": {
      "Endpoint": "http://localhost:9200",
      "AccessKey": "your-access-key",
      "Secret": "your-secret-key",
      "IndexName": "fcg-catalog",
      "Region": "your-region"
   }
}
```

---

Desenvolvido seguindo boas práticas de arquitetura para facilitar manutenção, testes e evolução do sistema de catálogos da FGC.
