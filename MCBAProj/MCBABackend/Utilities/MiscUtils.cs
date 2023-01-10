namespace MCBA.Utils;

internal static class MiscUtils
{
	internal static char Deposit = 'D';
	internal static object GetObjOrDbNull(this object value) => value ?? DBNull.Value; 
}