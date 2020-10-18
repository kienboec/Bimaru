namespace Bimaru.Logic.Local
{
    public class RawPitchProvider : IPitchProvider
    {
        public IPitch GetNextPitch()
        {
            return new Pitch(GetNextPitchRaw());
        }

        public void Close()
        {
        }
        /*

3 1
3 1
4 1
4 1
1 2
1 2
6 3
6 3
1 4
1 4
4 4
4 4
6 4
6 4
6 5
6 5
3 6
3 6

         */
        public string GetNextPitchRaw()
        {
            return @"
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
";
        }

        public void Dispose()
        {
            Close();
        }
    }
}
