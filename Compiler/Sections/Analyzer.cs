using System.Text;

namespace Lexica;
public class Analyzer
{
    private readonly string[] _keywords = Array.Empty<string>();

    private readonly string[] _specialCharacters = Array.Empty<string>();

    private readonly string[] _comments = Array.Empty<string>();

    private readonly string[] _operators = Array.Empty<string>();

    public Analyzer()
    {
        _keywords = Repository.Keywords;
        _specialCharacters = Repository.SpecialCharacters;
        _comments = Repository.Comments;
        _operators = Repository.Operators;
    }

    public string Parse(string item)
    {
        var str = new StringBuilder();

        if (int.TryParse(item, out int ok))
        {
            str.Append("(numerical constant, " + item + ") ");
            return str.ToString();
        }

        if (item.Equals("\r\n"))
        {
            return "\r\n";
        }

        if (CheckKeyword(item))
        {
            str.Append("(keyword, " + item + ") ");
            return str.ToString();
        }

        if (CheckOperator(item))
        {
            str.Append("(operator, " + item + ") ");
            return str.ToString();
        }

        if (CheckDelimiter(item))
        {
            str.Append("(separator, " + item + ") ");
            return str.ToString();
        }

        if (SpecialCharacter(item))
        {
            str.Append("(Special Character, " + item + ") ");
            return str.ToString();
        }


        str.Append("(identifier, " + item + ") ");
        return str.ToString();
    }


    public bool CheckOperator(string str) => _operators.Contains(str);

    public bool SpecialCharacter(string str) => _specialCharacters.Contains(str);

    public bool CheckDelimiter(string str) => _comments.Contains(str);

    public bool CheckKeyword(string str) => _keywords.Contains(str);

    public bool CheckComments(string str) => _comments.Contains(str);

    public string GetNextLexicalAtom(ref string item)
    {
        var token = new StringBuilder();
        for (int i = 0; i < item.Length; i++)
        {
            if (CheckDelimiter(item[i].ToString()))
            {
                if (i + 1 < item.Length && CheckDelimiter(item.Substring(i, 2)))
                {
                    token.Append(item.Substring(i, 2));
                    item = item.Remove(i, 2);
                    return Parse(token.ToString());
                }
                else
                {
                    token.Append(item[i]);
                    item = item.Remove(i, 1);
                    return Parse(token.ToString());
                }

            }
            else if (CheckOperator(item[i].ToString()))
            {
                if (i + 1 < item.Length && (CheckOperator(item.Substring(i, 2))))
                    if (i + 2 < item.Length && CheckOperator(item.Substring(i, 3)))
                    {
                        token.Append(item.Substring(i, 3));
                        item = item.Remove(i, 3);
                        return Parse(token.ToString());
                    }
                    else
                    {
                        token.Append(item.Substring(i, 2));
                        item = item.Remove(i, 2);
                        return Parse(token.ToString());
                    }
                else if (CheckComments(item.Substring(i, 2)))
                {
                    if (item.Substring(i, 2).Equals("//"))
                    {
                        do
                        {
                            i++;
                        } while (item[i] != '\n');
                        item = item.Remove(0, i + 1);
                        item = item.Trim(' ', '\t', '\r', '\n');
                        i = -1;
                    }
                    else
                    {
                        do
                        {
                            i++;
                        } while (item.Substring(i, 2).Equals("*/") == false);
                        item = item.Remove(0, i + 2);
                        item = item.Trim(' ', '\t', '\r', '\n');
                        i = -1;
                    }

                }
                else
                {
                    if (item[i] == '-' && Int32.TryParse(item[i + 1].ToString(), out int ok))
                        continue;
                    token.Append(item[i]);
                    item = item.Remove(i, 1);
                    return Parse(token.ToString());
                }

            }
            else if (item[i] == '\'')
            {
                int j = i + 1;
                if (item[j] == '\\')
                    j += 2;
                else
                    j++;

                token.Append("(literal constant, ").Append(item.Substring(i, j - i + 1)).Append(") ");
                item = item.Remove(i, j - i + 1);
                return token.ToString();
            }
            else if (item[i] == '"')
            {
                int j = i + 1;
                while (item[j] != '"')
                    j++;
                token.Append("(literal constant, ").Append(item.Substring(i, j - i + 1)).Append(") ");
                item = item.Remove(i, j - i + 1);
                return token.ToString();
            }
            else if
                (item[i + 1].ToString().Equals(" ") ||
                CheckDelimiter(item[i + 1].ToString()) == true ||
                CheckOperator(item[i + 1].ToString()) == true)
            {
                if (Parse(item.Substring(0, i + 1)).Contains("numerical constant") && item[i + 1] == '.')
                {
                    int j = i + 2;
                    while (item[j].ToString().Equals(" ") == false && CheckDelimiter(item[j].ToString()) == false && CheckOperator(item[j].ToString()) == false)
                        j++;
                    if (int.TryParse(item.Substring(i + 2, j - i - 2), out int ok))
                    {
                        token.Append("(numerical constant, ").Append(item.Substring(0, j)).Append(") ");
                        item = item.Remove(0, j);
                        return token.ToString();
                    }

                }
                token.Append(item.Substring(0, i + 1));
                item = item.Remove(0, i + 1);
                return Parse(token.ToString());
            }


        }
        return string.Empty;
    }
}
