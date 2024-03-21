using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpeechManagment : MonoBehaviour
{
    [Header("Assignments")]
    [SerializeField] private RectTransform speechPanel;
    [SerializeField] private TextMeshProUGUI speechArea;

    [Header("Settings")]
    [Header("Text And Panel Color Settings")]
    [SerializeField] private Color speechPanelBackgroundColor;
    [SerializeField] private Color speechTextColor;

    [Header("Text And Panel Settings")]
    [SerializeField] [Range(1f, 4f)] private int speechMaxLineHeigth = 2;
    [SerializeField] [Range(8f, 24f)] private float speechFontSize = 17f;
    [SerializeField] [Range(0f, 20f)] private float marginY = 5f;
    [SerializeField] [Range(0f, 20f)] private float marginX = 15f;
    [SerializeField] [Range(100f, 550f)] private float width = 680f;



    private List<SpeechGroup> speechGroups = new List<SpeechGroup>();
    private SpeechGroup currentSpeechGroup = null;
    [Range(-1f, 9999f)] private int currentGroupID = -1;
    [Range(-1f, 9999f)] private int currentSpeechIndex = -1;

    private void Awake()
    {
        speechPanel.GetComponent<Image>().color = speechPanelBackgroundColor;
        speechArea.color = speechTextColor;

        speechArea.fontSize = speechFontSize;

        speechArea.rectTransform.sizeDelta = new Vector2(width, speechFontSize * speechMaxLineHeigth);
        speechPanel.sizeDelta = new Vector2(width + (marginX * 2f), (speechFontSize * speechMaxLineHeigth) + (marginY * 2f));
    }

    void Start()
    {
        Hide();

        List<Speech> speeches = new List<Speech>();
        speeches.Add(new Speech(0, "Tester", "Bu bir test mesajýdýr."));
        speeches.Add(new Speech(1, "Tester", "Þimdi speecher name gelmeden geliyor mesaj. Ýzle"));
        speeches.Add(new Speech(2, "", "Adeta dýþ ses"));
        speeches.Add(new Speech(3, "Tester", "Evet, <i>Adeta dýþ ses</i>, deðil mi?"));

        SpeechGroup speechGroup = new SpeechGroup(0, speeches);

        speechGroups.Add(speechGroup);


        /*
        speeches.Add(new Speech("Tester", "Bu bir test mesajýdýr."));
        speeches.Add(new Speech("Tester", "Þimdi speecher name gelmeden geliyor mesaj. Ýzle"));
        speeches.Add(new Speech("", "Adeta dýþ ses"));
        speeches.Add(new Speech("Tester", "Evet, <i>Adeta dýþ ses</i>, deðil mi?"));

        if(speechIndex == -1)
        {
            Hide();
        }*/
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSpeechGroup(0);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            DisplaySpeechOutOfSpeechGroup("Bu da örnek bir out of speech grouptur.");
        }
    }

    public void SetSpeechGroup(int groupID)
    {
        StopCoroutine("HideAfterLifetime");

        currentGroupID = groupID;
        
        for(int i = 0; i < speechGroups.Count; i++)
        {
            Debug.Log("input: " + currentGroupID + " \t test: " + speechGroups[i].SpeechGroupID);
            if(currentGroupID == speechGroups[i].SpeechGroupID)
            {
                currentSpeechGroup = speechGroups[i];
                currentSpeechIndex = -1;
                return;
            }
        }

        currentGroupID = -1;
        currentSpeechIndex = -1;
        currentSpeechGroup = null;
        Debug.LogWarning("Speech Group not found. (Input ID: " + groupID + ")");
    }

    public void Next()
    {
        StopCoroutine("HideAfterLifetime");

        if (currentSpeechGroup == null)
        {
            Debug.Log("Not speech active");
            return;
        }

        currentSpeechIndex++;

        if(currentSpeechIndex >= currentSpeechGroup.Speeches.Count)
        {
            EndSpeech();
            return;
        }

        if(currentSpeechIndex == 0)
        {
            Show();
        }

        if (currentSpeechGroup.Speeches[currentSpeechIndex].Speecher == null || string.IsNullOrEmpty(currentSpeechGroup.Speeches[currentSpeechIndex].Speecher))
        {
            DisplaySpeech(currentSpeechGroup.Speeches[currentSpeechIndex].SpeechText);
        }
        else
        {
            DisplaySpeech(currentSpeechGroup.Speeches[currentSpeechIndex].Speecher, currentSpeechGroup.Speeches[currentSpeechIndex].SpeechText);
        }

        if (currentSpeechGroup.Speeches[currentSpeechIndex].SpeechAction != null)
        {
            currentSpeechGroup.Speeches[currentSpeechIndex].SpeechAction.Invoke();
        }
    }

    public void DisplaySpeechOutOfSpeechGroup(string speechText, float lifetime = 2.5f, UnityAction startActions = null, UnityAction endActions = null)
    {
        DisplaySpeechOutOfSpeechGroup("", speechText, lifetime);
    }

    public void DisplaySpeechOutOfSpeechGroup(string speecherName, string speechText, float lifetime = 2.5f, UnityAction startActions = null, UnityAction endActions = null)
    {
        if (currentSpeechGroup != null)
            return;


        lifetime = Mathf.Abs(lifetime);



        Show();
        
        if(startActions != null)
        {
            startActions.Invoke();
        }

        speechArea.text = ((speecherName == null || string.IsNullOrEmpty(speecherName)) ? "" : "<b>" + speecherName + ":</b> ") +  speechText;

        StartCoroutine(HideAfterLifetime(lifetime, endActions));
    }

    IEnumerator HideAfterLifetime(float lifetime, UnityAction endActions = null)
    {
        float _lifetime = Mathf.Abs(lifetime);
        while(_lifetime>=0f)
        {
            _lifetime -= Time.deltaTime;

            if(_lifetime <= 0f)
            {
                break;
            }

            yield return Time.deltaTime;
        }

        if(endActions!=null)
        {
            endActions.Invoke();
        }

        Hide();
    }

    public void EndSpeech()
    {
        Hide();
        currentSpeechIndex = -1;
        currentGroupID = -1;
        currentSpeechGroup = null;
        Debug.Log("Speech ended.");
    }

    public void DisplaySpeech(string speecherName, string speechText)
    {
        speechArea.text = ((speecherName == null || string.IsNullOrEmpty(speecherName)) ? "" : "<b>" + speecherName + ":</b> ") + speechText;
    }

    public void DisplaySpeech(string speechText)
    {
        speechArea.text = speechText;
    }

    public void Hide()
    {
        speechPanel.gameObject.SetActive(false);
    }

    public void Show()
    {
        speechPanel.gameObject.SetActive(true);
    }
}
