using System.Collections;
using UnityEngine;
using TMPro;

public class TextBuild
{
    private TextMeshProUGUI tmpro_UI;
    private TextMeshPro tmpro_Game;
    public TMP_Text tmpro => tmpro_UI != null ? tmpro_UI : tmpro_Game;

    public string currentTxt => tmpro.text;
    public string targetTxt { get; private set; } = "";
    public string preTxt { get; private set; } = "";
    private int preTxtLength = 0;

    public string fullTargetTxt => preTxt + targetTxt;

    public enum BuildMethod { typewriter, fade}
    public BuildMethod buildMethod = BuildMethod.typewriter;

    public Color txtColor { get { return tmpro.color; } set { tmpro.color = value; } }

    public float speed { get { return baseSpeed * speedMult; } set { speedMult = value; } }
    private const float baseSpeed = 1;
    private float speedMult = 1;

    public int charactersPerLoop { get { return speed <= 2f ? characterMult : speed <= 2.5f ? characterMult * 2 : characterMult * 3; } }
    private int characterMult = 1;

    public bool speedUp = false;

    public TextBuild(TextMeshProUGUI tmpro_UI)
    {
        this.tmpro_UI = tmpro_UI;
    }

    public TextBuild(TextMeshPro tmpro_Game)
    {
        this.tmpro_Game = tmpro_Game;
    }

    public Coroutine Build(string text)
    {
        preTxt = "";
        targetTxt = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    //Append text to 
    public Coroutine Append(string text)
    {
        preTxt = tmpro.text;
        targetTxt = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    public void Stop()
    {
        if (!isBuilding)
            return;

        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();

        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter();
                break;
        }

        OnComplete();
    }
    private void OnComplete()
    {
        buildProcess = null;
        speedUp = false;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
        }

        Stop();
        OnComplete();
    }
    private void Prepare()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preTxt;

        if (preTxt != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetTxt;
        tmpro.ForceMeshUpdate();
    }


    private IEnumerator Build_Typewriter()
    {
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += speedUp ? charactersPerLoop * 5 : charactersPerLoop;

            yield return new WaitForSeconds(0.015f / speed);
        }
    }
}
