using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;

        public TextBuild build = null;
        private bool userPrompt = false;
        public ConversationManager(TextBuild build)
        {
            this.build = build;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }
        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConvo(conversation));
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }
        IEnumerator RunningConvo(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                if (line.hasDialogue)
                    yield return Line_RunDialogue(line);

                if (line.hasCommands)
                    yield return Line_RunCommands(line);

            
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //If there is a speaker show name, otherwise hide it.
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);
            else
                dialogueSystem.HideSpeakerName();

            //Building dialogue
            yield return BuildDialogue(line.dialogue);

            //wait for user input
            yield return WaitForUserInput();
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        IEnumerator BuildDialogue(string dialogue)
        {
            build.Build(dialogue);

            while (build.isBuilding)
            {
                if (userPrompt)
                {
                    if (!build.speedUp)
                        build.speedUp = true;
                    else
                        build.ForceComplete();

                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
                yield return null;

            userPrompt = false;
        }
    }
}