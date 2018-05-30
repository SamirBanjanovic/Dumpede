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
            int skip = 0; // skip read based on number of characters we've copied
                          // in this case it's either 1 or 2; pending our state
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
                {// found a qualifying space (eg: testCase) -> we are at "t" nextChar is "C"
                 // insert separator
                 // insert chars: "t", %separator%, "C"
                 // yields: [t|e|s|t| |C|~]
                    hooomanReadable[insertIndex++] = currentChar;
                    hooomanReadable[insertIndex++] = separator;
                    hooomanReadable[insertIndex++] = nextChar;
                    skip = 2;
                }
                else if (char.IsUpper(currentChar) && i != 0 && (canPeek && char.IsLower(nextChar)))
                {// we've entered state of continous upper case letters (eg: HTTPClient) and have arrived
                 // at a position where the next char is lower. Thus, insert space before last upper case 
                 // insert chars: %separator%, "C", "l"
                 // yields: [H|T|T|P| |C|l|~]
                    hooomanReadable[insertIndex++] = separator;
                    hooomanReadable[insertIndex++] = currentChar;
                    hooomanReadable[insertIndex++] = nextChar;
                    skip = 2;
                }
                else
                { // we're in a continous lower or upper case state, keep copying character to new array                    
                    hooomanReadable[insertIndex++] = currentChar;
                    skip = 1;
                }
            }

            return new string(hooomanReadable).Substring(0, insertIndex);
        }
    }
}
