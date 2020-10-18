using System;
using System.IO;
using System.Text;

namespace Bimaru.Logic.Local
{
    public class Pitch : IPitch
    {
        public const int XDimension = 6;
        public const int YDimension = 6;
        public char[] Field { get; } = new char[YDimension * XDimension];
        public string AdditionalInfo { get; }

        public int[] LineConstraints { get; } = new int[6];
        public int[] ColumnConstraints { get; } = new int[6];

        public Pitch(string rawPitch)
        {
            try
            {
                StringReader reader = new StringReader(rawPitch);
                int mode = 0; // state machine
                int y = 0;
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    if ((mode == 0) && line.Trim() == "123456")
                    {
                        // optional column-description
                        continue;
                    }

                    if ((mode == 0 || mode == 2) && line.Trim() == "+------+")
                    {
                        mode++;
                        continue;
                    }

                    if (mode == 1 &&
                        char.IsDigit(line[0]) && 
                        line[1] == '|' &&
                        line[8] == '|' &&
                        char.IsDigit(line[9]))
                    {
                        for (int i = 0; i < XDimension; i++)
                        {
                            switch (line[2 + i])
                            {
                                case ' ':
                                case 'O':
                                case 'X':
                                    this.Field[y * XDimension + i] = line[2 + i];
                                    break;
                                default:
                                    throw new InvalidDataException(
                                        "invalid sign in line '" + line + "' at position " + i.ToString());
                            }
                        }

                        LineConstraints[y] = int.Parse(line[9].ToString());

                        y++;
                        if (y == 6)
                        {
                            mode++;
                        }

                        continue;
                    }

                    if (mode == 3 && line.StartsWith("  "))
                    {
                        for (int i = 0; i < XDimension; i++)
                        {
                            if (!char.IsDigit(line[2 + i]))
                            {
                                throw new InvalidDataException("invalid column constraints");
                            }

                            this.ColumnConstraints[i] = int.Parse(line.Substring(2 + i, 1));
                        }

                        mode++;
                        continue;
                    }

                    if (mode == 4)
                    {
                        AdditionalInfo = AdditionalInfo + line + Environment.NewLine;
                        continue;
                    }

                    throw new InvalidDataException("invalid line start in line '" + line + "'");
                }

                AdditionalInfo = AdditionalInfo?.Trim();
            }
            catch (Exception exc)
            {
                throw new InvalidDataException("unexpected data processed", exc);
            }
        }

        public int CalcIndex(int x, int y)
        {
            return (x - 1) + (y - 1) * Pitch.XDimension;
        }

        public int Toggle(int x, int y)
        {
            return Toggle(CalcIndex(x,y));
        }

        public int Toggle(in int index)
        {
            switch (this.Field[index])
            {
                case ' ':
                    this.Field[index] = 'O';
                    break;
                case 'O':
                    this.Field[index] = 'X';
                    break;
                case 'X':
                    this.Field[index] = ' ';
                    break;
            }

            return index;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(100);
            builder.AppendLine("  123456 ");
            builder.AppendLine(" +------+");
            for (int i = 0; i < this.Field.Length; i++)
            {
                if (i % XDimension == 0)
                {
                    builder.Append((i / XDimension) + 1);
                    builder.Append('|');
                }

                builder.Append(this.Field[i]);

                if ((i % XDimension) == XDimension - 1)
                {
                    builder.AppendLine("|" + LineConstraints[i / XDimension].ToString());
                }
            }
            builder.AppendLine(" +------+");
            builder.Append("  ");
            foreach (var constraint in this.ColumnConstraints)
            {
                builder.Append(constraint);
            }

            builder.AppendLine();
            builder.AppendLine(this.AdditionalInfo);

            var returnValue = builder.ToString();
            return returnValue;
        }
        public bool IsSolved()
        {
            for (int i = 0; i < Pitch.YDimension; i++)
            {
                int lineCount = 0;
                for (int j = 0; j < Pitch.XDimension; j++)
                {
                    if (this.Field[i * Pitch.XDimension + j] == 'X')
                    {
                        lineCount++;
                    }
                }

                if (lineCount != this.LineConstraints[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < Pitch.XDimension; i++)
            {
                int rowCount = 0;
                for (int j = 0; j < Pitch.YDimension; j++)
                {
                    if (this.Field[j * Pitch.XDimension + i] == 'X')
                    {
                        rowCount++;
                    }
                }

                if (rowCount != this.ColumnConstraints[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
