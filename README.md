# PROJETO OdontoAPIMinimal .NET API
GLENDA DELFY VELA MAMANI – RM 552667 LUCAS ALCÂNTARA CARVALHO – RM 95111 RENAN BEZERRA DOS SANTOS – RM 553228
# OdontoAPI Minimal 

## 📖 Descrição
A **OdontoAPI Minimal** é uma API para gerenciamento odontológico, integrando **Machine Learning**, **JWT** para segurança, **Idempotency Key** para evitar duplicidade de registros, e **Swagger** para documentação.

## ⚙️ Tecnologias Utilizadas
- 🔹 ASP.NET Minimal API
- 🔹 Entity Framework Core 8
- 🔹 Microsoft ML.NET
- 🔹 Idempotency Key
- 🔹 JWT Autenticação
- 🔹 Testes de integração Xunit

## 🛠️ Funcionalidades
✔ **Cadastro de usuários e pacientes**  
✔ **Previsão de risco odontológico via ML**  
✔ **Proteção dos dados via JWT**  
✔ **Evita duplicidade de registros via Idempotency Key**  
✔ **Swagger para documentação interativa**  
✔ **Teste de Integração com Xunit**  

## 🚀 Como Executar
```bash
# Clone o repositório
git clone https://github.com/glendadelfy/SPRINT3-DOTNET-

# Acesse a pasta do projeto
cd OdontoAPIMinimal

# Instale as dependências
dotnet restore

# Execute a API
dotnet run

#Antes de acessar endpoints protegidos(POST), gere um token JWT
POST /gerar-token

#Ou use o endpoint /admin/login e use para gerar o token user: admin e senha: 123456
POST /admin/login

#Para ML use o endpoint e adicione o json no formato do post
POST /risco-odonto/treinar-modelo

#Após o modelo treinado use o endpoint de predição, que retornará o risco odontológico
POST /risco-odonto/prever

## Como Executar os Testes Automatizados

A **OdontoAPI Minimal** possui testes automatizados escritos em **Xunit**, garantindo estabilidade e confiabilidade das funcionalidades implementadas. Os testes cobrem áreas essenciais como:

✔ **Autenticação JWT** → Garante que tokens JWT são gerados corretamente e protegem endpoints.  
✔ **Idempotency Key** → Valida que registros duplicados não são criados ao receber requisições repetidas.  
✔ **Criação de usuário** → Testa a criação, atualização, busca e deleção de usuários.

### 📌 **Passo a Passo para Executar os Testes**
1️⃣ **Certifique-se de que as dependências do projeto estão instaladas**
```bash
dotnet restore
use o dotnet test no terminal

