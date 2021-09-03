using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Utils.TestProject;

[TestClass]
public class IdGeneratorUnitTest
{
    [TestMethod]
    public void IdGenerator_TestMethod()
    {
        IdGenerator idGenerator = new IdGenerator();
        Assert.IsTrue(idGenerator.GenerateId().Length == 14);
    }
}

