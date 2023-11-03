using RNPCalculatorWepAPI.Models;
using System.Xml.Linq;

namespace RNPCalculatorWepAPI.Infra
{
    public interface IFileHandler
    {
        IEnumerable<KeyValueStack> ReadAll();
        void Save(IEnumerable<KeyValueStack> listOfElement);
    }
}
