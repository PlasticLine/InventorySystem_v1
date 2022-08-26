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

// Перемещение
Inventory.ItemDragCount(ItemCell fromCell, ItemCell whereCell, int count = 1); // Перемещает определенное кол-во предметов за раз
Inventory.ItemDragSplitter(ItemCell fromCell, ItemCell whereCell, float splitter = 1); // Перемещает разделенный предмет
```

###### Функции у Item.cs
```C#
Item.SetMetaData(string key, string value); // Установить meta данные
Item.GetMetaData(string key) => return string; // Получить meta данные
Item.GetMetaDatas() => return Dictionary<string, string>; // Получить весь список мета данных предмета
Item.SetMetaDatas(Dictionary<string, string> metaData); // Установить список мета данных
```

###### Функции у ItemCell.cs

```C#
ItemCell.SetCount(int count = 1); // Установить кол-во предметов
ItemCell.SetItem(Item targetItem, int count = 1); // Установить предмет и кол-во
ItemCell.SetBlock(bool flag); // Заблокировать/разблокировать ячейку
ItemCell.DeleteItem(); // Удалить предмет
ItemCell.ReloadVisual(); // Обновить отоброжение
```
