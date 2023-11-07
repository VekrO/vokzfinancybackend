# Vokz Financy

Back-end do sistema de gestão financeira Vokz Financy

Padrões utilizados:

Unit Of Work - Para criar uma camada de transação com o banco de dados, onde eu posso chamar os repositórios que estão dentro dessa camada.
Repository Pattern - Para criar uma camada que deixa apenas como responsabilidade retornar os dados requisitados sem nenhum tipo de validação, separando a lógica do transacional.
