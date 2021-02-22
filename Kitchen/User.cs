using System;
using System.Collections.Generic;
using System.Linq;

namespace abtest
{
    public class User
    {
        public int ID { get; set; }
        public DateTime DateRegistration { get; set; }
        public DateTime DateLastActivity { get; set; }

        public IEnumerable<User> GetAudit()
        {
            DBConnect DbConnect = new DBConnect(true);
            IEnumerable<User> Audit = Enumerable.Empty<User>();
            foreach (var row in DbConnect.DBQuery("SELECT \"UserID\", \"DateRegistration\", \"DateLastActivity\" FROM public.\"UserAudit\""))
            {
                User Item = new User();
                Item.ID = row.UserID;
                Item.DateRegistration = row.DateRegistration;
                Item.DateLastActivity = row.DateLastActivity;

                Audit = Audit.Concat(new[] { Item });
            }

            return Audit;
        }

        public bool SetAudit(UserMSG msg)
        {
            bool isUpdate = false;
            DBConnect DbConnect = new DBConnect(true);

            int nowID = Convert.ToInt32(msg.id);
            DateTime nowDateRegistration = DateTime.Parse(msg.dateRegistration);
            DateTime nowDateLastActivity = DateTime.Parse(msg.dateLastActivity);

            // Есть ли такой UserID в БД?
            dynamic res = DbConnect.DBQuerySingle("SELECT \"ID\" FROM public.\"UserAudit\" WHERE \"UserID\" = @0", nowID);
            if (res != null)
            {
                // Обновляем даты на существующего юзера
                DbConnect.DBExecute("UPDATE public.\"UserAudit\" SET \"DateRegistration\" = @0, \"DateLastActivity\" = @1 " +
                    "WHERE \"ID\" = @2", nowDateRegistration, nowDateLastActivity, res.ID);
            }
            else
            {
                // Добавляем нового юзера
                DbConnect.DBExecute("INSERT INTO public.\"UserAudit\"(\"UserID\", \"DateRegistration\", \"DateLastActivity\") " +
                    "VALUES(@0, @1, @2)", nowID, nowDateRegistration, nowDateLastActivity);
            }

            return isUpdate;
        }
    }

    public class UserMSG
    {
        public string id { get; set; }
        public string dateRegistration { get; set; }
        public string dateLastActivity { get; set; }
    }
}
