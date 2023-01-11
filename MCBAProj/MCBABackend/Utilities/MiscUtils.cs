using Microsoft.Data.SqlClient;
using System.Data;

namespace MCBA.Utils;

internal static class MiscUtils
{
    // Method Adapted from Rmit/Week3/WebServiceAndDatabaseExample
    internal static DataTable GetDataTable(this SqlCommand command)
    {
        using var adapter = new SqlDataAdapter(command);

        var table = new DataTable();
        adapter.Fill(table);

        return table;
    }

    // Method Adapted from Rmit/Week3/WebServiceAndDatabaseExample
    internal static object GetObjOrDbNull(this object value) => value ?? DBNull.Value;

    internal static char Deposit = 'D';
}