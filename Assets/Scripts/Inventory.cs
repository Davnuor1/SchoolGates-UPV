using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public int count;
        public int MaxAllowed;
        public Sprite icon;
        public Item item;
        public Slot()
        {
            itemName = "";
            count = 0;
            MaxAllowed = 2;
            item = null;
            
        }
        public bool IsEmpty
        {
            get
            {
                if(itemName==""&& count == 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CanAddItem(string itemName)
        {
            if (this.itemName==itemName && count < MaxAllowed)
            {
                return true;
            }
            return false;
        }
        public void AddItem(Item item)
        {
            this.itemName = item.data.itemName;
            //Debug.Log(this.itemName);
            this.icon = item.data.icon;
            this.item = item;
            count++;
        }

        public void AddItem(string itemName, Sprite icon, int maxAllowed, Item item)
        {
            this.itemName = itemName;
            this.icon = icon;
            count++;
            this.MaxAllowed = maxAllowed;
            this.item = item;
        }


        public void RemoveItem()
        {
           if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    icon=null;
                    item = null;
                    itemName = "";
                }
            }
        }
    }
    public List<Slot> slots = new List<Slot>();
    public Inventory(int numSlots)
    {
        for (int i=0; i< numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }
    public void Add(Item item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.itemName==item.data.itemName && slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item);
                return;
            }
        }
        foreach(Slot slot in slots)
        {
            if (slot.itemName =="")
            {
                slot.AddItem(item);
                return;
            }
        }
    }
    public void Remove(int index)
    {
        slots[index].RemoveItem();
        

    }
    public void Remove(int index,int numToRemove)
    {
        if(slots[index].count >= numToRemove)
        {
            for(int i=0; i < numToRemove; i++)
            {
                Remove(index);
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove=1)
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        for (int i = 0; i < numToMove; i++) 
        {
            if (toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemName))
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.MaxAllowed,fromSlot.item);
                fromSlot.RemoveItem();
            } 
        }
    }
}
