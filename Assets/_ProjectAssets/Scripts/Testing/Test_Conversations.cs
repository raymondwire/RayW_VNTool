using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class Test_Conversations : MonoBehaviour
{
    void Start()
    {
        StartConversation();
    }
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("TestFile");

        DialogueSystem.instance.Say(lines);
    }
}
