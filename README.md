# BOARD API

Este é um serviço completo para a criação e gerenciamento de boards de tarefas. Ele foi desenvolvido utilizando .NET 8 e MongoDB como repositório de dados e XUnit para testes unitários.
Foi utilizado a estrutura de Clean Code via Presentation Layer e DDD (Domain-Driven Design).

## Funcionalidades Principais

- Criação, atualização e exclusão de boards de tarefas (projetos).
- Adição, atualização e remoção de tarefas em boards (projetos) específicos.
- Atribuição de usuários a tarefas.
- Histórico de alterações em tarefas.
- Gestão de Comentário em tarefas.
- Gerenciamento de usuários e permissões.

## Inicialização via Docker

Para iniciar o serviço utilizando Docker, siga os passos abaixo:

1. Certifique-se de ter o Docker instalado na sua máquina. Você pode encontrar instruções de instalação [aqui](https://docs.docker.com/get-docker/).

2. Clone este repositório:

    ```
    git clone https://github.com/vitordarela/ew-board-service
    ```

3. Navegue até o diretório do projeto:

    ```
    cd ew-board-service\Presentation.Api
    ```

4. Altere o arquivo Docker Compose (`docker-compose.yml`) para iniciar o serviço informando o caminho completo do `context` e `dockerfile` de onde está o repositório, exemplo:

    ```
    board-api:
        build: 
            context: "C:\\Users\\vitor\\source\\repos\\ew-board-service"
            dockerfile: "C:\\Users\\vitor\\source\\repos\\ew-board-service\\Presentation.Api\\Dockerfile"
    ```
4. Execute o comando Docker Compose para iniciar o serviço:

    ```
    docker-compose up
    ```

6. Após a inicialização bem-sucedida, você poderá acessar o serviço em `http://localhost:8080/swagger/index.html`.

## Endpoints da API

A API contém os seguintes endpoints:

### User

- `GET /api/user`: Listar todos os usuários criados.
- `POST /api/user`: Cria um novo usuário.

### Project

- `GET /api/project`: Retornar todos os projetos do usuário.
- `POST /api/project`: Criar um novo projeto.
- `PUT /api/project/{projectId}`: Atualizar um projeto.
- `DELETE /api/project/{projectId}`: Remover um projeto.

### Task

- `GET /api/project/{projectId}/task`: Listar todas as tarefas de um projeto.
- `GET /api/project/{projectId}/task/{taskId}/history`: Listar todo o histórico de alteração e movimentação da tarefa.
- `POST /api/project/{projectId}/task`: Criar uma tarefa dentro de um projeto.
- `PUT /api/project/{projectId}/task/{taskId}`: Atualizar os dados da tarefa.
- `DELETE /api/project/{projectId}/task/{taskId}`: Remover uma tarefa do projeto.

### Comment

- `GET /api/task/{taskId}/comment`: Retornar todos os comentários da tarefa.
- `POST /api/task/{taskId}/comment`: Criar um novo comentário na tarefa.
- `PUT /api/task/{taskId}/Comment/{commentId}`: Atualizar um comentário na tarefas.
- `DELETE /api/task/{taskId}/comment/{commentId}`: Remover um comentário da tarefas.

### Report

- `GET /api/report/task/general`: Listar estatística genérica das tarefas.
- `GET /api/report/task/average/bystatus`: Lista a estatística gerando média das tarefas por status.


Para obter a lista completa de endpoints e parâmetros, consulte a documentação da API. `http://localhost:8080/swagger/index.html` rodando-a localmente.

## Refinamento

- Testes automatizados? haverá espaço para criarmos?
- Não possuimos mecanismos de logs da aplicação, iremos ter espaço para melhorar e incluirmos Observability no projeto?
- Não possuimos nenhuma layer de cache implementada, se pretendemos disponibilizar a aplicação para a internet e em grande escala precisamos nos preocupar com isso, seja cache via CDN como também cache compartilhado via Redis ou Memory Cache em alguns casos. Haverá a necessidade de nos preocuparmos com isso?
- Não possuimos uma documentação da API teremos espaço para criar?
- API de Estatística esta muito basica e não escala, a medida que vai adicionando reports mais complexos e mais dados este modelo atual é problemático.
- O Usuário no futuro não poderá compartilhar o board com outros usuários? Trabalhar em diferentes boards de outros usuários?
- Visando uma melhoria de produto não seria interessante adicionar TAGS nas tarefas?
- Seria interessante adicionar filtros nos boards para filtrar tarefas por "Status, Data de vencimento, Prioridade" e etc?

## Melhorias

- Middleware para padronização dos Erros.
- Users: deveria ser implementado via OAuth, para nao ter via parameters que enviar o userId sempre.
- Adicionar mais testes unitarios especificos para cobrir mais regras de negócios.
- Estou usando Entity Framework para MongoDB e desta forma estou utilizando a sobrescrita do metodo SaveChanges para detectar as alterações, porem como tem limitações para o MongoDB não consigo criar o histórico da forma que eu havia visualizado (estilo Jira), e para fazer manualmente eu precisaria de mais tempo para criar a comparação e detecção dos campos alterados.
- Em termos de escalabilidade era ideal ja deixar a aplicação preparada para Kubernetes
- Este ponto é apenas uma nota: Se eu fosse começar este projeto novamente, usaria o padrão CQS (Command Query Separation) que me daria muito mais agilidade e padronização dos endpoints.
