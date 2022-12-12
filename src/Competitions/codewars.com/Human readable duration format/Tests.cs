using NUnit.Framework;
using System;
using System.Linq;

[TestFixture]
public class Tests
{
    [Test]
    public void basicTests()
    {
        Assert.AreEqual("now", HumanTimeFormat.formatDuration(0));
        Assert.AreEqual("1 second", HumanTimeFormat.formatDuration(1));
        Assert.AreEqual("1 minute and 2 seconds", HumanTimeFormat.formatDuration(62));
        Assert.AreEqual("2 minutes", HumanTimeFormat.formatDuration(120));
        Assert.AreEqual("1 hour, 1 minute and 2 seconds", HumanTimeFormat.formatDuration(3662));
        Assert.AreEqual("182 days, 1 hour, 44 minutes and 40 seconds", HumanTimeFormat.formatDuration(15731080));
        Assert.AreEqual("4 years, 68 days, 3 hours and 4 minutes", HumanTimeFormat.formatDuration(132030240));
        Assert.AreEqual("6 years, 192 days, 13 hours, 3 minutes and 54 seconds", HumanTimeFormat.formatDuration(205851834));
        Assert.AreEqual("8 years, 12 days, 13 hours, 41 minutes and 1 second", HumanTimeFormat.formatDuration(253374061));
        Assert.AreEqual("7 years, 246 days, 15 hours, 32 minutes and 54 seconds", HumanTimeFormat.formatDuration(242062374));
        Assert.AreEqual("3 years, 85 days, 1 hour, 9 minutes and 26 seconds", HumanTimeFormat.formatDuration(101956166));
        Assert.AreEqual("1 year, 19 days, 18 hours, 19 minutes and 46 seconds", HumanTimeFormat.formatDuration(33243586));
    }

    private const int MINS = 60;
    private const int HOURS = 60 * MINS;
    private const int DAYS = 24 * HOURS;
    private const int YEARS = 365 * DAYS;
    private string Sol(int seconds)
    {
        var years = 0;
        var days = 0;
        var hours = 0;
        var minutes = 0;
        var toParse = seconds;
        var ret = "";
        if (toParse == 0) return "now";
        if (toParse >= YEARS)
        {
            years = toParse / YEARS;
            toParse = toParse % YEARS;
        }
        if (toParse >= DAYS)
        {
            days = toParse / DAYS;
            toParse = toParse % DAYS;
        }
        if (toParse >= HOURS)
        {
            hours = toParse / HOURS;
            toParse = toParse % HOURS;
        }
        if (toParse >= MINS)
        {
            minutes = toParse / MINS;
            toParse = toParse % MINS;
        }
        if (years > 0)
        {
            if (years == 1) ret += string.Format("{0} year,", years);
            else ret += string.Format("{0} years,", years);
        }
        if (days > 0)
        {
            if (days == 1) ret += string.Format("{0} day,", days);
            else ret += string.Format("{0} days,", days);
        }
        if (hours > 0)
        {
            if (hours == 1) ret += string.Format("{0} hour,", hours);
            else ret += string.Format("{0} hours,", hours);
        }
        if (minutes > 0)
        {
            if (minutes == 1) ret += string.Format("{0} minute,", minutes);
            else ret += string.Format("{0} minutes,", minutes);
        }
        if (toParse > 0)
        {
            if (toParse == 1) ret += string.Format("{0} second,", toParse);
            else ret += string.Format("{0} seconds,", toParse);
        }
        ret = ret.TrimEnd(',');
        var retSplit = ret.Split(',');
        var count = retSplit.Count();
        if (count > 1)
        {
            ret = string.Join(", ", retSplit.Take(count - 1));
            ret += " and ";
            ret += retSplit.Last();
        }
        return ret.Trim();
    }

    [Test]
    public void RandomTests()
    {
        Random r = new Random();
        for (int i = 0; i < 100; i++)
        {
            int n = r.Next(10000000);
            Assert.AreEqual(Sol(n), HumanTimeFormat.formatDuration(n));
        }
    }
}