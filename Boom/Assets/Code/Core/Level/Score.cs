using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI _txtScore;
    void Start()
    {
        _txtScore = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _txtScore.text = "Score : " + CharacterManager.Instance.Score;
    }
}
