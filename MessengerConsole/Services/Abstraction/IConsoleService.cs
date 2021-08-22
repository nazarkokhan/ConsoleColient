namespace MessengerConsole.Services.Abstraction
{
    public interface IConsoleService
    {
        void CleanConsole();
        
        void AvailableCommands();

        void ReadCommand(int l, int t);

        void WriteTab(string tab);

        void ReadLine(int l, int r);
    }
}