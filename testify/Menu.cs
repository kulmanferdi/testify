using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using static testify.Dictionary;

namespace testify
{
    internal class Menu
    {
        public Menu() { }

        private readonly string input = "input";
        private readonly string output = "output";

        private static short testNumber;
        private static int wordNumber;

        private bool outputsExist;

        private StringBuilder inputPath = new() ;
        private StringBuilder outputPath = new();

        private List<Dictionary> dictList = new List<Dictionary>();

        private void Print() 
        {
            Console.WriteLine("___________________MENU___________________\n");
            Console.WriteLine("| 0. Exit                                 |");
            Console.WriteLine("| 1. Generate tests -> TXT                |");
            Console.WriteLine("| 2. Generate tests -> XLSX               |");
            Console.WriteLine("| 3. Check if a word is in the input file |");
            Console.WriteLine("| 4. Count a word in every output file    |");
            Console.WriteLine("| 5. Load other input file                |");
            Console.WriteLine("__________________________________________");   
        }
        
        private void LoadInput()
        {     
            inputPath.Clear();
            inputPath.Append("input\\");
            outputsExist = false;

            //reading the name of the input file 
            Directory.CreateDirectory(input);
            string fileName;
            do
            {
                Console.Write("Please enter the name of the input file: ");
                fileName= Console.ReadLine();
            } while (String.IsNullOrEmpty(fileName));
            inputPath.Append(fileName);
            inputPath.Append(".txt");

            //reading the words, every word is added once
            StreamReader sr = new StreamReader(@inputPath.ToString());
            while (!sr.EndOfStream)
            {
                string currentWord = sr.ReadLine();
                if (!dictList.Any(Dictionary => Dictionary.word.Equals(currentWord)) && !String.IsNullOrEmpty(currentWord))
                {
                    dictList.Add(new Dictionary(dictList.Count + 1, currentWord));
                }
            }
            sr.Close();
        }

        private void GenerateTests()
        {
            //choosing the destination folder, then set the path            
            Directory.CreateDirectory(output);
            do
            {
                string folderName;
                Console.Write("Please enter the name of the destination folder: ");
                folderName = Path.Combine(output, Console.ReadLine());
                Directory.CreateDirectory(folderName);
                outputPath.Append(folderName);
            } while (String.IsNullOrEmpty(outputPath.ToString()));
            outputPath.Append("\\out");

            //list for the words
            List<string> outputList = new();

            Random random = new();
            //the amount of tests, that needs to generated
            do
            {
                Console.Write("Number of tests: ");
                testNumber = Convert.ToInt16(Console.ReadLine());
            } while (testNumber <= 0);

            //the number of words in the test
            do { 
                Console.Write("Number of words: ");
                wordNumber = Convert.ToInt16(Console.ReadLine());
            } while (wordNumber <= 0);

            //generating the different outputs
            for (short i = 1; i <= testNumber; ++i)
            {
                //fill up the output list with random words
                short j = 1;
                while (j <= wordNumber)
                { 
                    int randomNumber = random.Next(0, dictList.Count);
                    if (!outputList.Contains(dictList[randomNumber].word))
                    {
                        outputList.Add(dictList[randomNumber].word);
                        j++;
                    }
                }
                //create an output file
                File.WriteAllLines(@outputPath.ToString() + i.ToString() + ".txt", outputList, Encoding.UTF8);
                outputList.Clear();
            }
            dictList.Clear();
            outputsExist = true;
            Console.WriteLine("{0} tests are generated succesfully.", testNumber);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private void GenerateTests(int xlsx) { Console.WriteLine("This feature is yet to work."); }

        private void CheckInInput()
        {
            Console.Write("Enter the searched word: ");
            string searchedWord = Console.ReadLine();
            if (dictList.Any(Dictionary => Dictionary.word.Equals(searchedWord)))
            {
                Console.WriteLine("{0} is in the inputfile.", searchedWord);
            }
            else Console.WriteLine("{0} is not in the inputfile.", searchedWord);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }        

        private void CountInOutputs()
        {
            if (outputsExist)
            {
                Console.Write("Enter the searched word: ");
                string searchedWord = Console.ReadLine();
                int count = 0;
                for (short i = 1; i <= testNumber; ++i)
                {
                    StreamReader sr = new StreamReader(@outputPath.ToString() + i.ToString() + ".txt");
                    while (!sr.EndOfStream)
                    {
                        if (sr.ReadLine().ToString().Equals(searchedWord)) count++;
                    }
                    sr.Close();
                }
                Console.WriteLine("{0} output file(s) contains the \"{1}\" word.", count, searchedWord);
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Output files aren't generated yet.");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
            }
        }
        
        public void Run()
        {
            Console.WriteLine("TESTIFY\n");
            LoadInput();
            Console.Clear();
            short selection;
            do
            {    
                Print();
                do
                {
                    Console.Write("Select an option: ");
                    selection = Convert.ToInt16(Console.ReadLine());
                } while (selection < 0 | selection > 5);
                switch (selection)
                {
                    case 1: GenerateTests(); break;
                    case 2: GenerateTests(0); break;
                    case 3: CheckInInput(); break;
                    case 4: CountInOutputs(); break;
                    case 5: LoadInput(); break;
                }
                Console.Clear();
            } while (selection != 0);
        }
    }
}