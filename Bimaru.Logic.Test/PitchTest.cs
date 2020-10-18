using System.IO;
using Bimaru.Logic.Local;
using NUnit.Framework;

namespace Bimaru.Logic.Test
{
    public class PitchTests
    {
        [Test]
        [TestCase(@"
  123456
 +------+
1|O  O|2
2|      |1
3|      |1
4|  O   |3
5|      |1
6| X    |2
 +------+
  212203
1x3, 2x2, 3x1
", TestName = "short line")]
        [TestCase(@"
  123456
 +------+
1|O    O|2
2|      |1
3|      |1
4|  O   |3
5|      |1
 +------+
  212203
1x3, 2x2, 3x1", TestName = "5 lines only")]
        [TestCase(@"
  123456
 +------+
1|O    O|
2|      |1
3|      |1
4|  O   |3
5|      |1
6| X    |2
 +------+
  212203
1x3, 2x2, 3x1
", TestName = "no num at the end")]
        [TestCase(@"
  123456
 +------+
1|O    O|2
2|      |1
3|      |1
4|  O   |3
5|      |1
6| X    |2
  212203
1x3, 2x2, 3x1
", TestName = "invalid border at the end")]
        [TestCase(@"
  12345
 +-----+
1|O    |2
2|     |1
3|     |1
4|  O  |3
5|     |1
6| X   |2
 +-----+
  21220
1x3, 2x2, 3x1
", TestName = "too few column constraints")]
        public void BadLinesTest(string pitchValue)
        {
            Assert.Throws<InvalidDataException>(() =>
            {
                Pitch pitch = new Pitch(pitchValue);
            });
        }

        [Test]
        public void BadBorderTest()
        {
            Assert.Throws<InvalidDataException>(() =>
            {
                Pitch pitch = new Pitch(@"
|O    O|2
|      |1
|      |1
|  O   |3
|      |1
| X    |2
+------+
 212203
1x3, 2x2, 3x1

");
            });
        }

        [Test]
        public void OkTest()
        {
            Assert.DoesNotThrow(() =>
            {
                Pitch pitch = new Pitch(@"
  123456
 +------+
1|O    O|2
2|      |1
3|      |1
4|  O   |3
5|      |1
6| X    |2
 +------+
  212203
1x3, 2x2, 3x1
");
            });
        }
    }
}