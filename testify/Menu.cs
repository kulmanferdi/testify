using System.Text;

namespace testify
{
    internal class Menu
    {
        public Menu() { }

        private const string input = "input";
        private const string output = "output";

        private static ushort testNumber;
        private static ushort wordNumber;

        private bool outputsExist;
        private bool inputMissing;

        private StringBuilder inputPath = new();
        private StringBuilder outputPath = new();

        private readonly List<Dictionary> dictList = new();
        private readonly List<Dictionary> menuList = new();

        public StringBuilder InputPath { get => inputPath; set => inputPath = value; }
        public StringBuilder OutputPath { get => outputPath; set => outputPath = value; }
       
        private void InitInputFile()
        {            
            dictList.Clear();
            LoadInput();
        }
        private void LoadInput()
        {
            outputsExist = false;
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
                if (!InputPath.ToString().Contains(".txt"))
                {
                    InputPath.Append(".txt");
                }                
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
                        dictList.Add(new Dictionary(Convert.ToUInt32(dictList.Count + 1), currentWord));
                }
                sr.Close();
            }
            catch (FileNotFoundException e)
            {
                if (e.Source != null)
                    Console.WriteLine("FileNotFoundException source: {0}", e.Source);
                throw;
            }

            if (dictList.Count == 0)
                inputMissing = true;
            else
                inputMissing = false;
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
                    if (breakFunction(folderName)) return;
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
                    if (ushort.TryParse(Console.ReadLine().ToString(), out testNumber)) { }
                } while (testNumber <= 0);

                //the number of words in the test
                do { 
                    Console.Write("Number of words: ");
                    if (ushort.TryParse(Console.ReadLine().ToString(), out wordNumber)) { }
                } while (wordNumber <= 0 & wordNumber > dictList.Count);
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
            for (ushort i = 1; i <= testNumber; ++i)
            {
                //fill up the output list with random words
                ushort j = 1;
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
                if (breakFunction(searchedWord))
                    return;
                if (dictList.Any(Dictionary => Dictionary.word.Equals(searchedWord)))
                    Console.WriteLine("{0} is in the inputfile.", searchedWord);
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
                    if (breakFunction(searchedWord))
                        return;
                    ushort count = 0;
                    for (ushort i = 1; i <= testNumber; ++i)
                    {
                        StreamReader sr = new StreamReader(OutputPath.ToString() + i.ToString() + ".txt");
                        while (!sr.EndOfStream)
                            if (sr.ReadLine().ToString().Equals(searchedWord))
                                count++;
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

        private void PrintInput()
        {
            foreach (var it in dictList)
            {
                Console.WriteLine("{0}:\t{1}", it.wordID, it.word);
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private void Print()
        {
            Console.WriteLine(" __________________MENU___________________\n");
            Console.WriteLine("| 0. Exit                                 |");
            Console.WriteLine("| 1. Generate tests -> TXT                |");
            Console.WriteLine("| 2. Generate tests -> XLSX               |");
            Console.WriteLine("| 3. Check if a word is in the input file |");
            Console.WriteLine("| 4. Count a word in every output file    |");
            Console.WriteLine("| 5. Load other input file                |");
            Console.WriteLine("| 6. Load more input                      |");
            Console.WriteLine("| 7. Print all input                      |");
            Console.WriteLine(" _________________________________________");
        }

        private void InitApp()
        {
            //vars
            menuList.Add(new Dictionary(1, "Generate tests -> TXT"));
            menuList.Add(new Dictionary(2, "Generate tests -> XLSX"));
            menuList.Add(new Dictionary(3, "Check if a word is in the input file"));
            menuList.Add(new Dictionary(4, "Count a word in every output file"));
            menuList.Add(new Dictionary(5, "Load other input file"));
            menuList.Add(new Dictionary(6, "Load more input"));
            menuList.Add(new Dictionary(7, "Print all input"));

            inputMissing = false;

            //run
            Console.WriteLine("Testify\n");
            do { InitInputFile(); }
            while (inputMissing);
            Console.Clear();
        }

        private bool breakFunction(string str)
        {
            if (str.Equals("*esc"))
                return true;
            return false;
        }

        public void Run()
        {
            InitApp();
            ushort selectedMenuIndex;           
            do
            {
                try
                {
                    Print();
                    do
                    {
                        Console.Write("Select an option: ");
                        selectedMenuIndex = Convert.ToUInt16(Console.ReadLine());
                    } while (selectedMenuIndex < 0 | selectedMenuIndex > 7);
                }
                catch (IOException e)
                {                    
                    if (e.Source != null)
                        Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                    throw;
                }                
                switch (selectedMenuIndex)
                {
                    case 1: Console.Clear(); Console.WriteLine(menuList[selectedMenuIndex - 1].word); GenerateTxt(); break;
                    case 2: Console.Clear(); Console.WriteLine(menuList[selectedMenuIndex - 1].word); GenerateXlsx(); break;
                    case 3: Console.Clear(); Console.WriteLine(menuList[selectedMenuIndex - 1].word); CheckInInput(); break;
                    case 4: Console.Clear(); Console.WriteLine(menuList[selectedMenuIndex - 1].word); CountInOutputs(); break;
                    case 5: Console.Clear(); Console.WriteLine(menuList[selectedMenuIndex - 1].word); InitInputFile(); break;
                    case 6: Console.Clear(); Console.WriteLine(menuList[selectedMenuIndex - 1].word); LoadInput(); break;
                    case 7: Console.Clear(); Console.WriteLine(menuList[selectedMenuIndex - 1].word); PrintInput(); break;
                }
                Console.Clear();
            } while (selectedMenuIndex != 0);
        }
    }
}