using System.Collections;
using System.Collections.Generic;
using ru.cadia.pddlFramework;
using UnityEngine;

namespace ru.cadia.visualization
{
    public class UnaryRelation : ru.cadia.pddlFramework.UnaryRelation
    {
        public UnaryRelation(Entity source, IPredicate predicate, RelationValue value) : base(source, predicate, value)
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
        }
    }

}

