using Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Core.Util
{
    public static class FileCloud
    {
        public static string ArquivoDeDados = AppDomain.CurrentDomain.BaseDirectory + "DataBaseJson";

        public static Compactar Recuperar(Compactar compactar)
        {
            try
            {
                if (!File.Exists(ArquivoDeDados)) File.Create(ArquivoDeDados).Close();
                using (StreamReader s = File.OpenText(ArquivoDeDados))
                {
                    var file = File.ReadAllText(ArquivoDeDados);
                    return compactar = JsonConvert.DeserializeObject<Compactar>(file);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void Salvar(Compactar compactar)
        {
            try
            {
                using (StreamWriter file = File.CreateText(ArquivoDeDados))
                {
                    string strResultadoJson = JsonConvert.SerializeObject(compactar);
                    file.Write($"{strResultadoJson}");
                }
            }
            catch (Exception)
            {
            }
        }

    }
}