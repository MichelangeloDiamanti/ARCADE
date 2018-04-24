using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class State{
	
	private static List<IRelation> _relations = new List<IRelation>();
	public List<IRelation> Relations
	{
		get { return _relations; }  
	}

	public  static void AddRelation(IRelation r){
		if(relationExists(r) == false)
			_relations.Add(r);
	}

	public State(List<IRelation> relations){

		if(relations == null || relations.Count == 0)
			 throw new System.ArgumentException("List of relations cannot be null or empty", "List<IRelation> relations");
		
		_relations = relations;
	}

	public static bool relationExists(IRelation relation){
        foreach(IRelation r in _relations){
            if(r.GetType() == typeof(BinaryRelation) && relation == typeof(BinaryRelation))
            {
                BinaryRelation br1 = r as BinaryRelation;
				BinaryRelation br2 = relation as BinaryRelation;
                if(br1.Equals(br2) == false){
                    return true;
                }
            }
			else if(r.GetType() == typeof(UnaryRelation) && relation == typeof(UnaryRelation))
            {
                UnaryRelation br1 = r as UnaryRelation;
				UnaryRelation br2 = relation as UnaryRelation;
                if(br1.Equals(br2) == false){
                    return true;
                }
            }
        }
        return false;
    }


}
