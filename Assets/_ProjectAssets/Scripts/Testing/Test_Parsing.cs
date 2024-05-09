using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class Test_Parsing : MonoBehaviour
    {
        [SerializeField] private TextAsset file;
        void Start()
        {
            SendFileToParse();
        }
        void SendFileToParse()
        {
            List<string> lines = FileManager.ReadTextAsset("TestFile");

            foreach(string line in lines)
            {
                if (line == string.Empty)
                    continue;

                DIALOGUE_LINE dl = DialogueParser.Parse(line);
            }
        }
    }
}