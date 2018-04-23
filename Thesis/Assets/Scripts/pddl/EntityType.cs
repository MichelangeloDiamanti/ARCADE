using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityType{

    private string _type;

    public string Type{
        get {return _type;}
    }

    public EntityType(string type){
		if(Manager.GetManager().entityTypeExists(type))
			throw new System.ArgumentException("Entity type has already been declared", type);

        _type = type;
    }

	public override bool Equals(object obj)
	{
		var other = obj as EntityType;

		if (other == null)
		{
			return false;
		}

		if(this.Type.Equals(other.Type) == false){
			return false;
		}

		return true;
	}

	public override int GetHashCode()
	{
		return this._type.GetHashCode();
	}

	public override string ToString(){
		return _type;
	}
}
