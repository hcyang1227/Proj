using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace mysql
{
    class Program
    {
        public void starter()
        {
            string dbHost = "127.0.0.1";
            string dbUser = "root";
            string dbPass = "";
            string dbName = "game";

            // 如果有特殊的編碼在database後面請加上;CharSet=編碼, utf8請使用utf8_general_ci
            string connStr = "server="+dbHost+";uid="+dbUser+";pwd="+dbPass+";database="+dbName;
            MySqlConnection conn = new MySqlConnection(connStr);

            // 連線到資料庫
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) {
                switch (ex.Number)
                { 
                    case 0:
                        Console.WriteLine("無法連線到資料庫.");
                        break;
                    case 1045:
                        Console.WriteLine("使用者帳號或密碼錯誤,請再試一次.");
                        break;
                }
            }

            // 進行select
            string SQL = "select pw from game_acc order by id desc limit 0,10 ";
            try
            {
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                MySqlDataReader myData = cmd.ExecuteReader();

                if (!myData.HasRows)
                {
                    // 如果沒有資料,顯示沒有資料的訊息
                    Console.WriteLine("No data.");
                }
                else
                {
                    // 讀取資料並且顯示出來
                    while (myData.Read())
                    {
                        Console.WriteLine("Text={0}", myData.GetString(0));
                    }
                    myData.Close();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) {
                Console.WriteLine("Error " + ex.Number + " : " + ex.Message);
            }
        }
    }
}