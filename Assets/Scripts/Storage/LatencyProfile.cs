using UnityEngine;

[CreateAssetMenu(fileName="LatencyProfile", menuName="LatencyProfile")]
public class LatencyProfile : ScriptableObject
{
    public int latencyOnSending = 0;
    public int latencyOnReceiving = 0;
    public int variance = 0;
    public float nodeFailureRate = 0;
}