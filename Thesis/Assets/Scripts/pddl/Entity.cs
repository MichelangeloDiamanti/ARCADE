using System.Collections;
using System.Collections.Generic;

namespace PDDL
{
    /// <summary>
    /// </summary>
    public class Entity : System.IEquatable<Entity>
    {

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

        public Entity(EntityType type, string name)
        {
            if (type == null || name == null)
                throw new System.ArgumentNullException("Entity type and name cannot be null");

            _type = type;
            _name = name;
        }

        public override bool Equals(object obj)
        {
            Entity other = obj as Entity;

            if (other == null)
                return false;
            if (_type.Equals(other.Type) == false)
                return false;
            if (_name.Equals(other.Name) == false)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() * 17 + _type.GetHashCode();
        }

        public override string ToString()
        {
            return "type: " + _type + " name: " + _name;
        }

        public Entity Clone() { return new Entity(_type.Clone(), _name); }

        public bool Equals(Entity other)
        {
            if (other == null)
                return false;
            if (_type.Equals(other.Type) == false)
                return false;
            if (_name.Equals(other.Name) == false)
                return false;

            return true;
        }
    }
}
