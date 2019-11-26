using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;

public class RecordBtn : MonoBehaviour
{
    private MP4Recorder videoRecorder;
    private IClock clock;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Request microphone and camera
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone)) yield break;     

    }

    
    // Update is called once per frame
    void Update()
    {
        // Record frames
        if (videoRecorder != null)
        {
            StartCoroutine("recscreen");
        }
    }

    private IEnumerator recscreen()
    {
        yield return new WaitForEndOfFrame();
        Texture2D s = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        s.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        s.Apply();
        videoRecorder.CommitFrame(s.GetPixels32(), clock.Timestamp);
        Destroy(s);
    }


    public void StartRecording()
    {
        clock = new RealtimeClock();
        videoRecorder = new MP4Recorder(Screen.width, Screen.height, 30, 0, 0, OnRecording);
    }

    public void StopRecording()
    {
        // Stop recording
        videoRecorder.Dispose();
        videoRecorder = null;
    }

    void OnRecording(string path)
    {
        Debug.Log("Saved recording to: " + path);
        // Playback the video
        /*
        #if UNITY_IOS
                Handheld.PlayFullScreenMovie("file://" + path);
        #elif UNITY_ANDROID
                Handheld.PlayFullScreenMovie(path);
        #endif
        */
    }

   

    
}
