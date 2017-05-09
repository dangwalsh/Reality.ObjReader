using Xunit;
using Reality.ObjReader;

namespace MyFirstUWPTests {
    public class UnitTest {
        [Fact]
        public void PassingTest() {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest() {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y) {
            return x + y;
        }

        [Fact]
        public void ImportTest() {
            var path = @"C:\Users\22791\Documents\git\github.com\dangwalsh\objreader\data\test.obj";
            Assert.Equal(2, Facade.ImportObjects(path));
        }
    }
}
