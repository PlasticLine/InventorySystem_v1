# InventorySystem

###### Функции у Inventory.cs

```C#
// Ивенты
Inventory.OnChangeItems(); // Вызывается когда меняется кол-во предметов

// Функции 
Inventory.RemoveItem(Item targetItem, int count = 1) return bool; // Удалить предмет
Inventory.AddItem(Item targetItem, int count = 1); // Добавить предмет в инвентарь
Inventory.Clear(); // Очистить инвентарь
Inventory.GetNullCell() return ItemCell; // Найти свободную ячейку
Inventory.FindItems(Item targetItem) return List<ItemCell>; // Найти определенный предмет
Inventory.GetCountItem(Item targetItem) return int; // Получить кол-во определенных предметов
Inventory.GetAllItems() return List<ItemCell>; // Получить все предметы которые лежат в инвенторе
Inventory.GetSizeGrid() return Vector2Int; // Получить размер сетки
Inventory.GetSizeCell() return float; // Вернет размер ячейки
Inventory.MetadataComparison(Item oneMetaData, Item twoMetaData) return bool; // Проверка на совместимость мета данных

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
