
# Como utilizar

Uma breve descrição de como utilizar e testar o projeto.




## Documentação da API
- Ao rodar o projeto é possível fazer todas requisições pelo swagger.
#### O primeiro passo é validar o usuário, para isso é necessário informar um username e a password.


```http
  Post /ValidarUsuario
```

- Devem ser passados no body:
| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `username` | `string` | **Obrigatório**. O Username fornecido |
| `password` | `string` | **Obrigatório**. A Password fornecida |

#### Retorna os alunos correspondentes aos nomes fornecidos no body em CSV

```http
  Post /Listar
```
- Devem ser passados no body:
| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `[]`      | `[]` | **Obrigatório**. Nomes completos em um array, exemplo ["Nome completo um", "Nome completo dois", "Nome completo tres"] |


