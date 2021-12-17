using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    private bool isCameraAvailable;
    private WebCamTexture backCamera;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No Camera Detected.");
            isCameraAvailable = false;
            return;
        }

        //for (int i = 0; i < devices.Length; i++)
        //{
        //    Debug.Log("Looking at camera: " + devices[i].name);
        //    if (!devices[i].isFrontFacing)
        //    {
        //        Debug.Log("Chose camera: " + devices[i].name);
        //        backCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
        //    }
        //}

        int i = 3;
        Debug.Log("Looking at camera: " + devices[i].name);
        if (!devices[i].isFrontFacing)
        {
            Debug.Log("Chose camera: " + devices[i].name);
            backCamera = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
        }

        if (backCamera == null)
        {
            Debug.Log("Unable to find back Camera");
            return;
        }

        backCamera.Play();
        background.texture = backCamera;
        isCameraAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCameraAvailable)
            return;

        float ratio = (float)backCamera.width / (float)backCamera.height;
        fit.aspectRatio = ratio;

        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}
