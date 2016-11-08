using System;
using System.IO;
using System.Xml;

namespace Sudoku
{
	/// <summary>
	/// Summary description for SudokuReader.
	/// </summary>
	public class SudokuReader
	{
		#region Global Variables
		static private SudokuReader _reader = new SudokuReader();
		XmlDocument _xDoc  = new XmlDocument();
		#endregion
		#region static public SudokuReader Reader
		static public SudokuReader Reader
		{
			get 
			{
				return _reader;
			}
		}
		#endregion
		#region public SudokuReader()
		public SudokuReader()
		{
		}
		#endregion
		#region public SudokuGrid Read(string filename)
		public SudokuGrid Read(string filename)
		{
			try
			{
				_xDoc.Load(filename);

				SudokuGrid grid = new SudokuGrid(_xDoc);
				return grid;
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("Couldn't Load the Grid in " + filename + "-->" + ex.ToString());
			}
			SudokuGrid Grid = new SudokuGrid();
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++)
					Grid[row, col] = 0;
			return Grid;
		}
		#endregion
		#region public SudokuGrid Restart()
		public SudokuGrid Restart()
		{
			SudokuGrid grid = new SudokuGrid(_xDoc);
			return grid;
		}
		#endregion
		#region public void Save(string filename, SudokuGrid grid, int CM)
		public void Save(string filename, SudokuGrid grid, int CM)
		{  
			_xDoc = new XmlDocument();
			FillDocument(_xDoc, grid, CM);
			SaveToDisk(filename);
		}
		#endregion
		#region public void Save(string filename, int[,] grid, int CM)
		public void Save(string filename, int[,] grid, int CM)
		{  
			_xDoc = new XmlDocument();
			FillDocument(_xDoc, grid, CM);
			SaveToDisk(filename);
		}
		#endregion
		#region public void SaveTemplate(string filename, SudokuGrid grid)
		public void SaveTemplate(string filename, SudokuGrid grid)
		{  
			_xDoc = new XmlDocument();
			FillTemplate(_xDoc, grid);
			SaveToDisk(filename);
		}
		#endregion
		#region public void FillDocument(XmlDocument doc, SudokuGrid grid, int CM)
		public void FillDocument(XmlDocument doc, SudokuGrid grid, int CM)
		{
			XmlNode rows1 = doc.CreateNode(XmlNodeType.Element, "Rows", "");
			XmlNode rows2 = doc.CreateNode(XmlNodeType.Element, "Rows", "");
			XmlNode sudoku = doc.CreateNode(XmlNodeType.Element, "Sudoku", "");
			XmlNode hints = doc.CreateNode(XmlNodeType.Element, "Hints", "");
			XmlNode guesses = doc.CreateNode(XmlNodeType.Element, "Guesses", "");
			doc.AppendChild(sudoku);
			sudoku.AppendChild(hints);
			sudoku.AppendChild(guesses);
			guesses.AppendChild(rows1);
			hints.AppendChild(rows2);

			for (int row = 0; row < 10; row++)
			{
				XmlNode node = doc.CreateNode(XmlNodeType.Element, "Row", "");
				XmlNode node2 = doc.CreateNode(XmlNodeType.Element, "Row", "");
				rows1.AppendChild(node2);
				rows2.AppendChild(node);
				if (row == 9)
				{
					node2.InnerText += CM.ToString();
				}
				else
				{
					for (int col = 0; col < 9; col++)
					{
						if (grid[row, col] == 0)
						{
							node.InnerText += "-,";
							node2.InnerText += "-,";
						}
						else
						{
							if (grid.IsKnownElement(row, col))
							{
								node.InnerText += grid[row, col] + ",";
								node2.InnerText += "-,";
							}
							else
							{
								node.InnerText += "-,";
								node2.InnerText += grid[row, col] + ",";
							}
						}
					}

					if (node.InnerText.Length > 0)
					{
						node.InnerText = node.InnerText.Remove(node.InnerText.Length - 1, 1);
					}

					if (node2.InnerText.Length > 0)
					{
						node2.InnerText = node2.InnerText.Remove(node2.InnerText.Length - 1, 1);
					}
				}
			}
		}
		#endregion
		#region public void FillDocument(XmlDocument doc, int[,] grid, int CM)
		public void FillDocument(XmlDocument doc, int[,] grid, int CM)
		{
			XmlNode rows1 = doc.CreateNode(XmlNodeType.Element, "Rows", "");
			XmlNode rows2 = doc.CreateNode(XmlNodeType.Element, "Rows", "");
			XmlNode sudoku = doc.CreateNode(XmlNodeType.Element, "Sudoku", "");
			XmlNode hints = doc.CreateNode(XmlNodeType.Element, "Hints", "");
			XmlNode guesses = doc.CreateNode(XmlNodeType.Element, "Guesses", "");
			doc.AppendChild(sudoku);
			sudoku.AppendChild(hints);
			sudoku.AppendChild(guesses);
			guesses.AppendChild(rows1);
			hints.AppendChild(rows2);


			for (int row = 0; row < 10; row++)
			{
				XmlNode node = doc.CreateNode(XmlNodeType.Element,"Row", "");
				XmlNode node2 = doc.CreateNode(XmlNodeType.Element,"Row", "");
				rows1.AppendChild(node2);
				rows2.AppendChild(node);
				if (row == 9)
				{
					node2.InnerText += CM.ToString();
				}
				else
				{
					for (int col = 0; col<9; col++)
					{
						if (grid[row, col] == 0)
						{
							node.InnerText += "-" + ",";
							node2.InnerText += "-" + ",";
						}
						else
						{
							node.InnerText += grid[row, col] + ",";
							node2.InnerText += "-" + ",";
						}
					}

					if (node.InnerText.Length > 0)
					{
						node.InnerText = node.InnerText.Remove(node.InnerText.Length -1 , 1);
					}

					if (node2.InnerText.Length > 0)
					{
						node2.InnerText = node2.InnerText.Remove(node2.InnerText.Length -1 , 1);
					}
				}
			}
		}
		#endregion
		#region public void FillTemplate(XmlDocument doc, SudokuGrid grid)
		public void FillTemplate(XmlDocument doc, SudokuGrid grid)
		{
			XmlNode sudoku = doc.CreateNode(XmlNodeType.Element, "Sudoku", "");
			XmlNode hints = doc.CreateNode(XmlNodeType.Element, "Hints", "");
			XmlNode rows = doc.CreateNode(XmlNodeType.Element, "Rows", "");
			doc.AppendChild(sudoku);
			sudoku.AppendChild(hints);
			hints.AppendChild(rows);

			for (int row = 0; row < 9; row++)
			{
				XmlNode node = doc.CreateNode(XmlNodeType.Element, "Row", "");

				rows.AppendChild(node);

				for (int col = 0; col < 9; col++)
				{
					if (grid[row, col] == 0)
					{
						node.InnerText += "-,";
					}
					else
					{
						node.InnerText += grid[row, col] + ",";
					}
				}

				if (node.InnerText.Length > 0)
				{
					node.InnerText = node.InnerText.Remove(node.InnerText.Length -1 , 1);
				}
			}
		}
		#endregion
		#region private void SaveToDisk(string filename)
		private void SaveToDisk(string filename)
		{
			XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.Default);
			xmlWriter.IndentChar = '\t';
			xmlWriter.Indentation = 1;
			xmlWriter.Formatting = Formatting.Indented;

			_xDoc.Save(xmlWriter); 

			xmlWriter.Close();
		}
		#endregion
	}
}
