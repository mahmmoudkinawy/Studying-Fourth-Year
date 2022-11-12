//var text = File.ReadAllText("D:\\CPP\\CSharpConsoleTest\\Program.cs");
var text = File.ReadAllText("C:\\Users\\mahmm\\Desktop\\Temp.txt");
//var text = "int x = 5 + b + x;";

var analyzer = new Analyzer();

Console.WriteLine("=============Tokens===============");
while (text is not null)
{
    text = text.Trim(' ', '\t');
    var token = analyzer.GetTokens(ref text);
    Console.Write(token);
}

Console.ReadKey();
