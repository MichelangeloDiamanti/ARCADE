using System.Collections;
using System.Collections.Generic;

public class Entity {

	private EntityType _type;
	private string _name;

	public EntityType Type
	{
		get { return _type; }  
	}

	public string Name
	{
		get { return _name; }  
	}

	public Entity(EntityType type, string name){
		if(type == null || name == null)
			throw new System.ArgumentNullException("Entity type and name cannot be null");

		_type = type;
		_name = name;
	}

	public override bool Equals(object obj)
	{
		Entity other = obj as Entity;

		if (other == null)
			return false;
		if(_type.Equals(other.Type) == false)
			return false;
		if(_name.Equals(other.Name) == false)
			return false;

		return true;
	}

	public override int GetHashCode()
	{
        unchecked
        {
            int hashCode = 17;
			hashCode = hashCode * -1521134295 + _name.GetHashCode();
			hashCode = hashCode * -1521134295 + _type.GetHashCode();
            return hashCode;
        }
	}

	public override string ToString(){
		return "type: " + _type + " name: " + _name;
	}
	
}
