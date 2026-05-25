using UnityEngine;
using System.Collections.Generic;

public class MagnetronExperiment : MonoBehaviour
{
    private WireConnector selectedSocket = null;
    private Dictionary<string, string> expectedConnections = new Dictionary<string, string>();

    public Multimeter ammeter;      // левый мультиметр (амперметр)
    public Multimeter microAmmeter; // правый мультиметр (микроамперметр)
    public bool powerSolenoidOn = false;
    public bool powerAnodeOn = false;

    private PhysicsCalculator calculator; // ваш старый скрипт (адаптированный)

    public void SetSolenoidPower(bool on) { powerSolenoidOn = on; }
    public void SetAnodePower(bool on) { powerAnodeOn = on; }

    // Схема правильных соединений (согласно вашему скриншоту)
    void Start()
    {
        // Пример: левый мультиметр (амперметр) включается последовательно с соленоидом
        expectedConnections["SolenoidA"] = "Ammeter10A";
        expectedConnections["AmmeterCOM"] = "PowerSupplySolenoidNeg";
        expectedConnections["PowerSupplySolenoidPos"] = "SolenoidB";

        // Анодная цепь: анод лампы -> микроамперметр -> плюс анодного питания
        expectedConnections["DiodeAnode"] = "MicroAmmeterCOM";   // микроамперметр включается в разрыв анодной цепи
        expectedConnections["MicroAmmetermA"] = "AnodeSupplyPos";
        expectedConnections["AnodeSupplyNeg"] = "DiodeCathode";

        calculator = GetComponent<PhysicsCalculator>();

        // и т.д. – заполните все пары по вашей схеме
    }

    public void HandleSocketClick(WireConnector socket)
    {
        if (selectedSocket == null)
        {
            // первое гнездо выбрано
            selectedSocket = socket;
            HighlightSocket(socket, true);
        }
        else
        {
            // второе гнездо – пытаемся соединить
            string pairKey = selectedSocket.socketId + "->" + socket.socketId;
            string reverseKey = socket.socketId + "->" + selectedSocket.socketId;
            bool isValid = false;
            foreach (var kv in expectedConnections)
            {
                if (pairKey == kv.Key + "->" + kv.Value || reverseKey == kv.Key + "->" + kv.Value)
                {
                    isValid = true;
                    break;
                }
            }
            if (isValid)
            {
                // Соединить
                selectedSocket.CreateWire(socket.gameObject);
                selectedSocket.isConnected = true;
                socket.isConnected = true;
                Debug.Log($"Проводник создан между {selectedSocket.socketId} и {socket.socketId}");
            }
            else
            {
                // Неправильное соединение – красная подсветка на 0.5 сек
                StartCoroutine(FlashRed(socket));
                StartCoroutine(FlashRed(selectedSocket));
                Debug.Log("Ошибка: неверное соединение!");
            }
            // Сброс выделения
            HighlightSocket(selectedSocket, false);
            selectedSocket = null;
        }
    }

    void HighlightSocket(WireConnector socket, bool highlight)
    {
        // Изменить цвет материала или включить outline
        var renderer = socket.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = highlight ? Color.green : Color.white;
    }

    System.Collections.IEnumerator FlashRed(WireConnector socket)
    {
        var renderer = socket.GetComponent<Renderer>();
        Color original = renderer.material.color;
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        renderer.material.color = original;
    }

    public void SetSolenoidCurrent(double ic)
    {
        Debug.Log($"SetSolenoidCurrent called with ic = {ic}");

        if (!powerSolenoidOn || !powerAnodeOn)
        {
            Debug.Log("Питание не включено!");
            return;
        }

        // Обновить показания амперметра
        if (ammeter != null) ammeter.SetCurrent(ic);

        // Рассчитать анодный ток через PhysicsCalculator
        double ia = calculator.CalculateIa(ic);

        // Отладочные логи (теперь ia уже определена)
        Debug.Log($"ammeter = {(ammeter == null ? "NULL" : "OK")}");
        Debug.Log($"microAmmeter = {(microAmmeter == null ? "NULL" : "OK")}");
        Debug.Log($"calculator = {(calculator == null ? "NULL" : "OK")}");
        Debug.Log($"ia = {ia} μA");

        // Отобразить на микроамперметре
        if (microAmmeter != null) microAmmeter.SetCurrent(ia);
    }
}