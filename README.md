https://lab.coodesh.com/arthurvsr/challenge-20210221/-/blob/master/README.md

## Back-End Challenge 20210221

Entrega de projeto para a empresa Coodesh. Gostaria de ter incluido testes, suporte a docker e autenticação. Mas não consegui por conta de meu tempo em minha ocupação.

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
  }
},
"TheSpaceDevsLaunchEndPoint": "https://ll.thespacedevs.com/2.2.0/launch/",
```

Está segunda seção é referente ao endpoint da api da Space Devs que a aplicação irá consumir.
Na parte superior do Visual Studio, clique na barra de pesquisa e digite 'Console do Gerenciador de Pacotes'. Dentro do terminal do gerenciador de pacotes aberto em baixo, clique na opção 'Projeto padrão' e selecione a opção 'Data\Data'. No shell de execução do gerenciador de pacotes, faça um migration ao seu banco, digitando o comando 'Add-Migration First'. Após terminar o processo, digite o comando 'Update-Database' para adição das tabelas no banco de dados do Mysql.
A aplicação então está pronta para iniciar. Existe um job do Quartz com o trigger configurado para rodar um update do banco de dados às 4 da manhã. Foi adicionado na API pela interface do Swagger um método POST para atualização do banco de dados. Sendo o argumento 'skip' neste aceitando nulo, ou se preencher, ele vai skipar os launchs pelo inteiro informado.

Obs.: O projeto se inicia pela Services, sendo as demais camada de suporte para o sistema, como a repository, businesse e etc. Caso não esteja configurado, clique com o botão direito na solução 'FutureSpace' e selecione 'Propriedades'. Clique na opção 'Vários projetos de inicialização' e com a setinha da janela, mova o projeto 'Services' para a última posição, após clique na opção 'Nenhum' e selecione a opção 'Iniciar'. Clique em 'Aplicar e 'Ok' após essas alterações.
