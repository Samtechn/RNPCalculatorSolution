using Microsoft.Extensions.Caching.Memory;
using RNPCalculatorWepAPI.Models;

namespace RNPCalculatorWepAPI.Infra
{
    public class Repository : IRepository
    {
        private IFileHandler _fileHandler;

        public Repository(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public Dictionary<string, Stack<double>> GetAll()
        {
            return _fileHandler.ReadAll().ToDictionary(e => e.Key, e => new Stack<double>(e.Value.Split(',').Select(e => Convert.ToDouble(e))));
        }

        public void Save(Dictionary<string, Stack<double>> stackDic) 
        {
            _fileHandler.Save(stackDic.Select(e => new KeyValueStack { Key = e.Key, Value = String.Join(',', e.Value.Reverse())}));
        }
    }
}
