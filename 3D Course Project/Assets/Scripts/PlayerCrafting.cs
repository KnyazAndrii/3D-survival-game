using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    private PlayerController _playerController;
    private bool _panelController;

    public GameObject CraftPanel;
    public Transform ItemsContainer;

    public Craft[] crafts;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        EnablePanel();
    }

    private void EnablePanel()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CraftPanel.activeSelf == true)
            {
                _panelController = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                _panelController = true;
                Cursor.lockState = CursorLockMode.None;
            }

            CraftPanel.SetActive(_panelController);
            Cursor.visible = _panelController;
            _playerController.CanMove = !_panelController;

            for (int i = 0; i < crafts.Length; i++)
            {
                crafts[i].Refresh();
            }
        }
    }

    public void Craft(int number)
    {
        for (int i = 0; i < crafts[number].ResultCount; i++)
        {
            GameObject newItem = Instantiate(crafts[number].Result.gameObject, ItemsContainer);
            _playerInventory.AddItem(newItem.GetComponent<Item>());
        }

        for (int i = 0; i < _playerInventory.Items.Length; i++)
        {
            if (_playerInventory.Items[i])
            {
                if (_playerInventory.Items[i].ItemName == crafts[number].Ingridient0.ItemName)
                {
                    _playerInventory.Counts[i] -= crafts[number].Count0;
                }

                if (_playerInventory.Items[i].ItemName == crafts[number].Ingridient1.ItemName)
                {
                    _playerInventory.Counts[i] -= crafts[number].Count1;
                }
            }
        }

        _playerInventory.Refresh();

        CraftPanel.SetActive(!_panelController);
        Cursor.visible = !_panelController;
        Cursor.lockState = CursorLockMode.None;
        _playerController.CanMove = _panelController;
    }
}
