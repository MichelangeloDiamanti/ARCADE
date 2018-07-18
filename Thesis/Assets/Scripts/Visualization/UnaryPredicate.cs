using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ru.cadia.visualization
{
    public class UnaryPredicate : ru.cadia.pddlFramework.UnaryPredicate
    {
        private string _text;

        public string Text
        {
            get { return _text; }
        }

        public UnaryPredicate(EntityType source, string name, string text) : base(source, name)
        {
            if (text == null)
                throw new System.ArgumentNullException("Predicate text cannot be null", "text");

            _text = text;
        }
    }
}

