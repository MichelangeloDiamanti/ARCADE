using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class InitialWorld : MonoBehaviour {

	DataBaseAccess db;
	List<Entity> entities;

    // Use this for initialization
    void Start () {

		EntityType character = new EntityType("CHARACTER");
		EntityType location = new EntityType("LOCATION");

		Manager.addEntityType(character);
		Manager.addEntityType(location);

		Entity john = new Entity(character, "john");
		Entity school = new Entity(location, "school");

		Manager.addEntity(john);
		Manager.addEntity(school);

		BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
		UnaryPredicate isRich = new UnaryPredicate(character, "RICH");

		Manager.addPredicate(isAt);
		Manager.addPredicate(isRich);

		UnaryPredicate isRich2 = new UnaryPredicate(character, "RICH");
		BinaryPredicate isAt2 = new BinaryPredicate(character, "IS_AT", location);

        // Debug.Log("predicates count: " + Manager.getPredicates().Count);

		// foreach(IPredicate p in Manager.getPredicates()){
		// 	if(p.GetType() == typeof(BinaryPredicate))
        //         Debug.Log(p + " binary");
		// 	if(p.GetType() == typeof(UnaryPredicate))
        //         Debug.Log(p + " unary");
		// }

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
