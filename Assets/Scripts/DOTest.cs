using UnityEngine;
using DG.Tweening;
public class DOTest : MonoBehaviour
{
    public GameObject testObject;
    // Start is called before the first frame update
    void Start()
    {
        testObject.transform.DOScale(new Vector3(.4f, .4f, .4f), .2f);
        testObject.transform.DOScale(new Vector3(.6f, .6f, .6f), .2f).SetDelay(.2f);
        testObject.transform.DOScale(new Vector3(.4f, .4f, .4f), .2f).SetDelay(.4f);
        testObject.transform.DOScale(new Vector3(1f, 1f, 1f), .2f).SetDelay(.6f);
        //testObject.transform.DOShakePosition(2.0f, strength: new Vector3(0, 2, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
    }
}
