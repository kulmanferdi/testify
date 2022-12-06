using System;
using System.IO;
using System.Text;
using static testify.Dictionary;

namespace testify
{
    internal class Menu
    {
        private string inputFile, folderName, path;
        private short testNumber;
        private List<Dictionary> dictList = new List<Dictionary>();

        private void printMenu() 
        {
            Console.WriteLine("                 MENU");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Generate tests");
            Console.WriteLine("2. Check if word is in the input file");
            Console.WriteLine("3. Count a word in every output files");
            Console.WriteLine("--------------------------------------");            
        }

        private void init()
        {
            //reading the name of the input fileű
            StringBuilder pathString = new();
            pathString.Append("input\\");
            Console.Write("Please enter the name of the input file: ");
            pathString.Append(Console.ReadLine());
            pathString.Append(".txt");
            inputFile = pathString.ToString();
            

            //reading the words, every word is added once
            StreamReader sr = new StreamReader(inputFile);
            while (!sr.EndOfStream)
            {                
                if (!dictList.Any(Dictionary => Dictionary.GetWord() == sr.ReadLine()))
                {
                    Dictionary dict = new Dictionary(dictList.Count() + 1, sr.ReadLine());
                    dictList.Add(dict);
                }
            }
            sr.Close();

            //choosing the destination folder, then set the path
            Console.Write("Please enter the name of the destination folder: ");
            folderName = Console.ReadLine();
            if (!String.IsNullOrEmpty(folderName)) path = folderName + "\\out";
            else path = "out";
        }

        private void generateTests()
        {
            //declarations and initializing            
            short wordNumber;
            int randomNumber;
            
            List<string> outputList = new List<string>();

            Random random = new Random();           

            //the amount of tests, that needs to generated
            Console.Write("Number of tests: ");
            testNumber = Convert.ToInt16(Console.ReadLine());

            //the number of words in the test
            Console.Write("Number of words: ");
            wordNumber = Convert.ToInt16(Console.ReadLine());           

            //generating the different outputs
            for (short i = 1; i <= testNumber; ++i)
            {
                //fill up the output list with random words
                for (short j = 1; j <= wordNumber; ++j)
                {
                    randomNumber = random.Next(1, dictList.Count() + 1);
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
        }

        private void checkInInput()
        {
            Console.Write("Enter the searched word: ");
            string searchedWord = Console.ReadLine();
            if (dictList.Any(Dictionary => Dictionary.GetWord() == searchedWord))
            {
                Console.WriteLine("{0} is in the inputfile.", searchedWord);
            }
            else Console.WriteLine("{0} is not in the inputfile.", searchedWord);
        }        

        private void countInOutputs()
        {
            Console.Write("Enter the searched word: ");
            string searchedWord = Console.ReadLine();
            int count = 0;
            for (short i = 1; i <= testNumber; ++i)
            {
                StreamReader sr = new StreamReader(path + i.ToString()+".txt");
                while (!sr.EndOfStream)
                {                     
                    if(sr.ReadLine().Equals(searchedWord)) count++;
                }
                sr.Close();
            }
            Console.WriteLine("{0} output file contains the \"{1}\" word.", count, searchedWord);
        }

        public void Run()
        {            
            short selection;
            do
            {
                init();
                printMenu();
                do
                {
                    Console.Write("Select an option: ");
                    selection = Convert.ToInt16(Console.ReadLine());
                } while (selection < 0 | selection > 3);
                switch (selection)
                {
                    case 1: generateTests(); break;
                    case 2: checkInInput(); break;
                    case 3: countInOutputs(); break;
                }
            } while (selection != 0);
        }
    }
}