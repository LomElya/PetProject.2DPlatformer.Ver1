using UnityEngine;

    [System.Serializable]
    public struct MinMaxFloat
    {
        public float Min;
        public float Max;

    }

    [System.Serializable]
    public struct MinMaxColor
    {
        [ColorUsage(true, true)] public Color Min;
        [ColorUsage(true, true)] public Color Max;

    }
