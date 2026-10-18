using UnityEngine;

namespace EQS
{
    public struct EQSContext
    {
        public Transform Querier;
        public Transform Target;

        public EQSContext(Transform querier, Transform target)
        {
            Querier = querier;
            Target = target;
        }

        public static bool operator ==(EQSContext left, EQSContext right)
        {
            return left.Querier == right.Querier &&
                left.Target == right.Target;
        }

        public static bool operator !=(EQSContext left, EQSContext right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
