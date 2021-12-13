using Microsoft.VisualStudio.TestTools.UnitTesting;
using Question2;

namespace HighCardTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void WildCardNotNull()
        {
            HighCard game = new(20, 1);
            Assert.IsNotNull(game.wc);
        }
    }
}