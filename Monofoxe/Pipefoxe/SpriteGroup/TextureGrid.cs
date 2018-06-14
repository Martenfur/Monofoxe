using System.Collections.Generic;

namespace Pipefoxe.SpriteGroup
{
	public class TextureGrid
	{
		private List<int> _cellsH;
		private List<int> _cellsV;

		public int Width {get => _cellsH.Count - 1;}
		public int Height {get => _cellsV.Count - 1;}

		private List<List<bool>> _cells;

		public TextureGrid(int size)
		{
			// Creating one big cell with given size and value of False.
			_cellsH = new List<int>(new int[]{0, size});
			_cellsV = new List<int>(new int[]{0, size});

			_cells = new List<List<bool>>();
			_cells.Add(new List<bool>());
			_cells[0].Add(false);
		}
		
		public bool this[int y, int x]
		{
			get => _cells[y][x];
			set => _cells[y][x] = value;
		}
		
		public int GetCellX(int index) =>
			_cellsH[index];

		public int GetCellY(int index) =>
			_cellsV[index];
		
		public int GetCellW(int index) =>
			_cellsH[index + 1] - _cellsH[index];

		public int GetCellH(int index) =>
			_cellsV[index + 1] - _cellsV[index];
		

		public void SplitH(int newColumn)
		{
			/* 
			 * 0 1 6     0 1 4 6
			 * |o|o| ==> |o|n|o|
			 * |o|o| ==> |o|n|o|
			 */

			int index = 0;
			for(var i = 1; i < _cellsH.Count; i += 1)
			{
				if (newColumn == _cellsH[i]) // If there is already such row, no need to add it second time.
				{
					return;
				}
				
				if (newColumn < _cellsH[i])
				{
					_cellsH.Insert(i, newColumn);
					index = i;
					break;
				}
			}

			foreach(List<bool> row in _cells)
			{
				row.Insert(index - 1, row[index - 1]);
			}

		}

		public void SplitV(int newColumn)
		{
			/* 
			 * 0 |o|o| ==> 0 |o|o|
			 * 6 |o|o| ==> 4 |n|n|
			 *             6 |o|o|
			 */

			int index = 0;
			for(var i = 1; i < _cellsV.Count; i += 1)
			{
				if (newColumn == _cellsV[i]) // If there is already such row, no need to add it second time.
				{
					return;
				}
				
				if (newColumn < _cellsV[i])
				{
					_cellsV.Insert(i, newColumn);
					index = i;
					break;
				}
			}

			var buffer = new List<bool>(_cells[index - 1]);
			_cells.Insert(index - 1, buffer);
		}

	}
}
