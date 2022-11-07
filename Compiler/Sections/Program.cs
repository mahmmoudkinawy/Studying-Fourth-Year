using Lexica;

string text = File.ReadAllText("D:\\CPP\\Lexica\\Analyzer.cs");
string[] elements = text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
var analyzer = new Analyzer();
analyzer.Parse(text);

while (text != null)
{
    text = text.Trim(' ', '\t');
    string token = analyzer.GetNextLexicalAtom(ref text);
    Console.WriteLine(token);
}

foreach (string item in elements)
{
    analyzer.Parse(item);
}

Console.WriteLine(text);
Console.ReadKey();
