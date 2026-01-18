# Auth API .NET 8 - Micro-Tarefas de Implementação

> **Projeto:** Auth API (Identity Provider) para emissão de JWT
> **Stack:** .NET 8, Clean Architecture, Dapper, PostgreSQL, xUnit
> **Integração:** BarberBoss API (Resource Server)

---

## Legenda de Status

- [ ] Não iniciado
- [x] Concluído
- 🔒 Bloqueado (dependência não concluída)
- ⚠️ Atenção especial de segurança

---

## FASE 0: Preparação e Fundamentos Teóricos

### 0.1 Estudo de Criptografia e Hashing

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 0.1.1 | Pesquisar diferença entre encryption (reversível) e hashing (irreversível) | Estudo | 30min |
| 0.1.2 | Pesquisar o que é salt e por que previne rainbow tables | Estudo | 20min |
| 0.1.3 | Pesquisar por que bcrypt/Argon2 são preferidos sobre SHA256 para senhas | Estudo | 30min |
| 0.1.4 | Pesquisar o conceito de "work factor" / "cost factor" no bcrypt | Estudo | 15min |
| 0.1.5 | Criar console app de experimentação com nome `HashExperiments` | Código | 10min |
| 0.1.6 | Instalar pacote `BCrypt.Net-Next` no console app | Código | 5min |
| 0.1.7 | Implementar método que gera hash de uma senha | Código | 10min |
| 0.1.8 | Implementar método que verifica senha contra hash | Código | 10min |
| 0.1.9 | Experimento: hashear mesma senha 3x e comparar resultados | Validação | 10min |
| 0.1.10 | Experimento: verificar que hashes diferentes validam a mesma senha | Validação | 10min |
| 0.1.11 | Experimento: medir tempo de hash com work factor 10, 12, 14 | Validação | 15min |
| 0.1.12 | Documentar aprendizados em arquivo `NOTES_HASHING.md` | Doc | 20min |

### 0.2 Estudo de JWT (JSON Web Token)

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 0.2.1 | Pesquisar estrutura do JWT: Header.Payload.Signature | Estudo | 30min |
| 0.2.2 | Pesquisar encoding Base64Url (não é criptografia!) | Estudo | 15min |
| 0.2.3 | Pesquisar claims padrão: iss, sub, aud, exp, iat, nbf, jti | Estudo | 30min |
| 0.2.4 | Pesquisar diferença entre HS256 (simétrico) e RS256 (assimétrico) | Estudo | 30min |
| 0.2.5 | Pesquisar quando usar HS256 vs RS256 (cenários) | Estudo | 20min |
| 0.2.6 | Criar console app `JwtExperiments` | Código | 10min |
| 0.2.7 | Instalar pacote `System.IdentityModel.Tokens.Jwt` | Código | 5min |
| 0.2.8 | Gerar um JWT com HS256 manualmente | Código | 30min |
| 0.2.9 | Decodificar o JWT gerado em jwt.io | Validação | 10min |
| 0.2.10 | Experimento: modificar 1 caractere do payload e verificar assinatura inválida | Validação | 10min |
| 0.2.11 | Gerar par de chaves RSA (2048 bits) | Código | 20min |
| 0.2.12 | Salvar chaves em formato PEM (public e private separados) | Código | 15min |
| 0.2.13 | Gerar um JWT com RS256 usando a private key | Código | 30min |
| 0.2.14 | Validar o JWT RS256 usando apenas a public key | Código | 20min |
| 0.2.15 | Documentar aprendizados em arquivo `NOTES_JWT.md` | Doc | 25min |

### 0.3 Estudo de Autenticação vs Autorização

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 0.3.1 | Pesquisar definição precisa de Autenticação (AuthN) | Estudo | 15min |
| 0.3.2 | Pesquisar definição precisa de Autorização (AuthZ) | Estudo | 15min |
| 0.3.3 | Pesquisar o fluxo OAuth 2.0 simplificado | Estudo | 45min |
| 0.3.4 | Pesquisar diferença entre Access Token e Refresh Token | Estudo | 20min |
| 0.3.5 | Pesquisar conceito de Identity Provider (IdP) vs Resource Server | Estudo | 20min |
| 0.3.6 | Pesquisar o que é RBAC (Role-Based Access Control) | Estudo | 20min |
| 0.3.7 | Pesquisar o que é ABAC (Attribute-Based Access Control) | Estudo | 20min |
| 0.3.8 | Documentar aprendizados em arquivo `NOTES_AUTH_CONCEPTS.md` | Doc | 25min |

---

## FASE 1: Estrutura do Projeto Auth API

### 1.1 Criação da Solution e Projetos

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 1.1.1 | Criar pasta raiz `AuthService` | Setup | 2min |
| 1.1.2 | Executar `dotnet new sln -n AuthService` | Setup | 2min |
| 1.1.3 | Criar projeto `dotnet new classlib -n AuthService.Domain` | Setup | 2min |
| 1.1.4 | Criar projeto `dotnet new classlib -n AuthService.Application` | Setup | 2min |
| 1.1.5 | Criar projeto `dotnet new classlib -n AuthService.Infrastructure` | Setup | 2min |
| 1.1.6 | Criar projeto `dotnet new webapi -n AuthService.Api` | Setup | 2min |
| 1.1.7 | Criar projeto `dotnet new xunit -n AuthService.Tests` | Setup | 2min |
| 1.1.8 | Adicionar todos os projetos à solution | Setup | 5min |
| 1.1.9 | Configurar referências: Api → Application → Domain | Setup | 5min |
| 1.1.10 | Configurar referências: Infrastructure → Application | Setup | 3min |
| 1.1.11 | Configurar referências: Api → Infrastructure | Setup | 3min |
| 1.1.12 | Configurar referências: Tests → todos os projetos | Setup | 5min |
| 1.1.13 | Verificar que solution compila sem erros | Validação | 5min |
| 1.1.14 | Criar arquivo `.gitignore` apropriado | Setup | 5min |
| 1.1.15 | Inicializar repositório Git | Setup | 3min |
| 1.1.16 | Commit inicial: "chore: project structure" | Setup | 3min |

### 1.2 Estrutura de Pastas (Domain)

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 1.2.1 | Criar pasta `Entities` em Domain | Setup | 1min |
| 1.2.2 | Criar pasta `Interfaces` em Domain | Setup | 1min |
| 1.2.3 | Criar pasta `Interfaces/Repositories` em Domain | Setup | 1min |
| 1.2.4 | Criar pasta `Interfaces/Services` em Domain | Setup | 1min |
| 1.2.5 | Criar pasta `ValueObjects` em Domain (opcional) | Setup | 1min |
| 1.2.6 | Criar pasta `Exceptions` em Domain | Setup | 1min |
| 1.2.7 | Remover arquivo `Class1.cs` gerado automaticamente | Setup | 1min |

### 1.3 Estrutura de Pastas (Application)

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 1.3.1 | Criar pasta `UseCases` em Application | Setup | 1min |
| 1.3.2 | Criar pasta `UseCases/Register` em Application | Setup | 1min |
| 1.3.3 | Criar pasta `UseCases/Login` em Application | Setup | 1min |
| 1.3.4 | Criar pasta `UseCases/RefreshToken` em Application | Setup | 1min |
| 1.3.5 | Criar pasta `UseCases/Logout` em Application | Setup | 1min |
| 1.3.6 | Criar pasta `DTOs` em Application | Setup | 1min |
| 1.3.7 | Criar pasta `DTOs/Requests` em Application | Setup | 1min |
| 1.3.8 | Criar pasta `DTOs/Responses` em Application | Setup | 1min |
| 1.3.9 | Criar pasta `Interfaces` em Application | Setup | 1min |
| 1.3.10 | Remover arquivo `Class1.cs` gerado automaticamente | Setup | 1min |

### 1.4 Estrutura de Pastas (Infrastructure)

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 1.4.1 | Criar pasta `Repositories` em Infrastructure | Setup | 1min |
| 1.4.2 | Criar pasta `Services` em Infrastructure | Setup | 1min |
| 1.4.3 | Criar pasta `Data` em Infrastructure | Setup | 1min |
| 1.4.4 | Criar pasta `Security` em Infrastructure | Setup | 1min |
| 1.4.5 | Remover arquivo `Class1.cs` gerado automaticamente | Setup | 1min |

### 1.5 Estrutura de Pastas (Api)

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 1.5.1 | Criar pasta `Controllers` em Api (se não existir) | Setup | 1min |
| 1.5.2 | Criar pasta `Filters` em Api | Setup | 1min |
| 1.5.3 | Criar pasta `Middlewares` em Api | Setup | 1min |
| 1.5.4 | Criar pasta `Extensions` em Api | Setup | 1min |
| 1.5.5 | Remover `WeatherForecastController` e `WeatherForecast` | Setup | 2min |

### 1.6 Instalação de Pacotes NuGet

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 1.6.1 | Instalar `Dapper` em Infrastructure | Setup | 2min |
| 1.6.2 | Instalar `Npgsql` em Infrastructure | Setup | 2min |
| 1.6.3 | Instalar `BCrypt.Net-Next` em Infrastructure | Setup | 2min |
| 1.6.4 | Instalar `System.IdentityModel.Tokens.Jwt` em Infrastructure | Setup | 2min |
| 1.6.5 | Instalar `Microsoft.AspNetCore.Authentication.JwtBearer` em Api | Setup | 2min |
| 1.6.6 | Instalar `FluentValidation` em Application | Setup | 2min |
| 1.6.7 | Instalar `FluentValidation.DependencyInjectionExtensions` em Api | Setup | 2min |
| 1.6.8 | Instalar `Moq` em Tests | Setup | 2min |
| 1.6.9 | Instalar `FluentAssertions` em Tests (opcional) | Setup | 2min |
| 1.6.10 | Verificar que solution ainda compila | Validação | 3min |
| 1.6.11 | Commit: "chore: add nuget packages" | Setup | 2min |

---

## FASE 2: Entidades e Interfaces (Domain)

### 2.1 Entidade User

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 2.1.1 | Criar classe `User` em Domain/Entities | Código | 5min | |
| 2.1.2 | Adicionar propriedade `Id` (Guid) | Código | 2min | |
| 2.1.3 | Adicionar propriedade `Email` (string) | Código | 2min | |
| 2.1.4 | Adicionar propriedade `PasswordHash` (string) | Código | 2min | ⚠️ Nunca expor |
| 2.1.5 | Adicionar propriedade `Name` (string) | Código | 2min | |
| 2.1.6 | Adicionar propriedade `CreatedAt` (DateTime) | Código | 2min | |
| 2.1.7 | Adicionar propriedade `UpdatedAt` (DateTime?) | Código | 2min | |
| 2.1.8 | Adicionar propriedade `IsActive` (bool) | Código | 2min | |
| 2.1.9 | Adicionar propriedade `Roles` (List<string>) | Código | 3min | |
| 2.1.10 | Criar construtor privado (para Dapper) | Código | 3min | |
| 2.1.11 | Criar factory method `Create(...)` | Código | 10min | |
| 2.1.12 | Garantir que User é imutável externamente | Código | 5min | |
| 2.1.13 | Escrever teste unitário para criação de User válido | Teste | 15min | |
| 2.1.14 | Commit: "feat(domain): add User entity" | Setup | 2min | |

### 2.2 Entidade RefreshToken

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 2.2.1 | Criar classe `RefreshToken` em Domain/Entities | Código | 5min | |
| 2.2.2 | Adicionar propriedade `Id` (Guid) | Código | 2min | |
| 2.2.3 | Adicionar propriedade `Token` (string) - valor opaco | Código | 2min | ⚠️ Gerar com CSPRNG |
| 2.2.4 | Adicionar propriedade `UserId` (Guid) | Código | 2min | |
| 2.2.5 | Adicionar propriedade `ExpiresAt` (DateTime) | Código | 2min | |
| 2.2.6 | Adicionar propriedade `CreatedAt` (DateTime) | Código | 2min | |
| 2.2.7 | Adicionar propriedade `RevokedAt` (DateTime?) | Código | 2min | |
| 2.2.8 | Adicionar propriedade `ReplacedByToken` (string?) | Código | 2min | |
| 2.2.9 | Adicionar propriedade `DeviceInfo` (string?) | Código | 2min | |
| 2.2.10 | Adicionar método `IsExpired` (bool) | Código | 5min | |
| 2.2.11 | Adicionar método `IsRevoked` (bool) | Código | 3min | |
| 2.2.12 | Adicionar método `IsActive` (bool) | Código | 3min | |
| 2.2.13 | Adicionar método `Revoke(replacedBy?)` | Código | 5min | |
| 2.2.14 | Escrever teste unitário para RefreshToken | Teste | 15min | |
| 2.2.15 | Commit: "feat(domain): add RefreshToken entity" | Setup | 2min | |

### 2.3 Interfaces de Repositório

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 2.3.1 | Criar interface `IUserRepository` em Domain/Interfaces/Repositories | Código | 5min |
| 2.3.2 | Definir método `Task<User?> GetByIdAsync(Guid id)` | Código | 2min |
| 2.3.3 | Definir método `Task<User?> GetByEmailAsync(string email)` | Código | 2min |
| 2.3.4 | Definir método `Task<bool> ExistsAsync(string email)` | Código | 2min |
| 2.3.5 | Definir método `Task CreateAsync(User user)` | Código | 2min |
| 2.3.6 | Definir método `Task UpdateAsync(User user)` | Código | 2min |
| 2.3.7 | Criar interface `IRefreshTokenRepository` | Código | 5min |
| 2.3.8 | Definir método `Task<RefreshToken?> GetByTokenAsync(string token)` | Código | 2min |
| 2.3.9 | Definir método `Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId)` | Código | 2min |
| 2.3.10 | Definir método `Task CreateAsync(RefreshToken refreshToken)` | Código | 2min |
| 2.3.11 | Definir método `Task UpdateAsync(RefreshToken refreshToken)` | Código | 2min |
| 2.3.12 | Definir método `Task RevokeAllByUserIdAsync(Guid userId)` | Código | 2min |
| 2.3.13 | Commit: "feat(domain): add repository interfaces" | Setup | 2min |

### 2.4 Interfaces de Serviços

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 2.4.1 | Criar interface `IPasswordHasher` em Domain/Interfaces/Services | Código | 5min |
| 2.4.2 | Definir método `string Hash(string password)` | Código | 2min |
| 2.4.3 | Definir método `bool Verify(string password, string hash)` | Código | 2min |
| 2.4.4 | Criar interface `ITokenService` em Domain/Interfaces/Services | Código | 5min |
| 2.4.5 | Definir método `string GenerateAccessToken(User user)` | Código | 2min |
| 2.4.6 | Definir método `RefreshToken GenerateRefreshToken(Guid userId, string? deviceInfo)` | Código | 2min |
| 2.4.7 | Definir método `ClaimsPrincipal? ValidateAccessToken(string token)` | Código | 2min |
| 2.4.8 | Commit: "feat(domain): add service interfaces" | Setup | 2min |

### 2.5 Exceções de Domínio

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 2.5.1 | Criar classe base `DomainException` em Domain/Exceptions | Código | 5min |
| 2.5.2 | Criar exceção `UserAlreadyExistsException` | Código | 5min |
| 2.5.3 | Criar exceção `InvalidCredentialsException` | Código | 5min |
| 2.5.4 | Criar exceção `InvalidTokenException` | Código | 5min |
| 2.5.5 | Criar exceção `TokenExpiredException` | Código | 5min |
| 2.5.6 | Commit: "feat(domain): add domain exceptions" | Setup | 2min |

---

## FASE 3: Banco de Dados (Infrastructure)

### 3.1 Configuração de Conexão

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 3.1.1 | Criar interface `IDbConnectionFactory` em Application/Interfaces | Código | 5min |
| 3.1.2 | Definir método `IDbConnection CreateConnection()` | Código | 2min |
| 3.1.3 | Criar classe `NpgsqlConnectionFactory` em Infrastructure/Data | Código | 10min |
| 3.1.4 | Implementar injeção de connection string via construtor | Código | 5min |
| 3.1.5 | Criar classe de configuração `DatabaseSettings` | Código | 5min |
| 3.1.6 | Configurar connection string em appsettings.json | Config | 5min |
| 3.1.7 | Configurar connection string em appsettings.Development.json | Config | 5min |
| 3.1.8 | Registrar `IDbConnectionFactory` no DI container | Config | 5min |
| 3.1.9 | Commit: "feat(infra): add database connection factory" | Setup | 2min |

### 3.2 Docker e PostgreSQL

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 3.2.1 | Criar arquivo `docker-compose.yml` na raiz | Config | 10min |
| 3.2.2 | Configurar serviço PostgreSQL no docker-compose | Config | 10min |
| 3.2.3 | Configurar volume para persistência de dados | Config | 5min |
| 3.2.4 | Configurar variáveis de ambiente (user, password, db) | Config | 5min |
| 3.2.5 | Executar `docker-compose up -d` | Setup | 3min |
| 3.2.6 | Testar conexão com psql ou DBeaver | Validação | 5min |
| 3.2.7 | Commit: "chore: add docker-compose for PostgreSQL" | Setup | 2min |

### 3.3 Criação de Tabelas (Migrations)

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 3.3.1 | Criar pasta `Migrations` em Infrastructure | Setup | 1min | |
| 3.3.2 | Criar arquivo SQL `001_create_users_table.sql` | Código | 5min | |
| 3.3.3 | Definir coluna `id` (UUID, PK, DEFAULT gen_random_uuid()) | Código | 3min | |
| 3.3.4 | Definir coluna `email` (VARCHAR(255), UNIQUE, NOT NULL) | Código | 3min | |
| 3.3.5 | Definir coluna `password_hash` (VARCHAR(255), NOT NULL) | Código | 3min | ⚠️ Nunca indexar |
| 3.3.6 | Definir coluna `name` (VARCHAR(100), NOT NULL) | Código | 3min | |
| 3.3.7 | Definir coluna `roles` (TEXT[], DEFAULT '{}') | Código | 3min | |
| 3.3.8 | Definir coluna `is_active` (BOOLEAN, DEFAULT true) | Código | 3min | |
| 3.3.9 | Definir coluna `created_at` (TIMESTAMPTZ, DEFAULT NOW()) | Código | 3min | |
| 3.3.10 | Definir coluna `updated_at` (TIMESTAMPTZ, NULL) | Código | 3min | |
| 3.3.11 | Criar índice em `email` (já é UNIQUE, mas explícito) | Código | 3min | |
| 3.3.12 | Executar migration no banco | Setup | 3min | |
| 3.3.13 | Criar arquivo SQL `002_create_refresh_tokens_table.sql` | Código | 5min | |
| 3.3.14 | Definir coluna `id` (UUID, PK) | Código | 3min | |
| 3.3.15 | Definir coluna `token` (VARCHAR(255), UNIQUE, NOT NULL) | Código | 3min | |
| 3.3.16 | Definir coluna `user_id` (UUID, FK -> users.id) | Código | 3min | |
| 3.3.17 | Definir coluna `expires_at` (TIMESTAMPTZ, NOT NULL) | Código | 3min | |
| 3.3.18 | Definir coluna `created_at` (TIMESTAMPTZ, DEFAULT NOW()) | Código | 3min | |
| 3.3.19 | Definir coluna `revoked_at` (TIMESTAMPTZ, NULL) | Código | 3min | |
| 3.3.20 | Definir coluna `replaced_by_token` (VARCHAR(255), NULL) | Código | 3min | |
| 3.3.21 | Definir coluna `device_info` (VARCHAR(255), NULL) | Código | 3min | |
| 3.3.22 | Criar índice em `token` | Código | 3min | |
| 3.3.23 | Criar índice em `user_id` | Código | 3min | |
| 3.3.24 | Executar migration no banco | Setup | 3min | |
| 3.3.25 | Commit: "feat(infra): add database migrations" | Setup | 2min | |

### 3.4 Implementação dos Repositórios

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 3.4.1 | Criar classe `UserRepository` em Infrastructure/Repositories | Código | 10min |
| 3.4.2 | Injetar `IDbConnectionFactory` no construtor | Código | 3min |
| 3.4.3 | Implementar `GetByIdAsync` com Dapper | Código | 15min |
| 3.4.4 | Implementar `GetByEmailAsync` com Dapper | Código | 10min |
| 3.4.5 | Implementar `ExistsAsync` com Dapper | Código | 10min |
| 3.4.6 | Implementar `CreateAsync` com Dapper | Código | 15min |
| 3.4.7 | Implementar `UpdateAsync` com Dapper | Código | 15min |
| 3.4.8 | Escrever teste de integração para `CreateAsync` | Teste | 20min |
| 3.4.9 | Escrever teste de integração para `GetByEmailAsync` | Teste | 15min |
| 3.4.10 | Criar classe `RefreshTokenRepository` em Infrastructure/Repositories | Código | 10min |
| 3.4.11 | Implementar `GetByTokenAsync` com Dapper | Código | 15min |
| 3.4.12 | Implementar `GetByUserIdAsync` com Dapper | Código | 15min |
| 3.4.13 | Implementar `CreateAsync` com Dapper | Código | 15min |
| 3.4.14 | Implementar `UpdateAsync` com Dapper | Código | 15min |
| 3.4.15 | Implementar `RevokeAllByUserIdAsync` com Dapper | Código | 15min |
| 3.4.16 | Escrever teste de integração para RefreshTokenRepository | Teste | 30min |
| 3.4.17 | Registrar repositórios no DI container | Config | 5min |
| 3.4.18 | Commit: "feat(infra): implement repositories" | Setup | 2min |

---

## FASE 4: Serviços de Segurança (Infrastructure)

### 4.1 Password Hasher

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 4.1.1 | Criar classe `BcryptPasswordHasher` em Infrastructure/Security | Código | 5min | |
| 4.1.2 | Implementar interface `IPasswordHasher` | Código | 2min | |
| 4.1.3 | Implementar método `Hash` usando BCrypt com work factor 12 | Código | 10min | ⚠️ Não usar <10 |
| 4.1.4 | Implementar método `Verify` usando BCrypt.Verify | Código | 10min | ⚠️ Timing-safe |
| 4.1.5 | Escrever teste unitário: hash não é igual à senha original | Teste | 10min | |
| 4.1.6 | Escrever teste unitário: verify retorna true para senha correta | Teste | 10min | |
| 4.1.7 | Escrever teste unitário: verify retorna false para senha errada | Teste | 10min | |
| 4.1.8 | Escrever teste unitário: hashes diferentes para mesma senha | Teste | 10min | |
| 4.1.9 | Registrar `IPasswordHasher` no DI container | Config | 3min | |
| 4.1.10 | Commit: "feat(infra): add password hasher" | Setup | 2min | |

### 4.2 Token Service (JWT)

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 4.2.1 | Criar classe `JwtSettings` para configurações | Código | 5min | |
| 4.2.2 | Definir propriedade `Issuer` | Código | 2min | |
| 4.2.3 | Definir propriedade `Audience` | Código | 2min | |
| 4.2.4 | Definir propriedade `AccessTokenExpirationMinutes` | Código | 2min | |
| 4.2.5 | Definir propriedade `RefreshTokenExpirationDays` | Código | 2min | |
| 4.2.6 | Definir propriedade `PrivateKeyPath` (para RS256) | Código | 2min | ⚠️ Não commitar |
| 4.2.7 | Definir propriedade `PublicKeyPath` (para RS256) | Código | 2min | |
| 4.2.8 | Configurar JwtSettings em appsettings.json | Config | 5min | |
| 4.2.9 | Criar classe `JwtTokenService` em Infrastructure/Security | Código | 10min | |
| 4.2.10 | Implementar interface `ITokenService` | Código | 2min | |
| 4.2.11 | Injetar `JwtSettings` via IOptions | Código | 5min | |
| 4.2.12 | Carregar private key RSA do arquivo PEM | Código | 20min | ⚠️ |
| 4.2.13 | Implementar `GenerateAccessToken` com claims padrão | Código | 30min | |
| 4.2.14 | Incluir claim `sub` (user id) | Código | 3min | |
| 4.2.15 | Incluir claim `email` | Código | 3min | |
| 4.2.16 | Incluir claim `name` | Código | 3min | |
| 4.2.17 | Incluir claim `roles` (array) | Código | 5min | |
| 4.2.18 | Incluir claim `jti` (unique token id) | Código | 5min | |
| 4.2.19 | Configurar `iss`, `aud`, `exp`, `iat`, `nbf` | Código | 10min | |
| 4.2.20 | Assinar token com RS256 | Código | 10min | |
| 4.2.21 | Implementar `GenerateRefreshToken` com CSPRNG | Código | 15min | ⚠️ Usar RandomNumberGenerator |
| 4.2.22 | Implementar `ValidateAccessToken` | Código | 20min | |
| 4.2.23 | Escrever teste unitário: token gerado é válido | Teste | 15min | |
| 4.2.24 | Escrever teste unitário: token expirado é rejeitado | Teste | 15min | |
| 4.2.25 | Escrever teste unitário: token com issuer errado é rejeitado | Teste | 15min | |
| 4.2.26 | Escrever teste unitário: claims estão corretos no token | Teste | 15min | |
| 4.2.27 | Registrar `ITokenService` no DI container | Config | 5min | |
| 4.2.28 | Commit: "feat(infra): add JWT token service" | Setup | 2min | |

### 4.3 Gerenciamento de Chaves RSA

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 4.3.1 | Criar script para gerar par de chaves RSA (2048+ bits) | Código | 15min | |
| 4.3.2 | Salvar private key em arquivo `.pem` fora do repo | Setup | 5min | ⚠️ |
| 4.3.3 | Salvar public key em arquivo `.pem` (pode versionar) | Setup | 5min | |
| 4.3.4 | Adicionar `*.pem` ao `.gitignore` (exceto public) | Config | 3min | |
| 4.3.5 | Documentar processo de geração de chaves em README | Doc | 15min | |
| 4.3.6 | Configurar caminhos das chaves em appsettings | Config | 5min | |
| 4.3.7 | Commit: "chore: document key generation process" | Setup | 2min | |

---

## FASE 5: Use Cases (Application)

### 5.1 DTOs

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 5.1.1 | Criar record `RegisterRequest` em DTOs/Requests | Código | 5min | |
| 5.1.2 | Definir propriedade `Email` | Código | 2min | |
| 5.1.3 | Definir propriedade `Password` | Código | 2min | ⚠️ Nunca logar |
| 5.1.4 | Definir propriedade `Name` | Código | 2min | |
| 5.1.5 | Criar record `LoginRequest` em DTOs/Requests | Código | 5min | |
| 5.1.6 | Definir propriedade `Email` | Código | 2min | |
| 5.1.7 | Definir propriedade `Password` | Código | 2min | ⚠️ Nunca logar |
| 5.1.8 | Criar record `RefreshTokenRequest` em DTOs/Requests | Código | 5min | |
| 5.1.9 | Definir propriedade `RefreshToken` | Código | 2min | |
| 5.1.10 | Criar record `AuthResponse` em DTOs/Responses | Código | 5min | |
| 5.1.11 | Definir propriedade `AccessToken` | Código | 2min | |
| 5.1.12 | Definir propriedade `RefreshToken` | Código | 2min | |
| 5.1.13 | Definir propriedade `ExpiresAt` | Código | 2min | |
| 5.1.14 | Criar record `UserResponse` em DTOs/Responses | Código | 5min | |
| 5.1.15 | Definir propriedades (SEM password hash!) | Código | 5min | ⚠️ |
| 5.1.16 | Commit: "feat(app): add DTOs" | Setup | 2min | |

### 5.2 Validadores (FluentValidation)

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 5.2.1 | Criar pasta `Validators` em Application | Setup | 1min |
| 5.2.2 | Criar `RegisterRequestValidator` | Código | 10min |
| 5.2.3 | Validar Email: não vazio, formato válido | Código | 5min |
| 5.2.4 | Validar Password: mínimo 8 caracteres | Código | 5min |
| 5.2.5 | Validar Password: pelo menos 1 letra maiúscula | Código | 5min |
| 5.2.6 | Validar Password: pelo menos 1 número | Código | 5min |
| 5.2.7 | Validar Name: não vazio, 2-100 caracteres | Código | 5min |
| 5.2.8 | Escrever testes unitários para RegisterRequestValidator | Teste | 20min |
| 5.2.9 | Criar `LoginRequestValidator` | Código | 10min |
| 5.2.10 | Validar Email e Password não vazios | Código | 5min |
| 5.2.11 | Escrever testes unitários para LoginRequestValidator | Teste | 10min |
| 5.2.12 | Registrar validadores no DI (AddValidatorsFromAssembly) | Config | 5min |
| 5.2.13 | Commit: "feat(app): add validators" | Setup | 2min |

### 5.3 Use Case: Register

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 5.3.1 | Criar interface `IRegisterUseCase` em Application/UseCases/Register | Código | 5min | |
| 5.3.2 | Definir método `Task<UserResponse> ExecuteAsync(RegisterRequest request)` | Código | 2min | |
| 5.3.3 | Criar classe `RegisterUseCase` | Código | 10min | |
| 5.3.4 | Injetar `IUserRepository` | Código | 3min | |
| 5.3.5 | Injetar `IPasswordHasher` | Código | 3min | |
| 5.3.6 | Implementar: verificar se email já existe | Código | 10min | ⚠️ |
| 5.3.7 | Implementar: hashear senha ANTES de criar User | Código | 10min | |
| 5.3.8 | Implementar: criar entidade User com factory method | Código | 10min | |
| 5.3.9 | Implementar: salvar no repositório | Código | 5min | |
| 5.3.10 | Implementar: retornar UserResponse (sem hash!) | Código | 5min | ⚠️ |
| 5.3.11 | Escrever teste unitário: registro com sucesso | Teste | 20min | |
| 5.3.12 | Escrever teste unitário: email duplicado lança exceção | Teste | 15min | |
| 5.3.13 | Escrever teste unitário: senha é hasheada | Teste | 15min | |
| 5.3.14 | Registrar use case no DI container | Config | 3min | |
| 5.3.15 | Commit: "feat(app): add register use case" | Setup | 2min | |

### 5.4 Use Case: Login

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 5.4.1 | Criar interface `ILoginUseCase` em Application/UseCases/Login | Código | 5min | |
| 5.4.2 | Definir método `Task<AuthResponse> ExecuteAsync(LoginRequest request)` | Código | 2min | |
| 5.4.3 | Criar classe `LoginUseCase` | Código | 10min | |
| 5.4.4 | Injetar `IUserRepository` | Código | 3min | |
| 5.4.5 | Injetar `IRefreshTokenRepository` | Código | 3min | |
| 5.4.6 | Injetar `IPasswordHasher` | Código | 3min | |
| 5.4.7 | Injetar `ITokenService` | Código | 3min | |
| 5.4.8 | Implementar: buscar usuário por email | Código | 10min | |
| 5.4.9 | Implementar: se não encontrou, lançar InvalidCredentialsException | Código | 5min | ⚠️ Mensagem genérica |
| 5.4.10 | Implementar: verificar senha com hasher | Código | 10min | |
| 5.4.11 | Implementar: se senha errada, lançar InvalidCredentialsException | Código | 5min | ⚠️ Mesma exceção! |
| 5.4.12 | Implementar: verificar se usuário está ativo | Código | 5min | |
| 5.4.13 | Implementar: gerar access token | Código | 5min | |
| 5.4.14 | Implementar: gerar refresh token | Código | 5min | |
| 5.4.15 | Implementar: salvar refresh token no banco | Código | 5min | |
| 5.4.16 | Implementar: retornar AuthResponse | Código | 5min | |
| 5.4.17 | Escrever teste unitário: login com sucesso | Teste | 20min | |
| 5.4.18 | Escrever teste unitário: email inexistente | Teste | 15min | |
| 5.4.19 | Escrever teste unitário: senha incorreta | Teste | 15min | |
| 5.4.20 | Escrever teste unitário: usuário inativo | Teste | 15min | |
| 5.4.21 | Escrever teste: timing similar para user inexistente vs senha errada | Teste | 20min | ⚠️ |
| 5.4.22 | Registrar use case no DI container | Config | 3min | |
| 5.4.23 | Commit: "feat(app): add login use case" | Setup | 2min | |

### 5.5 Use Case: Refresh Token

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 5.5.1 | Criar interface `IRefreshTokenUseCase` | Código | 5min | |
| 5.5.2 | Criar classe `RefreshTokenUseCase` | Código | 10min | |
| 5.5.3 | Injetar dependências necessárias | Código | 5min | |
| 5.5.4 | Implementar: buscar refresh token no banco | Código | 10min | |
| 5.5.5 | Implementar: verificar se token existe | Código | 5min | |
| 5.5.6 | Implementar: verificar se token está expirado | Código | 5min | |
| 5.5.7 | Implementar: verificar se token foi revogado | Código | 5min | |
| 5.5.8 | Implementar: buscar usuário associado | Código | 5min | |
| 5.5.9 | Implementar: gerar novo access token | Código | 5min | |
| 5.5.10 | Implementar: gerar novo refresh token (rotação) | Código | 10min | ⚠️ |
| 5.5.11 | Implementar: revogar refresh token antigo | Código | 5min | |
| 5.5.12 | Implementar: salvar novo refresh token | Código | 5min | |
| 5.5.13 | Implementar: retornar AuthResponse | Código | 5min | |
| 5.5.14 | Escrever teste unitário: refresh com sucesso | Teste | 20min | |
| 5.5.15 | Escrever teste unitário: token inexistente | Teste | 15min | |
| 5.5.16 | Escrever teste unitário: token expirado | Teste | 15min | |
| 5.5.17 | Escrever teste unitário: token revogado | Teste | 15min | |
| 5.5.18 | Escrever teste: uso de token revogado revoga família inteira | Teste | 25min | ⚠️ |
| 5.5.19 | Registrar use case no DI container | Config | 3min | |
| 5.5.20 | Commit: "feat(app): add refresh token use case" | Setup | 2min | |

### 5.6 Use Case: Logout

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 5.6.1 | Criar interface `ILogoutUseCase` | Código | 5min |
| 5.6.2 | Criar classe `LogoutUseCase` | Código | 10min |
| 5.6.3 | Injetar `IRefreshTokenRepository` | Código | 3min |
| 5.6.4 | Implementar: buscar refresh token | Código | 5min |
| 5.6.5 | Implementar: revogar token | Código | 5min |
| 5.6.6 | Implementar: (opcional) revogar todos tokens do usuário | Código | 10min |
| 5.6.7 | Escrever teste unitário: logout com sucesso | Teste | 15min |
| 5.6.8 | Escrever teste unitário: logout com token inválido | Teste | 10min |
| 5.6.9 | Registrar use case no DI container | Config | 3min |
| 5.6.10 | Commit: "feat(app): add logout use case" | Setup | 2min |

---

## FASE 6: API Controllers

### 6.1 Exception Handling Middleware

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 6.1.1 | Criar classe `ExceptionHandlingMiddleware` em Api/Middlewares | Código | 10min | |
| 6.1.2 | Capturar exceções de domínio | Código | 10min | |
| 6.1.3 | Mapear `UserAlreadyExistsException` → 409 Conflict | Código | 5min | |
| 6.1.4 | Mapear `InvalidCredentialsException` → 401 Unauthorized | Código | 5min | ⚠️ Mensagem genérica |
| 6.1.5 | Mapear `InvalidTokenException` → 401 Unauthorized | Código | 5min | |
| 6.1.6 | Mapear `TokenExpiredException` → 401 Unauthorized | Código | 5min | |
| 6.1.7 | Mapear ValidationException → 400 Bad Request | Código | 10min | |
| 6.1.8 | Mapear exceções genéricas → 500 Internal Server Error | Código | 5min | ⚠️ Não expor detalhes |
| 6.1.9 | Adicionar logging de exceções | Código | 10min | ⚠️ Não logar senhas |
| 6.1.10 | Registrar middleware no pipeline | Config | 5min | |
| 6.1.11 | Escrever teste de integração para cada mapeamento | Teste | 30min | |
| 6.1.12 | Commit: "feat(api): add exception handling middleware" | Setup | 2min | |

### 6.2 Auth Controller

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 6.2.1 | Criar classe `AuthController` em Api/Controllers | Código | 10min | |
| 6.2.2 | Decorar com `[ApiController]` e `[Route("api/auth")]` | Código | 3min | |
| 6.2.3 | Injetar `IRegisterUseCase` | Código | 3min | |
| 6.2.4 | Injetar `ILoginUseCase` | Código | 3min | |
| 6.2.5 | Injetar `IRefreshTokenUseCase` | Código | 3min | |
| 6.2.6 | Injetar `ILogoutUseCase` | Código | 3min | |
| 6.2.7 | Criar endpoint `POST /register` | Código | 10min | |
| 6.2.8 | Retornar 201 Created com UserResponse | Código | 5min | |
| 6.2.9 | Criar endpoint `POST /login` | Código | 10min | |
| 6.2.10 | Retornar 200 OK com AuthResponse | Código | 5min | |
| 6.2.11 | Criar endpoint `POST /refresh` | Código | 10min | |
| 6.2.12 | Retornar 200 OK com AuthResponse | Código | 5min | |
| 6.2.13 | Criar endpoint `POST /logout` | Código | 10min | |
| 6.2.14 | Retornar 204 No Content | Código | 3min | |
| 6.2.15 | Adicionar comentários XML para Swagger | Doc | 10min | |
| 6.2.16 | Commit: "feat(api): add auth controller" | Setup | 2min | |

### 6.3 Testes de Integração da API

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 6.3.1 | Configurar WebApplicationFactory para testes | Setup | 20min |
| 6.3.2 | Configurar banco de teste (container ou in-memory) | Setup | 30min |
| 6.3.3 | Teste: POST /register com dados válidos → 201 | Teste | 20min |
| 6.3.4 | Teste: POST /register com email duplicado → 409 | Teste | 15min |
| 6.3.5 | Teste: POST /register com dados inválidos → 400 | Teste | 15min |
| 6.3.6 | Teste: POST /login com credenciais válidas → 200 + tokens | Teste | 20min |
| 6.3.7 | Teste: POST /login com email inexistente → 401 | Teste | 15min |
| 6.3.8 | Teste: POST /login com senha errada → 401 | Teste | 15min |
| 6.3.9 | Teste: POST /refresh com token válido → 200 + novos tokens | Teste | 20min |
| 6.3.10 | Teste: POST /refresh com token expirado → 401 | Teste | 15min |
| 6.3.11 | Teste: POST /logout → 204 | Teste | 15min |
| 6.3.12 | Commit: "test(api): add integration tests" | Setup | 2min |

---

## FASE 7: Integração no BarberBoss

### 7.1 Configuração de JWT no BarberBoss

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 7.1.1 | Instalar `Microsoft.AspNetCore.Authentication.JwtBearer` | Setup | 3min |
| 7.1.2 | Copiar public key da Auth API para BarberBoss | Setup | 5min |
| 7.1.3 | Criar classe `JwtSettings` no BarberBoss | Código | 10min |
| 7.1.4 | Configurar `Issuer` esperado | Config | 3min |
| 7.1.5 | Configurar `Audience` esperado | Config | 3min |
| 7.1.6 | Configurar path da public key | Config | 3min |
| 7.1.7 | Criar extension method `AddJwtAuthentication` | Código | 20min |
| 7.1.8 | Configurar `TokenValidationParameters` | Código | 15min |
| 7.1.9 | Habilitar validação de `ValidateIssuer` | Código | 3min |
| 7.1.10 | Habilitar validação de `ValidateAudience` | Código | 3min |
| 7.1.11 | Habilitar validação de `ValidateLifetime` | Código | 3min |
| 7.1.12 | Habilitar validação de `ValidateIssuerSigningKey` | Código | 3min |
| 7.1.13 | Carregar public key RSA para validação | Código | 15min |
| 7.1.14 | Chamar `AddJwtAuthentication` no Program.cs | Config | 3min |
| 7.1.15 | Adicionar `app.UseAuthentication()` no pipeline | Config | 3min |
| 7.1.16 | Adicionar `app.UseAuthorization()` no pipeline | Config | 3min |
| 7.1.17 | Commit: "feat(barberboss): add JWT authentication" | Setup | 2min |

### 7.2 Proteção de Endpoints

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 7.2.1 | Adicionar `[Authorize]` no BillingsController | Código | 3min |
| 7.2.2 | Adicionar `[Authorize]` em outros controllers protegidos | Código | 5min |
| 7.2.3 | Criar método de extensão para extrair UserId do token | Código | 10min |
| 7.2.4 | Usar UserId extraído nas queries (filtrar por usuário) | Código | 15min |
| 7.2.5 | Criar policy de autorização `RequireAdminRole` | Código | 15min |
| 7.2.6 | Aplicar policy em endpoints administrativos | Código | 10min |
| 7.2.7 | Commit: "feat(barberboss): protect endpoints" | Setup | 2min |

### 7.3 Testes de Integração no BarberBoss

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 7.3.1 | Teste: GET /api/billings sem token → 401 | Teste | 15min | |
| 7.3.2 | Teste: GET /api/billings com token inválido → 401 | Teste | 15min | |
| 7.3.3 | Teste: GET /api/billings com token expirado → 401 | Teste | 15min | |
| 7.3.4 | Teste: GET /api/billings com issuer errado → 401 | Teste | 15min | ⚠️ |
| 7.3.5 | Teste: GET /api/billings com audience errada → 401 | Teste | 15min | ⚠️ |
| 7.3.6 | Teste: GET /api/billings com token válido → 200 | Teste | 15min | |
| 7.3.7 | Teste: endpoint admin com role errada → 403 | Teste | 15min | |
| 7.3.8 | Teste: endpoint admin com role correta → 200 | Teste | 15min | |
| 7.3.9 | Commit: "test(barberboss): add auth integration tests" | Setup | 2min | |

---

## FASE 8: Hardening e Observabilidade

### 8.1 Rate Limiting

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 8.1.1 | Instalar `AspNetCoreRateLimit` ou usar built-in .NET 8 | Setup | 5min | |
| 8.1.2 | Configurar rate limit por IP para `/auth/login` | Config | 15min | ⚠️ Por IP, não user |
| 8.1.3 | Definir limite: 5 tentativas por minuto | Config | 5min | |
| 8.1.4 | Configurar resposta 429 Too Many Requests | Config | 10min | |
| 8.1.5 | Adicionar header `Retry-After` na resposta | Código | 10min | |
| 8.1.6 | Teste: 5ª tentativa passa | Teste | 10min | |
| 8.1.7 | Teste: 6ª tentativa retorna 429 | Teste | 10min | |
| 8.1.8 | Commit: "feat(api): add rate limiting" | Setup | 2min | |

### 8.2 Logging Estruturado

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 8.2.1 | Instalar Serilog (ou usar built-in ILogger) | Setup | 5min | |
| 8.2.2 | Configurar sink para console (dev) e arquivo (prod) | Config | 15min | |
| 8.2.3 | Log: tentativa de login (email, IP, timestamp) | Código | 10min | ⚠️ Não logar senha |
| 8.2.4 | Log: login bem-sucedido (userId, IP, timestamp) | Código | 10min | |
| 8.2.5 | Log: login falhou (IP, timestamp, motivo genérico) | Código | 10min | ⚠️ Não revelar se email existe |
| 8.2.6 | Log: refresh token usado (userId, tokenId, timestamp) | Código | 10min | |
| 8.2.7 | Log: logout (userId, timestamp) | Código | 10min | |
| 8.2.8 | Log: refresh token revogado suspeito (possível roubo) | Código | 10min | ⚠️ Alerta |
| 8.2.9 | Revisar logs para garantir que senhas nunca aparecem | Validação | 15min | ⚠️ |
| 8.2.10 | Commit: "feat(api): add structured logging" | Setup | 2min | |

### 8.3 Health Checks

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 8.3.1 | Adicionar `AddHealthChecks()` no DI | Config | 5min |
| 8.3.2 | Adicionar health check para PostgreSQL | Config | 10min |
| 8.3.3 | Mapear endpoint `/health` | Config | 5min |
| 8.3.4 | Teste: banco disponível → 200 Healthy | Teste | 10min |
| 8.3.5 | Teste: banco indisponível → 503 Unhealthy | Teste | 15min |
| 8.3.6 | Commit: "feat(api): add health checks" | Setup | 2min |

### 8.4 Documentação Swagger

| ID | Tarefa | Tipo | Tempo Est. | ⚠️ |
|----|--------|------|------------|-----|
| 8.4.1 | Configurar Swagger com suporte a JWT | Config | 15min | |
| 8.4.2 | Adicionar botão "Authorize" no Swagger UI | Config | 10min | |
| 8.4.3 | Documentar cada endpoint com resumo e exemplos | Doc | 30min | |
| 8.4.4 | Ocultar schemas de erro muito detalhados | Config | 10min | ⚠️ |
| 8.4.5 | Commit: "docs(api): improve swagger documentation" | Setup | 2min | |

---

## FASE 9: (Futura) Features Avançadas

### 9.1 JWKS Endpoint (para validação pública)

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 9.1.1 | Pesquisar formato JWKS (JSON Web Key Set) | Estudo | 30min |
| 9.1.2 | Criar endpoint `GET /.well-known/jwks.json` | Código | 20min |
| 9.1.3 | Expor public key em formato JWK | Código | 30min |
| 9.1.4 | Configurar BarberBoss para consumir JWKS ao invés de arquivo | Config | 30min |
| 9.1.5 | Commit: "feat(api): add JWKS endpoint" | Setup | 2min |

### 9.2 Multi-Audience / Scopes

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 9.2.1 | Refatorar para suportar múltiplas audiences no token | Código | 30min |
| 9.2.2 | Adicionar conceito de scopes (permissões granulares) | Código | 45min |
| 9.2.3 | Testar token com audience para BarberBoss + Gym API | Teste | 30min |

### 9.3 Key Rotation

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 9.3.1 | Pesquisar estratégias de key rotation | Estudo | 45min |
| 9.3.2 | Implementar suporte a múltiplas chaves (kid) | Código | 60min |
| 9.3.3 | Documentar processo de rotação | Doc | 30min |

### 9.4 MFA / OTP

| ID | Tarefa | Tipo | Tempo Est. |
|----|--------|------|------------|
| 9.4.1 | Pesquisar TOTP (Time-based One-Time Password) | Estudo | 45min |
| 9.4.2 | Implementar geração de secret TOTP | Código | 60min |
| 9.4.3 | Implementar verificação de código OTP | Código | 45min |
| 9.4.4 | Implementar step-up authentication | Código | 90min |

---

## Resumo de Estimativas

| Fase | Tarefas | Tempo Estimado |
|------|---------|----------------|
| Fase 0 | 35 | ~8h |
| Fase 1 | 48 | ~3h |
| Fase 2 | 33 | ~4h |
| Fase 3 | 38 | ~6h |
| Fase 4 | 38 | ~7h |
| Fase 5 | 65 | ~10h |
| Fase 6 | 28 | ~5h |
| Fase 7 | 26 | ~4h |
| Fase 8 | 29 | ~4h |
| **Total MVP** | **340** | **~51h** |

---

## Checklist de Segurança Final

Antes de considerar o MVP completo, verificar:

- [ ] Senhas nunca aparecem em logs
- [ ] Password hash nunca é retornado em responses
- [ ] Mensagens de erro não revelam se email existe
- [ ] Timing de resposta similar para user inexistente vs senha errada
- [ ] Rate limiting ativo no login
- [ ] Refresh token usa CSPRNG (RandomNumberGenerator)
- [ ] Access token tem exp <= 15 minutos
- [ ] Todas as validações de JWT estão ativas (iss, aud, exp, signature)
- [ ] Private key não está no repositório
- [ ] HTTPS obrigatório em produção
- [ ] Logs de tentativas de login existem
- [ ] Refresh token rotation implementada
- [ ] Uso de refresh token revogado gera alerta
