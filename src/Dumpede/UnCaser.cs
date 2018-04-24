using System;
using System.Collections.Generic;
using System.Text;

namespace Dumpede
{
    public static class UnCaser
    {
        public static string UnCamelPascal(string input, char separator = ' ')
        {
            char currentChar;
            char nextChar;
            bool canPeek = false;
            int skip = 0;
            int insertIndex = 0;
            int inputLength = input.Length;

            // double size assume, we have a space after each letter
            char[] hooomanReadable = new char[inputLength * 2];

            for (int i = 0; i < inputLength; i += skip)
            {
                currentChar = input[i];
                canPeek = (i + 1) != inputLength;
                nextChar = canPeek ? input[i + 1] : currentChar;

                if (char.IsLower(currentChar) && (canPeek && char.IsUpper(nextChar)))
                {
                    hooomanReadable[insertIndex++] = currentChar;
                    hooomanReadable[insertIndex++] = separator;
                    hooomanReadable[insertIndex++] = nextChar;
                    skip = 2;
                }
                else if (char.IsUpper(currentChar) && i != 0 && (canPeek && char.IsLower(nextChar)))
                {
                    hooomanReadable[insertIndex++] = separator;
                    hooomanReadable[insertIndex++] = currentChar;
                    hooomanReadable[insertIndex++] = nextChar;
                    skip = 2;
                }
                else
                {
                    hooomanReadable[insertIndex++] = currentChar;
                    skip = 1;
                }
            }

            return new string(hooomanReadable).Substring(0, insertIndex);
        }
    }
}
