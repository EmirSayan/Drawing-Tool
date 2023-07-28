using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotCapture : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;
    private AudioSource audioSource;
    public AudioClip takeAScreenshot;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeScreenShot()
    {
        audioSource.PlayOneShot(takeAScreenshot);
        StartCoroutine(CanvasHide());
    }

    IEnumerator CanvasHide()
    {
        canvas.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        ScreenCapture.CaptureScreenshot(Application.dataPath+"Screenshot_Line2d"+Random.Range(0,1000) +".png");
        yield return new WaitForSeconds(0.1f);
        canvas.SetActive(true);
    }
}
