using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using ExitGames.Client.Photon;

public class ChatScript : MonoBehaviour,IChatClientListener
{
    private ChatClient _chat;
    private string _userName;
    private string _string;
    [SerializeField]
    private Button _exit;
    [SerializeField]
    private Text _txt;
    [SerializeField]
    private InputField _input;
    [SerializeField]
    private InputField _userNameInput;
    private string _chatName = "chat1";
    

    private void Start()
    {
        _input.text = "";
        _exit.onClick.AddListener(OnQuit);
        _chat = new ChatClient(this);
        _chat.UseBackgroundWorkerForSending = true;
        _chat.Connect(PhotonNetwork.PhotonServerSettings.AppID, "ver 0.01", new Photon.Chat.AuthenticationValues(_userName));
    }

    private void Update()
    {
        SetNames();
        if (_chat != null)
        {
            _chat.Service();
        }
        CheckPlaceHolder();
        if ((Input.GetKeyDown(KeyCode.Return) || _input.touchScreenKeyboard != null && _input.touchScreenKeyboard.status == TouchScreenKeyboard.Status.Done)
            && !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_input.text))
        {
            OnSend();
        }
    }

    private void SetNames()
    {
        if (!string.IsNullOrEmpty(_userNameInput.text))
        {
            _userName = _userNameInput.text;
        }
    }

    private void CheckPlaceHolder()
    {
        _input.placeholder.gameObject.SetActive(!_input.isFocused);
        _userNameInput.placeholder.gameObject.SetActive(!_userNameInput.isFocused);
    }

    private void OnSend()
    {
        _chat.PublishMessage(_chatName,_userName +": " + _input.text + "\n");
        _input.text = "";
        Debug.Log("Message:" + _input.text);
    }
    private void OnQuit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("DebugLevel: " + level);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("ChatState: " + state);
    }

    public void OnConnected()
    {
        Debug.Log("Connected");
        _chat.Subscribe(new string[]{ _chatName},30);
        _chat.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconected");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
     
        for (int i = 0; i < messages.Length; i++)
        {
            _string += messages[i];
        }
        _txt.text = _string;
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log(message + "from:" + channelName);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log(user + "new status:" + gotMessage + message);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed:" + channels + results);
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("UnSubscribed:" + channels);
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log("UserSubscribed:" + user + channel);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log("UserUnSubscribed:" + user + channel);
    }
}
