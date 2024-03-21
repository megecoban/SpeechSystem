using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpeechGroup
{
    [SerializeField][Range(-1f, 9999f)] private int speechGroupID = -1;
    [SerializeField] private List<Speech> speeches = new List<Speech>();

    public int SpeechGroupID => speechGroupID;
    public List<Speech> Speeches => speeches;

    public SpeechGroup(int groupID, List<Speech> speeches)
    {
        this.speechGroupID = groupID;
        this.speeches = speeches;
    }
}