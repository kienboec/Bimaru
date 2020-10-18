using Bimaru.Logic.Local;

namespace Bimaru.Logic.RemoteObjects
{
    public class RemotePitch : IPitch
    {
        private readonly RemotePitchProvider _provider = null;
        private Pitch _pitch = null;

        public char[] Field => _pitch?.Field;
        public string AdditionalInfo => _pitch?.AdditionalInfo;
        public int[] LineConstraints => _pitch?.LineConstraints;
        public int[] ColumnConstraints => _pitch?.ColumnConstraints;

        public RemotePitch(RemotePitchProvider provider, string rawField)
        {
            this._provider = provider;
            this._pitch = new Pitch(rawField);
        }

        public int Toggle(int x, int y)
        {
            return this.Toggle(_pitch.CalcIndex(x, y));
        }

        public int Toggle(in int index)
        {
            _pitch = _provider.SendToggle(index);
            return index;
        }

        public bool IsSolved()
        {
            return _provider.SendSolvedCheck();
        }

        public override string ToString()
        {
            return _pitch.ToString();
        }
    }
}
