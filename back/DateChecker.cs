﻿namespace back;

public static class DateChecker
{
    public static bool IsReadyForNextCheck()
    {
        var curDate = DateOnly.FromDateTime(DateTime.Now);
        
        if (StaticStorage.lastCheckDate is not null)
        {
            return SimpleCheckAndUpdateForLastCheckDate(curDate, StaticStorage.lastCheckDate.Value);
        }

        if (File.Exists("LastCheckDate.txt"))
        {
            var sr = new StreamReader("LastCheckDate.txt");
            var dateText = sr.ReadLine();
            sr.Close();
            
            StaticStorage.lastCheckDate = DateOnly.Parse(dateText!);

            return SimpleCheckAndUpdateForLastCheckDate(curDate, StaticStorage.lastCheckDate.Value);
        }

        var fs = File.Create("LastCheckDate.txt");
        fs.Close();
        
        var sw = new StreamWriter(fs);
        sw.Write(curDate.ToString());
        sw.Close();

        StaticStorage.lastCheckDate = curDate;

        return true;
    }

    private static bool SimpleCheckAndUpdateForLastCheckDate(DateOnly dateToCheck, DateOnly lastCheckDate)
    {
        var res = dateToCheck > lastCheckDate.AddDays(3);
        
        if (res)
        {
            using var fs = File.Create("LastCheckDate.txt");
            using var sw = new StreamWriter(fs);
            sw.Write(DateOnly.FromDateTime(DateTime.Now).ToString());

            StaticStorage.lastCheckDate = DateOnly.FromDateTime(DateTime.Now);
        }

        return res;
    }
    
    public static bool IsDateExpired(DateOnly dateToCheck)
    {
        return dateToCheck < DateOnly.FromDateTime(DateTime.Now).AddDays(10);
    }
}