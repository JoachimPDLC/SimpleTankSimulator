using System.Collections.Generic;
using UnityEngine;

namespace EQS
{
    public struct EQSResult
    {
        public bool Success;
        public Vector3 Position;
        public float HighestScore;
        public float LowestScore;
        public int ChosenPointIndex;
        public List<EQSQueryPoint> AllPoints;
    }
}
