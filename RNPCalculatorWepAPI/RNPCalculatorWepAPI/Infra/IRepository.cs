using RNPCalculatorWepAPI.Models;

namespace RNPCalculatorWepAPI.Infra
{
    public interface IRepository
    {
        Dictionary<string, Stack<double>> GetAll();
        void Save(Dictionary<string, Stack<double>> stackDic);
    }
}
