﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Model_Converter
{
    class GeneralFunc
    {
 
        public byte[] ByteToNumber(byte[] bytes)
        {

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != '0' 
                 && bytes[i] != '1'
                 && bytes[i] != '2'
                 && bytes[i] != '3'
                 && bytes[i] != '4'
                 && bytes[i] != '5'
                 && bytes[i] != '6'
                 && bytes[i] != '7'
                 && bytes[i] != '8'
                 && bytes[i] != '9'
                 && bytes[i] != '.')
                    bytes[i] = 0x00;
            }
            return bytes;
        }

        public ulong GetEpochTime()
        {
            ulong epoch = Convert.ToUInt64((DateTime.Now - DateTime.MinValue).TotalMilliseconds);
            return epoch;
        }

       public int SearchBytes(byte[] Data, byte[] Find)
        {
            var len = Find.Length;
            var limit = Data.Length - len;
            for (var i = 0; i <= limit; i++)
            {
                var k = 0;
                for (; k < len; k++)
                {
                    if (Find[k] != Data[i + k]) break;
                }
                if (k == len) return i;
            }
            return -1;
        }

        public bool CreatePath(string path)
        {
            try 
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path)) 
                {
                    return true;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                return true;
            } 
            catch (Exception e) 
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return false;
            } 
        }

         public bool Contains(string searchPattern,string inputText)
          {
            string regexText = WildcardToRegex(searchPattern);
            Regex regex = new Regex(regexText , RegexOptions.IgnoreCase);

            if (regex.IsMatch(inputText ))
            {
                return true;
            }
                return false;
         }

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern)
                              .Replace(@"\*", ".*")
                              .Replace(@"\?", ".")
                       + "$";
        }


        public int GetZoneType(string line)
        {
            if (line == "[globe_data],")
                return 0;
            if (line == "[born_area],")
                return 1;
            if (line == "[revive_area],")
                return 2;
            if (line == "[gateway_area],")
                return 3;
            if (line == "[normal_area],")
                return 4;
            if (line == "[event_area],")
                return 5;
            if (line == "[position],")
                return 6;
            if (line == "[mob_patrol_mode],")
                return 7;
            if (line == "[biology],")
                return 8;
            if (line == "[environment_sound],")
                return 9;
            if (line == "[background_music],")
                return 10;
            if (line == "[color_correction],")
                return 11;
            if (line == "[event],")
                return 12;
            if (line == "[dynamic_block],")
                return 13;
            if (line == "[scene_music],")
                return 14;

            return 0;
        }

        public string CutLineComma(string line, int numtocut)
        {
            string returndata = string.Empty;
            if(!string.IsNullOrEmpty(line))
            {
                string[] substrings = line.Split(',');

                for(int i = 0; i < numtocut ;i++)
                {
                    returndata += substrings[i] + ",";
                }
            }

            return returndata;
        }

        public string[] CutLineVertical(string line)
        {
            string[] returndata = { "0", "0" };
            if (!string.IsNullOrEmpty(line))
            {
                returndata = line.Split('|');

            }

            return returndata;
        }

            int[,] Convertlist =
            {
                {85 , 46},
                {84 , 32},
                {83 , 32},
                {82 , 32},
                {81 , 32},
                {80 , 32},
                {79 , 32},
                {78 , 32},
                {75 , 32},
                {67 , 56},
                {66 , 55},
                {65 , 54},
                {64 , 53},
                {63 , 52},
                {62 , 51},
                {61 , 50},
                {60 , 49},
                {59 , 48},
                {58 , 47},
                {57 , 46},
                {56 , 45},
                {55 , 44},
                {54 , 43},
                {53 , 42},
                {52 , 41},
                {51 , 40},
                {50 , 39},
                {49 , 38},
                {48 , 37},
                {47 , 36},
                {46 , 35},
                {45 , 34},
                {44 , 33},
                {43 , 32},
                {42 , 31},
                {41 , 32},
                {39 , 28},
                {38 , 27},
                {37 , 26},
                {35 , 25},
                {34 , 24},
                {33 , 23},
                {32 , 22},
                {31 , 21}
            };

        public string DowngradeNumber(string number)
        {
            int num = 0;
            Int32.TryParse(number, out num);
            
            for (int i = 0; i < 44; i++)
            {
                if (Convertlist[i, 0] == num)
                {
                    num = Convertlist[i, 1];
                    break;
                }
            }

            if (num == 0)
                return number;
            else
                return num.ToString();
        }

        public List<string> TrimFileList(string filename, bool allowjumped)
        {
            string line = "";
            Encoding big5 = Encoding.GetEncoding("big5");
            System.IO.StreamReader file = new System.IO.StreamReader(filename, big5, true);

            int lignenumber = 1;
            int maxint = 0;
            StringBuilder text = new StringBuilder();
            bool istranslatefile = false;
            bool jumpline = false;
            bool dontcatch = false;
            bool BarreOnEnd = true;
            bool BarreOnEndBefore;
            int NumberofBarre;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Count(c => c == '|') < 3 && lignenumber == 1 && !filename.Contains("t_"))
                    break;

                if (filename.Contains("t_"))
                    istranslatefile = true;

                if (lignenumber != 1 && line.Contains('|') == true && maxint != 0)
                {
                    if (istranslatefile == true && lignenumber == 2)
                    {
                        line += "\r\n";
                        text.Append(line);
                        lignenumber++;
                        continue;
                    }

                    BarreOnEndBefore = BarreOnEnd;
                    NumberofBarre = line.Count(f => f == '|');
                    BarreOnEnd = line.EndsWith("|");

                    string[] returndata = CutLineVertical(line);
                    line = "";

                    int maxline = returndata.Count();
                    jumpline = false;

                    if (returndata[returndata.Count() - 1] != "")
                        jumpline = true;

                    for (int i = 0; i < maxline; i++)
                    {

                        if (i == 0)
                        {
                            int no = 0;
                            if (int.TryParse(returndata[i], out no))
                            {

                                //it's number OK no problem
                                line += "\r\n";
                                //maxline++;
                            }
                            else
                            {
                                if (allowjumped == false)
                                    returndata[i] = returndata[i].Replace("\r\n", string.Empty);
                                else
                                    returndata[i] = returndata[i].Replace("\r\n", "/r/n");

                                dontcatch = true;
                            }
                        }

                        // if(allowjumped == true && jumpline == true && i != 0)
                        //     line += returndata[i] + "£§$";
                        // else 

                        if (dontcatch == true && BarreOnEnd == true && i == maxline - 1)
                            line += returndata[i] + "|";
                        else if (dontcatch == true && i != maxline - 1)
                            line += returndata[i] + "|";
                        else if (dontcatch == true)
                            line += returndata[i];
                        else if (BarreOnEnd == false && i == maxline - 1)
                        {

                            if (allowjumped == false)
                                line += returndata[i] + "";
                            else
                                line += returndata[i] + "/r/n";
                        }
                        else
                            line += returndata[i] + "|";


                        //if (i == maxline - 1 && returndata.Count() > maxline)
                        //    maxline++;

                        dontcatch = false;
                    }
                }
                else if (lignenumber == 1 && istranslatefile == true)
                {

                    string[] returndata = CutLineVertical(line);
                    line = "";

                    if (returndata[0].Contains('V') || returndata[0] == "")
                        maxint = int.Parse(returndata[1]);
                    else
                        maxint = int.Parse(returndata[0]);

                    line += maxint + "|\r\n";
                }
                else if (lignenumber == 1 && istranslatefile == false)
                {

                    string[] returndata = CutLineVertical(line);
                    line = "";

                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 2)
                        {
                            maxint = int.Parse(returndata[i]) - 1;
                        }

                        line += returndata[i] + "|";
                    }
                }
                else if (line.Contains('|') == false && line != "" && allowjumped == true)
                {
                    line = line.Replace("\r\n", "/r/n");
                    line += "/r/n";

                }
                else if (line.Contains('|') == false && line != "" && allowjumped == false)
                {
                    line = line.Replace("\r\n", string.Empty);

                }
                else if (line.Contains('|') == false && line == "" && allowjumped == true)
                {
                    line = "/r/n";

                }

                string ligne;

                //if (allowjumped == true)
                //    ligne = line.Replace("\r\n", "£§$");
                //else
                ligne = line;

                text.Append(ligne);



                lignenumber++;
            }
            file.Close();
            string[] stringSeparators = new string[] { "\r\n" };
            return text.ToString().Split(stringSeparators, StringSplitOptions.None).ToList();
        }
    }
}
