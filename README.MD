# Dados públicos CNPJ - Auxiliador para download dos zips

Software desenvolvido para auxiliar no download dos CNPJ do site da receita https://www.gov.br/receitafederal/pt-br/assuntos/orientacao-tributaria/cadastros/consultas/dados-publicos-cnpj

## Motivação

O download dos arquivos zips do site da receita federal estão disponibilizados em arquivos zips separados por partes.
O download através do site é lento e os dados são atualizado apenas de 3 em 3 meses.
A ideia é ter um software que possa fazer o download desses arquivos sem a necessidade de abrir o site e baixar um por um.

Os dados podem ser usados com auxílio da ferramenta [CNPJ-FULL](https://github.com/fabioserpa/CNPJ-full) 

### Pré requisitos

Para compilar o sistema é necessário ter o .net 5.0 SDK instalado (https://dotnet.microsoft.com/download/dotnet/5.0/runtime?utm_source=getdotnetcore&utm_medium=referral)

Apenas rodar em windows não é necessário nenhum software (runtime já vai embarcado)

Rodar em linux vai ser necessário instalar o .net 5.0 runtime ou sdk


### Utilização
  
  Windows, baixe o pacote para windows, descompacte e dê dois cliques

  Linux, baixe o pacote, descompacte e execute `./CNPJ.Core`

### Download 

https://github.com/fernandolamp/CNPJDownload.Net/tags

