using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace abtest
{
    public class Calculate
    {
        public int X { get; set; }
        public double CountDateLastActivity { get; set; }
        public double CountDateRegistration { get; set; }
        public double RollingRetention { get; set; }
        public string TimeSQL { get; set; }
        public string TimeMath { get; set; }

        public Calculate()
        {
            DBConnect DbConnect = new DBConnect(true);
            StringBuilder Query = new StringBuilder();
            X = 7;

            // Данные будут браться разом за одно подкючение к БД, поэтому сформируем большой запрос
            Query.Append("SELECT COUNT(*) FROM public.\"UserAudit\"");
            Query.Append(" WHERE \"DateLastActivity\" >= NOW()::DATE-EXTRACT(DOW FROM NOW())::INTEGER-" + X);
            Query.Append(" UNION ALL");
            Query.Append(" SELECT COUNT(*) FROM public.\"UserAudit\"");
            Query.Append(" WHERE \"DateRegistration\" <= NOW()::DATE-EXTRACT(DOW FROM NOW())::INTEGER-" + X);

            // Отследим время подключения к БД и выполнения запроса
            Stopwatch Watch = new Stopwatch();
            Watch.Start();
            IEnumerable<dynamic> Result = DbConnect.DBQuery(Query.ToString());
            Watch.Stop();
            TimeSQL = (Watch.ElapsedMilliseconds).ToString();

            // Сохраняем даные из запроса
            bool isReg = false;
            foreach(dynamic row in Result)
            {
                if (isReg)
                {
                    CountDateRegistration = Convert.ToDouble(row.count);
                }
                else
                {
                    CountDateLastActivity = Convert.ToDouble(row.count);
                    isReg = true;
                }
            }

            // Если данные подходят - считам RollingRetention, заодно засекаем время
            Watch.Restart();
            if(CountDateRegistration != 0)
                RollingRetention = CountDateLastActivity / CountDateRegistration * 100;
            Watch.Stop();

            TimeMath = (Watch.ElapsedMilliseconds).ToString();
        }
    }
}
