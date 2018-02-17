using System;
using System.Collections.Generic;
using System.Linq;

namespace Orionik.SimpleTsvParser
{
    public class TsvList
    {
        private readonly HashSet<string> _header;
        private string[,] _rows;

        public string None { get; }
        public int ColumnCount => _header.Count;
        public int RowCount => _rows.GetLength(0);
        public string[] Header => _header.ToArray();
        public string[,] Rows => _rows;

        public TsvList(string[,] lines, string none)
        {
            _header = new HashSet<string>();
            foreach (var str in lines.GetRow(0))
            {
                if (!_header.Add(str))
                    throw new TsvException($"Header {str} already exists");
            }

            _rows = new string[lines.GetLength(0) - 1, _header.Count];
            for (int i = 0; i < _rows.GetLength(0); i++)
            {
                for (int j = 0; j < _header.Count; j++)
                {
                    _rows[i, j] = lines[i+1, j];
                }
            }
            
            None = none;
        }

        public string[] GetRow(int rowIndex)
        {
            return rowIndex >= 0 && rowIndex < _rows.GetLength(0) ? _rows.GetRow(rowIndex) : null;
        }

        public string[] GetColumn(int columnIndex)
        {
            return columnIndex >= 0 && columnIndex < _header.Count ? _rows.GetCol(columnIndex) : null;
        }

        public string[] GetColumn(string key)
        {
            return GetColumn(GetColumnIndex(key));
        }

        public int GetColumnIndex(string key)
        {
            for (int i = 0; i < _header.Count; i++)
            {
                var str = _header.ElementAt(i);
                if (str == key)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool AddHeader(string header, string[] column = null)
        {
            if (!_header.Add(header))
                return false;

            if (column == null)
            {
                column = new string[0];
            }
            var rows = new string[_rows.GetLength(0), _rows.GetLength(1)+1];
            for (int i = 0; i < _rows.GetLength(0); i++)
            {
                for (int j = 0; j < _rows.GetLength(1); j++)
                {
                    rows[i, j] = _rows[i, j];
                }
            }
            for (int i = 0; i < rows.GetLength(0); i++)
            {
                rows[i, _rows.GetLength(1)] = column.ElementAtOrDefault(i) ?? None;
            }
            _rows = rows;
            return true;
        }

        public void AddRow(string[] row = null)
        {
            if (row == null)
            {
                row = new string[0];
            }
            var rows = new string[_rows.GetLength(0) + 1, _rows.GetLength(1)];
            for (int i = 0; i < _rows.GetLength(0); i++)
            {
                for (int j = 0; j < _rows.GetLength(1); j++)
                {
                    rows[i, j] = _rows[i, j];
                }
            }
            for (int i = 0; i < rows.GetLength(1); i++)
            {
                rows[_rows.GetLength(0), i] = row.ElementAtOrDefault(i) ?? None;
            }
            _rows = rows;
        }

        public string this[int i, int j]
        {
            get => _rows[i, j];
            set => _rows[i, j] = value;
        }
    }
}