using UnityEngine;

namespace EQS
{
    [CreateAssetMenu(fileName = "EQSTest", menuName = "Scriptable Objects/EQSTest")]
    public abstract class EQSTest : ScriptableObject
    {
        public abstract float Score(ref EQSQueryPoint point, in EQSContext context);
    }
}
