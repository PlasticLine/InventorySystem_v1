# InventorySystem

###### Функции у Inventory.cs

```C#
Inventory.AddItem(Item targetItem, int count = 1); // Добавить предмет в инвентарь
Inventory.Clear(); // Очистить инвентарь
Inventory.GetNullCell(); // Найти свободную ячейку (return ItemCell)
Inventory.FindItems(Item targetItem); // Найти определенный предмет (return List<ItemCell>)
Inventory.GetCountItem(Item targetItem); // Получить кол-во определенных предметов (return int)
Inventory.GetAllItems(); // Получить все предметы которые лежат в инвенторе (return List<ItemCell>)
```

###### Функции у ItemCell.cs

```C#
ItemCell.SetCount(int count = 1); // Установить кол-во предметов
ItemCell.SetItem(Item targetItem, int count = 1); // Установить предмет и кол-во
ItemCell.DeleteItem(); // Удалить предмет
ItemCell.ReloadVisual(); // Обновить отоброжение
```
