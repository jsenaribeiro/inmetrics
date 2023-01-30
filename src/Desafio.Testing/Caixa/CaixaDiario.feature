#language: pt

Funcionalidade: consultar caixa diário
	Como um usuário qualquer
	Eu quero consultar o consolidado
	Para ter a soma de lancamentos do dia

Cenário: campo data é obrigatório
	Quando consultar caixa em ""
	Então retorna mensagem de data é obrigatório 

Cenário: consulta com sucesso
	Dada um crédito de R$ 100,00
	E cuja data é "2020-02-02"
	Quando consultar caixa em "2020-02-02"
	Então retorna o valor R$ 100,00
