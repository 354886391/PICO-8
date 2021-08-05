using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairFlow : MonoBehaviour
{
    public Transform[] HairTrans;
    public List<Vector3> PastPositions;


    public void ResetHairPlace()
    {
        foreach (var item in HairTrans)
        {
            item.transform.localPosition = transform.localPosition;
        }
    }

    public void SetHairColor(Color color)
    {
        foreach (var item in HairTrans)
        {
            item.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void SetXScale(int face)
    {
        foreach (var item in HairTrans)
        {
            item.localScale = Vector3.right * face;
        }
    }

    public void SetHairFlow(int face)
    {
        SetXScale(face);
        Vector3 currentPos = transform.localPosition;
        PastPositions.RemoveAt(0);
        PastPositions.Add(currentPos);
        float deltaIndex = PastPositions.Count / HairTrans.Length;
        for (int i = 0; i < HairTrans.Length; i++)
        {
            int index = Mathf.CeilToInt((i + 1) * deltaIndex) - 1;
            HairTrans[HairTrans.Length - i - 1].localPosition = PastPositions[index];
        }
    }
}
