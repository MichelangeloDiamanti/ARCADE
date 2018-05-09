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
		
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void villaggeBanditWorldFullDetail(){
        EntityType character = new EntityType("CHARACTER");
        Manager.addEntityType(character);
        
        EntityType location = new EntityType("LOCATION");
        Manager.addEntityType(location);

        Entity village1 = new Entity(location, "village1");
        Manager.addEntity(village1);
        Entity village2 = new Entity(location, "village2");
        Manager.addEntity(village2);

        Entity mayorVillage1 = new Entity(character, "mayorVillage1");
        Manager.addEntity(mayorVillage1);
        Entity mayorVillage2 = new Entity(character, "mayorVillage2");
        Manager.addEntity(mayorVillage2);
        Entity citizensVillage1 = new Entity(character, "citizensVillage1");
        Manager.addEntity(citizensVillage1);
        Entity citizensVillage2 = new Entity(character, "citizensVillage2");
        Manager.addEntity(citizensVillage2);
        Entity bandits = new Entity(character, "bandits");
        Manager.addEntity(bandits);

        BinaryPredicate isAt = new BinaryPredicate(character, "IS_AT", location);
        Manager.addPredicate(isAt);

        UnaryPredicate increaseTaxes = new UnaryPredicate(character, "INCREASE_TAXES");
        Manager.addPredicate(increaseTaxes);
        UnaryPredicate decreaseTaxes = new UnaryPredicate(character, "DECREASE_TAXES");
        Manager.addPredicate(decreaseTaxes);
        
        UnaryPredicate happyAboutTaxes = new UnaryPredicate(character, "HAPPY_ABOUT_TAXES");
        Manager.addPredicate(happyAboutTaxes);
        UnaryPredicate angryAboutTaxes = new UnaryPredicate(character, "ANGRY_ABOUT_TAXES");
        Manager.addPredicate(angryAboutTaxes);

        UnaryPredicate callForHelp = new UnaryPredicate(character, "CALL_FOR_HELP");
        Manager.addPredicate(callForHelp);

        BinaryPredicate attack = new BinaryPredicate(character, "ATTACK", location);
        Manager.addPredicate(attack);

        // citizens are in their own respecting villages
        BinaryRelation CV1IsAtV1 = new BinaryRelation(citizensVillage1, isAt, village1, true);
        BinaryRelation CV2IsAtV2 = new BinaryRelation(citizensVillage2, isAt, village2, true);

        // mayors are in their own respecting villages
        BinaryRelation MV1IsAtV1 = new BinaryRelation(mayorVillage1, isAt, village1, true);
        BinaryRelation MV2IsAtV2 = new BinaryRelation(mayorVillage2, isAt, village2, true);

    }

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

        // BinaryRelation heroIsAtV1 = new BinaryRelation(hero, isAt, v1);
        // BinaryRelation cat_ladyIsAtV1 = new BinaryRelation(cat_lady, isAt, v1);
        // BinaryRelation catIsAtV2 = new BinaryRelation(cat, isAt, v2);

        // UnaryRelation heroHandempty = new UnaryRelation(hero, handempty);


        List<Entity> parameters = new List<Entity>(); 
        Entity c1 = new Entity(character, "c1");
        parameters.Add(c1);
        Entity c2 = new Entity(character, "c2");
        // parameters.Add(c2);        
        Entity l1 = new Entity(location, "l1");
        parameters.Add(l1);
        Entity l2 = new Entity(location, "l2");
        parameters.Add(l2);
        Entity c3 = new Entity(character, "a1");
        // parameters.Add(c3);

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
        // ActionDefinition move = new ActionDefinition(pre, "MOVE", parameters, post);
        // Manager.addActionDefinition(move);

        // Debug.Log(move.ToString());

    }
}
