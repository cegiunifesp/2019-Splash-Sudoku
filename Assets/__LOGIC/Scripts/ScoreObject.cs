using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreObject : MonoBehaviour
{
    public TextMeshProUGUI Posicao;
    public TextMeshProUGUI Nome;
    public TextMeshProUGUI Score;

    public void Init(string positionText, string nameText, string pointsText)
    {
        gameObject.SetActive(true);
        Posicao.text = positionText;
        Nome.text = nameText;
        Score.text = pointsText;
    }
}
