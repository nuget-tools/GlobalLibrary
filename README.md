# GlobalLibrary

```
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Linq;
using Global;
using NUnit.Framework;
using System.Linq;
using Newtonsoft.Json.Linq;

Console.WriteLine("Version: {0}", Environment.Version.ToString());
Util.Print(DateTime.Now);
{
    var json = Util.ToJson(new[] { 777, 888 });
    Util.Print(json); // to stdout
    Util.Log(json); // to stderr
    Assert.AreEqual("[777,888]", json);
}
{
    var jsonData = Util.FromObject(new[] { 777, 888 });
    Util.Print(jsonData, "(1)");
    Assert.AreEqual("[777,888]", Util.ToJson(jsonData));
    Util.Print(jsonData[0], "(2)");
    Assert.AreEqual(777, (int)jsonData[0]);
    jsonData = Util.FromObject(jsonData);
    Util.Print(jsonData, "(3)");
    Assert.AreEqual("[777,888]", Util.ToJson(jsonData));
    jsonData = Util.FromObject(new { a = 1, b = 2 });
    Util.Print(jsonData, "(4)");
    Assert.AreEqual("{\"a\":1,\"b\":2}", Util.ToJson(jsonData));
    Util.Print(jsonData.a, "(5)");
    Assert.AreEqual("1", Util.ToJson(jsonData.a));
    jsonData.a = 777;
    Util.Print(jsonData, "(6)");
    Assert.AreEqual("{\"a\":777,\"b\":2}", Util.ToJson(jsonData));
    Util.Print(Util.FullName(jsonData), "(7)");
    Assert.AreEqual("Newtonsoft.Json.Linq.JObject", Util.FullName(jsonData));
    Util.Print(jsonData.c, "(8)");
    Assert.AreEqual(null, jsonData.c);
    jsonData["c"] = 888;
    Util.Print(jsonData, "(9)");
    Assert.AreEqual("{\"a\":777,\"b\":2,\"c\":888}", Util.ToJson(jsonData));
}

{
    string json = @"{
  'channel': {
    'title': 'James Newton-King',
    'link': 'http://james.newtonking.com',
    'description': 'James Newton-King\'s blog.',
    'item': [
      {
        'title': 'Json.NET 1.3 + New license + Now on CodePlex',
        'description': 'Announcing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'CodePlex'
        ]
      },
      {
        'title': 'LINQ to JSON beta',
        'description': 'Announcing LINQ to JSON',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'LINQ'
        ]
      }
    ]
  }
}";
    JObject rss = (JObject)Util.FromJson(json);
    Console.WriteLine(rss);
    JArray categories = (JArray)rss["channel"]["item"][0]["categories"];
    Console.WriteLine(categories);
    Assert.AreEqual("[\"Json.NET\",\"CodePlex\"]", Util.ToJson(categories));
}
{
    var n = Util.FromJson("18446744073709551615");
    Util.Print(n, "(10)");
    Assert.AreEqual(18446744073709551615, (UInt64)n);
}
{
    dynamic flexible = new ExpandoObject();
    flexible.Int = 3;
    flexible.String = "hi";
    flexible.Deep = new ExpandoObject();
    flexible.Deep.Deeper = 777;
    var dictionary = (IDictionary<string, object>)flexible;
    dictionary.Add("Bool", false);
    Util.Print(flexible, "(11)");
    Assert.AreEqual("{\"Int\":3,\"String\":\"hi\",\"Deep\":{\"Deeper\":777},\"Bool\":false}", Util.ToJson(flexible));
}
Util.Print(Dirs.ProfilePath(), "(12)");
Util.Print(Dirs.ProfilePath("abc"), "(13)");
Util.Print(Dirs.DocumentsPath(), "(14)");
Util.Print(Dirs.DocumentsPath("abc"), "(15)");
var settings = Util.FromJson("{}");
settings.a = (settings.a != null ? settings.a : Util.FromJson("{}"));
settings.a.b = 123;
settings.a.c = 456;
Util.Print(settings, "(16)");
Assert.AreEqual("{\"a\":{\"b\":123,\"c\":456}}", Util.ToJson(settings));
var results = Util.FromJson(@"{ 'a': 123 /* my commnet */}");
Util.Print(results, "(17)");
var xmlString = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes"" ?>
<チーム>
    <メンバー情報 attr=""abc"">
        <名前>佐藤</名前>
        <住所><![CDATA[北海道札幌市]]></住所>
        <年齢>28</年齢>
        <既婚>false</既婚>
        あああ
    </メンバー情報>
    <メンバー情報 attr=""xyz"">
        <名前>山田</名前>
        <住所><![CDATA[東京都北区]]></住所>
        <年齢>30</年齢>
        <既婚>true</既婚>
    </メンバー情報>
</チーム>";
//xmlファイルを指定する
XDocument xml = Util.ParseXml(xmlString);
//メンバー情報のタグ内の情報を取得する
IEnumerable<XElement> infos = from item in xml.Root!.Elements("メンバー情報")
                              select item;
//メンバー情報分ループして、コンソールに表示
foreach (XElement info in infos)
{
    Console.Write(info.Element("名前").Value + @",");
    Console.Write(info.Element("住所").Value + @",");
    Console.Write(info.Element("年齢").Value.FromJson() + @",");
    Console.Write((bool)info.Element("既婚") + @",");
    Console.WriteLine(info.Attribute("attr").Value);
    Util.Print(info, "(18)");
}
Util.Print(xml.ToStringWithDeclaration(), "(19)");
Util.Print(infos, "(20)");
Util.Print(xml, "(21)");
{
    string json = @"{
  'channel': {
    'title': 'James Newton-King',
    'link': { '#text': 'http://james.newtonking.com', '@target': '_blank' },
    'description': { '#cdata-section': 'James Newton-King\'s blog.' },
    'item': [
      {
        'title': 'Json.NET 1.3 + New license + Now on CodePlex',
        'description': 'Announcing the release of Json.NET 1.3, the MIT license and the source on CodePlex',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'CodePlex'
        ]
      },
      {
        'title': 'LINQ to JSON beta',
        'description': 'Announcing LINQ to JSON',
        'link': 'http://james.newtonking.com/projects/json-net.aspx',
        'categories': [
          'Json.NET',
          'LINQ'
        ]
      }
    ]
  }
}";
    JObject rss = (JObject)Util.FromJson(json);
    var xml2 = Util.ToXml(rss);
    Util.Print(xml2, "(22)");
    Util.Print(Util.FromXml(xml2), "(23)");
}
XElement data = new XElement("メンバー情報",
                             new XAttribute("属性1", true),
                             new XElement("名前", "田中"),
                             new XElement("住所", "大阪府大阪市"),
                             new XElement("年齢", "35"));
XElement list = new XElement("チーム", new XAttribute("属性2", DateTime.Now));
list.Add(data);
XDocument yourResult = new XDocument(list);
Util.Print(yourResult, "(24)");
Util.Print(Util.FromObject(yourResult), "(25)");
Util.Print((long)"0".FromJson(), "(26)");
Util.Print((bool)"false".FromJson(), "(27)");
Util.Print((bool)"true".FromJson(), "(28)");
Console.WriteLine(Util.AsDateTime(yourResult.Root.Attribute("属性2").Value));
Util.Print(DateTime.Now, "(29)");
Util.Print(Util.AsDateTime("2022-10-29T03:34:11.1741296+09:00"), "(30)");
var z = Util.FromJson("\"2022-10-29T03:34:11.1741296+09:00\"");
var zs = (string)z;
Util.Print(zs, "(31)");
Util.Print(Util.AsDateTime(z), "(32)");
var zz1 = Util.FromObject(new { t = DateTime.Now });
Util.Print(zz1, "(33)");
var zz2 = Util.FromObject(new { a = new[] { DateTime.Now } });
Util.Print(zz2, "(34)");
var zz3 = Util.FromObject(DateTime.Now);
Util.Print(zz3, "(35)");
Util.Print((string)zz3, "(36)");
Util.Print(Util.AsDateTime(zz3), "(37)");
Util.Print(Util.FullName(zz3), "(38)");
Util.Print(Util.FullName(null), "(39)");

Util.Print(Util.FreeTcpPort(), "(40)");
Util.Print(Util.FreeTcpPort(), "(41)");

Util.Print(Dirs.AppDataFolderPath(), "(42)");
Util.Print(Dirs.AppDataFolderPath("appName"), "(43)");
Util.Print(Dirs.AppDataFolderPath("orgName", "appName"), "(44)");
```

# JavaScript

```
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
```

# JSON

```
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

```
