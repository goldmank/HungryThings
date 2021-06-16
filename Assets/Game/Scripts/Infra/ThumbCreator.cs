using System.Collections;
using UnityEngine;

namespace Game.Scripts.Infra
{
    public class ThumbCreator : MonoBehaviour
    {
        [SerializeField] private CameraCapture _cameraCapture;

        private void Start()
        {
            Run();
        }
        
        [ContextMenu("Run")]
        public void Run()
        {
            StartCoroutine(DoRun());
        }

        private IEnumerator DoRun()
        {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
            }
            
            yield return new WaitForSeconds(0.3f);
            
            foreach (Transform child in transform) {
                child.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                _cameraCapture.Capture("Temp/Thumb/" + child.gameObject.name);
                child.gameObject.SetActive(false);
            }
        }
    }
}