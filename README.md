https://lab.coodesh.com/arthurvsr/challenge-20210221/-/blob/master/README.md

## Back-End Challenge 20210221

Entrega de projeto para a empresa Coodesh. Gostaria de ter incluido testes, suporte a docker e autenticação. Mas não consegui por conta de meu tempo em minha ocupação.

## O escopo

Introdução
Este é um desafio para testar seus conhecimentos de Back-end;
O objetivo é avaliar a sua forma de estruturação e autonomia em decisões para construir algo escalável utilizando os Frameworks de Back-end modernos.

Instruções iniciais obrigatórias

Utilizar o seu github pessoal para publicar o desafio. Confirme que a visibilidade do projeto é pública (não esqueça de colocar no readme a referência a este challenge);
Desenvolver uma Rotina para importar os dados do Projeto: https://ll.thespacedevs.com/2.0.0/launch/

Desenvolver uma REST API com um CRUD

Case
A empresa FutureSpace Inc, está trabalhando em um projeto para a comunidade espacial para facilitar a gestão e visualização da informação los lançamentos de foguetes de maneira simples e objetiva em um Dashboard onde podem listar, filtrar e expandir os dados disponíveis.
O seu objetivo nesse projeto, é trabalhar no desenvolvimento da REST API da empresa FutureSpace Inc seguindo os requisitos propostos neste desafio.

API

Modelo de Dados:
Para a definição do modelo, consultar o arquivo launchers.json com os principais campos que usaremos no projeto.

imported_t: campo do tipo Date com a dia e hora que foi importado;<br />
status: campo do tipo Enum com os possíveis valores draft, trash e published;<br />

Sistema do CRON
Para prosseguir com o desafio, precisaremos criar na API um sistema de atualização que vai importar os dados para a Base de Dados com a versão mais recente do dataset uma vez ao día. Adicionar aos arquivos de configuração o melhor horário para executar a importação.
Ter em conta que:

Todos os produtos deverão ter os campos personalizados imported_t e status.
Importar os dados de maneira paginada para não sobrecargar a API do The Space Devs. Por exemplo, de 100 registros.
Limitar a importação a somente 2000 registros;

A REST API
Na REST API teremos um CRUD com os seguintes endpoints:

GET /: Retornar uma mensagem "REST Back-end Challenge 20201209 Running".<br />
PUT /launchers/:launchId: Será responsável por receber atualizações realizadas.<br />
DELETE /launchers/:launchId: Remover o launch da base.<br />
GET /launchers/:launchId: Obter a informação somente de um launch da base de dados.<br />
GET /launchers: Listar os launchers da base de dados de maneira paginada.<br />

Extras

Diferencial 1 Escrever Unit Test para os endpoints da REST API<br />
Diferencial 2 Executar o projeto usando Docker<br />
Diferencial 3 Escrever um esquema de segurança utilizando API KEY nos endpoints. Ref: https://learning.postman.com/docs/sending-requests/authorization/#api-key<br />
Diferencial 4 Descrever a documentação da API utilizando o conceito de Open API 3.0;<br />

## Instruções

Requisitos:

- .Net 7.0
- MySql 8.0
- Visual Studio(Ou visual code, tutorial baseado no visual studio).

Faça o download do código acima.

Dentro do Mysql, crie um banco de dados com o nome que desejar para que a aplicação possa consumir.
Com o código em mãos, abra o arquivo CadastroProAuto.sln pelo visual studio na opção 'Abrir um projeto ou solução'. Dentro projeto, pela direita no 'Gerenciador de Soluções' abra a pasta 'Services' e clique com botão direito em cima do arquivo 'Services' e em 'adicionar/Novo item' crie um arquivo chamado 'appsettings.json'; esse arquivo será utilizado para as interações da aplicação com o seu banco de dados/etc. Dentro do appsettings.json(apague se vier algo escrito), crie uma seção chamada: 

```json
{
"ConnectionStrings":{
  "default":"server= ; database= ; user id= ; password= ;"
  }
}
```

Os campos dentro da opção 'default' a serem preenchidos são:

- Server: O servidor do seu banco de dados ex: localhost, 197.168.0.1 e etc.
- Database: O nome do banco de dados a ser utilizado.
- User id: Seu usuário do Mysql.
- Password: A senha do seu usuário Mysql.

Neste mesmo arquivo json, adicione uma vírgula após as chaves do objeto adicionado acima e crie esta seção:

```json
  "TheSpaceDevsLaunchEndPoint": "https://ll.thespacedevs.com/2.2.0/launch/",
```

Ficando então assim:

```json
{
"ConnectionStrings":{
  "default":"server= ; database= ; user id= ; password= ;"
  },
"TheSpaceDevsLaunchEndPoint": "https://ll.thespacedevs.com/2.2.0/launch/"
}
```

Está segunda seção é referente ao endpoint da api da Space Devs que a aplicação irá consumir.
Na parte superior do Visual Studio, clique na barra de pesquisa e digite 'Console do Gerenciador de Pacotes'. Dentro do terminal do gerenciador de pacotes aberto em baixo, clique na opção 'Projeto padrão' e selecione a opção 'Data\Data'. No shell de execução do gerenciador de pacotes, faça um update do seu banco, digitando o comando'Update-Database' para adição das tabelas no banco de dados do Mysql.
A aplicação então está pronta para iniciar. Lembre-se de configurar no visual studio para rodar a aplicação pelo IISExpress.
Existe um job do Quartz com o trigger configurado para rodar um update do banco de dados às 4 da manhã. Foi adicionado na API pela interface do Swagger um método POST para atualização do banco de dados. Sendo o argumento 'skip' neste aceitando nulo, ou se preencher, ele vai skipar os launchs pelo inteiro informado. O processo costuma demorar uns 10min pois seram carregados da API 1500 launchs(1500 pois a API da future space bloqueia temporariamente mais registros que esses.) com uma série de entidades. Pode ser feito o acompanhamento da atualização do banco via tabela `update_log`.

Obs.: O projeto se inicia pela Services, sendo as demais camada de suporte para o sistema, como a repository, businesse e etc. Caso não esteja configurado, clique com o botão direito na solução 'FutureSpace' e selecione 'Propriedades'. Clique na opção 'Vários projetos de inicialização' e com a setinha da janela, mova o projeto 'Services' para a última posição, após clique na opção 'Nenhum' e selecione a opção 'Iniciar'. Clique em 'Aplicar e 'Ok' após essas alterações.
