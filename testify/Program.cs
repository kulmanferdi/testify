using System;
using System.IO;
using static testify.Dictionary;

namespace testify
{
    internal class App
    {       
        public void Main()
        {
            //declarations and initializing
            string fileName, folderName, path;
            short lineNumber = 0, testNumber, wordNumber;
            int randomNumber;
            
            List<Dictionary> dictList = new List<Dictionary>();
            List<string> outputList = new List<string>();

            Random random = new Random();

            //reading the name of the input file/path
            Console.Write("Please enter the filename: ");
            fileName = Console.ReadLine();

            //reading the words, every word is added once
            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            { 
                string currentWord = sr.ReadLine();
                if (!dictList.Any(Dictionary => Dictionary.GetWord() == currentWord))
                {
                    Dictionary dict = new Dictionary(lineNumber++, currentWord);
                    dictList.Add(dict);
                }
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

            //generating the different outputs
            for (short i = 1; i <= testNumber; ++i)
            {
                //fill up the output list with random words
                for (short j = 1; j <= wordNumber; ++j)
                {
                    randomNumber = random.Next(1, lineNumber + 1);
                    if (!outputList.Contains(dictList[randomNumber].GetWord()))
                    {
                        outputList.Add(dictList[randomNumber].GetWord());
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
                outputList.Clear();
            }
            dictList.Clear();
                        
            Console.WriteLine("{0} tests are generated succesfully. Press any key to continue.", testNumber);
            Console.ReadKey();
        }
    }
}