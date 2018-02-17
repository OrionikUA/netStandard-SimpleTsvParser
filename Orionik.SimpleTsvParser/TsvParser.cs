using System;
using System.IO;
using System.Linq;

namespace Orionik.SimpleTsvParser
{
    public class TsvParser
    {
        private readonly string _delimiter;
        private readonly string _noneIn;
        private readonly string _noneOut;
        private readonly bool _firstIsHeader;

        public TsvParser(string delimiter = "\t", bool firstIsHeader = true, string noneIn = "", string noneOut = "")
        {
            _delimiter = delimiter;
            _noneIn = noneIn;
            _noneOut = noneOut;
            _firstIsHeader = firstIsHeader;
        }

        public TsvList Parse(string[][] textLines)
        {
            var max = 0;
            foreach (var line in textLines)
                if (line.Length > max)
                    max = line.GetLength(0);
            var linesCount = _firstIsHeader ? textLines.GetLength(0) : textLines.GetLength(0) + 1;
            var array = new string[linesCount, max];
            for (var i = 0; i < array.GetLength(1); i++)
                array[0, i] = _firstIsHeader ? GetHeaderText(textLines[0][i], i) : GetHeaderText(null, i);
            for (var i = 0; i < array.GetLength(0) - 1; i++)
            for (var j = 0; j < array.GetLength(1); j++)
                array[i + 1, j] = GetCellText(textLines[_firstIsHeader ? i + 1 : i].ElementAtOrDefault(j));
            return new TsvList(array, _noneOut);
        }

        private string GetHeaderText(string text, int index)
        {
            if (string.IsNullOrEmpty(text) || text.Equals(_noneIn))
            {
                return $"Item{index}";
            }
            return text;
        }

        private string GetCellText(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Equals(_noneIn))
            {
                return _noneOut;
            }
            return text;
        }

        public TsvList Parse(string[] textLines)
        {
            string[][] array = new string[textLines.Length][];
            for (int i = 0; i < textLines.Length; i++)
            {
                array[i] = textLines[i].Split(new []{ _delimiter }, StringSplitOptions.None);
            }
            return Parse(array);
        }

        public TsvList Parse(string text)
        {
            return Parse(text.Split('\n'));
        }

        public TsvList ParseFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            return Parse(lines);
        }
    }
}
