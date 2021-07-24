using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    private CardBundleData _currentCardBundle;
    private static string _correctAnswer;
    private Text _titleText;
    public Text TitleText
    {
        set
        {
            _titleText = value;
            _titleText.DOFade(0f, 2f).From().SetEase(Ease.OutQuad);
            //  ¯\_(ツ)_/¯
        }
    }

    private static ParticleSystem _particleSystem;
    public ParticleSystem ParticleSystem
    {
        set => _particleSystem = value;
    }

    private static LevelController _instance;
    public void Init(CardBundleData cardBundle)
    {
        _instance = this;
        _currentCardBundle = cardBundle;
    }

    private List<CardObject> cardsObjects = new List<CardObject>();
    public List<CardObject> CardsObjects => cardsObjects;
    public GameManager GameManager { get; set; }

    private GameObject _tableRowGameObject;
    private GameObject _card;
    private int _cardsPerRow;
    public void AddRow(GameObject tableRowGameObject, GameObject card, int cardsPerRow)
    {
        if (_tableRowGameObject == null)
            _tableRowGameObject = tableRowGameObject;
        if (_card == null)
            _card = card;
        if (_cardsPerRow == 0)
            _cardsPerRow = cardsPerRow;
        
        GameObject row = new GameObject();
        row.transform.parent = tableRowGameObject.transform;
        
        var horizontalLayoutGroup = row.AddComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.childControlHeight = false;
        horizontalLayoutGroup.childControlWidth = false;
        
        CardObject[] newCardsObjects = new CardObject[cardsPerRow];
        for (int i = 0; i < cardsPerRow; i++)
        {
            newCardsObjects[i] = Instantiate(card, row.transform).GetComponent<CardObject>();
        }
        
        cardsObjects.AddRange(newCardsObjects);
    }

    public void GenerateValues()
    {
        Dictionary<CardData, bool> used = new Dictionary<CardData, bool>();
        
        for (int i = 0; i < cardsObjects.Count; i++)
        {
            CardData cardData;
            
            do
            {
                cardData = _currentCardBundle._cardData[Random.Range(0, _currentCardBundle._cardData.Length)];
                
                if (used.Count >= _currentCardBundle._cardData.Length)
                {
                    Debug.Log("There are more slots than sprites! There will be duplicates");
                    break;
                }
                
                if (!used.ContainsKey(cardData))
                {
                    used.Add(cardData, true);
                    break;
                }
                
            } while (true);
            
            cardsObjects[i].Init(cardData);
        }

        SetRightAnswer(used);
    }

    private Dictionary<CardData, bool> lastAnswers = new Dictionary<CardData, bool>();
    private void SetRightAnswer(Dictionary<CardData, bool> dictionary)
    {
        CardData correctCard = dictionary.ElementAt(Random.Range(0, dictionary.Count)).Key;
        
        do
        {
            correctCard = dictionary.ElementAt(Random.Range(0, dictionary.Count)).Key;

            if (!lastAnswers.ContainsKey(correctCard))
            {
                lastAnswers.Add(correctCard, true);
                break;
            }
                
        } while (true);

        _correctAnswer = correctCard.Identifier;
        _titleText.text = $"Find {correctCard.Identifier}";
    }
    
    public static void DelegateMethod(CardObject cardObject)
    {
        if (cardObject.Identifier == _correctAnswer)
        {
            _particleSystem.Play();
            _instance.StartCoroutine(_instance.DoAfter(1f, () => _particleSystem.Stop()));
            cardObject.Image.gameObject.transform.DOShakePosition(1,new Vector3(3,3,0));
            
            print("Верно1!");
            
            _instance.StartCoroutine(_instance.DoAfter(2f, () =>
            {
                if (_instance.cardsObjects.Count == 9)
                {
                    _instance.GameManager.ShowRestartButton();
                    return;
                }
                _instance.AddRow(_instance._tableRowGameObject, _instance._card, _instance._cardsPerRow);
                _instance.GenerateValues();
            }));
        }
        else
        {
            print("Не верно!11");
            cardObject.Image.gameObject.transform.DOShakePosition(2.0f, strength: new Vector3(0, 2, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
        }
    }

    private IEnumerator DoAfter(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    public void FullTableDestroy()
    {
        int childs = _tableRowGameObject.transform.childCount;
        for (int i = childs - 1; i >= 0; i--) {
            DestroyImmediate( _tableRowGameObject.transform.GetChild( i ).gameObject );
        }
    }
}
