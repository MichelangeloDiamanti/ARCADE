using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class InitialWorld : MonoBehaviour {

	DataBaseAccess db;
	List<Entity> entities;


    // Use this for initialization
    void Start () {
		db = new DataBaseAccess();
		db.OpenDB("Database/initRealWorld.db");

        entities = new List<Entity>();
		parseEntities();


	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void parseEntities(){
		IDataReader res = db.BasicQuery("SELECT rowid, * FROM ENTITY", true);
        while(res.Read()) { 
			long id = -1;
			string type = "";
			string name = "";
			bool real = false;
			int world = -1;
            for (int i = 0; i < res.FieldCount; i++){
                string field = res.GetName(i);
				switch(field){
					case "rowid":
						id = res.GetInt64(i);
						break;
					case "TYPE":
						type = res.GetString(i);
						break;
					case "NAME":
						name = res.GetString(i);
						break;
					case "REAL":
						real = res.GetBoolean(i);
						break;
					case "WORLD":
						world = res.GetInt32(i);
						break;
				}
			}
			entities.Add(new Entity(id, type, name, real, world));
		}

		foreach(Entity e in entities){
			Debug.Log(e.Type + " " + e.Name + " " + e.Real + " " + e.World);
		}
		Debug.Log(entities);	
	}
}
