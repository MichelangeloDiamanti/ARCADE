using System.Collections;
using System.Collections.Generic;

public class EntityType{

    private string _type;

    public string Type{
        get {return _type;}
    }

    public EntityType(string type){
		if(type == null)
			throw new System.ArgumentNullException("EntityType cannot be null", "type");

        _type = type;
    }

	public override bool Equals(object obj)
	{
		EntityType other = obj as EntityType;

		if (other == null)
			return false;

		if(_type.Equals(other.Type) == false)
			return false;

		return true;
	}

	public override int GetHashCode()
	{
		return _type.GetHashCode();
	}

	public override string ToString(){
		return _type;
	}

	public EntityType Clone(){ return new EntityType(_type); }

}
