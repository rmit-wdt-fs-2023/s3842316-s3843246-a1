namespace MCBA.Utils;

internal static class MiscUtils
{
    internal static void PrintErrMsg(string errMsg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nErr: {errMsg}, Pleae Try Again\n");
        Console.ResetColor();
    }
}