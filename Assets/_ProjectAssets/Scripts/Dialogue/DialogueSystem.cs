using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextBuild build;

        public static DialogueSystem instance;

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        public bool isRunningConvo => conversationManager.isRunning;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Initialize();
            }
            else
                DestroyImmediate(gameObject);
        }
        bool _initialized = false;
        private void Initialize()
        {
            if (_initialized)
                return;

            build = new TextBuild(dialogueContainer.dialogueTxt);
            conversationManager = new ConversationManager(build);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }
        public void ShowSpeakerName(string speakerName = "") => dialogueContainer.nameContainer.Show(speakerName);
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();
        public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            Say(conversation);
        }

        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }
    }
}