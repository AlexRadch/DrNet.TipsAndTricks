namespace myjinxin
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class myjinxin
    {

        [Test]
        public void BasicTests()
        {
            var kata = new Kata();

            Assert.AreEqual(true, kata.IsThueMorse(new int[] { 0, 1, 1, 0, 1 }));

            Assert.AreEqual(true, kata.IsThueMorse(new int[] { 0 }));

            Assert.AreEqual(false, kata.IsThueMorse(new int[] { 1 }));

            Assert.AreEqual(false, kata.IsThueMorse(new int[] { 0, 1, 0, 0 }));


        }

    }
}