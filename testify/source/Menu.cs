using System.Text;
using testify.pdf_templates;

namespace testify.source
{
    internal class Menu
    {
        private uint _pdfNum ;
        
        private const string Input = "input";
        private const string Output = "output";

        private readonly List<string> _menuItems = null!;
        private readonly List<string> _testTypes = null!;
        private readonly List<string> _openedInputs = new();
        
        private static ushort _testNumber;
        private static ushort _wordNumber;

        private bool _outputsExist;
        private bool _inputMissing;
        
        private readonly List<Dictionary> _dictList = new();

        private StringBuilder InputPath { get; } = new();
        private StringBuilder OutputPath { get; } = new();

        private void InitInputFile()
        {            
            _dictList.Clear();
            LoadInput();
        }
        private void LoadInput()
        {
            _outputsExist = false;
            InputPath.Clear();

            InputPath.Append(Input);
            InputPath.Append('\\');

            //reading the name of the input file
            try
            {
                Directory.CreateDirectory(Input);
                string fileName;
                do
                {
                    Console.Write("Please enter the name of the input file: ");
                    fileName = Console.ReadLine()!;
                } while (string.IsNullOrEmpty(fileName) | _openedInputs.Contains(fileName));
                _openedInputs.Add(fileName);
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
                var sr = new StreamReader(InputPath.ToString());
                while (!sr.EndOfStream)
                {
                    var currentWord = sr.ReadLine()!;
                    if (!_dictList.Any(dictionary => dictionary.Word.Equals(currentWord)) && !string.IsNullOrEmpty(currentWord))
                        _dictList.Add(new Dictionary(Convert.ToUInt32(_dictList.Count + 1), currentWord));
                }
                sr.Close();
            }
            catch (FileNotFoundException e)
            {
                if (e.Source != null)
                    Console.WriteLine("FileNotFoundException source: {0}", e.Source);
                throw;
            }

            _inputMissing = _dictList.Count == 0;
        }

        private void GenerateTxt()
        {
            //choosing the destination folder, then set the path
            OutputPath.Clear();
            try
            {
                Directory.CreateDirectory(Output);
                do
                {
                    Console.Write("Please enter the name of the destination folder: ");
                    var folderName = Path.Combine(Output, Console.ReadLine()!);
                    if (BreakFunction(folderName)) return;
                    Directory.CreateDirectory(folderName);
                    OutputPath.Append(folderName);
                } while (string.IsNullOrEmpty(OutputPath.ToString()));

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
                    if (ushort.TryParse(Console.ReadLine(), out _testNumber)) { }
                } while (_testNumber <= 0);

                //the number of words in the test
                do { 
                    Console.Write("Number of words: ");
                    if (ushort.TryParse(Console.ReadLine(), out _wordNumber)) { }
                } while (_wordNumber <= 0 & _wordNumber > _dictList.Count);
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
            for (ushort i = 1; i <= _testNumber; ++i)
            {
                //fill up the output list with random words
                ushort j = 1;
                while (j <= _wordNumber)
                {
                    var randomNumber = random.Next(0, _dictList.Count);
                    if (outputList.Contains(_dictList[randomNumber].Word)) continue;
                    outputList.Add(_dictList[randomNumber].Word);
                    j++;
                }
                //create an output file
                File.WriteAllLines(OutputPath + i.ToString() + ".txt", outputList, Encoding.UTF8);
                outputList.Clear();
            }
            _outputsExist = true;

            Console.WriteLine("{0} tests are generated successfully.", _testNumber);
            Console.WriteLine("PDF Types");
            int counter = 1;
            foreach (var it in _testTypes)
            {
                Console.WriteLine("{0}. {1}", counter, it);
                counter++;
            }
            Console.Write("Select the test type:");
            _pdfNum = Convert.ToUInt32(Console.ReadLine());
            SelectTestType(_pdfNum);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
        
        private void CheckInInput()
        {
            try
            {
                Console.Write("Enter the searched word: ");
                var searchedWord = Console.ReadLine()!;
                if (BreakFunction(searchedWord))
                    return;
                Console.WriteLine(
                    _dictList.Any(dictionary => dictionary.Word.Equals(searchedWord))
                        ? "{0} is in the input file."
                        : "{0} is not in the input file.", searchedWord);
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
                if (_outputsExist)
                {
                    Console.Write("Enter the searched word: ");
                    var searchedWord = Console.ReadLine()!;
                    if (BreakFunction(searchedWord))
                        return;
                    ushort count = 0;
                    for (ushort i = 1; i <= _testNumber; ++i)
                    {
                        var sr = new StreamReader(OutputPath + i.ToString() + ".txt");
                        while (!sr.EndOfStream)
                            if (sr.ReadLine()!.Equals(searchedWord))
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
            Console.Write("Opened input files: ");
            foreach (var str in _openedInputs)
            {
                Console.Write("{0} ", str);
            }
            Console.WriteLine();
            foreach (var it in _dictList)
            {
                Console.WriteLine("{0}:\t{1}", it.WordId, it.Word);
            }
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private void Print()
        {
            Console.WriteLine(" __________________MENU____________________");
            foreach (var i in Enumerable.Range(0, _menuItems.Count))
                Console.WriteLine("| {0}", _menuItems[i]);
            Console.WriteLine(" __________________________________________");
        }

        private void InitApp()
        {
            //initializing lists and variables
            _inputMissing = false;
            
            _menuItems.Add("Exit");
            _menuItems.Add("Generate tests");
            _menuItems.Add( "Check if a word is in the input file");
            _menuItems.Add("Count a word in every output file");
            _menuItems.Add("Load other input file");
            _menuItems.Add("Load more input");
            _menuItems.Add( "Print all input");
            _menuItems.Add("Generating PDF...");
            
            _testTypes.Add("Verb forms test");
            _testTypes.Add("Translation test");
            

            //run the program and read the name of the input file
            Console.WriteLine("Testify\n");
            do { InitInputFile(); }
            while (_inputMissing);
            Console.Clear();
        }

        private static bool BreakFunction(string str)
        {
            return str.Equals("*esc");
        }

        private void SelectTestType(uint index)
        {
            VerbForms v = new();
            Translate t = new();
            switch (index)
            {
                case 0: v.GenerateDoc(_dictList); break;
                case 1: t.GenerateDoc(_dictList); break;
            }
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
                    } while ( selectedMenuIndex > 7 );
                }
                catch (IOException e)
                {                    
                    if (e.Source != null)
                        Console.WriteLine("IOException source: {0}\nInvalid input.", e.Source);
                    throw;
                }                
                switch (selectedMenuIndex)
                {
                    case 1: Console.Clear(); Console.WriteLine(_menuItems[selectedMenuIndex - 1]); 
                        GenerateTxt(); break;
                    case 2: Console.Clear(); Console.WriteLine(_menuItems[selectedMenuIndex - 1]); 
                        CheckInInput(); break;
                    case 3: Console.Clear(); Console.WriteLine(_menuItems[selectedMenuIndex - 1]); 
                        CountInOutputs(); break;
                    case 4: Console.Clear(); Console.WriteLine(_menuItems[selectedMenuIndex - 1]); 
                        _openedInputs.Clear(); InitInputFile(); break;
                    case 5: Console.Clear(); Console.WriteLine(_menuItems[selectedMenuIndex - 1]); 
                        LoadInput(); break;
                    case 6: Console.Clear(); Console.WriteLine(_menuItems[selectedMenuIndex - 1]); 
                        PrintInput(); break;
                    case 7: Console.Clear(); Console.WriteLine(_menuItems[selectedMenuIndex - 1]);
                        SelectTestType(_pdfNum); break;
                }
                Console.Clear();
            } while (selectedMenuIndex != 0);
        }
    }
}