using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SqlServerCache.Services;

namespace SqlServerCache.Controllers
{
    [Route("api/v1/exemplo")]
    public class ExemploController : Controller
    {
        //Método que receberá as requisições dos clientes.
        [HttpGet]
        public async Task<ActionResult> GetItens([FromServices] IDistributedCache _cache,
        [FromServices] ComissionamentoService _servico)
        {
            string conteudo = await _cache.GetStringAsync("lista"); // Buscando no banco em cache se existe o valor.

            if (conteudo == null)
            {
                string resultado = _servico.GetComissionamentos();

                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(20)); // Tempo em que o valor será valido.
                _cache.Set("lista", Encoding.UTF8.GetBytes(resultado), options);

                conteudo = resultado;
            }

            return Content(conteudo, "application/json");
        }
    }
}