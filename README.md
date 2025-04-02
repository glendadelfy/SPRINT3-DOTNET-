# PROJETO OdontoAPIMinimal .NET API
GLENDA DELFY VELA MAMANI – RM 552667 LUCAS ALCÂNTARA CARVALHO – RM 95111 RENAN BEZERRA DOS SANTOS – RM 553228
# 1. 	Definição do projeto ASP.NET CORE WEB API 
1.1	Objetivo do projeto
Objetivo do projeto Web API é fazer o cadastro de usuários e seu registro no banco de dados Oracle, assim, tendo o registro de usuários cadastrados. 
# 2. Escopo do projeto 
Classe Usuario Model: Id, Name, Email, Password, isComplete, isActive, Role e Avatar.
•	Cadastro do Usuario: ter usuário cadastrado na API;
•	Acesso aos usuários cadastrados pelo Id;
•	Acesso as infomações de todos os usuários;
•	Crud do Usuario: Usuario tem o create, read, update e delete.
A classe Usuario:
![image](https://github.com/user-attachments/assets/32de984e-4aad-412d-a154-2c55cac53710)

# 3. Infraestrutura do Projeto API .NET (Camada Application)

3.1	Criação da solução API Web do ASP.NET Core

3.2	 Implementação de domínio
•	Criação da classe na Models
•	Implementação do UsuarioModel 

3.3	Implementação da camada de aplicação 
•	Serviço de aplicação
•	Manipulação de erros no endpoint 

3.4	Implementação de camada de infraestutura
•	Mapeamento de Entidade do UsuarioModel (EntityFramework Core).
•	Repositorios Concretos no AppDbContext
•	Migrations no Banco de dados Oracle.

# 4.	Imagem com todas as camadas utilizadas
![image](https://github.com/user-attachments/assets/c35abae7-0f1b-4bd5-b250-b04c2be45147)

# 6.	Escolha da estrutura (arquitetura monolítica)
O projeto é pequeno, com requisitos bem definidos e poucas demandas por escalabilidade extrema, a arquitetura monolítica foi a escolha. Ela é mais fácil de configurar, rápida para colocar em produção e exige menos esforços de manutenção no início. E toda a lógica do sistema está unificada em um único projeto. O microsserviços não faria sentindo no projeto pois o sistema não é dividido em serviços independe. Logo, a escolha do monolítica foi aplicada no projeto por ter todos os componentes necessários.
# 7.	Documentação com o Swagger 
Implementação da documentação com o pacote Swashbuckle.AspNetCore para a visualização dos endpoints e seus requisições. 
O pacote do Swagger para descrição da API em si e documentação. O uso do singleton para gerenciar configurações dos endpoints.
Um CRUD Completo com o UsuarioModel utilizando o Swagger.
Vizualização da API:![image](https://github.com/user-attachments/assets/72c3faec-c35d-4002-a37e-03fa492b8cd0)

# Como rodar a aplicação? 
- Necessario ter a IDE Visual Studio 2022 ou RIder
- Ter o jdk 8 instalado que é o usado no projeto
- Abrir a solução do Projeto ![image](https://github.com/user-attachments/assets/a3bdfd5a-9eed-46ba-a500-21e9660f7daf)
- Compilar a solução no http: ![image](https://github.com/user-attachments/assets/ec382340-bf76-4d19-bf5c-ceaa9f8d796d)
- Pronto! Irá abrir uma janela no seu browser com a API do swagger igual a imagem 7. 
- Aqui você poderá fazer um create, read, update e delete
  Considerações finais é aprimorar a API  











Vizualização da API:
 

 








