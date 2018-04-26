using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class InitialWorld : MonoBehaviour
{

    DataBaseAccess db;
    List<Entity> entities;

    // Use this for initialization
    void Start()
    {
        Manager.initManager();
		animalInizialitation();
		// example();
    }

    // Update is called once per frame
    void Update()
    {

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

    private void animalInizialitation()
    {
        EntityType character = new EntityType("CHARACTER");
        Manager.addEntityType(character);
        
        EntityType location = new EntityType("LOCATION");
        Manager.addEntityType(location);
        
        // EntityType animal = new EntityType("ANIMAL");
        // Manager.addEntityType(animal);
        
        Entity hero = new Entity(character, "hero");
        Manager.addEntity(hero);
        Entity cat_lady = new Entity(character, "cat_lady");
        Manager.addEntity(cat_lady);
        Entity v1 = new Entity(location, "village1");
        Manager.addEntity(v1);
        Entity v2 = new Entity(location, "village2");
        Manager.addEntity(v2);
        Entity cat = new Entity(character, "cat");
        Manager.addEntity(cat);

        BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
        Manager.addPredicate(isAt);
        UnaryPredicate handempty = new UnaryPredicate(character, "HANDEMPTY");
        Manager.addPredicate(handempty);

        BinaryRelation heroIsAtV1 = new BinaryRelation(hero, isAt, v1);
        BinaryRelation cat_ladyIsAtV1 = new BinaryRelation(cat_lady, isAt, v1);
        BinaryRelation catIsAtV2 = new BinaryRelation(cat, isAt, v2);

        UnaryRelation heroHandempty = new UnaryRelation(hero, handempty);


        List<Entity> parameters = new List<Entity>(); 
        Entity c1 = new Entity(character, "c1");
        parameters.Add(c1);
        Entity c2 = new Entity(character, "c2");
        parameters.Add(c2);        
        Entity l1 = new Entity(location, "l1");
        parameters.Add(l1);
        Entity l2 = new Entity(location, "l2");
        parameters.Add(l2);
        Entity c3 = new Entity(character, "a1");
        parameters.Add(c1);

        BinaryRelation c1IsAtl1 = new BinaryRelation(c1, isAt, l1, true);
        BinaryRelation c2IsAtl1 = new BinaryRelation(c2, isAt, l1, true);
        BinaryRelation a1IsAtl2 = new BinaryRelation(c3, isAt, l2, true);
        BinaryRelation c1IsAtl2 = new BinaryRelation(c1, isAt, l2, true);

        UnaryRelation c1Handempty = new UnaryRelation(c1, handempty, true);
        
        List<KeyValuePair<IRelation, bool>> pre = new List<KeyValuePair<IRelation, bool>>(); 
        pre.Add(new KeyValuePair<IRelation, bool>(c1IsAtl1, true));
        pre.Add(new KeyValuePair<IRelation, bool>(c1IsAtl2, false));
        List<KeyValuePair<IRelation, bool>> post = new List<KeyValuePair<IRelation, bool>>(); 
        post.Add(new KeyValuePair<IRelation, bool>(c1IsAtl1, false));
        post.Add(new KeyValuePair<IRelation, bool>(c1IsAtl2, true));        
        ActionDefinition move = new ActionDefinition(pre, "MOVE", parameters, post);
        Manager.addActionDefinition(move);

        //GameObject.Find("GameManager").SendMessage("Manager", "Inn1");

        Debug.Log(move.ToString());

    }
    private void example()
    {
        // EntityType character = new EntityType("CHARACTER");
        // EntityType location = new EntityType("LOCATION");

        // Manager.addEntityType(location);

        // Entity john = new Entity(character, "john");
        // Entity school = new Entity(location, "school");

        // Manager.addEntity(john);
        // Manager.addEntity(school);

        // BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
        // UnaryPredicate isRich = new UnaryPredicate(character, "RICH");

        // Manager.addPredicate(isAt);
        // Manager.addPredicate(isRich);

        // UnaryPredicate isRich2 = new UnaryPredicate(character, "RICH");
        // BinaryPredicate isAt2 = new BinaryPredicate(character, "IS_AT", location);

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

        // UnaryRelation johnIsRich = new UnaryRelation(john, isRich);
        // BinaryRelation johnIsAtSchool = new BinaryRelation(john, isAt, school);
    }
}
