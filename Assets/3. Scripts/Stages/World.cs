using System;
using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Stages
{
    [Serializable]
    public class World
    {
        [SerializeField] private int id;
        [SerializeField] private List<Stage> stages = new ();

        public int ID => id;
        public List<Stage> Stages => stages;
    }
}