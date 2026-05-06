using UnityEngine;
using TMPro;


public class ScoreRegistration : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI textForRegistration = GetComponent<TextMeshProUGUI>();
        EndGameManager.endManager.RegisterScoreText(textForRegistration);
        textForRegistration.text = "Score: 0";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
