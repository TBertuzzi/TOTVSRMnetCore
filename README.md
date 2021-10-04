# TOTVSRMnetCore

Este repositorio foi criado para auxiliar na utilização de Webservices do TOTVS RM com asp.net Core.

Inicialmente as classes da TOTVS foram feitas para .Net Framework, porem existem algumas mudanças para o Asp.net core.

A Documentação do WEBService do TOTVS RM pode ser encontrada [Aqui](https://api.totvs.com.br/legado/devrm/)

As classes foram refeitas utilizando as disponibilizadas gratuitamente pela [TOTVS](https://www.totvs.com/) :

* http://tdn.totvs.com/download/attachments/211064343/Utils.cs 
* http://tdn.totvs.com/download/attachments/211064343/DataClient.cs  

## Utilizando o WEBService :

Precisamos adicionar em nosso código a referencia para alguns tipos primitivos, isso pode ser resolvido instalando o pacote [System.ServiceModel.Primitives](https://www.nuget.org/packages/System.ServiceModel.Primitives/) através do nuget :

<img src="https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/Resources/nugetSystem.jpg?raw=true" >

Em seguida precisamos configurar o Webservice:

<img src="https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/Resources/manageServices.jpg?raw=true">

<img src="https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/Resources/configureSoap.jpg?raw=true">

Então conforme a propria documentação da TOTVS devemos configurar o nosso webervice (por exemplo ttp://localhost:8051/wsDataServer/MEX?wsdl ) :

<img src="https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/Resources/rmWebService.jpg?raw=true">

Importante o nome do serviço referenciado deve ser *DataServer*

Em seguida devemos importar as classes refeitas para asp.net core desse repositorio :

* [DataClient](https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/ClassesWebService/DataClient.cs)
* [Utils](https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/ClassesWebService/Utils.cs)

E renomear os namespaces para sua aplicação :

<img src="https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/Resources/novoNamespace.jpg?raw=true">

Pronto!

Agora você pode utilizar o Webservice do TOTVSRM em projetos asp.net Core :

<img src="https://github.com/TBertuzzi/TOTVSRMnetCore/blob/main/Resources/exemploRetornoRM.jpg?raw=true">

Caso queira o exemplo completo pode ser adiquirido [clicando aqui](https://github.com/TBertuzzi/TOTVSRMnetCore/tree/main/TOTVSRMnetCore)
