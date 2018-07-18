using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ru.cadia.pddlFramework;

namespace ru.cadia.visualization
{
    public class BinaryPredicate : ru.cadia.pddlFramework.BinaryPredicate
    {

        private string _text;

        public string Text
        {
            get { return _text; }
        }

        public BinaryPredicate(EntityType source, string name, EntityType destination, string text) : base(source, name, destination)
        {
            _text = text;
        }
    }
}

