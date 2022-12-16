using System.Text;

namespace testify
{
    internal class Menu
    {
        public Menu() { }

        private const string input = "input";
        private const string output = "output";

        private static short testNumber;
        private static short wordNumber;

        private bool outputsExist;

        private StringBuilder inputPath = new();
        private StringBuilder outputPath = new();

        private readonly List<Dictionary> dictList = new();

        public StringBuilder InputPath { get => inputPath; set => inputPath = value; }
        public StringBuilder OutputPath { get => outputPath; set => outputPath = value; }

        private void Print() 
        {
            Console.WriteLine(" __________________MENU___________________\n");
            Console.WriteLine("| 0. Exit                                 |");
            Console.WriteLine("| 1. Generate tests -> TXT                |");
            Console.WriteLine("| 2. Generate tests -> XLSX               |");
            Console.WriteLine("| 3. Check if a word is in the input file |");
            Console.WriteLine("| 4. Count a word in every output file    |");
            Console.WriteLine("| 5. Load other input file                |");
            Console.WriteLine(" _________________________________________");   
        }
        
        private void LoadInput()
        {
            outputsExist = false;

            dictList.Clear();
            InputPath.Clear();

            InputPath.Append(input);
            InputPath.Append("\\");

            //reading the name of the input file
            try
            {
                Directory.CreateDirectory(input);
                string fileName;
                do
                {
                    Console.Write("Please enter the name of the input file: ");
                    fileName = Console.ReadLine();
                } while (String.IsNullOrEmpty(fileName));
                InputPath.Append(fileName);
                InputPath.Append(".txt");
            }
            catch (IOException e)
            {                
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                throw;
            }

            //reading the words, every word is added once
            try
            {
                StreamReader sr = new StreamReader(InputPath.ToString());
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
            catch (FileNotFoundException e)
            {
                if (e.Source != null)
                    Console.WriteLine("FileNotFoundException source: {0}", e.Source);
                throw;
            }
        }

        private void GenerateTxt()
        {
            //choosing the destination folder, then set the path
            OutputPath.Clear();
            try
            {
                Directory.CreateDirectory(output);
                do
                {
                    string folderName;
                    Console.Write("Please enter the name of the destination folder: ");
                    folderName = Path.Combine(output, Console.ReadLine());
                    Directory.CreateDirectory(folderName);
                    OutputPath.Append(folderName);
                } while (String.IsNullOrEmpty(OutputPath.ToString()));
                OutputPath.Append("\\out");
            }
            catch (IOException e)
            {                
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                throw;
            }

            try { 
                //the amount of tests, that needs to generated
                do
                {
                    Console.Write("Number of tests: ");
                    if (short.TryParse(Console.ReadLine().ToString(), out testNumber)) { }
                } while (testNumber <= 0);

                //the number of words in the test
                do { 
                    Console.Write("Number of words: ");
                    if (short.TryParse(Console.ReadLine().ToString(), out wordNumber)) { }
                } while (wordNumber <= 0);
            }
            catch (IOException e)
            {
               
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                throw;
            }
            
            //generating the different outputs
            List<string> outputList = new();
            Random random = new();
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
                File.WriteAllLines(OutputPath.ToString() + i.ToString() + ".txt", outputList, Encoding.UTF8);
                outputList.Clear();
            }
            outputsExist = true;

            Console.WriteLine("{0} tests are generated succesfully.", testNumber);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private void GenerateXlsx()
        {
            Console.WriteLine("This feature is yet to work.");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private void CheckInInput()
        {
            try
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
            catch (IOException e)
            {               
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                throw;
            }
        }        

        private void CountInOutputs()
        {
            try 
            {
                if (outputsExist)
                {
                    Console.Write("Enter the searched word: ");
                    string searchedWord = Console.ReadLine();
                    short count = 0;
                    for (short i = 1; i <= testNumber; ++i)
                    {
                        StreamReader sr = new StreamReader(OutputPath.ToString() + i.ToString() + ".txt");
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
            catch (IOException e)
            {                
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                throw;
            }
        }
        
        public void Run()
        {
            Console.WriteLine("Testify\n");
            LoadInput();
            Console.Clear();
            short selectedMenuIndex;
            do
            {
                try
                {
                    Print();
                    do
                    {
                        Console.Write("Select an option: ");
                        selectedMenuIndex = Convert.ToInt16(Console.ReadLine());
                    } while (selectedMenuIndex < 0 | selectedMenuIndex > 5);
                }
                catch (IOException e)
                {                    
                    if (e.Source != null)
                        Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                    throw;
                }
                switch (selectedMenuIndex)
                {
                    case 1: GenerateTxt(); break;
                    case 2: GenerateXlsx(); break;
                    case 3: CheckInInput(); break;
                    case 4: CountInOutputs(); break;
                    case 5: LoadInput(); break;
                }
                Console.Clear();
            } while (selectedMenuIndex != 0);
        }
    }
}