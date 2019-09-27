using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace TNUE_Patron_Excel.DBConnect
{
	internal class DBConnecting
	{
		public static OracleConnection conn = null;

		public static OracleConnection GetDBConnection(DataOracle oracle)
		{
			Console.WriteLine("Getting Connection ...");
			string connectionString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = " + oracle.host + ")(PORT = " + oracle.port + "))(CONNECT_DATA = (SID = " + oracle.sid + ")));Persist Security Info=true;User ID=" + oracle.user + "; Password=" + oracle.password + ";";
			conn = new OracleConnection();
			conn.ConnectionString = connectionString;
			return conn;
		}

		public OracleDataReader GetDataReader(string sql)
		{
			OracleDataReader result = null;
			try
			{
				if (conn != null && conn.State == ConnectionState.Closed)
				{
					conn.Open();
				}
				Console.WriteLine("Connection...");
				OracleCommand oracleCommand = new OracleCommand();
				oracleCommand.CommandTimeout = 60;
				oracleCommand.Connection = conn;
				oracleCommand.CommandText = sql;
				result = oracleCommand.ExecuteReader();
				oracleCommand.Dispose();
			}
			catch (OracleException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return result;
		}

		public static void CloseConnection()
		{
			try
			{
				conn.Dispose();
				conn.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}
	}
}
