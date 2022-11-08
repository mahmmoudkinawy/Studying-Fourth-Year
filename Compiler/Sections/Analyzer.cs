namespace Lexica;
public class Analyzer
{
    private readonly string[] _keywords = Repository.Keywords;
    private readonly string[] _specialCharacters = Repository.SpecialCharacters;
    private readonly string[] _comments = Repository.Comments;
    private readonly string[] _operators = Repository.Operators;

    private string Parse(string item)
    {
        var str = new StringBuilder();

        if (int.TryParse(item, out int ok))
        {
            str.AppendLine($"Numerical: {item}");
            return str.ToString();
        }

        if (item.Equals("\r\n"))
        {
            return "\r\n";
        }

        if (CheckKeyword(item))
        {
            str.AppendLine($"Keyword: {item}");
            return str.ToString();
        }

        if (CheckOperator(item))
        {
            str.AppendLine($"Operator: {item}");
            return str.ToString();
        }

        if (CheckDelimiter(item))
        {
            str.AppendLine($"Special Characters: {item}");
            return str.ToString();
        }

        str.AppendLine($"Identifier: {item}");
        return str.ToString();
    }

    private bool CheckOperator(string str) => _operators.Contains(str);
    private bool CheckDelimiter(string str) => _specialCharacters.Contains(str);
    private bool CheckKeyword(string str) => _keywords.Contains(str);
    private bool CheckComments(string str) => _comments.Contains(str);

    public string GetTokens(ref string item)
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
                    int ok;
                    if (item[i] == '-' && Int32.TryParse(item[i + 1].ToString(), out ok))
                        continue;
                    token.Append(item[i]);
                    item = item.Remove(i, 1);
                    return Parse(token.ToString());
                }

            }
            else
                if (item[i] == '\'')
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
            else
                if (item[i] == '"')
            {
                int j = i + 1;
                while (item[j] != '"')
                    j++;
                token.Append("(literal constant, ").Append(item.Substring(i, j - i + 1)).Append(") ");
                item = item.Remove(i, j - i + 1);
                return token.ToString();
            }
            else
                if (item[i + 1].ToString().Equals(" ") || CheckDelimiter(item[i + 1].ToString()) == true || CheckOperator(item[i + 1].ToString()) == true)
            {
                if (Parse(item.Substring(0, i + 1)).Contains("numerical constant") && item[i + 1] == '.')
                {
                    int j = i + 2;
                    while (item[j].ToString().Equals(" ") == false && CheckDelimiter(item[j].ToString()) == false && CheckOperator(item[j].ToString()) == false)
                        j++;
                    int ok;
                    if (Int32.TryParse(item.Substring(i + 2, j - i - 2), out ok))
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
