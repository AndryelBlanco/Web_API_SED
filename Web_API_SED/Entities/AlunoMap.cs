using CsvHelper.Configuration;

namespace Web_API_SED.Entities
{
    public class AlunoMap : ClassMap<Aluno>
    {
        public AlunoMap()
        {
            Map(a => a.outNumRa).Name("NumeroRA");
            Map(a => a.outDigitoRA).Name("DigitoRA");
            Map(a => a.outSiglaUFRA).Name("SiglaUFRA");
            Map(a => a.outNomeAluno).Name("NomeAluno");
            Map(a => a.outDataNascimento).Name("DataNasc");
            Map(a => a.outNomeMae).Name("NomeMae");


        }
    }
}
