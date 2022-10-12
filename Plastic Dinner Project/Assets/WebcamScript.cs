using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WebcamScript : MonoBehaviour
{
    WebCamTexture webcamTexture;
    public Renderer Saverenderer;
    public List<Texture2D> TexturesList;
    // Start is called before the first frame update
    void Start()
    {
        webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TakePhoto());
        }
    }
    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {

        // NOTE - you almost certainly have to do this here:

        yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height);
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();
        TexturesList.Add(photo);
        Saverenderer.material.mainTexture = photo;
        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        File.WriteAllBytes("C:\\Users\\leopo\\Desktop\\" + "photo.png", bytes);
        webcamTexture.Stop();

        yield return new WaitForSeconds(0.5f);
        webcamTexture.Play();
    }


}
