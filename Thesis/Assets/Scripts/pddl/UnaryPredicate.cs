using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ru.cadia.pddlFramework
{
    /// <summary>
    /// </summary>
    public class UnaryPredicate : IPredicate, System.IEquatable<IPredicate>
    {

        private EntityType _source;
        private string _name;

        public EntityType Source
        {
            get { return _source; }
        }
        public string Name
        {
            get { return _name; }
        }

        public UnaryPredicate(EntityType source, string name)
        {
            if (source == null)
                throw new System.ArgumentNullException("Predicate source type cannot be null", "source type");
            if (name == null)
                throw new System.ArgumentNullException("Predicate name cannot be null", "name");

            _source = source;
            _name = name;
        }

        public override int GetHashCode()
        {
            int hash = _source.GetHashCode() * 17;
            hash += _name.GetHashCode() * 17;
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(UnaryPredicate))
                return false;

            UnaryPredicate otherPredicate = obj as UnaryPredicate;
            if (_source.Equals(otherPredicate.Source) == false)
                return false;
            if (_name.Equals(otherPredicate.Name) == false)
                return false;
            return true;
        }
        public IPredicate Clone()
        {
            return new UnaryPredicate(_source.Clone(), _name);
        }

        public override string ToString()
        {
            return _source + " " + _name;
        }

        public bool Equals(IPredicate other)
        {
            if (other.GetType() != typeof(UnaryPredicate))
                return false;
            if (_source.Equals(other.Source) == false)
                return false;
            if (_name.Equals(other.Name) == false)
                return false;
            return true;
        }
    }
}
