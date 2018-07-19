using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

namespace ru.cadia.visualization
{
    public class BinaryRelation : ru.cadia.pddlFramework.BinaryRelation
    {
        public BinaryRelation(Entity source, IPredicate predicate, Entity destination, RelationValue value) : base(source, predicate, destination, value)
        {
            if (source == null)
                throw new System.ArgumentNullException("Relation source cannot be null", "source");
            if (predicate == null)
                throw new System.ArgumentNullException("Relation predicate cannot be null", "predicate");
            if (destination == null)
                throw new System.ArgumentNullException("Relation destination cannot be null", "destination");

            if (predicate.GetType() != typeof(ru.cadia.visualization.BinaryPredicate))
                throw new System.ArgumentNullException("Binary relation predicate must be a binary predicate", "predicate");
            
        }
    }
}

