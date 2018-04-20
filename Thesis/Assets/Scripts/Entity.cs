using System.Collections;
using System.Collections.Generic;

public class Entity {
	private long _id;
	private string _type;
	private string _name;
	private bool _real;
	private int _world;
	private List<Relation> _relations;

	public long ID
	{
		get { return _id; }  
	}

	public string Type
	{
		get { return _type; }  
	}

	public string Name
	{
		get { return _name; }  
	}
	public bool Real
	{
		get { return _real; }  
	}
	public int World
	{
		get { return _world; }  
	}
	public List<Relation> Relations
	{
		get { return _relations; }
		set { _relations = value; }
	}

	public Entity(long ID, string Type, string Name, bool Real, int World){
		_id = ID;
		_type = Type;
		_name = Name;
		_real = Real;
		_world = World;
	}
	
}
