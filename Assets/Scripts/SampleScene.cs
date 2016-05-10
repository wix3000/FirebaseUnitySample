using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class SampleScene : MonoBehaviour {

    [SerializeField]
    InputField nameField, contentField;
    [SerializeField]
    Text showText;

    Dictionary<string, object> snapshots = new Dictionary<string, object>();

	// Use this for initialization
	void Start () {
        RealTimeChat.onDataChange += OnDataChange;
    }
	
	// Update is called once per frame
	void Update () {
	    if(snapshots != null) {
            Refresh();
            snapshots = null;
        }
	}

    /// <summary>
    /// 重新整理
    /// </summary>
    void Refresh() {

        StringBuilder sb = new StringBuilder();
        List<string> keys = new List<string>(snapshots.Keys);
        keys.Sort((x, y) => (snapshots[x] as Dictionary<string, object>)["time"].ToString().CompareTo(
                            (snapshots[y] as Dictionary<string, object>)["time"].ToString()));

        for (int i = 0; i < keys.Count; i++) {
            Dictionary<string, object> chat = snapshots[keys[i]] as Dictionary<string, object>;
            sb.Append((chat["IP"].ToString() == Network.player.ipAddress) ? "您" : chat["name"]);
            sb.Append(" : ");
            sb.Append(chat["content"]);
            sb.Append("  (" + chat["time"] + ")\n");
        }

        showText.text = sb.ToString();
    }

    void OnDataChange(FirebaseChangedEventArgs e) {
        snapshots = e.DataSnapshot.DictionaryValue;
    }

    /// <summary>
    /// 按下發送時的事件
    /// </summary>
    public void OnSendClick() {
        if (nameField == null || contentField == null) return;

        StartCoroutine(RealTimeChat.Push(nameField.text, contentField.text));
        contentField.text = "";
    }

    /// <summary>
    /// 檢查姓名是否為空
    /// </summary>
    public void OnNameEndEdit() {
        if(nameField.text == "") {
            nameField.text = "不具名";
        }
    }
}
