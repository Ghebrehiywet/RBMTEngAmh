using RBMTEngAmh.Models.RBTM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBMTEngAmh.Logic
{


    class StateMachine<T> : IEnumerable<Transition<T>>
    {
        string start;
        public string Name { get; set; }
        HashSet<string> ends = new HashSet<string>();
        Dictionary<string, Dictionary<T, string>> transitions = new Dictionary<string, Dictionary<T, string>>();

        public StateMachine(string name, string start, params string[] ends)
        {
            Name = name;
            this.start = start;
            this.ends.UnionWith(ends);
        }

        public void Add(string from, string to, T value)
        {
            if (transitions.ContainsKey(from) == false)
                transitions[from] = new Dictionary<T, string>();

            transitions[from][value] = to;
        }

        public override string ToString()
        {
            string nodes = string.Join(", ", this.transitions.Keys);
            string values = string.Join(", ", this.transitions[start].Keys);
            string ends = string.Join(", ", this.ends);
            string transitions = string.Join("\n   ", this);

            return $"{Name} = ({{{nodes}}}, {{{values}}}, Q, {start}, {{{ends}}})\nQ: {transitions}";
        }

        public IEnumerator<Transition<T>> GetEnumerator()
        {
            return transitions.SelectMany(
                node => node.Value.Select(to => new Transition<T>(node.Key, to.Value, to.Key))).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Accepts(IEnumerable<T> @string)
        {
            string state = start;

            if (@string != null)
            {
                foreach (T value in @string)
                {
                    try
                    {
                        state = transitions[state][value];
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return ends.Contains(state);
        }
    }

    class Transition<T>
    {
        public string From { get; }
        public string To { get; }
        public T Value { get; }

        public Transition(string from, string to, T value)
        {
            From = from;
            To = to;
            Value = value;
        }

        public override string ToString() => $"q({From}, {Value}) = {To}";
    }

    public class WordPropNoun
    {
        public string WordRule { get; set; }
        public string RootWordPlural { get; set; }
        public string RootWord { get; set; }
        public string sourceLangValue { get; set; }
        public WordType Type { get; set; }   //  Type; 0 = Regular, 1 = Irregular, 2 = Unchanged
        public Number number { get; set; } //  1 = singlular, 2 = pulural
        public bool selected { get; set; }
        public string PluralForIrregular { get; set; }
        public WordPOSType WordPOSType { get; set; }
        public Gender Gender { get; set; }
        public string Translated { get; set; }
    }
    public class WordPropVerb : WordPropNoun
    {
        public string IrregularPastVerb { get; internal set; }
        public string IrregularPPVerb { get; internal set; }

        public string RegularPastVerb { get; internal set; }
        public string RegularPPVerb { get; internal set; }

        public string PresentParticiple { get; internal set; }
        public string ThirpPersonSinglular { get; internal set; }
    }

}