using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnaryPredicate : IPredicate {

	private EntityType _source;
	private string _name;

	public string Name
	{
		get { return _name; }
	}

	public EntityType Source
	{
		get { return _source; }
	}

	public UnaryPredicate(EntityType source, string name){
		if(source == null)
			throw new System.ArgumentNullException("Predicate source type cannot be null", "source type");
		if(name == null)
			throw new System.ArgumentNullException("Predicate name cannot be null", "name");

		_source = source;
		_name = name;
	}

	public override string ToString(){
		return _source + " " + _name; 
	}

    public override bool Equals(object obj)
    {
		if (obj == null)
			return false;

        if(obj.GetType() != typeof(UnaryPredicate))
            return false;

		UnaryPredicate otherPredicate = obj as UnaryPredicate;
        if(_source.Equals(otherPredicate.Source) == false)
            return false;
        if(_name.Equals(otherPredicate.Name) == false)
            return false;
        return true;
    }

    public override int GetHashCode()
    {
        int hashCode = 181846194;
        hashCode = hashCode * -1521134295 + _source.GetHashCode();
        hashCode = hashCode * -1521134295 + _name.GetHashCode();
        return hashCode;
    }

}
