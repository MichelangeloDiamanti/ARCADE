using System.Collections;
using System.Collections.Generic;
using ru.cadia.pddlFramework;
using UnityEngine;

namespace ru.cadia.visualization
{
    public class UnaryRelation : ru.cadia.pddlFramework.BinaryRelation
    {
        public UnaryRelation(Entity source, IPredicate predicate, Entity destination, RelationValue value) : base(source, predicate, destination, value)
        {
        }
    }
}

