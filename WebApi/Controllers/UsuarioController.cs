using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository UsuarioRepository;

        /*
       Construtor com a dependência de uma interface de repositório. Esta interface terá sua instância injetada pelo contexto
       de injeção do .net core, configurado no arquivo startup.cs na linha 46
        */
        public UsuarioController(UsuarioRepository UsuarioRepository)
        {
            this.UsuarioRepository = UsuarioRepository;
        }

        // GET: api/Usuario
        [HttpGet]
        public IActionResult Get()
        {
            var Usuario = UsuarioRepository.FindAll();

            if (Usuario.Count() == 0)
                return NoContent();

            return Ok(Usuario);
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var usuario = UsuarioRepository.FindByID(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuario/5
        //[FromForm]
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] long id, [FromBody] Usuario Usuario)
        {
            ConvertDataSQL(Usuario);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id < 1)
            {
                return BadRequest();
            }

            var usuario = UsuarioRepository.FindByID(id);
            if (usuario == null)
            {
                return NotFound();
            }

            Usuario.Id = usuario.Id;

            try
            {
                UsuarioRepository.Update(Usuario);
                return Ok(Usuario);
            }
            catch (Exception ex)
            {
                string.Format("Erro Update/Put no cliente Nº {0}, erro Exception{1}", Usuario.Id, ex);
            }


            return NoContent();



        }

        // POST: api/Usuario
        [HttpPost]
        public IActionResult Post([FromBody] Usuario Usuario)
        {
            ConvertDataSQL(Usuario);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Usuario.Id = UsuarioRepository.Add(Usuario);


            }
            catch (Exception ex)
            {
                string.Format("Erro Post/Adicionar cliente Nº {0}, erro Exception{1}", Usuario.Id, ex);
                return NotFound();
            }


            return CreatedAtAction("Get", new { id = Usuario.Id }, Usuario);
        }



        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = UsuarioRepository.FindByID(id);
            if (usuario == null)
            {
                return NotFound();
            }


            try
            {
                UsuarioRepository.Remove(id);

            }
            catch (Exception ex)
            {
                string.Format("Erro Delete cliente Nº {0}, erro Exception{1}", usuario.Id, ex);
            }

            return Ok(usuario);
        }


        private static void ConvertDataSQL(Usuario Usuario)
        {
            Usuario.Nascimento = DateTime.Parse(Usuario.Nascimento.ToString()).ToString("yyyy-MM-dd");
        }

    }
}