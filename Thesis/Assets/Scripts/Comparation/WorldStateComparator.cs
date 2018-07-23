using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;
using CollectionUtils;

public class WorldStateComparator
{

    private HashSet<WorldStateComparated> _worldStateComparated;
    private HashSet<KeyValuePair<WorldStateComparated, WorldStateComparated>> _listCoupleDone;
    private HashSet<CommonRelations> _allCommonRelations;

    public WorldStateComparator(HashSet<WorldStateComparated> hash)
    {
        _worldStateComparated = hash;
        _listCoupleDone = new HashSet<KeyValuePair<WorldStateComparated, WorldStateComparated>>();
        _allCommonRelations = new HashSet<CommonRelations>();
    }

    public void Compare()
    {
        foreach (WorldStateComparated item in _worldStateComparated)
        {
            foreach (WorldStateComparated wsc in _worldStateComparated)
            {
                KeyValuePair<WorldStateComparated, WorldStateComparated> kvp = new KeyValuePair<WorldStateComparated, WorldStateComparated> (item, wsc);
                KeyValuePair<WorldStateComparated, WorldStateComparated> kvpr = new KeyValuePair<WorldStateComparated, WorldStateComparated> (wsc, item);
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
        Debug.Log(_allCommonRelations.Count);
        printCommonRelations();
    }

    private CommonRelations CompareRelations(WorldStateComparated first, WorldStateComparated second)
    {
        HashSet<IRelation> commonRelations = new HashSet<IRelation>();
        foreach (IRelation item in first.DifferentRelations)
        {
            if (second.DifferentRelations.Contains(item))
            {
                commonRelations.Add(item.Clone());
            }
        }
        return new CommonRelations(first, second, commonRelations);
    }

    private void printCommonRelations()
    {
        string result = "";
        foreach (CommonRelations item in _allCommonRelations)
        {
            result += item.ToString();
            result += "\n";
        }
        new FileWriter().SaveFile(FileWriter.GenerateLogName(), result);
    }
}
