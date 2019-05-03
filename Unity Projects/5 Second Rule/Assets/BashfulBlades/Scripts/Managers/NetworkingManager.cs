using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class NetworkManager : MonoBehaviour
{
    [DllImport("NetworkManager")]
    public static extern void Init();

    [DllImport("NetworkManager")]
    public static extern void Shutdown();

    [DllImport("NetworkManager")]
    public static extern System.IntPtr GetRecvU();

    [DllImport("NetworkManager")]
    public static extern System.IntPtr GetRecvT();

    [DllImport("NetworkManager")]
    public static extern System.IntPtr GetStatus();

    [DllImport("NetworkManager")]
    public static extern int StartUDP();

    [DllImport("NetworkManager")]
    public static extern int ServerSendStart();

    [DllImport("NetworkManager", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SendPackU(string info);

    [DllImport("NetworkManager", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SendPackT(string info);

    [DllImport("NetworkManager", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetIP(string Address);

    [DllImport("NetworkManager")]
    public static extern int ListenStart();

    [DllImport("NetworkManager")]
    public static extern int ListenUpdate();

    [DllImport("NetworkManager")]
    public static extern int ListenEnd();

    [DllImport("NetworkManager")]
    public static extern bool ConnectionCheck();

    [DllImport("NetworkManager")]
    public static extern bool GetIsServer();

    [DllImport("NetworkManager")]
    public static extern int ClientCount();
}