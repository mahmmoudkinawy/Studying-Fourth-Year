//string text = File.ReadAllText("D:\\CPP\\CSharpConsoleTest\\Program.cs");
string text = "int x = 5 + b + x;";

var analyzer = new Analyzer();

Console.WriteLine("=============Tokens===============");
while (text is not null)
{
    text = text.Trim(' ', '\t');
    string token = analyzer.GetTokens(ref text);
    Console.Write(token);
}

Console.ReadKey();
