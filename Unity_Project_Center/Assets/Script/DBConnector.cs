using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

public class DBConnector : MonoBehaviour
{
    private static MySqlConnection dbConnection;
    private static DBConnector instance = null;

    public static DBConnector Instance
    {
        get
        {
            if (instance == null)
            {
                lock (typeof(DBConnector))
                {
                    if (instance == null)
                    {
                        instance = new DBConnector();
                    }
                }
            }
            return instance;
        }
    }


    public DBConnector()
    {
        openSqlConnection();
        //doQuery();
    }


    // Connect to database
    private static void openSqlConnection()
    {

        string connectionString = "Server=34.64.84.174;" +

            "Database=unity;" +

            "User ID=unity;" +

            "Password=1234;" +

            "Pooling=false;" +
            
            "Character set=utf8";

        dbConnection = new MySqlConnection(connectionString);

        dbConnection.Open();

        Debug.Log("Connected to database.");

    }

    // MySQL Query
    public MySqlDataReader doQuery(string sqlQuery)
    {

        MySqlCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = sqlQuery;
        MySqlDataReader reader = dbCommand.ExecuteReader();

        dbCommand.Dispose();
        dbCommand = null;

        return reader;
    }


    // Disconnect from database
    public static void closeSqlConnection()
    {
        dbConnection.Close();
        dbConnection = null;
        Debug.Log("Disconnected from database.");
    }
}