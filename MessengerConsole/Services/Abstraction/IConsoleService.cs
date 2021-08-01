namespace MessengerConsole.Services.Abstraction
{
    public interface IConsoleService
    {
        void SetupConsole();
        
        void Commands();

        void WriteTab(string tab);

        void ReadCmd();
    }
}