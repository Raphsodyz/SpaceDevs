Projeto inicialmente feito como teste de um processo seletivo a qual estava participando. Agora estou incluindo uma série de funcionalidades e coisas que estou usando para testar meus conhecimentos, e tambem para aprender coisas novas.

## Requisitos
- Docker

## Execução
Para rodar o projeto e necessário somente possuir docker.
Faça o download do código acima. Extraia o arquivo. Dentro da raiz do projeto extraido(onde estão os dockerfile e docker-compose.yml) rode o comando no shell ou prompt de comando:

``` docker compose up -d ```

O container inicialmente esta configurado na portal 5000. A API fica disponível e a documentação pode ser visualizada no projeto entrando em /swagger. (http://localhost:5000/swagger).

Obs.: Dentre os endpoints disponíveis que fazem o CRUD, o método POST faz um update do dataset do banco para trazer os dados da API da spacedevs. Esse processo costuma demorar um pouquinho pois a cada requisição trazem 1500 registros de 100 em 100. Aguarde para popular o banco de dados.
