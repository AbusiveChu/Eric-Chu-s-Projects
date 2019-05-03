using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

[System.Serializable]
public class LobbyEvent : UnityEvent<int, string, int> { }

