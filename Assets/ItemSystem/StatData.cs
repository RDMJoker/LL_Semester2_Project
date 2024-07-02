using System;

namespace ItemSystem
{
    [Serializable]
    public class StatData
    {
        public bool IsElevated;
        public float StatValue;
        public int TierValue;

        public override string ToString()
        {
            return $"Value: {StatValue} -- Tier: {TierValue} -- IsElevated: {IsElevated}";
        }
    }
}