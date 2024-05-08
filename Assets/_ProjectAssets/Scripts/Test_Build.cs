using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TESTING
{
    public class Test_Build : MonoBehaviour
    {
        DialogueSystem ds;
        TextBuild build;

        string[] lines = new string[5]
        {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            "Iaculis urna id volutpat lacus laoreet non curabitur gravida arcu.",
            "Turpis egestas integer eget aliquet nibh praesent tristique magna sit.",
            "Ut faucibus pulvinar elementum integer enim neque volutpat.",
            "Vel pharetra vel turpis nunc eget lorem dolor."
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            build = new TextBuild(ds.dialogueContainer.dialogueTxt);
            build.buildMethod = TextBuild.BuildMethod.typewriter;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (build.isBuilding)
                {
                    if (!build.speedUp)
                        build.speedUp = true;
                    else
                        build.ForceComplete();
                }
                else
                build.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                build.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}
