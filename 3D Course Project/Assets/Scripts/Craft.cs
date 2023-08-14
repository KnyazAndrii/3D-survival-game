using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    public Item Ingridient0;
    public Item Ingridient1;
    public Image IngridientImage0;
    public Image IngridientImage1;
    public TMP_Text IngridientText0;
    public TMP_Text IngridientText1;
    public TMP_Text ResultText;
    public PlayerCrafting PlayerCrafting;
    public PlayerInventory PlayerInventory;
    public Item Result;
    public Image ResultImage;
    public Button CraftButton;
    public int ResultCount;
    public int Count0;
    public int Count1;

    public void Refresh()
    {
        bool isCanCraft = false;
        bool isIngredient0 = false;
        bool isIngredient1 = false;

        IngridientImage0.sprite = Ingridient0.Icon;
        IngridientImage1.sprite = Ingridient1.Icon;
        
        IngridientText0.text = Count0.ToString();
        IngridientText1.text = Count1.ToString();

        ResultImage.sprite = Result.Icon;
        ResultText.text = ResultCount.ToString();

        for (int i = 0; i < PlayerInventory.Items.Length; i++)
        {
            if (PlayerInventory.Items[i])
            {
                if (PlayerInventory.Items[i].ItemName == Ingridient0.ItemName)
                {
                    if (PlayerInventory.Counts[i] >= Count0)
                    {
                        isIngredient0 = true;
                    } 
                }

                if (PlayerInventory.Items[i].ItemName == Ingridient1.ItemName)
                {
                    if (PlayerInventory.Counts[i] >= Count1)
                    {
                        isIngredient1 = true;
                    }
                }
            }
        }

        if (isIngredient0 && isIngredient1)
        {
            isCanCraft = true;
        }
        else
        {
            isCanCraft = false;
        }

        if (isCanCraft)
        {
            CraftButton.interactable = true;
        }
        else
        {
            CraftButton.interactable = false;
        }
    }
}
