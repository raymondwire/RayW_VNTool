
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject base_;
        public TextMeshProUGUI nameTxt;
        public TextMeshProUGUI dialogueTxt;
        public NameContainer nameContainer = new NameContainer();
    }
}