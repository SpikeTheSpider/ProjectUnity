public enum ButtonAction
{
    Info,
    Interact,
    Hack
}

public interface IInteractable
{
    void ShowInfo(); // Показывает информацию об объекте
    void Interact(); // Взаимодействие с объектом
    void Hack(); // Взлом объекта (открывает редактор кода)

    string GetDefaultCode(); // Возвращает стандартный код для объекта
    string GetHelpInfo(); // Возвращает справочную информацию для объекта

    // Новые методы для сохранения и получения текущего кода
    string GetCurrentCode(); // Возвращает текущий код, введенный игроком
    void SetCurrentCode(string code); // Сохраняет текущий код
}