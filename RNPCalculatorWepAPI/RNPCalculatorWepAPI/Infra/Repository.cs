using Microsoft.Extensions.Caching.Memory;
using RNPCalculatorWepAPI.Models;

namespace RNPCalculatorWepAPI.Infra
{
    public class Repository : IRepository
    {
        private IFileHandler<KeyValueStack> _fileHandler;

        public Repository(IFileHandler<KeyValueStack> fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public Dictionary<string, Stack<double>> GetAll()
        {
            return _fileHandler.ReadAll().ToDictionary(e => e.Key, e => new Stack<double>(e.Value.Split(',').Select(e => Convert.ToDouble(e)).Reverse()));
        }

        public void Save(Dictionary<string, Stack<double>> stackDic) 
        {
            _fileHandler.Save(stackDic.Select(e => new KeyValueStack { Key = e.Key, Value = String.Join(',', e.Value.Reverse())}));
        }
    }
}
