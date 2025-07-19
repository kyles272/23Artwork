using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    TextMeshProUGUI interactPrompt;

    Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        player = GameObject.Find("Player").GetComponent<Player>();
        interactPrompt = transform.Find("Interact Prompt").GetComponent<TextMeshProUGUI>();
        if(interactPrompt == null)
        {
            Debug.LogError("Interact Prompt TextMeshProUGUI not found in HUD.");
        }
        else
        {
            ShowInteractPrompt(false);
            interactPrompt.text = "Press 'E' to interact";
        }
    }

    public void ShowInteractPrompt(bool show)
    {
        if(interactPrompt != null)
        {
            interactPrompt.gameObject.SetActive(show);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
