using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;

public class GetIP : MonoBehaviour {

    public Text address;
	void Start () 
    {
        
        address.text = "Host IP: " + GetLocalIPAddress();
	}

    string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        Debug.LogError("Local IP Address not found");
        return "";
    }
}

