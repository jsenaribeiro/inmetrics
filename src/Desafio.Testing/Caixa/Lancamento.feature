#language: pt

Funcionalidade: executar lancamentos
	Como um usuário qualquer
	Eu quero executar um lancamento
	Para operar débitos e créditos

Cenário: campo valor é obrigatório
	Dada um lancamento de R$ 0,00
	Quando executar seu lancamento
	Então acusará que valor é obrigatório

Cenário: crédito com sucesso
	Dada um lancamento de R$ 100,00
	E cuja data é "1001-10-01"
	Quando executar seu lancamento
	Então registra saldo de R$ 100,00

Cenário: débito com sucesso
	Dada um lancamento de R$ -100,00
	E cuja data é "1001-10-01"
	Quando executar seu lancamento
	Então registra saldo de R$ -100,00
