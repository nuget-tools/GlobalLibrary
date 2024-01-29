#pragma warning disable NUnit2005
#pragma warning disable CS0649
using System;
using Global;
using NUnit.Framework;


Console.WriteLine("Version: {0}", Environment.Version.ToString());

{
    Util.Print("Sample(01)");
    var engine = JavaScript.CreateEngine();
    engine.Execute(@"
    print('Hello World to stdout');
    log('Hello World to stderr');
");
}

{
    Util.Print("Sample(02)");
    dynamic square = JavaScript.CreateEngine()
        .SetValue("x", 3) // define a new variable
        .Evaluate("x * x") // evaluate a statement
        .ToNewton(); // converts the value to .NET
    Util.Print(square, "square");
}

{
    Util.Print("Sample(03)");
    var engine = JavaScript.CreateEngine();
    var list = engine.Evaluate(@"
    let list = [];
    for (let i=0; i<3; i++)
    {
      let x = i + 1;
      log(x, 'x');
      list.push(x);
    }
    print(list, 'list(1)');
    list").ToNewton();
    Util.Print(Util.FullName(list));
    Util.Print(list, "list(2)");
}

{
    Util.Print("Sample(04)");
    var engine = JavaScript.CreateEngine();
    var dict = engine.Evaluate(@"
    let dict = {};
    dict['a'] = 11;
    dict['b'] = 22;
    dict['c'] = null;
    print(dict, 'dict(1)');
    dict").ToNewton();
    Util.Print(Util.FullName(dict));
    Util.Print(dict, "dict(2)");
}

{
    Util.Print("Sample(05)");
    var p = new Person
    {
        Name = "Mickey Mouse"
    };
    var engine = JavaScript.CreateEngine()
        .SetValue("p", p)
        .Execute("p.Name = 'Minnie'");
    Assert.AreEqual("Minnie", p.Name);
}

{
    Util.Print("Sample(06)");
    var engine = JavaScript.CreateEngine();
    var add = engine
        .Execute("function add(a, b) { return a + b; }")
        .GetValue("add");
    dynamic x = engine.Invoke(add, 1, 2).ToNewton(); // -> 3
    Util.Print(x, "x");
}

{
    Util.Print("Sample(07)");
    var engine = JavaScript.CreateEngine()
       .Execute("function add(a, b) { return a + b; }");
    dynamic y = engine.Invoke("add", 1, 2).ToNewton(); // -> 3
    Util.Print(y, "y");
}

{
    Util.Print("Sample(08)");
    var engine = JavaScript.CreateEngine();
    engine.Execute("""
                   var file = new System.IO.StreamWriter('log.txt');
                   file.WriteLine('Hello World !');
                   file.Dispose();
                   """);
}

{
    Util.Print("Sample(09)");
    var engine = JavaScript.CreateEngine(typeof(Global.Util).Assembly);
    engine.Execute("""
                   var Global = importNamespace('Global');
                   var Util = Global.Util;
                   Util.Print(777-1, "Result");
                   """);
}

{
    Util.Print("Sample(10)");
    var engine = JavaScript.CreateEngine(typeof(Global.Util).Assembly);
    engine.SetValue("MyMath", GScript.Runtime.Interop.TypeReference.CreateTypeReference(engine, typeof(MyMath)));
    engine.Execute("""
                   var Global = importNamespace('Global');
                   var Util = Global.Util;
                   var o = new MyMath();
                   Util.Print(o.Add2(111, 222));
                   """);
}

{
    Util.Print("Sample(11)");
    var engine = JavaScript.CreateEngine(typeof(Global.Util).Assembly);
    engine.Execute("""
                   var Global = importNamespace('Global');
                   var Util = Global.Util;
                   var ListOfString = System.Collections.Generic.List(System.String);
                   var list = new ListOfString();
                   list.Add('foo');
                   list.Add(1); // automatically converted to String
                   Util.Print(list.Count); // 2
                   """);
}

class Person
{
    public string Name;
    public int age;
};

class MyMath
{
    public int Add2(int a, int b)
    {
        return a + b;
    }
}
