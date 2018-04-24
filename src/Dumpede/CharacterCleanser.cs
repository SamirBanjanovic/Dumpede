using System;
using System.IO;
using System.Linq;

namespace Dumpede
{
    public static class CharacterCleanser
    {
        public static int ProcessInput(FileStream fs, StreamWriter sw, char[] newLineDelimiters = null, char[] itemsToClean = null)
        {
            newLineDelimiters = new[] { '\r', '\n' };
            itemsToClean = new[] { '\"' };

            // set default values for 
            // processing variables
            var index = 1;
            var isEof = false;
            var finalStatus = 0;
            var toClean = false;
            var error = false;
            var c = '\0';

            // set stream to beginning
            fs.Seek(0, SeekOrigin.Begin);

            // set initial state
            var q = 0;

            while (isEof == false)
            {// begin reading the stream
                c = (char)fs.ReadByte();

                if ((q == 0 || q == 4) && c == '"')
                {// opening quotes of item
                    sw.Write(c);
                    q = 1;
                }
                else if ((q == 1 || q == 3) && c == '"')
                {// closing quote of item
                    sw.Write(c);
                    q = 2;
                }
                else if ((q == 1 || q == 3 || q == 4) && (toClean = itemsToClean.Contains(c)))
                {// skip ignore values; replace new lines with space...prevents string concatination
                    if (toClean)
                        sw.Write(" ");

                    q = 3;
                }
                else if (q == 2 && c == ',')
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
                        finalStatus = -1;
                    else
                        finalStatus = 1;

                    isEof = true;
                }
            }// end while

            return finalStatus;
        }// end function    
    }
}
