using System.IO;
using NUnit.Framework;

namespace Bimaru.Logic.Test
{
    public class PitchTests
    {
        [Test]
        [TestCase(@"
+------+
|O  O|2
|      |1
|      |1
|  O   |3
|      |1
| X    |2
+------+
 212203
1x3, 2x2, 3x1

", TestName = "short line")]
        [TestCase(@"
+------+
|O    O|2
|      |1
|      |1
|  O   |3
|      |1
+------+
 212203
1x3, 2x2, 3x1

", TestName = "5 lines only")]
        [TestCase(@"
+------+
|O    O|
|      |1
|      |1
|  O   |3
|      |1
| X    |2
+------+
 212203
1x3, 2x2, 3x1

", TestName = "no num at the end")]
        [TestCase(@"
+------+
|O    O|
|      |1
|      |1
|  O   |3
|      |1
| X    |2
 212203
1x3, 2x2, 3x1

", TestName = "invalid border at the end")]
        [TestCase(@"
+------+
|O    O|
|      |1
|      |1
|  O   |3
|      |1
| X    |2
+------+
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
+------+
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
    }
}