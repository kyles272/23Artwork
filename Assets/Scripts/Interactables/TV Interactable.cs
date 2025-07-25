using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TVInteractable : Interactable
{

    [SerializeField] private GameObject _camera;

    private VideoPlayer videoPlayer;

    private GameObject _screen;

    private Renderer _screenRenderer;

    [SerializeField] private VideoClip defaultVideoClip;

    [SerializeField] private Material TVOffMaterial;

    [SerializeField] private Material TVOnMaterial;

    bool isPlaying = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Transform screenTransform = transform.Find("Screen");
        if (screenTransform != null)
        {
            _screen = screenTransform.gameObject;
        }
        _screenRenderer = _screen.GetComponent<Renderer>();
        videoPlayer = _screen.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found on the screen object.");
            return;
        }
        if (defaultVideoClip != null)
        {
            videoPlayer.clip = defaultVideoClip;
        }
        else
        {
            Debug.LogWarning("Default video clip is not assigned.");
        }


    }

    public override void Interact(Player player)
    {
        isPlaying = !isPlaying;
        Debug.Log("TV Interacted with. Is Playing: " + isPlaying);
        //If the tv was off, turn it on and player the video
        if (isPlaying)
        {
            StartCoroutine(WaitForVideoToStart());
        }
        else
        {
            _screenRenderer.material = TVOffMaterial;
            videoPlayer.Stop();
        }
    }

    IEnumerator WaitForVideoToStart()
    {
        //Wait for video to start playing before changing the material
        videoPlayer.Play();
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        _screenRenderer.material = TVOnMaterial;
    }
}
