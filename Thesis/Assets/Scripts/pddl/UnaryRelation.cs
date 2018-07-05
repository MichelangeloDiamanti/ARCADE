using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ru.cadia.pddlFramework
{
    /// <summary>
    /// </summary>
    public class UnaryRelation : IRelation, System.IEquatable<IRelation>
    {

        private Entity _source;
        private UnaryPredicate _predicate;
        private RelationValue _value;
        public Entity Source
        {
            get { return _source; }
        }
        public IPredicate Predicate
        {
            get { return _predicate; }
        }
        public RelationValue Value
        {
            get { return _value; }
        }
        public UnaryRelation(Entity source, IPredicate predicate, RelationValue value)
        {
            if (source == null)
                throw new System.ArgumentNullException("Relation source cannot be null", "source");
            if (predicate == null)
                throw new System.ArgumentNullException("Relation predicate cannot be null", "predicate");
            if (predicate.GetType() != typeof(UnaryPredicate))
                throw new System.ArgumentNullException("Unary relation predicate must be a unary predicate", "predicate");

            UnaryPredicate unaryPredicate = predicate as UnaryPredicate;
            if (source.Type.Equals(predicate.Source) == false)
                throw new System.ArgumentException("Relation source is not of the specified predicate type", source + " " + predicate.Source);

            _source = source;
            _predicate = unaryPredicate;
            _value = value;
        }

        public override string ToString()
        {
            return _source.Name + " " + _predicate.Name + ": " + _value;
        }

        public override int GetHashCode()
        {
            int hashCode = _source.GetHashCode() * 17;
            hashCode += _predicate.GetHashCode() * 17;
            hashCode += (int)_value * 17;
            return hashCode;
        }
        public IPredicate getPredicate()
        {
            return _predicate;
        }

        public IRelation Clone() { return new UnaryRelation(_source.Clone(), _predicate.Clone() as UnaryPredicate, _value); }

        public bool Equals(IRelation other)
        {
            if (other == null)
                return false;

            if (other.GetType() != typeof(UnaryRelation))
                return false;

            if (_source.Equals(other.Source) == false)
                return false;

            if (_predicate.Equals(other.Predicate) == false)
                return false;

            if (_value.Equals(other.Value) == false)
                return false;
            return true;
        }

        public bool EqualsThroughPredicate(IRelation other)
        {
            if (other == null)
                return false;

            UnaryRelation otherUnaryRelation = other as UnaryRelation;
            if (_source.Type.Equals(otherUnaryRelation.Source.Type) == false)
                return false;

            if (_predicate.Equals(otherUnaryRelation.Predicate) == false)
                return false;

            return true;
        }

        public bool EqualsWithoutValue(IRelation other)
        {
            if (other == null)
                return false;

            if (other.GetType() != typeof(UnaryRelation))
                return false;

            UnaryRelation otherUnaryRelation = other as UnaryRelation;
            if (_source.Equals(otherUnaryRelation.Source) == false)
                return false;
            if (_predicate.Equals(otherUnaryRelation.Predicate) == false)
                return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(UnaryRelation))
                return false;

            UnaryRelation other = obj as UnaryRelation;
            if (other.Source.Equals(_source) == false)
                return false;
            if (other.Predicate.Equals(_predicate) == false)
                return false;

            if (other.Value.Equals(_value) == false)
                return false;

            return true;
        }
    }
}