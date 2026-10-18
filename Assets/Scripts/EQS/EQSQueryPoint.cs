using UnityEngine;

namespace EQS
{
    public struct EQSQueryPoint
    {
        public Vector3 Position;
        public float Score;
        public bool Valid;

        public EQSQueryPoint(Vector3 pos)
        {
            Position = pos;
            Score = 1;
            Valid = true;
        }
    }
}
