using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static CombinationModel;

[ExecuteAlways]
public class GameMasterScript : MonoBehaviour
{
    public List<InnerCombinationModel> Combinations = new();

    void Awake()
    {
        LoadCombinations(Utils.GetHiddenGameSettings().CombinationsPath);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public bool CombineElements(ElementScript element1, ElementScript element2)
    {
        var e1 = element1.Name.ToLower();
        var e2 = element2.Name.ToLower();

        foreach (var combination in Combinations)
        {
            if ((combination.element1.ToLower() == e1 && combination.element2.ToLower() == e2) ||
                (combination.element1.ToLower() == e2 && combination.element2.ToLower() == e1))
            {
                DoCombination(element1, element2, combination.result);
                return true;
            }
        }

        return false;
    }

    private void DoCombination(ElementScript e1, ElementScript e2, string result)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Generated/{result}.prefab");
        var instantiated = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instantiated.transform.position = e1.transform.position;
        Destroy(e1.gameObject);
        Destroy(e2.gameObject);
    }

    private void LoadCombinations(string path)
    {
        var json = File.ReadAllText(path);
        var combinations = JsonUtility.FromJson<CombinationModel>(json);
        Combinations = combinations.combinations.ToList();
    }
}
