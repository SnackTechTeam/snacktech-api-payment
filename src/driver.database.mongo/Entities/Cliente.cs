using common.DataSource;

namespace driver.database.mongo.Entities
{
    public class Cliente
    {
        public string ClienteId {get; set;} = default!;
        public string Email {get; set;} = default!;
        public string Nome {get; set;} = default!;

        public static implicit operator Cliente(ClienteDto clienteDto){
            return new Cliente{
                ClienteId = clienteDto.Id.ToString(),
                Email = clienteDto.Email,
                Nome = clienteDto.Nome
            };
        }

        public static implicit operator ClienteDto(Cliente cliente){
            return new ClienteDto{
                Id = Guid.Parse(cliente.ClienteId),
                Email = cliente.Email,
                Nome = cliente.Nome
            };
        }
    }
}