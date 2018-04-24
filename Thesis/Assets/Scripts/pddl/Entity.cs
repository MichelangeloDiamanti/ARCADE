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
		if(Manager.GetManager().entityExists(type, name))
			throw new System.ArgumentException("Entity has already been declared", type + " " + name);

		_type = type;
		_name = name;
	}

	public override string ToString(){
		return "type: " + _type + " name: " + _name;
	}
	
}
