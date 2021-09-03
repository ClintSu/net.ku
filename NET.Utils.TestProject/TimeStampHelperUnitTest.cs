using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NET.Utils.TestProject;
[TestClass]
public class TimeStampHelperUnitTest
{
    [TestMethod]
    public void GetTimeStamp_TestMethod()
    { 
        DateTime time = DateTime.Now;
        var ts = TimestampHelper.GetTimestamp(time);
        var dt = TimestampHelper.GetDateTime(ts);
        Assert.AreEqual(time, dt); 
    }

    [TestMethod]
    public void Test()
    {
    }

     
}