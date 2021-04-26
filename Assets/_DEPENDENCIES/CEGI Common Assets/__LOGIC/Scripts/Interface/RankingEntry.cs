using TMPro;
using UnityEngine;

public class RankingEntry : MonoBehaviour
{
    public TextMeshProUGUI PositionText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI PointsText;

    public void Init(string positionText, string nameText, string pointsText)
    {
        gameObject.SetActive(true);
        PositionText.text = positionText;
        NameText.text = nameText;
        PointsText.text = pointsText;
    }
}
