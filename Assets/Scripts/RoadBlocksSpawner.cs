using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlocksSpawner : MonoBehaviour
{
    [SerializeField]private int _blocksToChangeLevel = 15;
    private int _blocksLeftToChangeLevel;
    public int LevelsCount
    {
        get { return _levels.Count; }
    }
    [SerializeField] private List<LevelAlgorithm> _levels; // настроенные уровни
    [SerializeField] private int _startedBlocksCount = 15; // начальное количество блоков для генерации
    [SerializeField] private float _blockOffset = 3f; // смещение блоков друг к другу

    private List<GameObject> _roadBlocksPrefab; // блоки для генерации текущего уровня
    private List<GameObject> _currentRoadBlocksOnScene = new List<GameObject>(); // существующие на сцене блоки
    private GameObject _firstRoadBlock;
    private bool _newLevel;

    private float _lengthBlockToForDestroy;
    private float _posForBlockSpawn; 
    private Transform _playerPosition;

    void Start()
    {
        _blocksLeftToChangeLevel = _blocksToChangeLevel;
        _playerPosition = FindObjectOfType<PlayerController>().transform;

        int randomLevel = Random.Range(0, LevelsCount); // первый уровень рандомный
        GameObject firstBlock = Instantiate(_levels[randomLevel].FirstBlock, transform.position, Quaternion.identity); //генерация самого первого блока на сцене в поцизиции родительского объекта
        _currentRoadBlocksOnScene.Add(firstBlock); //относительно первого блока генерируются остальные в методе SpawnNextBlock
        _lengthBlockToForDestroy = (_currentRoadBlocksOnScene[0].GetComponent<BoxCollider>().bounds.size.z); // размер первого блока для вычисления позиции уничтожения

        ChangeLevelPrefabs(randomLevel);
        SpawnNextBlock(firstBlock); // дубликат первого блока чтобы игрок вошел в процесс игры

        for (int i = 0; i <= _startedBlocksCount; i++) // генерация первых блоков в количестве _blockCount
        {
            int randomBlockIndex = Random.Range(0, _roadBlocksPrefab.Count);
            SpawnNextBlock(_roadBlocksPrefab[randomBlockIndex]);
        }

    }

    void LateUpdate()
    {
        CheckForSpawn();
    }

    private void CheckForSpawn() // уничтожение блока который пробежал игрок и генерация нового
    {
        if (Vector3.Distance(_playerPosition.transform.position,_currentRoadBlocksOnScene[0].transform.position) > _lengthBlockToForDestroy * 2) 
        {
            DestroyBlock();
            int randomBlockIndex = Random.Range(0, _roadBlocksPrefab.Count);
            SpawnNextBlock(_roadBlocksPrefab[randomBlockIndex]);
        }
    }

    private void SpawnNextBlock(GameObject blockToSpawn)
    {
        GameObject lastBlock = _currentRoadBlocksOnScene[_currentRoadBlocksOnScene.Count-1]; //последний блок на сцене
        GameObject block;
        if (_newLevel)
        {
             block = Instantiate(_firstRoadBlock, transform); //блок который нужно сгенерировать
            _newLevel = false;
        }
        else
        {
             block = Instantiate(blockToSpawn, transform); //блок который нужно сгенерировать
        }


        float currentBlockLength = block.GetComponent<BoxCollider>().bounds.size.z; // вычисление длины по Z
        float lastBlockLength = lastBlock.GetComponent<BoxCollider>().bounds.size.z;

        float distanceBtwBlocks = currentBlockLength / 2 + lastBlockLength / 2 - _blockOffset; // расстоние между центрами последнего блока и блока который нужно сгенерировать

        _posForBlockSpawn = lastBlock.gameObject.transform.position.z + distanceBtwBlocks;
        block.transform.position = new Vector3(0, 0, _posForBlockSpawn);
        _currentRoadBlocksOnScene.Add(block); 
    }

    private void DestroyBlock()
    {
        Destroy(_currentRoadBlocksOnScene[0]);
        _currentRoadBlocksOnScene.RemoveAt(0);
        _blocksLeftToChangeLevel--;
        if (_blocksLeftToChangeLevel <= 0)
        {
            _newLevel = true;
            _blocksLeftToChangeLevel = _blocksToChangeLevel;
            ChangeLevelPrefabs(Random.Range(0, LevelsCount));
        }
        _lengthBlockToForDestroy = (_currentRoadBlocksOnScene[0].GetComponent<BoxCollider>().bounds.size.z);
    }
    public void ChangeLevelPrefabs(int levelIndex) // смена блоков для генерации на блоки в выбранном уровне levelIndex
    {
        if (levelIndex>=0 && levelIndex < _levels.Count)
        {
            _firstRoadBlock = _levels[levelIndex].FirstBlock;
            _roadBlocksPrefab = _levels[levelIndex].LevelPrefabs;

        }
    }
}
