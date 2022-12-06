using System;
using System.IO;
using static testify.Dictionary;

namespace testify
{
    internal class App
    {       
        public void main()
        {
            Random random = new Random();

            //initial variables
            string fileName, folderName, path;
            short lineNumber = 1, testNumber, wordNumber;

            //initial lists
            List<Dictionary> dictList = new List<Dictionary>();
            List<int> used = new List<int>();
            List<string> outputList = new List<string>();

            //reading the name of the input file
            Console.Write("Please enter the filename: ");
            fileName = Console.ReadLine();

            //reading the words
            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            { 
                string currentWord = sr.ReadLine();
                Dictionary dict = new Dictionary(lineNumber, currentWord);
                dictList.Add(dict);
                lineNumber++;
            }
            sr.Close();

            //the amount of test, that needs to generated
            Console.Write("Number of tests: ");
            testNumber = Convert.ToInt16(sr.ReadLine());

            //the number of words in the test
            Console.Write("Number of words: ");
            wordNumber = Convert.ToInt16(sr.ReadLine());

            //choosing the destination folder, then set the path
            Console.Write("Please enter the destination folder name: ");
            folderName = Console.ReadLine();
            if (!String.IsNullOrEmpty(folderName)) path = folderName + "\\out";
            else path = "out";

            for (short i = 1; i <= testNumber; ++i)
            {
                //fill up the output list with random words
                for (short j = 1; j <= wordNumber; ++j)
                {
                    int n = random.Next(1, lineNumber + 1);
                    if (!used.Contains(n))
                    {
                        outputList.Add(dictList[n].GetWord());
                        used.Add(n);
                    }
                }

                //create an output file
                StreamWriter sw = new StreamWriter(path + i.ToString() + ".txt");
                int temp = 0;
                while (temp < wordNumber)
                {
                    sw.WriteLine(outputList[temp]);
                    temp++;
                }
                sw.Close();

                //clearing the lists before the next iteration
                used.Clear();
                outputList.Clear();
            }
            dictList.Clear();
                        
            Console.WriteLine("{0} tests are generated succesfully. Press any key to continue.", testNumber);
            Console.ReadKey();
        }
    }
}