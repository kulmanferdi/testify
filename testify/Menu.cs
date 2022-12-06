using System;
using System.IO;
using static testify.Dictionary;

namespace testify
{
    internal class Menu
    {
        private string fileName, folderName, path;
        private List<Dictionary> dictList = new List<Dictionary>();

        private void printMenu() 
        {
            Console.WriteLine("                 MENU");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("1. Generate tests");
            Console.WriteLine("2. Check if word is in the input file");
            Console.WriteLine("3. Check if word is in the output file");
            Console.WriteLine("4. Count a word in every output files");
            Console.WriteLine("--------------------------------------");            
        }

        private void generateTests()
        {
            //declarations and initializing            
            short testNumber, wordNumber;
            int randomNumber;
            
            List<string> outputList = new List<string>();

            Random random = new Random();           

            //the amount of tests, that needs to generated
            Console.Write("Number of tests: ");
            testNumber = Convert.ToInt16(Console.ReadLine());

            //the number of words in the test
            Console.Write("Number of words: ");
            wordNumber = Convert.ToInt16(Console.ReadLine());

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
        private void init()
        {
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
                    Dictionary dict = new Dictionary(dictList.Count() + 1, currentWord);
                    dictList.Add(dict);
                }
            }
            sr.Close();
        }
        public void Run()
        {            
            short selection;
            init();
            printMenu();
            do
            {
                Console.Write("Select an option: ");
                selection = Convert.ToInt16(Console.ReadLine());
            }while(selection < 0 | selection > 4);
            switch (selection)
            {
                case 1:generateTests();break;
                case 2: break;
                case 3: break;
                case 4: break;
                case 0: break;
                default: break;
            }
        }
    }
}