using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class MysqlAccess
{
    public static MySqlConnection mySqlConnection;
    static string host = "127.0.0.1";
    static string user = "root";
    static string pwd = "";
    static string db = "game";
    static string port = "5000";

    public void connectDB()
    {
        Open();
    }
    public static void Open()
    {
        try
        {
            string connectionString = string.Format("Server = {0};port={4};Database = {1}; User ID = {2}; Password = {3};", host, db, user, pwd, port);
            mySqlConnection = new MySqlConnection();
            mySqlConnection.Open();
        }catch(System.Exception e)
        {
            throw new System.Exception(""+e.Message.ToString());
        }
    }

    public void Close()
    {
        if (mySqlConnection != null)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
        }
    }
}