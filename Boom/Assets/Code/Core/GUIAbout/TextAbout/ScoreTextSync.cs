using TMPro;
using UnityEngine;

public class ScoreTextSync : MonoBehaviour
{
    TextMeshProUGUI _txtScore;
    void Start()
    {
        _txtScore = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _txtScore.text = CharacterManager.Instance.Score.ToString();
    }
}