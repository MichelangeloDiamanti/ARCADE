using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PDDL
{
    /// <summary>
    /// </summary>
    public interface IRelation
    {
        Entity Source { get; }
        IPredicate Predicate { get; }
        RelationValue Value { get; }
        bool EqualsThroughPredicate(IRelation other);
        bool EqualsWithoutValue(IRelation other);
        bool Equals(object obj);
        IRelation Clone();
        int GetHashCode();
        string ToString();
    }
}
