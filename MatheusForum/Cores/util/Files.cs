using Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Core.Util
{
    public static class Files
    {
        public static string ArquivoDeDados = AppDomain.CurrentDomain.BaseDirectory + "DataBaseJson";

        public static void Salvar(Arquivo arquivo)
        {
            try
            {
                using (StreamWriter file = File.CreateText(ArquivoDeDados))
                {
                    string strResultadoJson = JsonConvert.SerializeObject(arquivo);
                    file.Write($"{strResultadoJson}");
                }
            }
            catch
            {
                throw;
            }
        }

        public static Arquivo Puxar(Arquivo arquivo)
        {
            try
            {
                if (!File.Exists(ArquivoDeDados)) File.Create(ArquivoDeDados).Close();
                using (StreamReader s = File.OpenText(ArquivoDeDados))
                {
                    var file = File.ReadAllText(ArquivoDeDados);
                    return arquivo = JsonConvert.DeserializeObject<Arquivo>(file);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        

    }
}