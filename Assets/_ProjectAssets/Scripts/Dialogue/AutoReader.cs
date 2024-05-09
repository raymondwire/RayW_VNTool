using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    public class AutoReader : MonoBehaviour
    {
        private const int DEFAULT_CHARACTER_READ_PER_SECOND = 18;
        private const float READ_TIME_PADDING = 0.5F;
        private const float MIN_READ_TIME = 1f;
        private const float MAX_READ_TIME = 99f;
        private const string STATUS_TEXT_AUTO = "Auto";
        private const string STATUS_TEXT_SKIP = "Skip";

        private ConversationManager conversationManager;
        private TextBuild build => conversationManager.build;

        public bool skip { get; set; } = false;
        public float speed { get; set; } = 1f;

        public bool isOn => co_running != null;
        private Coroutine co_running = null;

        [SerializeField] private TextMeshProUGUI statusText;

        public void Initialize(ConversationManager conversationManager)
        {
            this.conversationManager = conversationManager;

            statusText.text = string.Empty;
        }

        public void Enable()
        {
            if (isOn)
                return;

            co_running = StartCoroutine(AutoRead());
        }

        public void Disable()
        {
            if (!isOn)
                return;

            StopCoroutine(co_running);
            skip = false;
            co_running = null;
            statusText.text = string.Empty;
        }

        private IEnumerator AutoRead()
        {
            if (!conversationManager.isRunning)
            {
                Disable();
                yield break;
            }
            if (!build.isBuilding && build.currentTxt != string.Empty)
                DialogueSystem.instance.OnUserPrompt_Next();

            while (conversationManager.isRunning)
            {
               //Read & wait
                if (!skip)
                {
                    while (!build.isBuilding)
                        yield return null;

                    float timeStarted = Time.time;

                    while (build.isBuilding)
                        yield return null;

                    float timeToRead = Mathf.Clamp(((float)build.tmpro.textInfo.characterCount / DEFAULT_CHARACTER_READ_PER_SECOND), MIN_READ_TIME, MAX_READ_TIME);
                    timeToRead = Mathf.Clamp((timeToRead - (Time.time - timeStarted)), MIN_READ_TIME, MAX_READ_TIME);
                    timeToRead = (timeToRead / speed) + READ_TIME_PADDING;

                    yield return new WaitForSeconds(timeToRead);
                }
                //skip
                else
                {
                    build.ForceComplete();
                    yield return new WaitForSeconds(0.05f);
                }
                DialogueSystem.instance.OnUserPrompt_Next();          
            }
            Disable();
        }

        public void Toggle_Auto()
        {
            bool prevState = skip;
            skip = false;

            if (prevState)
                Enable();
            else
            {
                if (!isOn)
                    Enable();
                else
                    Disable();
            }

            statusText.text = STATUS_TEXT_AUTO;
        }

        public void Toggle_Skip()
        {
            bool prevState = skip;
            skip = true;

            if (!prevState)
                Enable();
            else
            {
                if (!isOn)
                    Enable();
                else
                    Disable();
            }

            statusText.text = STATUS_TEXT_SKIP;
        }
    }
}
