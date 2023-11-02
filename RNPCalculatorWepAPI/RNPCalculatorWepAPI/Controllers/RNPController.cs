using Microsoft.AspNetCore.Mvc;
using RNPCalculatorWepAPI.Calculator;
using RNPCalculatorWepAPI.Models;

namespace RNPCalculatorWepAPI.Controllers
{
    [ApiController]
    [Route("rnp")]
    public class RNPController : ControllerBase
    {
        private Dictionary<int, Stack<double>> _stackDic;
        private readonly ILogger<RNPController> _logger;
        private ICalculator _calculator;

        public RNPController(ILogger<RNPController> logger)
        {
            _logger = logger;
            _stackDic = new Dictionary<int, Stack<double>>();
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

                    throw new NotImplementedException();
                }
                else 
                {
                    throw new NotImplementedException();
                }   
            }
            else 
            {
                throw new NotImplementedException();
            }
        }

        [HttpPost("stack")]
        public IActionResult CreateNewStack()
        {
            _stackDic.Add(_stackDic.Keys.Max() + 1, new Stack<double>());
            return Ok();
        }

        [HttpGet("stack")]
        public IEnumerable<Stack<double>> GetAllStacks()
        {
            return _stackDic.Values;
        }

        [HttpDelete("stack/{stack_id}")]
        public void DeleteStack(int stack_id)
        {
            if(_stackDic.TryGetValue(stack_id, out var stack))
            {
                _stackDic.Remove(stack_id);
            }
            else
            { 
                throw new NotImplementedException(); 
            }
        }

        [HttpPost("stack/{stack_id}")]
        public void PushValueIntoStack(int stack_id, [FromBody] ElementToPush body)
        {
            try
            {
                double d = Convert.ToDouble(body.Value);

                if (_stackDic.TryGetValue(stack_id, out var stack))
                {
                    stack.Push(d);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex) 
            {
                throw new NotImplementedException();
            }
        }

        [HttpGet("stack/{stack_id}")]
        public Stack<double> GetStack(int stack_id)
        {
            if (_stackDic.TryGetValue(stack_id, out var stack))
            {
                return stack;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}