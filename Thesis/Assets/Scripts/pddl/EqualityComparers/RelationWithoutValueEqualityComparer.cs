using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationWithoutValueEqualityComparer : IEqualityComparer<IRelation>
{
    public bool Equals(IRelation x, IRelation y)
    {
        if (y == null && x == null)
            return true;
        else if (x == null | y == null)
            return false;

        if (x.GetType() != y.GetType())
            return false;

        if (x.Source.Equals(y.Source) == false)
            return false;

        if (x.Predicate.Equals(y.Predicate) == false)
            return false;

        if (x.GetType() == typeof(BinaryRelation) &&
            y.GetType() == typeof(BinaryRelation))
        {
            BinaryRelation binaryRelationx = x as BinaryRelation;
            BinaryRelation binaryRelationy = y as BinaryRelation;
            if (binaryRelationx.Destination.Equals(binaryRelationy.Destination) == false)
                return false;
        }

        return true;
    }

    public int GetHashCode(IRelation relation)
    {
        int hashCode = relation.Source.GetHashCode() * 17;
        hashCode += relation.Predicate.GetHashCode() * 17;
        if(relation.GetType() == typeof(BinaryRelation))
        {
            BinaryRelation binaryRelation = relation as BinaryRelation;
            hashCode += binaryRelation.Destination.GetHashCode() * 17;
        }
        return hashCode;
    }
}
