using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AugmentedImageVisualizer : MonoBehaviour
{
    [SerializeField] public VideoClip[] _videoClips;
    public AugmentedImage Image;
    private VideoPlayer _videoPlayer;
    private GameObject _text;
    public GameObject current;
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += OnStop;
        
    }

    private void OnStop(VideoPlayer source)
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            return;
        }
        if (!_videoPlayer.isPlaying)
        {
            _videoPlayer.clip = _videoClips[Image.DatabaseIndex];
            _videoPlayer.Play();
            if (_videoPlayer.clip == _videoClips[0])
            {
                _text = current.transform.Find("Plane").transform.Find("Specs").transform.Find("Sprocket").gameObject;
                _text.SetActive(true);
            }
        }
        transform.localScale = new Vector3(Image.ExtentX, Image.ExtentZ, 1);
    }
}
