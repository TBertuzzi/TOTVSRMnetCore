using System;
using System.Data;
using System.Threading.Tasks;

namespace TOTVSRMnetCore
{
    class Program
    {
        //usuário e senha do aplicativo RM. O mesmo utilizado para logar no sistema e que tenha permissão de   
        //acesso ao cadastro que deseja utilizar  
        static string _usuario = "";
        static string _senha = "";
        static string _codSistema = "G";
        static string _codColigada = "";

        // ajuste o nome o servidor e porta. Em caso de dúvidas, consulte o link abaixo:  
        // http://tdn.totvs.com/pages/releaseview.action?pageId=89620766    
        static string _url = "http://localhost:8053";

        //importante passar no contexto o mesmo código de usuário usado para logar no webservice  
        static string _contexto = $"CODSISTEMA={_codSistema};CODCOLIGADA={_codColigada};CODUSUARIO={_usuario}";

        public async static Task<Tuple<DataSet, string>> ReadRecord(string dataServerName, string filtro)
        {
            var dataclient = new DataClient(_url, _contexto, _usuario, _senha);

            //O ReadRecord retorna o registro da edição do cadastro RM respeitando a chave primária  
            var retorno = await dataclient.ReadRecord(dataServerName, filtro);

            return retorno;
        }

        static void Main(string[] args)
        {
            //Obter Movimento
            var retornoMovimento = ReadRecord("MOVMOVIMENTOTBCDATA", "codligada;movimento").Result;

            var datatableMovimento = retornoMovimento.Item1.Tables[0];
        }
    }
}
