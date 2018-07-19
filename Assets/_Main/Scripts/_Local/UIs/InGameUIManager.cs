using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour {

    [Header("Variables")]
    public IntReference score;


    [Header("UIs")]
    public Text scoreText;


    protected void Update()
    {
        scoreText.text = score.Variable.Value.ToString();

    }




}
