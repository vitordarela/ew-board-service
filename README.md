# BOARD API

Este � um servi�o completo para a cria��o e gerenciamento de boards de tarefas. Ele foi desenvolvido utilizando .NET 8 e MongoDB como reposit�rio de dados e XUnit para testes unit�rios.
Foi utilizado a estrutura de Clean Code via Presentation Layer e DDD (Domain-Driven Design).

## Funcionalidades Principais

- Cria��o, atualiza��o e exclus�o de boards de tarefas (projetos).
- Adi��o, atualiza��o e remo��o de tarefas em boards (projetos) espec�ficos.
- Atribui��o de usu�rios a tarefas.
- Hist�rico de altera��es em tarefas.
- Gest�o de Coment�rio em tarefas.
- Gerenciamento de usu�rios e permiss�es.

## Inicializa��o via Docker

Para iniciar o servi�o utilizando Docker, siga os passos abaixo:

1. Certifique-se de ter o Docker instalado na sua m�quina. Voc� pode encontrar instru��es de instala��o [aqui](https://docs.docker.com/get-docker/).

2. Clone este reposit�rio:

    ```
    git clone https://github.com/vitordarela/ew-board-service
    ```

3. Navegue at� o diret�rio do projeto:

    ```
    cd ew-board-service\Presentation.Api
    ```

4. Altere o arquivo Docker Compose (`docker-compose.yml`) para iniciar o servi�o informando o caminho completo do `context` e `dockerfile` de onde est� o reposit�rio, exemplo:

    ```
    board-api:
        build: 
            context: "C:\\Users\\vitor\\source\\repos\\ew-board-service"
            dockerfile: "C:\\Users\\vitor\\source\\repos\\ew-board-service\\Presentation.Api\\Dockerfile"
    ```
4. Execute o comando Docker Compose para iniciar o servi�o:

    ```
    docker-compose up
    ```

6. Ap�s a inicializa��o bem-sucedida, voc� poder� acessar o servi�o em `http://localhost:8080/swagger/index.html`.

## Endpoints da API

A API cont�m os seguintes endpoints:

### User

- `GET /api/user`: Listar todos os usu�rios criados.
- `POST /api/user`: Cria um novo usu�rio.

### Project

- `GET /api/project`: Retornar todos os projetos do usu�rio.
- `POST /api/project`: Criar um novo projeto.
- `PUT /api/project/{projectId}`: Atualizar um projeto.
- `DELETE /api/project/{projectId}`: Remover um projeto.

### Task

- `GET /api/project/{projectId}/task`: Listar todas as tarefas de um projeto.
- `GET /api/project/{projectId}/task/{taskId}/history`: Listar todo o hist�rico de altera��o e movimenta��o da tarefa.
- `POST /api/project/{projectId}/task`: Criar uma tarefa dentro de um projeto.
- `PUT /api/project/{projectId}/task/{taskId}`: Atualizar os dados da tarefa.
- `DELETE /api/project/{projectId}/task/{taskId}`: Remover uma tarefa do projeto.

### Comment

- `GET /api/task/{taskId}/comment`: Retornar todos os coment�rios da tarefa.
- `POST /api/task/{taskId}/comment`: Criar um novo coment�rio na tarefa.
- `PUT /api/task/{taskId}/Comment/{commentId}`: Atualizar um coment�rio na tarefas.
- `DELETE /api/task/{taskId}/comment/{commentId}`: Remover um coment�rio da tarefas.

### Report

- `GET /api/report/task/general`: Listar estat�stica gen�rica das tarefas.
- `GET /api/report/task/average/bystatus`: Lista a estat�stica gerando m�dia das tarefas por status.


Para obter a lista completa de endpoints e par�metros, consulte a documenta��o da API. `http://localhost:8080/swagger/index.html` rodando-a localmente.

## Refinamento

- Testes automatizados? haver� espa�o para criarmos?
- N�o possuimos mecanismos de logs da aplica��o, iremos ter espa�o para melhorar e incluirmos Observability no projeto?
- N�o possuimos nenhuma layer de cache implementada, se pretendemos disponibilizar a aplica��o para a internet e em grande escala precisamos nos preocupar com isso, seja cache via CDN como tamb�m cache compartilhado via Redis ou Memory Cache em alguns casos. Haver� a necessidade de nos preocuparmos com isso?
- N�o possuimos uma documenta��o da API teremos espa�o para criar?
- API de Estat�stica esta muito basica e n�o escala, a medida que vai adicionando reports mais complexos e mais dados este modelo atual � problem�tico.
- O Usu�rio no futuro n�o poder� compartilhar o board com outros usu�rios? Trabalhar em diferentes boards de outros usu�rios?
- Visando uma melhoria de produto n�o seria interessante adicionar TAGS nas tarefas?
- Seria interessante adicionar filtros nos boards para filtrar tarefas por "Status, Data de vencimento, Prioridade" e etc?

## Melhorias

- Middleware para padroniza��o dos Erros.
- Users: deveria ser implementado via OAuth, para nao ter via parameters que enviar o userId sempre.
- Adicionar mais testes unitarios especificos para cobrir mais regras de neg�cios.
- Estou usando Entity Framework para MongoDB e desta forma estou utilizando a sobrescrita do metodo SaveChanges para detectar as altera��es, porem como tem limita��es para o MongoDB n�o consigo criar o hist�rico da forma que eu havia visualizado (estilo Jira), e para fazer manualmente eu precisaria de mais tempo para criar a compara��o e detec��o dos campos alterados.
- Em termos de escalabilidade era ideal ja deixar a aplica��o preparada para Kubernetes
- Este ponto � apenas uma nota: Se eu fosse come�ar este projeto novamente, usaria o padr�o CQS (Command Query Separation) que me daria muito mais agilidade e padroniza��o dos endpoints.
