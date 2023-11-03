using Microsoft.AspNetCore.Mvc;
using RNPCalculatorWepAPI.Calculator;
using RNPCalculatorWepAPI.Models;
using RNPCalculatorWepAPI.Infra;
using RNPCalculatorWepAPI.Constantes;

namespace RNPCalculatorWepAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("rnp")]
    public class RNPController : ControllerBase
    {
        private Dictionary<string, Stack<double>> _stackDic;
        private readonly ILogger<RNPController> _logger;
        private ICalculator _calculator;
        private IRepository _repository ;

        public RNPController(ILogger<RNPController> logger, IRepository repository, ICalculator calculator)
        {
            _logger = logger;
            _repository = repository;
            _calculator = calculator;
            _stackDic = _repository.GetAll();
        }

        [HttpGet("op")]
        public IEnumerable<char> GetOperands() 
        {
            return OperandValues.OperandsList;
        }

        [HttpPost("op/{op}/stack/{stack_id}")]
        public IActionResult ApplyOperandOnStack(char op, string stack_id) 
        {
            if (OperandValues.IsValidOperator(op) && _stackDic.TryGetValue(stack_id, out Stack<double> stack)) 
            {
                if (stack.Count >= 2)
                {
                    try 
                    {
                        double d1 = stack.Pop();
                        double d2 = stack.Pop();

                        stack.Push(_calculator.Calculate(op, d1, d2));

                        _repository.Save(_stackDic);

                        return new ObjectResult(stack.ToList());
                    }
                    catch (Exception ex) 
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, ex);
                    }
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
                key = _stackDic.Keys.Select(e => Convert.ToInt32(e)).Max() + 1;
            }

            _stackDic.Add(key.ToString(), new Stack<double>(new List<double> { 0}));
            _repository.Save(_stackDic);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("stack")]
        public IActionResult GetAllStacks()
        {
            return new ObjectResult(_stackDic.Values.ToList());
        }

        [HttpDelete("stack/{stack_id}")]
        public IActionResult DeleteStack(string stack_id)
        {
            if(_stackDic.TryGetValue(stack_id, out var stack))
            {
                _stackDic.Remove(stack_id);
                _repository.Save(_stackDic);
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }

        [HttpPost("stack/{stack_id}")]
        public IActionResult PushValueIntoStack(string stack_id, [FromBody] ElementToPush body)
        {
            try
            {
                double d = Convert.ToDouble(body.Value);

                if (_stackDic.TryGetValue(stack_id, out var stack))
                {
                    stack.Push(d);
                    _repository.Save(_stackDic);
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
        public IActionResult GetStack(string stack_id)
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