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
<�`�[��>
    <�����o�[��� attr=""abc"">
        <���O>����</���O>
        <�Z��><![CDATA[�k�C���D�y�s]]></�Z��>
        <�N��>28</�N��>
        <����>false</����>
        ������
    </�����o�[���>
    <�����o�[��� attr=""xyz"">
        <���O>�R�c</���O>
        <�Z��><![CDATA[�����s�k��]]></�Z��>
        <�N��>30</�N��>
        <����>true</����>
    </�����o�[���>
</�`�[��>";
//xml�t�@�C�����w�肷��
XDocument xml = Util.ParseXml(xmlString);
//�����o�[���̃^�O���̏����擾����
IEnumerable<XElement> infos = from item in xml.Root!.Elements("�����o�[���")
                              select item;
//�����o�[��񕪃��[�v���āA�R���\�[���ɕ\��
foreach (XElement info in infos)
{
    Console.Write(info.Element("���O").Value + @",");
    Console.Write(info.Element("�Z��").Value + @",");
    Console.Write(info.Element("�N��").Value.FromJson() + @",");
    Console.Write((bool)info.Element("����") + @",");
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
XElement data = new XElement("�����o�[���",
                             new XAttribute("����1", true),
                             new XElement("���O", "�c��"),
                             new XElement("�Z��", "���{���s"),
                             new XElement("�N��", "35"));
XElement list = new XElement("�`�[��", new XAttribute("����2", DateTime.Now));
list.Add(data);
XDocument yourResult = new XDocument(list);
Util.Print(yourResult, "(24)");
Util.Print(Util.FromObject(yourResult), "(25)");
Util.Print((long)"0".FromJson(), "(26)");
Util.Print((bool)"false".FromJson(), "(27)");
Util.Print((bool)"true".FromJson(), "(28)");
Console.WriteLine(Util.AsDateTime(yourResult.Root.Attribute("����2").Value));
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
