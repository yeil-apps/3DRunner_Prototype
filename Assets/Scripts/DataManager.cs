using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // обработчик данных о прогрессе игрока 

    public static DataManager Instance;
    private string _ppGems = "Gems"; // название в PlayerPrefs для подсчета алмазов
    private string _ppScore = "Score"; // название в PlayerPrefs для подсчета очков
    public int GemsCount
    {
        get { return PlayerPrefs.GetInt(_ppGems); }
        set { PlayerPrefs.SetInt(_ppGems, value); }
    }

    public int RecordScore 
    {
        get { return PlayerPrefs.GetInt(_ppScore); }
        set { PlayerPrefs.SetInt(_ppScore, value); }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlusGems(int gemsCount)
    {
        if (gemsCount > 0)
        {
            GemsCount = GemsCount + gemsCount;
        }
    }


}
