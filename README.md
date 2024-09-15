# MediatorCqrsApi
Utilização do padrão Mediator com CQRS (Command Query Responsibility Segregation) separando operações de leitura (queries) das operações de escrita (commands), e o Mediator para enviar essas requisições e distribui-las aos respectivos manipuladores.

Command: Representa uma ação que altera o estado do sistema (inserir, atualizar, deletar dados).
Query: Representa uma operação de leitura que obtém dados do sistema.
CQRS: visa isolar essas duas responsabilidades, de modo que comandos e consultas não se misturem. Dessa forma, cada operação tem um manipulador específico.
Mediator: age como intermediário para enviar comandos e consultas aos manipuladores corretos.
