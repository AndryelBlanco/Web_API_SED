using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Web_API_SED.Entities;

namespace Web_API_SED.Entities
{
    public class Aluno
    {
        public Aluno(
            string numRa,
            string digitoRa,
            string siglaUfRa,
            string nomeAluno,
            string nomeMae,
            string dataNascimento)
        {

            outNumRa = numRa;
            outDigitoRA = digitoRa;
            outSiglaUFRA = siglaUfRa;
            outNomeAluno = nomeAluno;
            outNomeMae = nomeMae;
            outDataNascimento = dataNascimento;
        }
        public string outNumRa { set; get; }
        public string outDigitoRA { set; get; }
        public string outSiglaUFRA { set; get; }
        public string outNomeAluno { set; get; }
        public string outNomeMae { set; get; }
        public string outDataNascimento { set; get; }

        public static implicit operator string(Aluno aluno)
            => $"{aluno.outNumRa},{aluno.outDigitoRA},{aluno.outSiglaUFRA},{aluno.outNomeAluno},{aluno.outNomeMae},{aluno.outDataNascimento}";

      

    }
}
