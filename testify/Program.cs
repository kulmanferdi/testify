using static testify.Menu;

namespace testify
{
    internal class App
    {       
        public void Main()
        {
            Menu menu = new Menu();
            menu.Run();
            Console.ReadKey();
        }
    }
}