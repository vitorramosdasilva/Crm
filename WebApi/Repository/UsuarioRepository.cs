using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UsuarioRepository: IRepository<Usuario>
    {
        private string connectionString;
        public UsuarioRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public long Add(Usuario item)
        {
            string sql = @"INSERT INTO Tb_Usuario (name,nascimento,email,IdTipoUsuario, CpfCnpj, senha) VALUES(@Name,@Nascimento,@Email,1, @CpfCnpj, @Senha);  select currval(pg_get_serial_sequence('Tb_Usuario', 'Id'));; ";
            using (IDbConnection dbConnection = Connection)
            {
                
                item.Id = dbConnection.ExecuteScalar<long>(sql, new
                {
                    nome = item.Nome,
                    nascimento = item.Nascimento,
                    email = item.Email,
                    idtipoUsuario = item.IdTipoUsuario,
                    cpfcnpj = item.CpfCnpj,
                    senha = item.Senha

                });
                return item.Id;
            }

        }
        

        public IEnumerable<Usuario> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Usuario>("SELECT * FROM Tb_Usuario");
            }
        }

        public Usuario FindByID(long id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Usuario>("SELECT * FROM Tb_Usuario WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        public void Remove(long id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Tb_Usuario WHERE Id=@Id", new { Id = id });
            }
        }

        public void Update(Usuario item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE Tb_Usuario SET nome = @Nome,  nascimento  = @Nascimento, email= @Email, idTipoUsuario = @idTipoUsuario, CpfCnpj = @CpfCnpj, Senha = @Senha WHERE id = @Id", item);
            }
        }

       
    }

}

