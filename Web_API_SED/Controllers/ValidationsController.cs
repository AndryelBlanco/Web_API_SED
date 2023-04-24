using Microsoft.AspNetCore.Mvc;
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

        public ValidationsController()
        {
            _httpClient = new HttpClient();
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
                var token = Json["outAutenticacao"].ToString();
                TempData["Token"] = token; //Expira em 30 min...
                TempData["Credenciais"] = credenciais; // Salvar para usar quando o token expirar

                return Ok(token);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }

        }
    }
}
