using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab; 
    [SerializeField] private int _gridSize = 30;     
    [SerializeField] private float _spacing = 1.5f;  
    [SerializeField] private float _waveSpeed = 2.0f; 
    [SerializeField] private float _waveHeight = 2.0f; 
    [SerializeField] private float _colorChangeInterval = 5.0f; 

    private GameObject[,] _cubes;  
    private int _currentFlagIndex = 0; 
    private float _timeSinceLastChange = 0f; 

    void Start()
    {
        CreateWaveGrid();
    }

    void Update()
    {
        ApplyWaveEffect();
        _timeSinceLastChange += Time.deltaTime;

        // Transition to the next flag every _colorChangeInterval seconds
        if (_timeSinceLastChange >= _colorChangeInterval)
        {
            _timeSinceLastChange = 0f;
            _currentFlagIndex = (_currentFlagIndex + 1) % 6; // Cycle through 6 flags
        }
    }

    private bool IsPartOfItaly(int x, int z)
    {
        int third = _gridSize / 3;
        return (x < third) || (x < 2 * third && x >= third) || (x >= 2 * third);
    }

    private bool IsPartOfCzechRepublic(int x, int z)
    {
        return x < _gridSize * 0.6f && z > _gridSize * 0.4f;
    }

    private bool IsPartOfBelgium(int x, int z)
    {
        int third = _gridSize / 3;
        if (x < third) return true; 
        if (x < 2 * third) return z < _gridSize * 0.5f; 
        return z >= _gridSize * 0.5f; 
    }

    private void CreateWaveGrid()
    {
        _cubes = new GameObject[_gridSize, _gridSize];

        // Create the grid
        for (int x = 0; x < _gridSize; x++)
        {
            for (int z = 0; z < _gridSize; z++)
            {
                Vector3 position = new Vector3(x * _spacing, 0, z * _spacing);
                GameObject cube = Instantiate(_cubePrefab, position, Quaternion.identity);
                _cubes[x, z] = cube;
            }
        }
    }

    private void ApplyWaveEffect()
    {
        for (int x = 0; x < _gridSize; x++)
        {
            for (int z = 0; z < _gridSize; z++)
            {
                // Calculate wave effect using cosine function
                float wave = Mathf.Cos((x + z) * 0.5f + Time.time * _waveSpeed) * _waveHeight;

                // Update the Y position of the cubes
                Vector3 newPosition = _cubes[x, z].transform.position;
                newPosition.y = wave;
                _cubes[x, z].transform.position = newPosition;

                // Update cube color based on the current flag
                Renderer cubeRenderer = _cubes[x, z].GetComponent<Renderer>();
                switch (_currentFlagIndex)
                {
                    case 0:
                        // Italy flag 
                        if (x < _gridSize / 3)
                            cubeRenderer.material.color = Color.green;
                        else if (x < 2 * _gridSize / 3)
                            cubeRenderer.material.color = Color.white;
                        else
                            cubeRenderer.material.color = Color.red;
                        break;

                    case 1:
                        // Czech Republic flag 
                        if (x < _gridSize * 0.5f && z > _gridSize * 0.4f)
                            cubeRenderer.material.color = Color.blue;
                        else if (z > _gridSize * 0.6f)
                            cubeRenderer.material.color = Color.red;
                        else
                            cubeRenderer.material.color = Color.white;
                        break;

                    case 2:
                        // Belgium flag
                        if (x < _gridSize / 3)
                            cubeRenderer.material.color = Color.black;
                        else if (x < 2 * _gridSize / 3)
                            cubeRenderer.material.color = Color.yellow;
                        else
                            cubeRenderer.material.color = Color.red;
                        break;

                    case 3:
                        // Poland flag
                        cubeRenderer.material.color = z > _gridSize / 2 ? Color.red : Color.white;
                        break;

                    case 4:
                        // Germany flag
                        if (z > 2 * _gridSize / 3)
                            cubeRenderer.material.color = Color.black;
                        else if (z > _gridSize / 3)
                            cubeRenderer.material.color = Color.red;
                        else
                            cubeRenderer.material.color = Color.yellow;
                        break;

                    case 5:
                        // France flag
                        if (x < _gridSize / 3)
                            cubeRenderer.material.color = Color.blue;
                        else if (x < 2 * _gridSize / 3)
                            cubeRenderer.material.color = Color.white;
                        else
                            cubeRenderer.material.color = Color.red;
                        break;

                    case 6:
                        // Netherlands flag
                        if (z > 2 * _gridSize / 3)
                            cubeRenderer.material.color = Color.red;
                        else if (z > _gridSize / 3)
                            cubeRenderer.material.color = Color.white;
                        else
                            cubeRenderer.material.color = Color.blue;
                        break;
                }
            }
        }
    }
}
