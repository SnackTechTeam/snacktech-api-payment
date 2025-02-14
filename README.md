# snacktech-api-payment


## Descrição

Serviço API que lida com as features que envolvem pagamento dos pedidos da lanchonete

## Tecnologias Utilizadas

- **C#**: Linguagem de programação usada no desenvolvimento do projeto
- **.NET 8**: Framework como base em que a API é executada
- **MongoDB**: Base de dados para armazenar os dados trabalhados pela API em forma de documentos
- **Swagger**: Facilita a documentação da API
- **SQS**: Tecnologia de mensageria que permite comunicação assíncrona através do envio de mensagens a uma fila específica.
- **Docker**: Permite criar uma imagem do serviço e rodá-la em forma de contâiner

## Como Utilizar

### Pré-requisitos

Antes de rodar o projeto SnackTech, certifique-se de que você possui os seguintes pré-requisitos:

- **.NET SDK**: O projeto foi desenvolvido com o .NET SDK 8. Instale a versão necessária para garantir a compatibilidade com o código.
- **Docker**: O projeto utiliza Docker para contêinerizar a aplicação e o banco de dados. Instale o Docker Desktop para Windows ou Mac, ou configure o Docker Engine para Linux. O Docker Compose também é necessário para orquestrar os containers.
- **MongoDB (Opcional)**: O projeto tem um arquivo de docker-compose que configura e gerencia uma instância do MongoDB dentro de um container Docker. Sendo assim, a instalação ou uso de uma solução em nuvem é opcional.
- **AWS SQS (Opcional)**: A API faz uma comunicação assíncrona através do AWS SQS publicando notificações quando um pagamento foi realizado. O recebimento do pagamento é via Webhook com o Mercado Pago. O arquivo de docker-compose sobe um serviço de LocalStack onde é possível criar uma fila SQS para ser utilizada no lugar da AWS se desejado.

### Preparando o ambiente

TODO

Depois de rodar o comando docker-compose up, necessário executar o comando:
aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name snacktech-processed-payments

para criar a fila SQS dentro do LocalStack.

Pode ser usado o comando:

 aws --endpoint-url=http://localhost:4566 sqs receive-message     --queue-url http://localhost:4566/000000000000/snacktech-processed-payments

 para ver a mensagem dentro da fila, porem o comando só retorna a primeira mensagem.

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