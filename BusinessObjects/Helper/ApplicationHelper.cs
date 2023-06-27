using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Helper
{
    public static class ApplicationHelper
    {
        public static string GetMonthName(int monthInNumber)
        {
            string monthName = string.Empty;
            switch (monthInNumber)
            {
                case 1: { 
                        monthName = "January";
                        break;
                    }
                case 2:
                    {
                        monthName = "Feburary";
                        break;
                    }
                case 3:
                    {
                        monthName = "March";
                        break;
                    }
                case 4:
                    {
                        monthName = "April";
                        break;
                    }
                case 5:
                    {
                        monthName = "May";
                        break;
                    }
                case 6:
                    {
                        monthName = "June";
                        break;
                    }
                case 7:
                    {
                        monthName = "July";
                        break;
                    }
                case 8:
                    {
                        monthName = "August";
                        break;
                    }
                case 9:
                    {
                        monthName = "September";
                        break;
                    }
                case 10:
                    {
                        monthName = "October";
                        break;
                    }
                case 11:
                    {
                        monthName = "November";
                        break;
                    }
                case 12:
                    {
                        monthName = "December";
                        break;
                    }

            }
            return monthName;
        }
    }
}
