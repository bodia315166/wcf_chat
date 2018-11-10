using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IServiceChat" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IServerChatCallback))]    // інформація про те що ми використовуємо 2 інтерфейс
    public interface IServiceChat
    {
        [OperationContract]    //цей атрибут дозволяє виконувати зі сторони клієнта методи
        int Connect(string name);//цей метод вертає id підключеного клієнта

        [OperationContract]
        void Disconnect(int id);  // преиймає значення id клієнта якого потрібно відключити

        [OperationContract(IsOneWay = true)] // щоб не чикати відповіді від сервіса клієнту
        void SendMsg(string msg, int id);

    }

    public interface IServerChatCallback
    {
        [OperationContract(IsOneWay = true)]   //без цього сервер буде чикати відповіді від клієнта
        void MsgCallback(string msg);
    }
}
