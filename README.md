# InventorySystem

###### Функции у Inventory.cs

```C#
Inventory.AddItem(Item targetItem, int count = 1); // Добавить предмет в инвентарь
Inventory.Clear(); // Очистить инвентарь
Inventory.GetNullCell(); // Найти свободную ячейку
Inventory.FindItems(Item targetItem); // Найти определенный предмет (return List<Item>)
Inventory.GetCountItem(Item targetItem); // Получить кол-во определенных предметов
Inventory.GetAllItems(); // Получить все предметы которые лежат в инвенторе
```
