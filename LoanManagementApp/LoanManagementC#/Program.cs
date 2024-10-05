using LoanManagementC_.MainModule;

namespace LoanManagementC_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoanManagementMenu menu = new LoanManagementMenu();
            menu.DisplayMenu();
        }
    }
}
