using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerInventory : MonoBehaviour
{
    public GameObject CenterText;
    public Transform Hand;
    public GameObject BuildingPrefab;
    public GameObject NewBuildingPrefab;

    public GameObject[] Cells;
    public Item[] Items = new Item[9];
    public KeyCode[] KeysToSelect = new KeyCode[9];
    public TMP_Text[] CountsTexts = new TMP_Text[9];
    public int[] Counts = new int[9];

    private GameObject _newBuilding;
    private GameObject _building;
    private GameObject _selectedGameObject;
    private Image[] _images = new Image[9];
    private GameObject[] _selection = new GameObject[9];
    private PlayerStats _playerStats;
    private Tool _selectedTool;
    private PlayerController _playerController;
    private Animator _animator;
    private float _distance = 5;
    private int _selectedItem = 0;
    private int _maxItemsInCell = 5;
    private Resource _hitResourse;
    public GameObject _hitGameObject;

    private void Start()
    {
        _playerStats = gameObject.GetComponent<PlayerStats>();
        _playerController = gameObject.GetComponent<PlayerController>();
        _animator = gameObject.GetComponentInChildren<Animator>();

        for (int i = 0; i < Cells.Length; i++)
        {
            _images[i] = Cells[i].transform.GetChild(0).GetComponent<Image>();
            CountsTexts[i] = Cells[i].transform.GetChild(1).GetComponent<TMP_Text>();
            _selection[i] = Cells[i].transform.GetChild(2).gameObject;
        }

        Refresh();
    }

    private void Update()
    {
        FindScreenCenter();

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            MouseScrollWheel();
        }

        for (int i = 0; i < KeysToSelect.Length; i++)
        {
            if (Input.GetKeyDown(KeysToSelect[i]))
            {
                _selectedItem = i;

                ChangeSelected();
            }
        }

        Eating();
    }

    public void StartBuilding()
    {
        _building = Instantiate(BuildingPrefab, transform.position, Quaternion.identity);
        _newBuilding = Instantiate(NewBuildingPrefab, _building.transform.GetChild(0)).gameObject;
    }

    private void Eating()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Items[_selectedItem])
            {
                if (Items[_selectedItem].GetComponent<Food>())
                {
                    _playerStats.Hunger += Items[_selectedItem].GetComponent<Food>().FoodRegen;
                    Counts[_selectedItem]--;

                    Refresh();
                }
            }
        }
    }

    private void FindScreenCenter()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _distance))
        {
            if (_building)
            {
                Vector3 newPosition = hit.point;
                newPosition += Vector3.up / 2;

                _building.transform.position = newPosition;

                if (Input.GetKey(KeyCode.N))
                {
                    _building.transform.Rotate(0, -1, 0);
                }
                else if (Input.GetKey(KeyCode.M))
                {
                    _building.transform.Rotate(0, 1, 0);
                }

                if (Input.GetMouseButtonDown(0) && _building.GetComponent<Building>().InTrigger.Count == 0)
                {
                    _newBuilding.transform.parent = null;
                    _newBuilding.GetComponent<MeshCollider>().enabled = true;

                    Destroy(_building);
                    _building = null;

                    Counts[_selectedItem]--;

                    Refresh();
                }
            }
            else
            {
                _hitGameObject = hit.collider.gameObject;

                if (_hitGameObject)
                {
                    if (_hitGameObject.GetComponent<Resource>())
                    {
                        _hitResourse = _hitGameObject.GetComponent<Resource>();
                    }

                    if (Input.GetKeyDown(KeyCode.Q) && Items[_selectedItem])
                    {
                        Items[_selectedItem].gameObject.SetActive(true);
                        Items[_selectedItem].transform.position = hit.point;
                        Items[_selectedItem].transform.position += Vector3.up * 0.2f;

                        Counts[_selectedItem]--;

                        Refresh();
                    }

                    if (_hitGameObject.GetComponent<Item>())
                    {
                        CenterText.SetActive(true);

                        if (Input.GetKey(KeyCode.F))
                        {
                            AddItem(hit.collider.GetComponent<Item>());
                        }
                    }
                    else
                    {
                        CenterText.SetActive(false);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Items[_selectedItem])
            {
                _animator.Play("attackHand");

                if (_selectedTool && _hitResourse)
                {
                    if (_selectedTool.type == _hitResourse.type)
                    {
                        _hitResourse.Damage(_selectedTool.Damage);

                        if (_hitResourse.type == Resource.Type.wood)
                        {
                            if (_hitResourse.Health <= 0)
                            {
                                if (_hitGameObject)
                                {
                                    _hitGameObject.GetComponent<Rigidbody>().AddForce(_playerController.Forward, ForceMode.Impulse);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void AddItem(Item newItem)
    {
        if (newItem)
        {
            bool isHaveItem = false;
            bool canAdd = false;

            for (int i = 0; i < Items.Length; i++)
            {
                if (!Items[i])
                {
                    canAdd = true;
                    break;
                }
            }

            if (canAdd)
            {
                if (newItem.IsStackable)
                {
                    for (int i = 0; i < Items.Length; i++)
                    {
                        if (Items[i] != null)
                        {
                            if (Items[i].ItemName == newItem.ItemName)
                            {
                                if (Counts[i] < _maxItemsInCell)
                                {
                                    Counts[i]++;
                                    isHaveItem = true;
                                    newItem.gameObject.SetActive(false);

                                    break;
                                }
                            }
                        }
                    }
                }

                if (!isHaveItem)
                {
                    for (int i = 0; i < Items.Length; i++)
                    {
                        if (!Items[i])
                        {
                            Items[i] = newItem;
                            Counts[i] = 1;
                            newItem.gameObject.SetActive(false);

                            break;
                        }
                    }
                }
            }

            Refresh();
        }
    }

    public void Refresh()
    {
        for(int i = 0; i < Items.Length; i++)
        {
            if (Counts[i] == 0)
            {
                Items[i] = null;
            }

            if (Items[i])
            {
                _images[i].enabled = true;
                _images[i].sprite = Items[i].Icon;

                if (Counts[i] > 1)
                {
                    CountsTexts[i].text = Counts[i].ToString();
                }
                else
                {
                    CountsTexts[i].text = "";
                }

            }
            else
            {
                _images[i].enabled = false;
                CountsTexts[i].text = "";
            }
        }

        ChangeSelected();
    }

    private void MouseScrollWheel()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _selectedItem--;
        }
        else
        {
            _selectedItem++;
        }

        if (_selectedItem < 0)
        {
            _selectedItem = Items.Length - 1;
        }
        else if (_selectedItem >= Items.Length)
        {
            _selectedItem = 0;
        }
        _animator.Play("SwitchHand");
        ChangeSelected();
    }

    public void ChangeSelected()
    {
        if (_building)
        {
            Destroy(_newBuilding);
            _newBuilding = null;
            Destroy(_building);
            _building = null;
        }

        for (int i = 0; i < _selection.Length; i++)
        {
            if(_selectedItem != i)
            {
                _selection[i].SetActive(false);
            }
            else
            {
                _selection[i].SetActive(true);
            }
        }

        if (Items[_selectedItem])
        {
            if (Items[_selectedItem].BuildingPrefab)
            {
                StartBuilding();
            }

            for (int i = 0; i < Hand.childCount; i++)
            {
                if(Hand.GetChild(i).name == Items[_selectedItem].ItemName)
                {
                    Hand.GetChild(i).gameObject.SetActive(true);
                    _selectedGameObject = Hand.GetChild(i).gameObject;

                    if (Items[_selectedItem].GetComponent<Tool>())
                    {
                        _selectedTool = Items[_selectedItem].GetComponent<Tool>();
                    }
                    else
                    {
                        _selectedTool = null;
                    }
                }
                else
                {
                    Hand.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if(_selectedGameObject)
            {
                _selectedGameObject.SetActive(false);
                _selectedGameObject = null;
            }
        }
    }
}