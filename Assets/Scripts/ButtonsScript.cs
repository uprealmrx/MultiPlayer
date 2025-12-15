using TMPro;
using UnityEngine;

public class ButtonsScript : MonoBehaviour
{
    public int _myIndex;
    private int _index;
    public float _minindex;
    public float _maxindex;
    public TextMeshProUGUI _prefIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void next()
    {
        if (_index > _minindex)
        {
            _index++;
        }
        _prefIndex.text = _index.ToString();
    }
    public void previous()
    {
        if(_index < _minindex)
        {
            _index--;
        }
        _prefIndex.text = _index.ToString();
    }
    public void Lock()
    {
        Manager.Instance.playersinLobby[_myIndex]._prefabIndex = _index;
    }
}
