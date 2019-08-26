using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Threading;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 扩展日历,主要计算波斯日历
    /// </summary>
    public class xCalendar
    {
        #region "转换波斯日历"
        /// <summary>
        /// 转换日期到波斯时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public  DateTime GetDateOfPersian(DateTime date)
        {
            
            //如果客户是波斯日历需要开启这里
            System.Globalization.PersianCalendar jc = new System.Globalization.PersianCalendar();
            if (Thread.CurrentThread.CurrentCulture.Name == "fa-IR" && 
                (Thread.CurrentThread.CurrentCulture.Calendar.ToString() == "DotNetNuke.PersianLibrary.DNNPersianDate" 
                || Thread.CurrentThread.CurrentCulture.Calendar.ToString() == "System.Globalization.PersianCalendar"  ))// if (cu.CalendarExists(CultureInfo.CreateSpecificCulture("fa-IR")))  
            {
                
                return jc.ToDateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, System.Globalization.PersianCalendar.PersianEra);
            }
            return date;
        }
        /// <summary>
        /// 转换日期到波斯时间（获取当前时间）
        /// </summary>
        /// <returns></returns>
        public  DateTime GetDateOfPersian()
        {
            return GetDateOfPersian(xUserTime.UtcTime());
        }

        #endregion

    

        /// <summary>
        /// 处理波斯历
        /// </summary>
        /// <param name="Time"></param>
        public static DateTime Process(DateTime Time)
        {
            xCalendar x = new xCalendar();
            //return x.GetDateOfPersian(Time);
            return Time;
        }

 
    }


     
}