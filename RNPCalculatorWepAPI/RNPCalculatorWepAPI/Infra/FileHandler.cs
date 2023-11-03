using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RNPCalculatorWepAPI.Models;
using RNPCalculatorWepAPI.Constantes;

namespace RNPCalculatorWepAPI.Infra
{
    public class FileHandler : IFileHandler
    {
        private readonly string _filePath;
        public FileHandler() 
        {
            _filePath = ConstantesValues.path;
        }

        public IEnumerable<KeyValueStack> ReadAll()
        {
            using (StreamReader streamReader = new StreamReader(_filePath, System.Text.Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<List<KeyValueStack>>(streamReader.ReadToEnd(), new JsonSerializerSettings { });
            }
        }

        public void Save(IEnumerable<KeyValueStack> listOfElement)
        {
            using (StreamWriter streamWriter = new StreamWriter(_filePath, false, System.Text.Encoding.UTF8))
            {
                streamWriter.Write(JsonConvert.SerializeObject(listOfElement));
            }
        }
    }
}
