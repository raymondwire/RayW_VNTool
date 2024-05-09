using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Test_Files : MonoBehaviour
{
    private string fileName = "TestFile";

    void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        List<string> lines = FileManager.ReadTextAsset(fileName, false);

        foreach (string line in lines)
            Debug.Log(line);

        yield return null;
    }
}