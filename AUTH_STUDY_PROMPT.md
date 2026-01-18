# Prompt de Estudos: Autenticação e Segurança com .NET 8

> **Use este prompt para iniciar uma sessão de mentoria técnica com Claude ou outro LLM sobre autenticação.**

---

## Como Usar Este Prompt

1. Copie todo o conteúdo da seção "PROMPT COMPLETO" abaixo
2. Cole em uma nova conversa com o Claude
3. Responda às perguntas de definição de escopo
4. Siga o plano incremental de aprendizado

---

## PROMPT COMPLETO

```
Você é meu mentor técnico para eu aprender autenticação e segurança construindo uma Auth API em .NET 8.

## Meu Contexto Técnico

- Nível: Desenvolvedor com experiência em .NET, mas iniciante em segurança/autenticação
- Projetos existentes: [DESCREVA SEUS PROJETOS AQUI - ex: "API BarberBoss com Clean Architecture, Dapper, PostgreSQL"]
- Objetivo: Criar uma Auth API separada (Identity Provider) que emite JWT para ser reutilizada por múltiplas APIs
- Motivação: Entender profundamente JWT, autenticação vs autorização, e decisões de segurança — não apenas implementar

## Regras de Mentoria

### O Que EU Quero:
1. Explicações conceituais antes de qualquer código
2. Entender o "porquê" de cada decisão de segurança
3. Conhecer as armadilhas e vulnerabilidades comuns
4. Implementar eu mesmo e ter meu código revisado
5. Aprender de forma incremental, validando cada etapa

### O Que EU NÃO Quero:
1. ❌ Código pronto — no máximo trechos de 10-15 linhas para ilustrar conceitos
2. ❌ Implementações completas de controllers, services ou arquivos inteiros
3. ❌ Pular etapas ou assumir que entendi algo sem eu confirmar
4. ❌ Respostas superficiais sobre segurança

### Formato das Respostas

Para cada tópico, responda SEMPRE com estes 4 blocos:

**1. Conceito**
- Explicação teórica clara e acessível
- Analogias quando apropriado
- Contexto histórico se relevante

**2. Decisão/Trade-offs**
- Opções disponíveis
- Prós e contras de cada uma
- Sua recomendação e justificativa

**3. Passo a Passo de Implementação**
- Tarefas pequenas e específicas
- Sem código completo — apenas descrição ou pseudo-código
- Ordem de execução clara

**4. Checklist de Testes/Validação e Ameaças**
- O que testar para garantir que funciona
- Ameaças de segurança relevantes para esta etapa
- Armadilhas comuns a evitar

## Objetivos do MVP

### Funcionalidades Core:
1. **Registro de usuário** com hash seguro de senha
2. **Login** que emite Access Token (JWT) e Refresh Token
3. **Refresh Token Flow** para renovar access token
4. **Logout** que revoga refresh token

### Integração:
1. **Proteger endpoints** da API consumidora (ex: BarberBoss)
2. **Validar JWT** corretamente: assinatura, iss, aud, exp, nbf
3. **Extrair claims** e usar para autorização

### Requisitos de Segurança:
1. Senhas NUNCA em texto puro ou logs
2. Mensagens de erro genéricas (sem user enumeration)
3. Timing de resposta consistente
4. Rate limiting no login
5. Refresh token rotation

## Objetivos Futuros (Pós-MVP)

- MFA / OTP via email ou SMS
- JWKS endpoint público
- Key rotation
- Múltiplas audiences/scopes
- Trilha de auditoria completa

## Dinâmica da Sessão

1. **Você pergunta** → eu respondo para alinhar contexto
2. **Você explica conceito** → eu confirmo se entendi ou peço esclarecimento
3. **Você propõe tarefa** → eu implemento e colo o código aqui
4. **Você revisa** → aponta melhorias, vulnerabilidades, boas práticas
5. **Eu corrijo** → você valida e seguimos para próxima etapa

## Começar

Por favor, inicie com:

A) **Perguntas de Definição** — faça 10-12 perguntas essenciais para definir o escopo do MVP (incluindo decisões de segurança como algoritmo de assinatura, tempo de vida de tokens, etc.)

B) **Arquitetura Textual** — desenhe a arquitetura (Auth API vs API consumidora) com responsabilidades claras

C) **Contrato de Tokens** — defina a estrutura do JWT (claims, iss, aud, exp) e do Refresh Token

D) **Plano Incremental** — proponha 6-8 etapas de implementação com Definition of Done e exercícios de aprendizado em cada uma
```

---

## Tópicos de Estudo Detalhados

Use esta seção como referência para aprofundar cada área durante o projeto.

### 1. Fundamentos de Criptografia

#### 1.1 Hashing vs Encryption
```
Perguntas-guia:
- Qual a diferença fundamental entre hashing e encryption?
- Por que hashing é irreversível?
- Quando usar cada um?

Conceitos-chave:
- One-way function
- Determinismo (mesma entrada = mesma saída)
- Collision resistance
- Pre-image resistance
```

#### 1.2 Por Que Não Usar SHA-256 para Senhas?
```
Perguntas-guia:
- O que é uma rainbow table?
- Por que velocidade é ruim para hash de senhas?
- O que é salt e por que ajuda?

Conceitos-chave:
- Rainbow table attack
- Brute force attack
- Salt (valor aleatório único por senha)
- Work factor / Cost factor
```

#### 1.3 Algoritmos de Password Hashing
```
Algoritmos a estudar:
- bcrypt (recomendado para maioria dos casos)
- Argon2 (vencedor do PHC, mais moderno)
- scrypt (memory-hard, bom contra GPUs)
- PBKDF2 (mais antigo, ainda aceitável)

Por que NÃO usar:
- MD5 (quebrado, muito rápido)
- SHA-1 (quebrado)
- SHA-256 sem salt e iterations (muito rápido)
```

### 2. JSON Web Tokens (JWT)

#### 2.1 Anatomia do JWT
```
Estrutura: Header.Payload.Signature

Header:
{
  "alg": "RS256",    // Algoritmo de assinatura
  "typ": "JWT",      // Tipo do token
  "kid": "key-id"    // ID da chave (para rotation)
}

Payload (Claims):
{
  "iss": "issuer",           // Quem emitiu
  "sub": "subject",          // Sobre quem é (user ID)
  "aud": "audience",         // Para quem é
  "exp": 1234567890,         // Expiração (Unix timestamp)
  "iat": 1234567890,         // Emitido em
  "nbf": 1234567890,         // Não válido antes de
  "jti": "unique-id"         // ID único do token
}

Signature:
ALGORITHM(base64(header) + "." + base64(payload), secret_or_key)
```

#### 2.2 Algoritmos de Assinatura
```
Simétricos (segredo compartilhado):
- HS256 (HMAC-SHA256)
  - Prós: Simples, rápido
  - Contras: Segredo precisa ser compartilhado
  - Quando: Monolito ou APIs que confiam totalmente entre si

Assimétricos (par de chaves):
- RS256 (RSA-SHA256)
  - Prós: Validação sem conhecer segredo
  - Contras: Tokens maiores, mais lento
  - Quando: Múltiplas APIs, microservices

- ES256 (ECDSA-SHA256)
  - Prós: Tokens menores que RS256, seguro
  - Contras: Implementação mais complexa
  - Quando: Mobile, IoT, onde tamanho importa

Perguntas-guia:
- Por que RS256 é melhor para múltiplas APIs?
- O que acontece se o segredo HS256 vaza?
- Qual a diferença entre assinar e criptografar?
```

#### 2.3 Claims Padrão vs Custom
```
Claims Registered (padrão RFC 7519):
- iss, sub, aud, exp, nbf, iat, jti

Claims Custom (definidos por você):
- roles, permissions, email, name, tenant_id

Perguntas-guia:
- Que dados NUNCA colocar no payload?
- Por que JWT não é criptografado por padrão?
- Quando usar JWE (JWT criptografado)?
```

### 3. Autenticação vs Autorização

#### 3.1 Definições Precisas
```
Autenticação (AuthN):
- "Quem é você?"
- Verificar identidade
- Exemplos: login, biometria, certificado

Autorização (AuthZ):
- "O que você pode fazer?"
- Verificar permissões
- Exemplos: roles, policies, scopes

Fluxo típico:
1. Usuário se autentica (login)
2. Sistema emite token com identidade + claims
3. A cada request, sistema autoriza baseado nos claims
```

#### 3.2 Modelos de Autorização
```
RBAC (Role-Based Access Control):
- Permissões atribuídas a roles
- Usuários recebem roles
- Simples, bom para maioria dos casos

ABAC (Attribute-Based Access Control):
- Permissões baseadas em atributos
- Mais flexível, mais complexo
- Ex: "usuário pode acessar documento se for autor OU gerente do departamento"

Claims-Based:
- Permissões derivadas de claims no token
- Híbrido entre RBAC e ABAC
```

### 4. Access Token vs Refresh Token

#### 4.1 Propósitos Diferentes
```
Access Token:
- Propósito: Autorizar acesso a recursos
- Duração: Curta (5-30 minutos)
- Formato: JWT (stateless)
- Onde usar: Header Authorization em cada request

Refresh Token:
- Propósito: Obter novos access tokens
- Duração: Longa (dias a meses)
- Formato: Opaco ou JWT (stateful recomendado)
- Onde usar: Apenas no endpoint /refresh
```

#### 4.2 Por Que Dois Tokens?
```
Se usássemos apenas Access Token com vida longa:
- Risco: Token roubado dá acesso por muito tempo
- Sem forma de revogar (stateless)

Com Access Token curto + Refresh Token:
- Access roubado: Dano limitado (expira rápido)
- Refresh roubado: Podemos revogar no banco
- Melhor UX: Usuário não precisa logar frequentemente
```

#### 4.3 Refresh Token Rotation
```
Problema:
- Refresh token pode ser roubado
- Atacante usa refresh antes do usuário legítimo

Solução (Rotation):
1. Cada uso do refresh token emite um NOVO refresh token
2. Refresh antigo é invalidado
3. Se refresh antigo for usado → toda família é revogada

Perguntas-guia:
- O que acontece se atacante usa refresh primeiro?
- Como detectar uso de refresh token revogado?
- Por que isso indica possível comprometimento?
```

### 5. Vulnerabilidades e Ataques

#### 5.1 User Enumeration
```
O que é:
- Descobrir se um email/username existe no sistema
- Primeiro passo para ataques direcionados

Como acontece:
- "Email não encontrado" vs "Senha incorreta"
- Tempo de resposta diferente
- Status code diferente

Prevenção:
- Mensagem genérica: "Credenciais inválidas"
- Timing constante (sempre fazer hash, mesmo se user não existe)
- Mesmo status code para ambos casos
```

#### 5.2 Brute Force
```
O que é:
- Tentar muitas combinações de senha
- Automatizado, milhares de tentativas

Prevenção:
- Rate limiting por IP
- Account lockout temporário
- CAPTCHA após N tentativas
- Atraso exponencial entre tentativas
```

#### 5.3 Token Replay
```
O que é:
- Reutilizar um token interceptado
- Válido se token não expirou

Prevenção:
- Tokens com vida curta
- jti (JWT ID) único
- Blacklist de tokens (para logout imediato)
- HTTPS obrigatório
```

#### 5.4 Timing Attacks
```
O que é:
- Inferir informação pelo tempo de resposta
- Ex: comparação de strings que para no primeiro byte diferente

Prevenção:
- Comparação timing-safe (compara todos bytes sempre)
- BCrypt.Verify já é timing-safe
- Tempo de resposta consistente para todos cenários
```

### 6. Implementação Segura

#### 6.1 Checklist de Segurança
```
Senhas:
[ ] Hash com bcrypt/Argon2 (work factor >= 10)
[ ] Nunca logar senhas
[ ] Nunca retornar hash em responses
[ ] Validar senha ANTES de hashear

Tokens:
[ ] Access token vida curta (<= 15min)
[ ] Refresh token stateful (no banco)
[ ] Gerar refresh com CSPRNG
[ ] Refresh token rotation
[ ] Revogar família se token reusado

Validação:
[ ] Validar TODAS as claims: iss, aud, exp, nbf, signature
[ ] Não aceitar algoritmo "none"
[ ] Verificar kid antes de validar

Comunicação:
[ ] HTTPS obrigatório
[ ] Rate limiting no login
[ ] Logging sem dados sensíveis
[ ] Mensagens de erro genéricas
```

#### 6.2 Erros Comuns a Evitar
```
❌ Armazenar senha em texto puro
❌ Usar MD5/SHA para senhas
❌ Salt fixo ou previsível
❌ Logar tentativas de login com senha
❌ Mensagens de erro específicas ("email não existe")
❌ Aceitar algoritmo "none" no JWT
❌ Não validar issuer/audience
❌ Access token com vida muito longa
❌ Refresh token stateless sem forma de revogar
❌ Commitar secrets/private keys
❌ Rate limit por user ID (permite enumeration)
```

### 7. Arquitetura

#### 7.1 Identity Provider (IdP) vs Resource Server
```
Identity Provider (Auth API):
- Conhece credenciais
- Emite tokens
- Valida refresh tokens
- Gerencia usuários/sessões

Resource Server (BarberBoss, etc):
- NÃO conhece credenciais
- Valida tokens (assinatura, claims)
- Protege recursos
- Autoriza baseado em claims
```

#### 7.2 Fluxo de Autenticação
```
1. Login:
   Client → POST /auth/login (email, senha) → Auth API
   Auth API → Valida credenciais
   Auth API → Gera Access Token + Refresh Token
   Auth API → Retorna tokens → Client

2. Acesso a Recurso:
   Client → GET /api/billings (Authorization: Bearer <access_token>) → BarberBoss
   BarberBoss → Valida JWT (assinatura, exp, iss, aud)
   BarberBoss → Extrai claims (sub, roles)
   BarberBoss → Autoriza e retorna dados

3. Refresh:
   Client → POST /auth/refresh (refresh_token) → Auth API
   Auth API → Valida refresh token no banco
   Auth API → Gera novos tokens
   Auth API → Revoga refresh antigo
   Auth API → Retorna novos tokens → Client

4. Logout:
   Client → POST /auth/logout (refresh_token) → Auth API
   Auth API → Revoga refresh token
   Auth API → Retorna 204
```

---

## Exercícios Práticos Sugeridos

### Exercício 1: Explorando Hashing
```
Objetivo: Entender comportamento do bcrypt

Tarefas:
1. Hash a mesma senha 5 vezes
2. Observe que cada hash é diferente
3. Verifique que todos validam contra a senha original
4. Tente verificar com senha errada
5. Meça tempo de hash com work factors 10, 12, 14

Perguntas:
- Por que hashes são diferentes se senha é igual?
- Por que tempo aumenta exponencialmente?
```

### Exercício 2: Anatomia do JWT
```
Objetivo: Entender estrutura e validação do JWT

Tarefas:
1. Gere um JWT com claims customizados
2. Decodifique em jwt.io
3. Modifique 1 caractere do payload
4. Observe a assinatura invalidar
5. Tente validar o token modificado no código

Perguntas:
- O payload é criptografado?
- O que impede alguém de criar tokens falsos?
```

### Exercício 3: Timing Attack
```
Objetivo: Entender vulnerabilidade de timing

Tarefas:
1. Implemente login básico
2. Meça tempo para: (a) email inexistente, (b) senha errada
3. Se tempos são diferentes, você tem vulnerabilidade
4. Corrija fazendo hash dummy para email inexistente
5. Meça novamente

Perguntas:
- Como atacante explora diferença de timing?
- Por que é importante mesmo sendo milissegundos?
```

### Exercício 4: Refresh Token Rotation
```
Objetivo: Implementar e testar rotation

Tarefas:
1. Implemente refresh que emite novo refresh token
2. Salve relacionamento "old_token → new_token"
3. Teste usar refresh token antigo
4. Implemente revogação de família inteira
5. Teste novamente usar token antigo

Perguntas:
- Por que revogar toda família?
- Como isso detecta roubo de token?
```

---

## Recursos Adicionais

### Documentação Oficial
- [RFC 7519 - JSON Web Token](https://tools.ietf.org/html/rfc7519)
- [RFC 6749 - OAuth 2.0](https://tools.ietf.org/html/rfc6749)
- [OWASP Authentication Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html)
- [Microsoft Identity Platform Docs](https://docs.microsoft.com/en-us/azure/active-directory/develop/)

### Artigos Recomendados
- [The Ultimate Guide to handling JWTs on frontend clients](https://hasura.io/blog/best-practices-of-using-jwt-with-graphql/)
- [Stop using JWT for sessions](http://cryto.net/~joepie91/blog/2016/06/13/stop-using-jwt-for-sessions/)
- [Why Refresh Tokens are more secure](https://auth0.com/docs/secure/tokens/refresh-tokens)

### Ferramentas
- [jwt.io](https://jwt.io) - Debugger de JWT
- [BCrypt Calculator](https://bcrypt-generator.com/) - Testar bcrypt online
- [OWASP ZAP](https://www.zaproxy.org/) - Scanner de segurança

---

## Template de Revisão de Código

Ao submeter código para revisão, inclua:

```markdown
## Contexto
- Etapa: [ex: "5.4 - Use Case Login"]
- Objetivo: [ex: "Implementar autenticação com JWT"]

## Estrutura do Projeto
[cole a estrutura de pastas relevante]

## Código Implementado

### Interface
[cole a interface]

### Implementação
[cole a classe principal]

### Testes
[cole os testes unitários]

## Dúvidas Específicas
1. [sua dúvida 1]
2. [sua dúvida 2]

## Checklist de Segurança (auto-avaliação)
- [ ] Não há senhas em logs
- [ ] Mensagens de erro são genéricas
- [ ] Hash é feito antes de comparar
- [ ] [outros itens relevantes]
```

---

## Glossário

| Termo | Definição |
|-------|-----------|
| **AuthN** | Autenticação - verificar identidade |
| **AuthZ** | Autorização - verificar permissões |
| **Claim** | Afirmação sobre o sujeito do token |
| **CSPRNG** | Cryptographically Secure Pseudo-Random Number Generator |
| **IdP** | Identity Provider - serviço que autentica e emite tokens |
| **JWKS** | JSON Web Key Set - conjunto de chaves públicas |
| **JWT** | JSON Web Token - token auto-contido assinado |
| **RBAC** | Role-Based Access Control |
| **Resource Server** | API que protege recursos e valida tokens |
| **Salt** | Valor aleatório adicionado à senha antes de hashear |
| **Work Factor** | Custo computacional do algoritmo de hash |
