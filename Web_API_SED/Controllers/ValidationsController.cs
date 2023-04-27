using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using Web_API_SED.Entities;

namespace Web_API_SED.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidationsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlBaseHomologacao = "https://homologacaointegracaosed.educacao.sp.gov.br/ncaapi/api";
        private readonly IMemoryCache _cache;

        public ValidationsController(IMemoryCache cache)
        {
            _httpClient = new HttpClient();
            _cache = cache;
        }

        [HttpPost]
        [Route("/ValidarUsuario")]
        public async Task<IActionResult> ValidarUsuario([FromBody] User loginData)
        {
            //Por enquanto sem criptografia
            if (!ModelState.IsValid) return BadRequest("Por favor, informe os de login!");

            string credenciais = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{loginData.username}:{loginData.password}"));


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credenciais);
            HttpResponseMessage response = await _httpClient.GetAsync($"{_urlBaseHomologacao}/Usuario/ValidarUsuario");

            if (response.IsSuccessStatusCode)
            {
               
                string conteudoResposta = await response.Content.ReadAsStringAsync();
                var Json = JObject.Parse(conteudoResposta);
                try
                {
                    var token = Json["outAutenticacao"].ToString();
                    _cache.Set("Token", token, TimeSpan.FromMinutes(30));
                    _cache.Set("Credenciais", credenciais, TimeSpan.FromMinutes(60)); //Salva para quando o token expirar

                    return Ok(token);
                }catch (Exception ex)
                {
                    return BadRequest(ex.Message); 
                }

            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }

        }
    }
}
