using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectButton : MonoBehaviour
{
    public Text Failure;

    Button connectButton;
    bool connectCheck = false;
    bool failedConnect = false;

    void Start()
    {
        connectButton = GetComponent<Button>();

        connectButton.onClick.AddListener(AttemptConnect);
    }

    void Update()
    {
        if (connectCheck)
        {
            if (NetworkManager.ListenUpdate() == 0)
            {
                SceneManager.LoadScene(9);
            }
        }
        if (ControllerManager.GetButtonDown(0) == true && NetworkToggle.joingameisago == true && Time.time > SelectVert.NextA)
        {
            AttemptConnect();
        }
    }

    void AttemptConnect()
    {
        NetworkManager.SetIP(CheckInput.IPAddress);
        NetworkManager.Init();
        connectCheck = true;

        
    }

    IEnumerator AttemptMethod()
    {
        Failure.enabled = true;
        yield return new WaitForSeconds(2.0f);
        Failure.enabled = false;
    }
}
