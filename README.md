# InventorySystem

###### Функции у Inventory.cs

```C#
// Ивенты
Inventory.OnChangeItems(); // Вызывается когда меняется кол-во предметов

// Функции 
Inventory.RemoveItem(Item targetItem, int count = 1); // Удалить предмет (return bool) "true - успешно удалено false - возникли ошибки"
Inventory.AddItem(Item targetItem, int count = 1); // Добавить предмет в инвентарь
Inventory.Clear(); // Очистить инвентарь
Inventory.GetNullCell(); // Найти свободную ячейку (return ItemCell)
Inventory.FindItems(Item targetItem); // Найти определенный предмет (return List<ItemCell>)
Inventory.GetCountItem(Item targetItem); // Получить кол-во определенных предметов (return int)
Inventory.GetAllItems(); // Получить все предметы которые лежат в инвенторе (return List<ItemCell>)
Inventory.GetSizeGrid(); // Получить размер сетки (return Vector2Int) "x - weight y - height"
```

###### Функции у ItemCell.cs

```C#
ItemCell.SetCount(int count = 1); // Установить кол-во предметов
ItemCell.SetItem(Item targetItem, int count = 1); // Установить предмет и кол-во
ItemCell.SetBlock(bool flag); // Заблокировать/разблокировать ячейку
ItemCell.DeleteItem(); // Удалить предмет
ItemCell.ReloadVisual(); // Обновить отоброжение
```
