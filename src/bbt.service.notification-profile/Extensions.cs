
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class Extensions
{
    public static bool Evaluate(string expression, dynamic message)
    {
        ScriptOptions options = ScriptOptions.Default
         .AddReferences(
             Assembly.GetAssembly(typeof(System.Dynamic.DynamicObject)),
             Assembly.GetAssembly(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)),
             Assembly.GetAssembly(typeof(System.Dynamic.ExpandoObject)));


        var x = CSharpScript.EvaluateAsync(expression, options, new Globals { Message = message }).Result;

        return (bool)x;
    }

    public class Globals
    {
        public dynamic Message { get; set; }
    }

}


