using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public ItemDB[] itemDataBases = null;

    public ItemDB this[ItemCode code] => itemDataBases[(int)code];
    public ItemDB this[int index] => itemDataBases[index];
}
