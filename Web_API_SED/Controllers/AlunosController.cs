using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
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

            //Por padrão, cadastramos os seguintes alunos e esses devem ser usados para testes ["ISAAC MONTANHEZ DO NASCIMENTO", "HUGO MURILO DOS SANTOS PEREIRA", "VITORIA RODRIGUES GUEDES", "DEBORA MARIA BUENO RODRIGUES", "JAMILLY VITORIA CARVALHO DO CARMO RODRIGUES"] 

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

                    List<Aluno> AlunosCSV = new List<Aluno>();

                    foreach (var nome in alunosNomes)
                    {
                        string nomeFormatado = Uri.EscapeUriString(nome);
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                        HttpResponseMessage response = await _httpClient.GetAsync($"{_urlBaseHomologacao}/Aluno/ListarAlunos?inNomeAluno={nomeFormatado}");

                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                            if (responseString.Contains("outErro"))
                            {
                                continue;
                            }
                            var Json = JsonConvert.DeserializeObject<ListAlunosResponse>(responseString);

                            foreach (Aluno aluno in Json.outListaAlunos)
                            {
                                AlunosCSV.Add(aluno);
                            }

                        }
                        else
                        {
                            return BadRequest("Aconteceu um Erro!");
                        }
                    }

                    using var stream = new MemoryStream();
                    using var writer = new StreamWriter(stream, Encoding.UTF8);
                    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    object p = csv.Context.RegisterClassMap<AlunoMap>(); // configurações do mapeamento do CSV
                    csv.WriteRecords(AlunosCSV);
                    await writer.FlushAsync();
                    stream.Seek(0, SeekOrigin.Begin);
                    var csvContent = stream.ToArray();

                    var result = new FileContentResult(csvContent, "text/csv")
                    {
                        FileDownloadName = "alunos.csv"
                    };

                    return result;
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
