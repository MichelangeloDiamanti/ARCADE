using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptThatUsesTheDatabase : MonoBehaviour {

	/*  Script for testing out SQLite in Javascript
          2011 - Alan Chatham
          Released into the public domain
 
        This script is a GUI script - attach it to your main camera.
        It creates/opens a SQLite database, and with the GUI you can read and write to it.
                                        */
 
// This is the file path of the database file we want to use
// Right now, it'll load TestDB.sqdb in the project's root folder.
// If one doesn't exist, it will be automatically created.
public string DatabaseName = "TestDB.sqdb";
 
// This is the name of the table we want to use
public string TableName = "TestTable";
DataBaseAccess db;

// These variables just hold info to display in our GUI
	string firstName = "First Name";
	string lastName = "Last Name"; 
	int DatabaseEntryStringWidth = 100;
	Vector2 scrollPosition;
	ArrayList databaseData = new ArrayList();

	// Use this for initialization
	void Start () {
		db = new DataBaseAccess();
		db.OpenDB("sqlite3.db");
		List<string> list = db.SingleSelectWhere("people", "*", "1", "=", "1");
		foreach(string s in list){
			Debug.Log(s);
		}
		// db = new DataBaseAccess();
		// db.OpenDB(DatabaseName);
		// // Let's make sure we've got a table to work with as well!
		// string tableName = TableName;
		// ArrayList columnNames = new ArrayList();
		// columnNames.Add("firstName");
		// columnNames.Add("lastName");
		// ArrayList columnValues = new ArrayList();
		// columnNames.Add("text");
		// columnNames.Add("text");
		// try {
		// 	db.CreateTable(tableName, columnNames, columnValues);
		// }
		// catch(Exception e)
        // {
        //     Debug.Log("Exeption");
        // }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// This GUI provides us with a way to enter data into our database
	//  as well as a way to view it
	void OnGUI() {
		GUI.Box(new Rect (25,25,Screen.width - 50, Screen.height - 50),""); 
		GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height - 100));
		// This first block allows us to enter new entries into our table
			GUILayout.BeginHorizontal();
				firstName = GUILayout.TextField(firstName, GUILayout.Width (DatabaseEntryStringWidth));
				lastName = GUILayout.TextField(lastName, GUILayout.Width (DatabaseEntryStringWidth));
			GUILayout.EndHorizontal();
	
			if (GUILayout.Button("Add to database")) {
				// Insert the data
				InsertRow(firstName,lastName);
				// And update the readout of the database
				databaseData = ReadFullTable();
			}
			// This second block gives us a button that will display/refresh the contents of our database
			GUILayout.BeginHorizontal();
				if (GUILayout.Button ("Read Database")) 
					databaseData = ReadFullTable();
				if (GUILayout.Button("Clear"))
					databaseData.Clear();
			GUILayout.EndHorizontal();
	
			GUILayout.Label("Database Contents");
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100));
				foreach (ArrayList line in databaseData) {
					GUILayout.BeginHorizontal();
					foreach (string s in line) {
						GUILayout.Label(s.ToString(), GUILayout.Width(DatabaseEntryStringWidth));
					}
					GUILayout.EndHorizontal();
				}
	
			GUILayout.EndScrollView();
			if (GUILayout.Button("Delete All Data")) {
				DeleteTableContents();
				databaseData = ReadFullTable();
			}
	
		GUILayout.EndArea();
	}
	
	// Wrapper function for inserting our specific entries into our specific database and table for this file
	void InsertRow(string firstName, string lastName) {
		ArrayList values = new ArrayList();
		values.Add("'"+firstName+"'");
		values.Add("'"+lastName+"'");
		db.InsertInto(TableName, values);
	}
	
	// Wrapper function, so we only mess with our table.
	ArrayList ReadFullTable() {
		return db.ReadFullTable(TableName);
	}
	
	// Another wrapper function...
	void DeleteTableContents() {
		db.DeleteTableContents(TableName);
	}


}