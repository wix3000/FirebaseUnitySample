using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class RealTimeChat {

    public const string FIREBASE_URL = "https://glowing-heat-7000.firebaseio.com";
    public const string FIREBASE_CHILD = "Message";

    static IFirebase firebase;

    public static Action<FirebaseChangedEventArgs> onDataChange;

    static RealTimeChat() {
        firebase = Firebase.CreateNew(FIREBASE_URL).Child(FIREBASE_CHILD);
        firebase.ValueUpdated += OnValueUpdated;
        firebase.Error += OnError;
    }

    public static IEnumerator Push(string name, string content) {
        IFirebase pushed = firebase.Push();
        pushed.Child("IP").SetValue(Network.player.ipAddress);
        pushed.Child("name").SetValue(name);
        pushed.Child("content").SetValue(content);
        pushed.Child("time").SetValue(DateTime.Now.ToString());
        yield return null;
    }

    static void OnValueUpdated(object sender, FirebaseChangedEventArgs e) {
        if (onDataChange != null) onDataChange(e);
    }

    private static void OnError(object sender, FirebaseErrorEventArgs e) {
        Debug.LogError(e.Error);
    }

    //static void GetChats(Firebase sender, DataSnapshot snapshot) {
    //    Dictionary<string, object> outdata = snapshot.Value<Dictionary<string, object>>();

    //    foreach (string key in outdata.Keys) {
    //        chats.Add(new ChatSet(outdata[key] as Dictionary<string, object>));
    //    }
    //}

    [UnityEditor.MenuItem("Tool/Test Firebase")]
    public static void TestFirebase() {
        Dictionary<string, object> dir = new Dictionary<string, object>();
        dir.Add("Test", "TTTest");
        firebase.Push().SetValue(dir as IDictionary<string, object>);
    }

}