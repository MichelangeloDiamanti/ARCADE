using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class InitialWorld : MonoBehaviour {

	DataBaseAccess db;
	List<Entity> entities;
	Manager man;

    // Use this for initialization
    void Start () {

		man = Manager.GetManager();

		EntityType character = new EntityType("CHARACTER");
		EntityType location = new EntityType("LOCATION");

		man.addEntityType(character);
		man.addEntityType(location);

		Entity john = new Entity(character, "john");
		Entity school = new Entity(location, "school");

		man.addEntity(john);
		man.addEntity(school);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		// BinaryPredicate isAt2 = new BinaryPredicate(character, "IS_AT", location);
		UnaryPredicate isRich = new UnaryPredicate(character, "RICH");

		man.addPredicate(isAt);
		man.addPredicate(isRich);

		Debug.Log("predicates count: " + man.getPredicates().Count);

		foreach(IPredicate p in man.getPredicates()){
			if(p.GetType() == typeof(BinaryPredicate))
				Debug.Log(p + " binary");
			if(p.GetType() == typeof(UnaryPredicate))
				Debug.Log(p + " unary");
		}

		// Manager.GetManager().addPredicate(new Predicate("IS_AT",2));
		// foreach( Entity e in man.getEntities()){
		// 	Debug.Log(e);
		// }

		UnaryRelation johnIsRich = new UnaryRelation(john, isRich);
		BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school);


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// private void parseEntities(){
	// 	IDataReader res = db.BasicQuery("SELECT rowid, * FROM ENTITY", true);
    //     while(res.Read()) { 
	// 		long id = -1;
	// 		string type = "";
	// 		string name = "";
	// 		bool real = false;
	// 		int world = -1;
    //         for (int i = 0; i < res.FieldCount; i++){
    //             string field = res.GetName(i);
	// 			switch(field){
	// 				case "rowid":
	// 					id = res.GetInt64(i);
	// 					break;
	// 				case "TYPE":
	// 					type = res.GetString(i);
	// 					break;
	// 				case "NAME":
	// 					name = res.GetString(i);
	// 					break;
	// 				case "REAL":
	// 					real = res.GetBoolean(i);
	// 					break;
	// 				case "WORLD":
	// 					world = res.GetInt32(i);
	// 					break;
	// 			}
	// 		}
	// 		entities.Add(new Entity(id, type, name, real, world));
	// 	}

	// 	foreach(Entity e in entities){
	// 		Debug.Log(e.Type + " " + e.Name + " " + e.Real + " " + e.World);
	// 	}
	// 	Debug.Log(entities);	
	// }
}
