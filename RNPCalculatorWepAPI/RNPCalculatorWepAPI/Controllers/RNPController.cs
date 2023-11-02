using Microsoft.AspNetCore.Mvc;
using RNPCalculatorWepAPI.Calculator;
using RNPCalculatorWepAPI.Models;
using Newtonsoft.Json;

namespace RNPCalculatorWepAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("rnp")]
    public class RNPController : ControllerBase
    {
        private Dictionary<int, Stack<double>> _stackDic;
        private readonly ILogger<RNPController> _logger;
        private ICalculator _calculator;

        public RNPController(ILogger<RNPController> logger)
        {
            _logger = logger;
            _stackDic = BuildStackDictionary(Stacks);
        }

        public static List<KeyValueStack> Stacks
        {
            get
            {
                using (StreamReader streamReader = new StreamReader(@".\BackUpFile\Stacks.json", System.Text.Encoding.UTF8))
                {
                    return JsonConvert.DeserializeObject<List<KeyValueStack>>(streamReader.ReadToEnd(), new JsonSerializerSettings { });
                }
            }

            set
            {
                using (StreamWriter streamWriter = new StreamWriter(@".\BackUpFile\Stacks.json", false, System.Text.Encoding.UTF8))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(value));
                }
            }
        }

        private Dictionary<int, Stack<double>> BuildStackDictionary(List<KeyValueStack> list)
        {
            var result = new Dictionary<int, Stack<double>>();

            foreach(var e in list)
            {
                //result.Add(e.Key, new Stack<double>(e.Value.Split(',').ToList()));
            }

            return result;
        }

        private List<KeyValueStack> TransformDictionary(Dictionary<int, Stack<double>> dic)
        {
            var result = new List<KeyValueStack>();

            foreach (var e in dic)
            {
                //result.Add(new KeyValueStack { Key = e.Key, Value = e.Value.ToList()});
            }

            return result;
        }

        private void WriteToJson(Dictionary<int, Stack<double>> dic)
        {
            Stacks = TransformDictionary(dic);
        }

        private Dictionary<int, Stack<double>> ReadFromJson()
        {
            return BuildStackDictionary(Stacks);
        }

        [HttpGet("op")]
        public IEnumerable<char> GetOperands() 
        {
            return Operands.OperandsList;
        }


        [HttpPost("op/{op}/stack/{stack_id}")]
        public IActionResult ApplyOperandOnStack(char op, int stack_id) 
        {
            if (_stackDic.TryGetValue(stack_id, out Stack<double> stack)) 
            {
                if (stack.Count >= 2)
                {
                    double d1 = stack.Pop();
                    double d2 = stack.Pop();

                    stack.Push(_calculator.Calculate(op, d1, d2));

                    WriteToJson(_stackDic);

                    return new ObjectResult(stack.ToList());
                }
                else 
                {
                    return NoContent();
                }   
            }
            else 
            {
                return NoContent();
            }
        }

        [HttpPost("stack")]
        public IActionResult CreateNewStack()
        {
            int key;
            if (_stackDic.Count == 0)
            {
                key = 1;
            }
            else 
            { 
                key = _stackDic.Keys.Max() + 1;
            }

            _stackDic.Add(key, new Stack<double>());
            WriteToJson(_stackDic);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("stack")]
        public IActionResult GetAllStacks()
        {
            return new ObjectResult(_stackDic.Values.ToList());
        }

        [HttpDelete("stack/{stack_id}")]
        public IActionResult DeleteStack(int stack_id)
        {
            if(_stackDic.TryGetValue(stack_id, out var stack))
            {
                _stackDic.Remove(stack_id);
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }

        [HttpPost("stack/{stack_id}")]
        public IActionResult PushValueIntoStack(int stack_id, [FromBody] ElementToPush body)
        {
            try
            {
                double d = Convert.ToDouble(body.Value);

                if (_stackDic.TryGetValue(stack_id, out var stack))
                {
                    stack.Push(d);
                    WriteToJson(_stackDic);
                    return new ObjectResult(stack.ToList());
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpGet("stack/{stack_id}")]
        public IActionResult GetStack(int stack_id)
        {
            if (_stackDic.TryGetValue(stack_id, out var stack))
            {
                return new ObjectResult(stack.ToList());
            }
            else
            {
                return NoContent();
            }
        }
    }
}