using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ResumeParser.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var path = "/Users/borabilir/Projects/ResumeParser/ResumeParser.API/wwwroot/Profile1.pdf";
            string dir = "/Users/borabilir/Projects/ResumeParser/ResumeParser.API/wwwroot";

            //var resumeParser = new LinkedinResumeParser(path);

            List<Expression<Func<int, bool>>> expressions = new List<Expression<Func<int, bool>>>();

            Func<string, bool> rule = (x) => x.EndsWith("4");
            expressions.Add(x => rule.Invoke(x.ToString()));

            var result = expressions.First().Compile().Invoke(3342);

            //Console.WriteLine(resumeParser.Test());
            Console.ReadKey();
        }


    }
}
