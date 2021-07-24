using DG.Tweening;
using UnityEngine;

public class SpawnCard : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(new Vector3(.4f, .4f, .4f), .2f);
        transform.DOScale(new Vector3(.6f, .6f, .6f), .2f).SetDelay(.2f);
        transform.DOScale(new Vector3(.4f, .4f, .4f), .2f).SetDelay(.4f);
        transform.DOScale(new Vector3(.8f, .8f, .8f), .2f).SetDelay(.6f);
    }
}
