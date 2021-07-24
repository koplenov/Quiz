using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private GameObject card;
    [SerializeField] private new ParticleSystem particleSystem;
    
    [SerializeField] public int cardsPerRow;
    [SerializeField] public GameObject cardTableGameObject;
    [SerializeField] private CardBundleData[] cardBundles;


    private LevelController _levelController;
    private void Start()
    {
        _levelController = gameObject.AddComponent<LevelController>();
        _levelController.Init(cardBundles[Random.Range(0, cardBundles.Length)]);
        _levelController.TitleText = text;
        _levelController.ParticleSystem = particleSystem;
        _levelController.GameManager = this;
        _levelController.AddRow(cardTableGameObject,card,cardsPerRow);
        _levelController.GenerateValues();
    }

    public void NewGame()
    {
        _levelController.FullTableDestroy();
        Destroy(_levelController);
        
        restartButton.GetComponent<Image>().DOFade(0f, 1f);
        StartCoroutine(DoAfter(1.2f, () => restartButton.SetActive(false)));

        Start();
        
        print("new game");
    }

    [SerializeField] private GameObject restartButton;
    public void ShowRestartButton()
    {
        restartButton.GetComponent<Image>().DOFade(1, 0.5f);
        restartButton.SetActive(true);
    }

    // debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _levelController.AddRow(cardTableGameObject,card,cardsPerRow);
            _levelController.GenerateValues();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            _levelController.GenerateValues();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ShowRestartButton();
        }
    }
    
    private IEnumerator DoAfter(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}
