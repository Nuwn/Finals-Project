using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Finals_API.Models;

public partial class Quiz
{
    public int Id { get; set; }
    public string Operators { get; set; } = null!;

    public string Results { get; set; } = null!;

    public string Numbers { get; set; } = null!;
    public DateTime Date { get; set; }
}

public static class QuizGenerator {  
    #region Generation


    private static readonly Random _random = new();
    private static readonly int[] _range = { 0, 9 };
    private static readonly char[] _operators = { '+', '-', 'x' };

    public static Quiz GenerateNew(Quiz quiz)
    {
        // generate numbers
        var numbers = Enumerable.Range(0, 9)
                .Select(i => _random.Next(_range[0], _range[1])).ToArray();

        // generate operators
        var operators = Enumerable.Range(0, 12)
                .Select(i => _operators[_random.Next(0, 3)]).ToArray();

        int[] results = new int[6]{
            Calculate(numbers.Take(3).ToArray(), operators.Take(2).ToArray()),
            Calculate(numbers.Skip(3).Take(3).ToArray(), operators.Skip(5).Take(2).ToArray()),
            Calculate(numbers.Skip(6).Take(3).ToArray(), operators.Skip(10).Take(2).ToArray()),

            Calculate(numbers.Where((num, index) =>  index == 0 || index == 3 || index == 6).ToArray(),
                      operators.Where((op, index) =>  index == 2 || index == 7).ToArray()),

            Calculate(numbers.Where((num, index) =>  index == 1 || index == 4 || index == 7).ToArray(),
                      operators.Where((op, index) =>  index == 3 || index == 8).ToArray()),

            Calculate(numbers.Where((num, index) =>  index == 2 || index == 5 || index == 8).ToArray(),
                      operators.Where((op, index) =>  index == 4 || index == 9).ToArray())
        };

        int[] randomNumbers = Enumerable.Range(0, 3)
                .Select(i => _random.Next(0, 9)).ToArray();

        int num1 = numbers[randomNumbers[0]];
        int num2 = numbers[randomNumbers[1]];
        int num3 = numbers[randomNumbers[2]];

        // Create a JSON object with the three random numbers
        Dictionary<string, int> jsonObject = new() { 
            { randomNumbers[0].ToString(), num1 }, 
            { randomNumbers[1].ToString(), num2 }, 
            { randomNumbers[2].ToString(), num3 } 
        };

        quiz.Numbers = JsonConvert.SerializeObject(jsonObject);
        quiz.Operators = string.Join(',', operators);
        quiz.Results = string.Join(',', results);

        quiz.Date = DateTime.Now;

        return quiz;
    }

    public static int Calculate(IEnumerable<int> numbers, IEnumerable<char> operators)
    {
        // 3 ints 2 operators

        int result;

        bool skip = HasLowerPrecedence(operators.ElementAt(0), operators.ElementAt(1));

        // perform first calc, if the last operator is > first, calc next set
        result = (!skip) 
            ? PerformCalculation(numbers.ElementAt(0), numbers.ElementAt(1), operators.ElementAt(0))
            : PerformCalculation(numbers.ElementAt(1), numbers.ElementAt(2), operators.ElementAt(1));

        // perform the remaining calc, if we skipped first, we add or sub the first number, if we didnt skip calculate last number and op.
        result = (!skip)
            ? PerformCalculation(result, numbers.ElementAt(2), operators.ElementAt(1))
            : PerformCalculation(result, numbers.ElementAt(0), operators.ElementAt(0));

        return result;
    }

    private static int PerformCalculation(int left, int right, char op)
    {
        return op switch
        {
            '+' => left + right,
            '-' => left - right,
            'x' => left * right,
            _ => throw new ArgumentException("Invalid operator: " + op),
        };
    }

    private static bool HasLowerPrecedence(char op1, char op2)
    {
        if (op1 == 'x' || op1 == '/')
            return false;

        return (op2 == 'x' || op2 == '/');
    }

    #endregion
}
