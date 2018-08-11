using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;
using CollectionUtils;

public class WorldStateComparator
{

    private HashSet<WorldStateComparated> _worldStateComparated;
    private HashSet<KeyValuePair<WorldStateComparated, WorldStateComparated>> _listCoupleDone;
    private HashSet<CommonRelation> _allCommonRelations;
    private Dictionary<IPredicate, int> _countPredicatesOnWorldSate;
    private Dictionary<IPredicate, int> _countPredicatesOnActionsPreConditions, _countPredicatesOnActionsPostConditions;
    private Domain _currentDomain;

    public WorldStateComparator(HashSet<WorldStateComparated> hash)
    {
        _worldStateComparated = hash;
        _listCoupleDone = new HashSet<KeyValuePair<WorldStateComparated, WorldStateComparated>>();
        _allCommonRelations = new HashSet<CommonRelation>();
        _countPredicatesOnWorldSate = new Dictionary<IPredicate, int>();
        _countPredicatesOnActionsPreConditions = new Dictionary<IPredicate, int>();
        _countPredicatesOnActionsPostConditions = new Dictionary<IPredicate, int>();
        HashSet<IPredicate> _predicate = new HashSet<IPredicate>();
        foreach (WorldStateComparated item in _worldStateComparated)
        {
            _predicate = item.CurrentState.Domain.Predicates;
            _currentDomain = item.CurrentState.Domain.Clone();
            break;
        }

        foreach (IPredicate item in _predicate)
        {
            _countPredicatesOnWorldSate.Add(item, 0);
            _countPredicatesOnActionsPreConditions.Add(item, 0);
            _countPredicatesOnActionsPostConditions.Add(item, 0);
        }
    }

    public void Compare()
    {
        //This method compare each worldstatecomparated to make an analysis on all the relations that are in common
        foreach (WorldStateComparated item in _worldStateComparated)
        {
            foreach (WorldStateComparated wsc in _worldStateComparated)
            {
                KeyValuePair<WorldStateComparated, WorldStateComparated> kvp = new KeyValuePair<WorldStateComparated, WorldStateComparated>(item, wsc);
                KeyValuePair<WorldStateComparated, WorldStateComparated> kvpr = new KeyValuePair<WorldStateComparated, WorldStateComparated>(wsc, item);
                if (!(_listCoupleDone.Contains(kvp) || _listCoupleDone.Contains(kvpr)))
                {
                    if (!item.Equals(wsc))
                    {
                        _allCommonRelations.Add(CompareRelations(item, wsc));
                        _listCoupleDone.Add(kvp);
                    }
                }
            }
        }
        printCommonRelations();
        //Counting occurance of each predicate inside common relations of each worldstatecomparated
        foreach (CommonRelation item in _allCommonRelations)
        {
            foreach (IRelation rel in item.CommonRelations)
            {
                _countPredicatesOnWorldSate[rel.Predicate]++;
            }
        }
        string res = "CommonRelations Preconditions:\n";
        foreach (IPredicate item in _countPredicatesOnWorldSate.Keys)
        {
            res += item.Name + " : " + _countPredicatesOnWorldSate[item] + "\n";
        }
        //Making analysis on the actions of the domain to check the occurence of the predicates
        foreach (Action item in _currentDomain.Actions)
        {
            foreach (IRelation ip in item.PreConditions)
            {
                _countPredicatesOnActionsPreConditions[ip.Predicate]++;
            }
            foreach (IRelation ip in item.PostConditions)
            {
                _countPredicatesOnActionsPostConditions[ip.Predicate]++;
            }
        }
        res += "Action preconditions predicates:\n";
        foreach (IPredicate item in _countPredicatesOnActionsPreConditions.Keys)
        {
            res += item.Name + " : " + _countPredicatesOnActionsPreConditions[item] + "\n";
        }
        res += "Action postConditions predicates:\n";
        foreach (IPredicate item in _countPredicatesOnActionsPostConditions.Keys)
        {
            res += item.Name + " : " + _countPredicatesOnActionsPostConditions[item] + "\n";
        }
        Debug.Log(res);

    }

    private CommonRelation CompareRelations(WorldStateComparated first, WorldStateComparated second)
    {
        HashSet<IRelation> commonRelations = new HashSet<IRelation>();
        foreach (IRelation item in first.DifferentRelations)
        {
            if (second.DifferentRelations.Contains(item))
            {
                commonRelations.Add(item.Clone());
            }
        }
        return new CommonRelation(first, second, commonRelations);
    }

    private void printCommonRelations()
    {
        string result = "";
        foreach (CommonRelation item in _allCommonRelations)
        {
            result += item.ToString();
            result += "\n";
        }
        new FileWriter().SaveFile(FileWriter.GenerateLogName(), result);
    }
}
