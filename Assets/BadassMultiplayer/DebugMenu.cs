using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DebugMenu : MonoBehaviour
{
    public static bool isHost;
    public UnityEngine.UI.Toggle hostToggle;
    public BadassServer badassServer;
    public TMP_InputField ipTextField;
    public TextMeshProUGUI localServerText;
    public static string IP;
    public void ConnectClicked()
    {
        IP = ipTextField.text;
        isHost = hostToggle.isOn;
        if (hostToggle.isOn)
        {
            badassServer.Init();
            DontDestroyOnLoad(badassServer.gameObject);
        }
        KNetworkManager.instance.Init();
        SceneManager.LoadScene("MP_Debug");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ipTextField.text == "127.0.0.1")
        {
            localServerText.gameObject.SetActive(true);
        }
        else
        {
            localServerText.gameObject.SetActive(false);
        }
    }
}
