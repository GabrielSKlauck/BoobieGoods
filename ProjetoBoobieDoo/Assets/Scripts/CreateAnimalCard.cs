using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAnimalCard : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject animalCard;

    [Header("Lista de imagens")]
    public List<Texture> animalTargets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Texture target in animalTargets)
        {
            GameObject instantiatedObject = Instantiate(animalCard, this.transform);

            Transform childTransform = instantiatedObject.transform.Find("Image");

            if (childTransform != null)
            {
                GameObject objectChild = childTransform.gameObject;

                RawImage rawImage = objectChild.GetComponent<RawImage>();

                if (rawImage != null)
                {
                    rawImage.texture = target;
                }
            }
        }
    }
}
