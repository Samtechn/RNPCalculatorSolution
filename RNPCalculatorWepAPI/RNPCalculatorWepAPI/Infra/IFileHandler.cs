using RNPCalculatorWepAPI.Models;
using System.Xml.Linq;

namespace RNPCalculatorWepAPI.Infra
{
    public interface IFileHandler<T>
    {
        IEnumerable<T> ReadAll();
        void Save(IEnumerable<T> listOfElement);
    }
}
