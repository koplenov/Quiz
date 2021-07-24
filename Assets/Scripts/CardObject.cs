using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{

    [SerializeField] private Image _image;
    public Image Image => _image;
    
    private string _identifier;
    public string Identifier
    {
        get => _identifier;
        set => _identifier = value;
    }

    public void Init(CardData cardData)
    {
        _identifier = cardData.Identifier;
        _image.sprite = cardData.Sprite;
    }

    public void OnClick()
    {
        LevelController.DelegateMethod(this);
    }
}
