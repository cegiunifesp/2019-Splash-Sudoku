using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class ScoreObject : MonoBehaviour
{
    [FormerlySerializedAs("Posicao")]
    public TextMeshProUGUI positionText;

    [FormerlySerializedAs("Nome")]
    public TextMeshProUGUI nameText;

    [FormerlySerializedAs("Score")]
    public TextMeshProUGUI scoreText;

    // Backward compatibility properties
    public TextMeshProUGUI Posicao => positionText;
    public TextMeshProUGUI Nome => nameText;
    public TextMeshProUGUI Score => scoreText;

    public void Init(string positionStr, string nameStr, string pointsStr)
    {
        gameObject.SetActive(true);
        if (positionText != null) positionText.text = positionStr;
        if (nameText != null) nameText.text = nameStr;
        if (scoreText != null) scoreText.text = pointsStr;
    }
}
