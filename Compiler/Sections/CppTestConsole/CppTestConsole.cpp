#include <iostream>
#include <stdbool.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

bool isValidDelimiter(char ch) {
	char delimiters[16] = { ' ', '+' , '-' ,'*' ,'/' ,',' ,';' ,'>','<','=','(' ,')','[',']','{','}' };

	for (int i = 0; i < 16; i++)
	{
		if (delimiters[i] == ch) {
			return true;
		}
	}

	return false;
}

bool isValidOperator(char ch) {
	char operators[7] = { '+' , '-', '*','<' ,'>','/', '=' };

	for (int i = 0; i < 7; i++)
	{
		if (operators[i] == ch) {
			return true;
		}
	}

	return false;
}

bool isvalidIdentifier(char* str) {
	char identifiers[10] = { '0' ,'1','2','3','4','5','6','7','8','9' };

	for (int i = 0; i < 10; i++)
	{
		if (identifiers[i] != str[0]) {
			return true;
		}
	}

	return false;
}

bool isValidKeyword(char* str)
{
	const char* keywords[22] = {
		"if",
		"else",
		"while",
		"do",
		"break",
		"continue",
		"int",
		"double",
		"float",
		"return",
		"char",
		"case",
		"void",
		"static",
		"struct",
		"goto",
		"sizeof",
		"long",
		"short",
		"typedef",
		"unsigned",
		"switch"
	};

	for (int i = 0; i < 22; i++)
	{
		if (!strcmp(str, keywords[i])) {
			return true;
		}
	}

	return false;
}

bool isValidInteger(char* str) {
	char intengers[10] = { '0', '1','2','3','4','5','6','7','8','9' };
	int len = strlen(str);

	if (len == 0)
	{
		return (false);
	}

	for (int i = 0; i < len; i++)
	{
		if (str[i] != intengers[i]) {
			return false;
		}
	}

	return true;
}

bool isRealNumber(char* str) {
	char numbers[11] = { '0', '1','2','3','4','5','6','7','8','9','.' };
	int len = strlen(str);
	bool isDecimal = false;

	if (len == 0)
	{
		return false;
	}

	for (int i = 0; i < len; i++) {
		if (str[i] != numbers[i]) {
			return false;
		}

		if (str[i] == '.') {
			isDecimal = true;
		}
	}
	return isDecimal;
}

char* subString(char* str, int left, int right) {
	char* subStr = (char*)malloc(sizeof(char) * (static_cast<unsigned long long>(right) - left + 2));

	for (int i = left; i <= right; i++) {
		subStr[i - left] = str[i];
	}

	subStr[right - left + 1] = '\0';

	return subStr;
}

//char* isSpecialCharacter(char str[]) {
//	char returnedArray[100];
//
//	for (int i = 0; i < strlen(str); ++i) {
//
//		if (str[i] == '!' || str[i] == '@' || str[i] == '#' || str[i] == '$'
//			|| str[i] == '%' || str[i] == '^' || str[i] == '&' || str[i] == '*'
//			|| str[i] == '(' || str[i] == ')' || str[i] == '-' || str[i] == '{'
//			|| str[i] == '}' || str[i] == '[' || str[i] == ']' || str[i] == ':'
//			|| str[i] == ';' || str[i] == '"' || str[i] == '\'' || str[i] == '<'
//			|| str[i] == '>' || str[i] == '.' || str[i] == '/' || str[i] == '?'
//			|| str[i] == '~' || str[i] == '`')
//		{
//			printf("Code: %c\n", str[i]);
//			return returnedArray[0];
//		}
//	}
//
//	return false;
//}

void discoverTokens(char* str) {
	int left = 0;
	int right = 0;
	int length = strlen(str);

	while (right <= length && left <= right) {

		if (isValidDelimiter(str[right]) == false) {
			right++;
		}

		if (isValidDelimiter(str[right]) == true && left == right) {
			if (isValidOperator(str[right]) == true) {
				printf("Valid operator : %c\n", str[right]);
			}

			right++;
			left = right;
		}
		else if (isValidDelimiter(str[right]) && left != right || (right == length && left != right)) {
			char* subStr = subString(str, left, right - 1);

			if (isValidKeyword(subStr)) {
				printf("Keyword: %s\n", subStr);
			}

			/*if (isSpecialCharacter(str)) {

			}*/

			else if (isValidInteger(subStr) || isRealNumber(subStr)) {
				printf("Numeric Constant: %s\n", subStr);
			}

			else if (isvalidIdentifier(subStr) && !isValidDelimiter(str[right - 1])) {
				printf("Valid Identifier: %s\n", subStr);
			}

			else if (!isvalidIdentifier(subStr) && !isValidDelimiter(str[right - 1])) {
				printf("Invalid Identifier: %s\n", subStr);
			}

			left = right;
		}
	}
	return;
}

int main() {
	char expression[] = "for(int i = 0 ; i < 5; i++) { if(x > y) {return 0;} }";

	printf("You Expression is : '%s'\n", expression);

	printf("=======================Tokens======================\n");

	discoverTokens(expression);
	printf("===================================================\n");
}