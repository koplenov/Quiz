using UnityEngine;

[CreateAssetMenu(fileName = "New CardBundleData", menuName = "Card Bundle Data", order = 10)]
public class CardBundleData : ScriptableObject
{
    [SerializeField] public CardData[] _cardData;
    
    public CardData[] CardData => _cardData;
}
