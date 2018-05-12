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
		var other = obj as Entity;

		if (other == null)
		{
			return false;
		}

		if(this.Type.Equals(other.Type) == false){
			return false;
		}

		if(this.Name.Equals(other.Name) == false){
			return false;
		}

		return true;
	}

	public override int GetHashCode()
	{
		string s = _name + _type;
		return s.GetHashCode();
	}

	public override string ToString(){
		return "type: " + _type + " name: " + _name;
	}
	
}
