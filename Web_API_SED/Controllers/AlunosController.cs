using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Web_API_SED.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlBaseHomologacao = "https://homologacaointegracaosed.educacao.sp.gov.br/ncaapi/api";
        public AlunosController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        [Route("/Listar")]
        public async Task<IActionResult> Listar()
        {

            string Token = TempData["Token"] as string;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = await _httpClient.GetAsync($"{_urlBaseHomologacao}/Aluno/ListarAlunos");

            if (response.IsSuccessStatusCode)
            {

                string conteudoResposta = await response.Content.ReadAsStringAsync();

                return Ok(Token);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}
