using System;
using Global;
using NUnit.Framework;

Console.WriteLine("Version: {0}", Environment.Version.ToString());

dynamic ary = Util.FromJson("[]");
Util.Print(ary, "(1)");
ary.Add(111);
ary.Add(222);
Util.Print(ary, "(2)");
Util.Print(ary.Count, "(3)");
for (int i = 0; i < ary.Count; i++)
{
    Util.Print(ary[i], "(4)");
}
dynamic obj = Util.FromJson("{}");
Util.Print(obj, "(5)");
obj["a"] = 111;
obj["b"] = 222;
Util.Print(obj, "(6)");
foreach(var pair in obj)
{
    Util.Print(pair, "(7)");
    Util.Print(pair.Name, "(8)");
    Util.Print(pair.Value, "(9)");
}
Util.Print(obj.ContainsKey("a"), "(10)");
Util.Print(obj.ContainsKey("c"), "(11)");
obj["c"] = ary;
Util.Print(obj, "(12)");
Assert.AreEqual(222, (int)obj["c"][1]);
var obj2 = Util.ToObject(obj);
Util.Print(obj2, "(13)");
var engine = JavaScript.CreateEngine();
engine.SetValue("obj2", obj2);
engine.Execute("""
               let ary = obj2.c;
               for (let i=0; i<ary.length; i++)
               {
                 ary[i]++;
               }
               """);
Util.Print(obj2, "(14)");
var ary2 = Util.FromJson("['a', 'b', null]");
Util.Print(Util.FullName(ary2[2]), "(15)");
var ary3 = Util.ToObject(ary2);
Util.Print(Util.FullName(ary3[2]), "(16)");
Util.Print(obj2["a"], "(17)");
Util.Print(obj2["b"], "(18)");
Util.Print(obj2["c"], "(19)");
Util.Print(obj2["c"][1], "(20)");

