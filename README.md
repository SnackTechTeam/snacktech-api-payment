# snacktech-api-payment


## Descrição

Serviço API que lida com as features que envolvem pagamento dos pedidos da lanchonete

## Tecnologias Utilizadas

- **C#**: Linguagem de programação usada no desenvolvimento do projeto
- **.NET 8**: Framework como base em que a API é executada
- **MongoDB**: Base de dados para armazenar os dados trabalhados pela API em forma de documentos
- **Swagger**: Facilita a documentação da API
- **Docker**: Permite criar uma imagem do serviço e rodá-la em forma de contâiner

## Como Utilizar

### Pré-requisitos

Antes de rodar o projeto SnackTech, certifique-se de que você possui os seguintes pré-requisitos:

- **.NET SDK**: O projeto foi desenvolvido com o .NET SDK 8. Instale a versão necessária para garantir a compatibilidade com o código.
- **Docker**: O projeto utiliza Docker para contêinerizar a aplicação e o banco de dados. Instale o Docker Desktop para Windows ou Mac, ou configure o Docker Engine para Linux. O Docker Compose também é necessário para orquestrar os containers.
- **MongoDB (Opcional)**: O projeto tem um arquivo de docker-compose que configura e gerencia uma instância do MongoDB dentro de um container Docker. Sendo assim, a instalação ou uso de uma solução em nuvem é opcional.

### Preparando o ambiente

TODO

### Uso

Este é um projeto desenvolvido em .NET, utilizando arquitetura Clean. A aplicação é um microsserviço focado em operações que envolvem o processo de pagamento. 
No momento o projeto utiliza somente a plataforma do MercadoPago e esse serviço funciona como uma interface para outros serviços dentro da estrutura possam focar 
em suas features e caso precise interagir com pagamento acionar esta API

## Desenvolvimento

### Estrutura do Código

TODO

### Tests

TODO

### Modificabilidade

TODO