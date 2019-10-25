using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Usuario: BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Nascimento { get; set; }
        public string Email { get; set; }
        public long IdTipoUsuario { get; set; }
        public string CpfCnpj { get; set; }
        public string Senha { get; set; }



    }
}
