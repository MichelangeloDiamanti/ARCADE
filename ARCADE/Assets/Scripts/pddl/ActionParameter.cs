using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace ru.cadia.pddlFramework
{
    public class ActionParameter : Entity, System.IEquatable<ActionParameter>
    {
        private ActionParameterRole _role;

        public ActionParameterRole Role
        {
            get { return _role; }
        }

        [JsonConstructor]
        public ActionParameter(EntityType type, string name, ActionParameterRole role) : base(type, name)
        {
            _role = role;
        }

        public ActionParameter(Entity baseEntity, ActionParameterRole role) : base(baseEntity.Type, baseEntity.Name)
        {
            _role = role;
        }

        public new ActionParameter Clone()
        {
            Entity baseEntity = new Entity(base.Type, base.Name);
            return new ActionParameter(baseEntity, _role);
        }

        public bool Equals(ActionParameter other)
        {
            if (other == null)
                return false;
            if (base.Type.Equals(other.Type) == false)
                return false;
            if (base.Name.Equals(other.Name) == false)
                return false;
            if (_role.Equals(other.Role) == false)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            ActionParameter other = obj as ActionParameter;

            if (other == null)
                return false;
            if (base.Type.Equals(other.Type) == false)
                return false;
            if (base.Name.Equals(other.Name) == false)
                return false;
            if (_role.Equals(other.Role) == false)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + _role.GetHashCode() * 17;
        }
    }
}