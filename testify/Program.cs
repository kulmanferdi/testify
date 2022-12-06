using System;
using System.IO;
using static testify.Dictionary;

namespace testify
{
    internal class App
    {       
        public void main(string[] args)
        {
            Random random = new Random();

            //initial variables
            string fileName = "";
            short lineNumber = 1;
            short testNumber = 0;
            short wordNumber = 0;

            //initial lists
            List<Dictionary> dictList = new List<Dictionary>();
            List<int> used = new List<int>();
            List<string> outputList = new List<string>();

            //reading the input filename/path
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

            Console.Write("Number of tests: ");
            testNumber = Convert.ToInt16(sr.ReadLine());

            Console.Write("Number of words: ");
            wordNumber = Convert.ToInt16(sr.ReadLine());

            for (int i = 1; i <= testNumber; ++i)
            {
                for (int j = 1; j <= wordNumber; ++j)
                {
                    int n = random.Next(1, lineNumber + 1);
                    if (!used.Contains(n))
                    {
                        outputList.Add(dictList[n].GetWord());
                        used.Add(n);
                    }
                }
                StreamWriter sw = new StreamWriter("out"+i+".txt");
                int temp = 0;
                while (temp < wordNumber)
                {
                    sw.WriteLine(outputList[temp]);
                    temp++;
                }
                used.Clear();
                outputList.Clear();
            }
            Console.WriteLine("{0} tests are generated succesfully.", testNumber);
            Console.ReadKey();
        }
    }
}