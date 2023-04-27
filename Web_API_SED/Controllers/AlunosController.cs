using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using Web_API_SED.Entities;

namespace Web_API_SED.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlBaseHomologacao = "https://homologacaointegracaosed.educacao.sp.gov.br/ncaapi/api";
        private readonly IMemoryCache _cache;

        public AlunosController(IMemoryCache cache)
        {
            _httpClient = new HttpClient();
            _cache = cache;
        }

        [HttpPost]
        [Route("/Listar")]
        public async Task<IActionResult> Listar([FromBody] List<string> alunosNomes)
        {

            try
            {
                string Token;
                if (!_cache.TryGetValue("Token", out Token))
                {
                    return BadRequest("O token expirou");
                }
                else
                {
                    if (alunosNomes.Count == 0) return BadRequest("Informe ao menos um Aluno no padrão ['Nome Aluno Completo']");

                    List<string> alunos = new List<string>();

                    foreach(var nome in alunosNomes)
                    {
                        string nomeFormatado = Uri.EscapeUriString(nome);
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                        HttpResponseMessage response = await _httpClient.GetAsync($"{_urlBaseHomologacao}/Aluno/ListarAlunos?inNomeAluno={nomeFormatado}");

                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                            alunos.Add(responseString);
                        }
                        else
                        {
                            return BadRequest("Aconteceu um Erro!");
                        }
                    }
                    return Ok("BELEZA");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("O Usuário não foi validado. Por favor faça a requisição para a API de validação!");
            }
        }
    }
}
