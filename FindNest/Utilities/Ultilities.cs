namespace FindNest.Utilities
{
    public static class Ultilities
    {
        public static string FormatCurrency(long amount)
        {
            if (amount >= 1_000_000_000)
            {
                decimal billions = amount / 1_000_000_000m;
                return $"{billions:N1} tỉ";
            }
            else if (amount >= 1_000_000)
            {
                decimal millions = amount / 1_000_000m;
                return $"{millions:N1} triệu";
            }
            else if (amount >= 1_000)
            {
                decimal thousands = amount / 1_000m;
                return $"{thousands}k";
            }
            else { return amount.ToString(); }
        }
        
        public static string GetRelativeTime(DateTime date)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeDifference = now - date;

            if (timeDifference.TotalDays >= 365)
            {
                int years = (int)(timeDifference.TotalDays / 365);
                return years == 1 ? "1 năm trước" : $"{years} năm trước";
            }
            else if (timeDifference.TotalDays >= 30)
            {
                int months = (int)(timeDifference.TotalDays / 30);
                return months == 1 ? "1 tháng trước" : $"{months} tháng trước";
            }
            else if (timeDifference.TotalDays >= 7)
            {
                int weeks = (int)(timeDifference.TotalDays / 7);
                return weeks == 1 ? "1 tuần trước" : $"{weeks} tuần trước";
            }
            else if (timeDifference.TotalDays >= 1)
            {
                int days = (int)timeDifference.TotalDays;
                return days == 1 ? "1 ngày trước" : $"{days} ngày trước";
            }
            else if (timeDifference.TotalHours >= 1)
            {
                int hours = (int)timeDifference.TotalHours;
                return hours == 1 ? "1 giờ trước" : $"{hours} giờ trước";
            }
            else if (timeDifference.TotalMinutes >= 1)
            {
                int minutes = (int)timeDifference.TotalMinutes;
                return minutes == 1 ? "1 phút trước" : $"{minutes} phút trước";
            }
            else
            {
                return "Vừa xong";
            }
        }
        
        public static string HidePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 3)
            {
                return phoneNumber;
            }
    
            return phoneNumber.Substring(0, 3) + new string('*', phoneNumber.Length - 3);
        }

    }
}