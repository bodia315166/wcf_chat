using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace wcf_chat
{
  
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]//для того щоб не створювався кожного разу новий сервіc а був один для всіх клієнтів
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();  // список юзерів з класу зі змінними
        int nextId = 1;//для регенерації нового id

        public int Connect(string name)
        {
            
            ServerUser user = new ServerUser() {     //присвоюємо дані клієнта при створенні
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;

            SendMsg(": "+user.Name+" Connect",0);
            users.Add(user);       //добавим юзера в список
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);    //пошук юзера
            if (user!=null)
            {
                users.Remove(user);      //видаляєм юзера
                SendMsg(": "+user.Name + " disonnect!",0);
            }
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)      //перебераєм юзерів
            {
                string answer = DateTime.Now.ToShortTimeString();    // час відправлення смс

                var user = users.FirstOrDefault(i => i.ID == id);    //пошук юзера
                if (user != null)
                {
                    answer += ": " + user.Name+" ";
                }
                answer += msg;           // та саме смс (формування)
                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
            }    //відправслення смс іншим юзерам
        }
    }
}
