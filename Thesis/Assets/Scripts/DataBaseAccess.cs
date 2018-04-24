using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;

public class DataBaseAccess {

	private string connection;
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    private IDataReader reader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenDB(string p) {
		connection = "URI=file:" + p; // we set the connection to our database
		dbcon = new SqliteConnection(connection);
		dbcon.Open();
    }

	public IDataReader BasicQuery(string q, bool r){ // run a basic Sqlite query
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = q; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        if(r) { // if we want to return the reader
            return reader; // return the reader
        }
		return null;
    }

    // This returns a 2 dimensional ArrayList with all the
    //  data from the table requested
    public ArrayList ReadFullTable(string tableName) {
        string query;
        query = "SELECT * FROM " + tableName;
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
        ArrayList readArray = new ArrayList();
        while(reader.Read()) { 
            ArrayList lineArray = new ArrayList();
            for (int i = 0; i < reader.FieldCount; i++)
                lineArray.Add(reader.GetName(i)); // This reads the entries in a row
            readArray.Add(lineArray); // This makes an array of all the rows
        }
        return readArray; // return matches
    }

	// This function deletes all the data in the given table.  Forever.  WATCH OUT! Use sparingly, if at all
    public void DeleteTableContents(string tableName) {
		string query;
		query = "DELETE FROM " + tableName;
		dbcmd = dbcon.CreateCommand();
		dbcmd.CommandText = query; 
		reader = dbcmd.ExecuteReader();
    }

	public void CreateTable(string name, ArrayList col, ArrayList colType) { // Create a table, name, column array, column type array
        string query;
        query  = "CREATE TABLE " + name + "(" + col[0] + " " + colType[0];
        for(int i = 1; i < col.Count; i++) {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ")";
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = query; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
 
    }


    public void InsertIntoSingle(string tableName, string colName, string value) { // single insert 
        string query;
        query = "INSERT INTO " + tableName + "(" + colName + ") " + "VALUES (" + value + ")";
        dbcmd = dbcon.CreateCommand(); // create empty command
        dbcmd.CommandText = query; // fill the command
        reader = dbcmd.ExecuteReader(); // execute command which returns a reader
    }
 
    public void InsertIntoSpecific(string tableName, ArrayList col, ArrayList values) { // Specific insert with col and values
        string query;
        query = "INSERT INTO " + tableName + "(" + col[0];
        for(int i = 1; i < col.Count; i++) {
            query += ", " + col[i];
        }
        query += ") VALUES (" + values[0];
        for(int i=1; i<values.Count; i++) {
            query += ", " + values[i];
        }
        query += ")";
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
    }
 
    public void InsertInto(string tableName, ArrayList values) { // basic Insert with just values
        string query;
        query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for(int i = 1; i < values.Count; i++) {
            query += ", " + values[i];
        }
        query += ")";
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader(); 
    }
 
    // This function reads a single column
    //  wCol is the WHERE column, wPar is the operator you want to use to compare with, 
    //  and wValue is the value you want to compare against.
    //  Ex. - SingleSelectWhere("puppies", "breed", "earType", "=", "floppy")
    //  returns an array of matches from the command: SELECT breed FROM puppies WHERE earType = floppy;
    //function SingleSelectWhere(tableName : String, itemToSelect : String, wCol : String, wPar : String, wValue : String):Array { // Selects a single Item
    
	public List<string> SingleSelectWhere(string tableName, string itemToSelect, string wCol, string wPar, string wValue ){ // Selects a single Item
        string query;
        query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + wCol + wPar + wValue; 
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = query; 
        reader = dbcmd.ExecuteReader();
        //var readArray = new Array();
        List<string> readArray = new List<string>();
        while(reader.Read()) { 
            //readArray.Push(reader.GetString(0)); // Fill array with all matches
            string japanese = reader.GetString(0);
            Debug.Log(japanese);
            readArray.Add(japanese); // Fill array with all matches
            string url = reader.GetString(1);
            Debug.Log(url);
            readArray.Add(url); // Fill array with all matches
        }
        return readArray; // return matches
    }
 
    public void CloseDB() {
        reader.Close(); // clean everything up
        reader = null; 
        dbcmd.Dispose(); 
        dbcmd = null; 
        dbcon.Close(); 
        dbcon = null; 
    }
 
}
