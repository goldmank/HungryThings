using System.IO;
using UnityEngine;

namespace Game.Scripts.Infra
{
    public class CameraCapture : MonoBehaviour
    {
         public int fileCounter;
        //
        // private Camera _camera;
        //
        // [ContextMenu("Capture")] 
        // public void Capture()
        // {
        //     _camera = GetComponent<Camera>();
        //     
        //     RenderTexture activeRenderTexture = RenderTexture.active;
        //     RenderTexture.active = _camera.targetTexture;
        //
        //     _camera.Render();
        //
        //     Texture2D image = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
        //     image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        //     image.Apply();
        //     RenderTexture.active = activeRenderTexture;
        //
        //     byte[] bytes = image.EncodeToPNG();
        //     DestroyImmediate(image);
        //
        //     File.WriteAllBytes(Application.dataPath + "/" + fileCounter + ".png", bytes);
        //     fileCounter++;
        // }
        
        [SerializeField]
        RenderTexture rt;

        Camera cam;

        [SerializeField]
        string fileName;

        public void Capture(string fileName)
        {
            cam = GetComponent<Camera>();
            
            RenderTexture mRt = new RenderTexture(rt.width, rt.height, rt.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            mRt.antiAliasing = rt.antiAliasing;

            var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
            cam.targetTexture = mRt;
            cam.Render();
            RenderTexture.active = mRt;

            tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
            tex.Apply();

            File.WriteAllBytes(Application.dataPath + "/" + fileName + ".png", tex.EncodeToPNG());
            fileCounter++;
            Debug.Log("Saved file");

            DestroyImmediate(tex);

            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;

            DestroyImmediate(mRt);
        }
        
        [ContextMenu("Capture")] 
        public void SavePNG()
        {
            Capture(fileCounter.ToString());
        }
    }
}