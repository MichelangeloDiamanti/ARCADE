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
            
        }
    }
}

