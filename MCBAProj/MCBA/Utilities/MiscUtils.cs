namespace MCBA.Utils;

internal static class MiscUtils
{
    internal static void PrintErrMsg(string errMsg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nErr: {errMsg}, Pleae Try Again\n");
        Console.ResetColor();
    }

    internal static bool IsInRange(this int value, int min, int max) => value >= min && value <= max;

    internal static string GetAccStrFromChar(this char value) => value == 'S' ? "Savings" : "Checking";
}