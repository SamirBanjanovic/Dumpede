using System;
using System.IO;
using System.Linq;

namespace Dumpede
{
    public static class CharacterCleanser
    {
        public static bool? ProcessInput(FileStream fs, StreamWriter sw, char valueWrapper = '"', char valueSeparator = ',', char newValue = ' ',char[] newLineDelimiters = null, char[] itemsToClean = null)
        {
            if (newLineDelimiters == null || newLineDelimiters.Length < 1)
                newLineDelimiters = new[] { '\r', '\n' };

            if (itemsToClean == null || itemsToClean.Length < 1)
                itemsToClean = new[] { '\r','\n' };

            // set default values for 
            // processing variables
            int index = 1;
            bool isEof = false;
            bool? finalStatus = null;
            bool toClean = false;
            bool error = false;
            char c;

            // set stream to beginning
            fs.Seek(0, SeekOrigin.Begin);

            // set initial state
            byte q = 0;

            while (isEof == false)
            {// begin reading the stream
                c = (char)fs.ReadByte();

                if ((q == 0 || q == 4) && c == valueWrapper)
                {// opening quotes of item
                    sw.Write(c);
                    q = 1;
                }
                else if ((q == 1 || q == 3) && c == valueWrapper)
                {// closing quote of item
                    sw.Write(c);
                    q = 2;
                }
                else if ((q == 1 || q == 3 || q == 4) && (toClean = itemsToClean.Contains(c)))
                {// skip ignore values; replace new lines with space...prevents string concatination
                    if (toClean)
                        sw.Write(newValue);

                    q = 3;
                }
                else if (q == 2 && c == valueSeparator)
                {// transition back to initial state
                    sw.Write(c);
                    q = 0;
                }
                else if (q == 2 && newLineDelimiters.Contains(c))
                {// reached end of the line

                    sw.Write(newLineDelimiters);

                    q = 4;
                    Console.WriteLine(++index);
                }
                else if (q == 1 || q == 2 || q == 3)
                {// valid character write to file
                    sw.Write(c);
                    q = 1;
                }
                else if ((error = q == 0) || c == char.MaxValue)
                {// exit reader...
                    if (error)
                        finalStatus = false;
                    else
                        finalStatus = true;

                    isEof = true;
                }
            }// end while

            return finalStatus;
        }// end function    
    }
}
