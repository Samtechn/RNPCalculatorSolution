﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RNPCalculatorWepAPI.Models;
using RNPCalculatorWepAPI.Constantes;

namespace RNPCalculatorWepAPI.Infra
{
    public class FileHandler<T> : IFileHandler<T>
    {
        private readonly string _filePath;
        public FileHandler() 
        {
            _filePath = FilePathValues.jsonPath;
        }

        public IEnumerable<T> ReadAll()
        {
            IEnumerable<T> result;
            using (StreamReader streamReader = new StreamReader(_filePath, System.Text.Encoding.UTF8))
            {
                result = JsonConvert.DeserializeObject<List<T>>(streamReader.ReadToEnd(), new JsonSerializerSettings { }) ?? new List<T>();
                return result;
            }
        }

        public void Save(IEnumerable<T> listOfElement)
        {
            using (StreamWriter streamWriter = new StreamWriter(_filePath, false, System.Text.Encoding.UTF8))
            {
                streamWriter.Write(JsonConvert.SerializeObject(listOfElement));
            }
        }
    }
}
