using System;
using System.Xml;
using System.Xml.XPath;

namespace Sudoku {
	/// <summary>
	/// Summary description for SudokuGrid.
	/// </summary>
	public class SudokuGrid {
		#region Global Variables
		bool _ACH = false;
		bool _EH = false;
		bool _TM = false;
		bool _T = true;
		bool _E = false;
		bool _M = true;
		bool _H = false;
		int[,,] _gridHints = new int[9, 9, 9];  // initialize array
		int[,,] _solvedHints = new int[9, 9, 9];
		int[,] _grid = new int[9, 9];  // initialize array
		int[,] _knownElements = new int[9, 9];  // initialize array
		int[,] _solvedGrid = new int[9, 9]; // initialize array
		int _Level = 0;
		bool _2Slns = false;
		bool _2SlnsShow = false;
		bool _hard = false;
		bool _easy = false;
		XmlNodeList _nodes = null;
		#endregion
		#region public void twoSlns(bool twoSlns)
		public void twoSlns(bool twoSlns) {
			_2Slns = twoSlns;
		}
		#endregion
		#region public void twoSlnsShow(bool twoSlnsShow)
		public void twoSlnsShow(bool twoSlnsShow) {
			_2SlnsShow = twoSlnsShow;
		}
		#endregion
		#region public SudokuGrid(XmlDocument xdoc)
		public SudokuGrid(XmlDocument xdoc) {
			_nodes = xdoc.SelectNodes("//Hints/Rows/*");
			ParseHintNodes();

			_nodes = xdoc.SelectNodes("//Guesses/Rows/*");
			ParseGuessNodes();
		}
		#endregion
		#region public SudokuGrid()
		public SudokuGrid() {
		}
		#endregion
		#region public void ParseHintNodes()
		public void ParseHintNodes() {
			int row = 0;

			if (_nodes == null)
				return;

			foreach (XmlNode n in _nodes) {
				if (row == 9) {
					if (n.InnerText != "") {
						CheckBoxes(Convert.ToInt32(n.InnerText));
					}
				} else {
					string[] rowValues = n.InnerText.Split(new char[] { ',' });
					int col = 0;
					foreach (string num in rowValues) {
						if (num == "-") {
							_grid[row, col] = 0;
						} else {
							_grid[row, col] = Convert.ToInt32(num);
							_knownElements[row, col] = Convert.ToInt32(num);
						}

						col++;
					}
				}
				row++;
			}
		}
		#endregion
		#region public void ParseGuessNodes()
		public void ParseGuessNodes() {
			if (_nodes == null)
				return;

			int row = 0;
			foreach (XmlNode n in _nodes) {
				if (row == 9) {
					if (n.InnerText != "") {
						CheckBoxes(Convert.ToInt32(n.InnerText));//////////////////
					}
				} else {
					string[] rowValues = n.InnerText.Split(new char[] { ',' });
					int col = 0;
					foreach (string num in rowValues) {
						if (num == "-") {
							//_grid[row, col] = 0;
						} else {
							_grid[row, col] = Convert.ToInt32(num);
						}

						col++;
					}
				}
				row++;
			}
		}
		#endregion
		#region public void hard(bool tf)
		public void hard(bool tf) {
			_hard = tf;
		}
		#endregion
		#region public void easy(bool tf)
		public void easy(bool tf) {
			_easy = tf;
		}
		#endregion
		#region public void ClearHints()
		public void ClearHints() {
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					ClearRCHints(i, j);
		}
		#endregion
		#region public void ClearRCHints()
		public void ClearRCHints(int row, int col) {
			for (int i = 0; i < 9; i++)
				_gridHints[row, col, i] = 0;
		}
		#endregion
		#region public int[,,] GridHints
		public int[,,] GridHints {
			get {
				return _gridHints;
			}
		}
		#endregion
		#region public bool GridCompleted()
		/// <summary>
		/// Check to make sure all of the grid is finished
		/// </summary>
		/// <returns></returns>
		public bool GridCompleted() {
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++)
					if (this[row, col] == 0) return false;
			return true;
		}
		#endregion
		#region public bool CheckAll()
		/// <summary>
		/// Check to make sure all of the grid is finished
		/// </summary>
		/// <returns></returns>
		public bool CheckAll() {
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++) {
					if (CheckCell(row, col) == false) {
						return false;
					}
				}
			return true;
		}
		#endregion
		#region public bool CheckCell(int currentrow, int currentcol)
		public bool CheckCell(int currentrow, int currentcol) {
			int val = this[currentrow, currentcol]; // checks current cell

			// precondition, cells of 0 are okay
			if (val == 0) {
				return true;
			}

			for (int row = 0; row < 9; row++) {
				if (row != currentrow) {
					if (this[row, currentcol] == val) {
						return false;
					}
				}
			}

			for (int col = 0; col < 9; col++) {
				if (col != currentcol) {
					if (this[currentrow, col] == val) {
						return false;
					}
				}
			}

			int cornerrow = currentrow / 3 * 3;
			int cornercol = currentcol / 3 * 3;
			for (int row = cornerrow; row < cornerrow + 3; row++) {
				for (int col = cornercol; col < cornercol + 3; col++) {
					if (col != currentcol || row != currentrow) {
						if (this[row, col] == val) {
							return false;
						}
					}
				}
			}
			return true;
		}
		#endregion
		#region public void Generate()
		public void Generate() {
			int[,] S = new int[9, 9];
			S = GenerateGrid();
			int r = new Random((int)DateTime.Now.Ticks).Next();
		zero:
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++)
					_knownElements[row, col] = S[row, col];
			for (int row = 0; row < 5; row += 2) {
				for (int col = 0; col < 4; col += 2) {
					if (r % 2 == 1)
						_knownElements[row, col] = 0;
					r /= 2;
					if (r == 0)
						r = new Random((int)DateTime.Now.Ticks).Next();
				}
			}
			for (int row = 1; row < 5; row += 2) {
				for (int col = 1; col < 4; col += 2) {
					if (r % 2 == 1)
						_knownElements[row, col] = 0;
					r /= 2;
					if (r == 0)
						r = new Random((int)DateTime.Now.Ticks).Next();
				}
			}
			for (int row = 0; row < 5; row += 2) {
				for (int col = 1; col < 4; col += 2) {
					if (r % 2 == 1)
						_knownElements[row, col] = 0;
					r /= 2;
					if (r == 0)
						r = new Random((int)DateTime.Now.Ticks).Next();
				}
			}
			for (int row = 1; row < 5; row += 2) {
				for (int col = 0; col < 4; col += 2) {
					if (r % 2 == 1)
						_knownElements[row, col] = 0;
					r /= 2;
					if (r == 0)
						r = new Random((int)DateTime.Now.Ticks).Next(); ;
				}
			}
			if (r % 2 == 0)
				_knownElements[4, 4] = 0;
			int solved = 0;
			int count = 0;
			do {
				count++;
				for (int row = 0; row < 5; row++) {
					for (int col = 0; col < 5; col++) {
						if (_knownElements[row, col] == 0 && solved == 0) {
							if (count > 1) {
								if (_solvedGrid[row, col] == 0) {
									_knownElements[row, col] = S[row, col];
									_knownElements[8 - row, 8 - col] = S[8 - row, 8 - col];
									_knownElements[8 - col, row] = S[8 - col, row];
									_knownElements[col, 8 - row] = S[col, 8 - row];
									solved = 1;
								}
							} else {
								_knownElements[8 - row, 8 - col] = 0;
								_knownElements[8 - col, row] = 0;
								_knownElements[col, 8 - row] = 0;
							}
						}
					}
				}
				if (count > 1 && solved == 0) {
					Generate();
					return;
				}
				solved = 0;
			} while ((_hard ? false : !Solve(true, true, true)));
			int zeros = 0;
			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					_grid[row, col] = _knownElements[row, col];
					if (_grid[row, col] == 0)
						zeros++;
				}
			}
			if (_easy) {
				if (zeros > 30)
					goto zero;
			} else if (zeros < 30)
				goto zero;
		}
		#endregion
		#region public int[,] GenerateGrid()
		public int[,] GenerateGrid() {
			int[,] S = new int[9, 9];
			int a, b, c = 0, d = 0, n = 0, f = 0, g, h, i = 0, j, k, l, p = 0;
			int[] a9 = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			Random r;
			a = 0;
		AA:
			b = 0;
		BB:
			if (a % 3 == 0) {
				c = 0;
				d = 2;
			} else if (a % 3 == 1) {
				c = -1;
				d = 1;
			} else if (a % 3 == 2) {
				c = -2;
				d = 0;
			}
			if (b % 3 == 0) {
				n = 0;
				f = 2;
			} else if (b % 3 == 1) {
				n = -1;
				f = 1;
			} else if (b % 3 == 2) {
				n = -2;
				f = 0;
			}
			r = new Random((int)DateTime.Now.Ticks);
			for (i = 8; i >= 0; i--) {
				h = r.Next(i);
				int temp = a9[i];
				a9[i] = a9[h];
				a9[h] = temp;
			}
			g = 0;
			i = 0;
		RND:
			i++;
			g += 1;
			if (g == 10)
				g = 1;
			h = a9[g-1];
			if (i == 10) {
				p++;
				if (a > 6 || p == 5000) {
					a = 6;
					if (p == 5000) {
						a = 0;
						p = 0;
					}
					for (int temp = a; temp < 9; temp++) {
						for (int temp1 = 0; temp1 < 9; temp1++) {
							S[temp, temp1] = 0;
						}
					}
					goto AA;
				}
				for (int temp = 0; temp < 9; temp++)
					S[a, temp] = 0;
				goto AA;
			}
			j = c;
		JJ:
			k = n;
		KK:
			if (j != 0 || k != 0) {
				if (h == S[a + j, b + k])
					goto RND;
			}
			k++;
			if (k < f + 1)
				goto KK;
			j++;
			if (j < d + 1)
				goto JJ;
			l = 0;
		LL:
			if ((h == S[a, l] && b != l) || (h == S[l, b] && a != l))
				goto RND;
			l++;
			if (l < 9)
				goto LL;
			S[a, b] = h;
			b++;
			if (b < 9)
				goto BB;
			a++;
			if (a < 9)
				goto AA;
			return S;
		}
		#endregion
		#region public bool CheckEmpty()
		public bool CheckEmpty() {
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++)
					if (this[row, col] != 0)
						return false;
			return true;
		}
		#endregion
		#region public bool CheckKnown()
		public bool CheckKnown() {
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++)
					if (_knownElements[row, col] != 0)
						return true;
			return false;
		}
		#endregion
		#region public void ChangeHints(int row, int col, int num)
		public void ChangeHints(int row, int col, int num) {
			if (_gridHints[row, col, num - 1] != 0)
				_gridHints[row, col, num - 1] = 0;
			else
				_gridHints[row, col, num - 1] = num;
		}
		#endregion
		#region public int[,,] CalculateHints(int[,] grid)
		public int[,,] CalculateHints(int[,] grid) {
			int[,,] gridHints = new int[9, 9, 9];
			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					if (grid[row, col] == 0) {
						// check row column square

						// check row
						int[] possibleValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
						for (int i = 0; i < 9; i++) {
							if (grid[row, i] != 0) {
								possibleValues[grid[row, i] - 1] = 0;
							}

							if (grid[i, col] != 0) {
								possibleValues[grid[i, col] - 1] = 0;
							}

							int[] quad = new int[] { row / 3 * 3, col / 3 * 3 };
							for (int j = quad[0]; j < quad[0] + 3; j++) {
								for (int k = quad[1]; k < quad[1] + 3; k++) {
									if (grid[j, k] != 0) {
										possibleValues[grid[j, k] - 1] = 0;
									}
								}
							}
						}

						// now place in array
						//int count = 0;
						for (int vals = 0; vals < 9; vals++) {
							//if (possibleValues[vals] != 0)
							//{
							gridHints[row, col, vals/*count*/] = possibleValues[vals];
							//count++;
							//}
						}
					}
				}
			}
			return gridHints;
		}
		#endregion
		#region public void CalculateHints()
		public void CalculateHints() {
			ClearHints();

			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					if (_grid[row, col] == 0) {
						// check row column square

						// check row
						int[] possibleValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
						for (int i = 0; i < 9; i++) {
							if (_grid[row, i] != 0) {
								possibleValues[_grid[row, i] - 1] = 0;
							}

							if (_grid[i, col] != 0) {
								possibleValues[_grid[i, col] - 1] = 0;
							}

							int[] quad = FindQuadStart(row, col);
							for (int j = quad[0]; j < quad[0] + 3; j++) {
								for (int k = quad[1]; k < quad[1] + 3; k++) {
									if (_grid[j, k] != 0) {
										possibleValues[_grid[j, k] - 1] = 0;
									}
								}
							}
						}

						// now place in array
						//int count = 0;
						for (int vals = 0; vals < 9; vals++) {
							//if (possibleValues[vals] != 0)
							//{
							_gridHints[row, col, vals/*count*/] = possibleValues[vals];
							//count++;
							//}
						}
					}
				}
			}
		}
		#endregion
		#region public void Clear()
		public void Clear() {
			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					this[row, col] = 0;
				}
			}

			// Also clear known elements
			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					_knownElements[row, col] = 0;
				}
			}
		}
		#endregion
		#region public int[] FindQuadStart(int row, int col)
		public int[] FindQuadStart(int row, int col) {
			int val1 = row / 3 * 3;
			int val2 = col / 3 * 3;

			return new int[] { val1, val2 };
		}
		#endregion
		#region public void SetSolvedGrid(int row, int col, int val)
		public void SetSolvedGrid(int row, int col, int val) {
			_solvedGrid[row, col] = val;
		}
		#endregion
		#region public void SetKnownElement(int row, int col, int val)
		public void SetKnownElement(int row, int col, int val) {
			_knownElements[row, col] = val;
		}
		#endregion
		#region public int GetSolvedGrid(int row, int column)
		public int GetSolvedGrid(int row, int column) {
			return _solvedGrid[row, column];
		}
		#endregion
		#region public int GetKnownElement(int row, int column)
		public int GetKnownElement(int row, int column) {
			return _knownElements[row, column];
		}
		#endregion
		#region public bool IsKnownElement(int row, int col)
		public bool IsKnownElement(int row, int col) {
			if (_knownElements[row, col] != 0)
				return true;

			return false;
		}
		#endregion
		#region public void CalcSolvedHints()
		public void CalcSolvedHints() {
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++)
					for (int num = 0; num < 9; num++)
						_solvedHints[row, col, num] = 0;

			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					if (_solvedGrid[row, col] == 0) {
						int[] possibleValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
						for (int i = 0; i < 9; i++) {
							if (_solvedGrid[row, i] != 0) {
								possibleValues[_solvedGrid[row, i] - 1] = 0;
							}

							if (_solvedGrid[i, col] != 0) {
								possibleValues[_solvedGrid[i, col] - 1] = 0;
							}

							int[] quad = FindQuadStart(row, col);
							for (int j = quad[0]; j < quad[0] + 3; j++) {
								for (int k = quad[1]; k < quad[1] + 3; k++) {
									if (_solvedGrid[j, k] != 0) {
										possibleValues[_solvedGrid[j, k] - 1] = 0;
									}
								}
							}
						}

						for (int vals = 0; vals < 9; vals++) {
							_solvedHints[row, col, vals] = possibleValues[vals];
						}
					}
				}
			}
		}
		#endregion
		#region public bool Solve(bool solve, bool known, bool gen)
		public bool Solve(bool solve, bool known, bool gen) {
			_Level++;
			bool gridChange;
			if (known) {
				if (!CheckKnown()) {
					for (int row = 0; row < 9; row++)
						for (int col = 0; col < 9; col++)
							_knownElements[row, col] = this[row, col];
				}
				for (int row = 0; row < 9; row++)
					for (int col = 0; col < 9; col++)
						_solvedGrid[row, col] = _knownElements[row, col];
			}
			CalcSolvedHints();
			do {
				gridChange = false;
				for (int row = 0; row < 9; row++) {
					for (int col = 0; col < 9; col++) {
						int temp = 0;
						int tempcol;
						int temprow;
						int tempcell;
						int tempone;
						int crow = row / 3 * 3;
						int ccol = col / 3 * 3;
						int tempi = 0;
						if (_solvedGrid[row, col] == 0) {
							for (int i = 0; i < 9; i++) {
								if (_solvedHints[row, col, i] == 0) {
									tempi++;
								} else {
									temprow = 0;
									tempcol = 0;
									tempcell = 0;
									tempone = 0;
									for (int j = 0; j < 9; j++) {
										if (_solvedHints[row, j, i] != 0)
											tempcol++;
										if (_solvedHints[j, col, i] != 0)
											temprow++;
										if (_solvedHints[row, col, j] != 0) {
											tempone++;
											temp = j + 1;
										}
									}
									for (int k = 0; k < 3; k++) {
										for (int l = 0; l < 3; l++) {
											if (_solvedHints[crow + k, ccol + l, i] != 0)
												tempcell++;
										}
									}
									if (tempcol == 1) {
										_solvedGrid[row, col] = i + 1;
										gridChange = true;
										CalcSolvedHints();
									} else if (temprow == 1) {
										_solvedGrid[row, col] = i + 1;
										gridChange = true;
										CalcSolvedHints();
									} else if (tempcell == 1) {
										_solvedGrid[row, col] = i + 1;
										gridChange = true;
										CalcSolvedHints();
									} else if (tempone == 1) {
										_solvedGrid[row, col] = temp;
										gridChange = true;
										CalcSolvedHints();
									}
								}
							}
							if (tempi == 9) {
								if (!solve) {
									System.Windows.Forms.MessageBox.Show("There is no solution\nto this puzzle.", "No Solution", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
								}
								_Level--;
								return false;

							}
						}
					}
				}
				if (!gridChange) {
					for (int row = 0; row < 9; row++) {
						for (int col = 0; col < 9; col++) {
							if (_solvedGrid[row, col] == 0) {
								if (!gen) {
									int hints = 0;
									int num1 = 0;
									int num2 = 0;
									for (int i = 0; i < 9; i++) {
										if (_solvedHints[row, col, i] != 0) {
											hints++;
											if (hints == 1) {
												num1 = i + 1;
											} else if (hints == 2) {
												num2 = i + 1;
											}
										}
									}
									if (hints == 2) {
										int[,] tempg = new int[9, 9];
										for (int tempr = 0; tempr < 9; tempr++) {
											for (int tempc = 0; tempc < 9; tempc++) {
												tempg[tempr, tempc] = _solvedGrid[tempr, tempc];
											}
										}
										_solvedGrid[row, col] = num1;
										if (Solve(true, false, false)) {
											for (int tempr = 0; tempr < 9; tempr++) {
												for (int tempc = 0; tempc < 9; tempc++) {
													_solvedGrid[tempr, tempc] = tempg[tempr, tempc];
												}
											}
											_solvedGrid[row, col] = num2;
											if (Solve(true, false, false)) {
												if (!_2Slns) {
													if (System.Windows.Forms.MessageBox.Show("There is more than one solution to this puzzle.\n Do you want to see one solution?", "Two Solutions", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes) {
														_2SlnsShow = true;
													}
													_2Slns = true;
												}
												if (!_2SlnsShow) {
													for (int tempr = 0; tempr < 9; tempr++) {
														for (int tempc = 0; tempc < 9; tempc++) {
															_solvedGrid[tempr, tempc] = tempg[tempr, tempc];
														}
													}
												}
											} else {
												for (int tempr = 0; tempr < 9; tempr++) {
													for (int tempc = 0; tempc < 9; tempc++) {
														_solvedGrid[tempr, tempc] = tempg[tempr, tempc];
													}
												}
												_solvedGrid[row, col] = num1;
												Solve(true, false, false);
											}
											_Level--;
											return true;
										} else {
											for (int tempr = 0; tempr < 9; tempr++) {
												for (int tempc = 0; tempc < 9; tempc++) {
													_solvedGrid[tempr, tempc] = tempg[tempr, tempc];
												}
											}
											_solvedGrid[row, col] = num2;
											if (Solve(true, false, false)) {
												_Level--;
												return true;
											} else {
												for (int tempr = 0; tempr < 9; tempr++) {
													for (int tempc = 0; tempc < 9; tempc++) {
														_solvedGrid[tempr, tempc] = tempg[tempr, tempc];
													}
												}
												if (_Level == 1) {
													System.Windows.Forms.MessageBox.Show("There is no solution\nto this puzzle.", "No Solution", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
												}
												_Level--;
												return false;
											}
										}
									}
								} else {
									_Level--;
									return false;
								}
							}
						}
					}
				}
			} while (gridChange);
			_Level--;
			return true;
		}
		#endregion
		#region public void ClearWrong()
		public void ClearWrong() {
			for (int currentrow = 0; currentrow < 9; currentrow++) {
				for (int currentcol = 0; currentcol < 9; currentcol++) {
					int val = this[currentrow, currentcol]; // checks current cell
					bool legal = true;
					// precondition, cells of 0 are okay
					if (val != 0 && !IsKnownElement(currentrow, currentcol)) {

						// check column first
						for (int row = 0; row < 9; row++) {
							if (row != currentrow) // skip current row
							{
								if (this[row, currentcol] == val) {
									if (!IsKnownElement(row, currentcol))
										this[row, currentcol] = 0;
									legal = false;
								}
							}
						}

						// check row next
						for (int col = 0; col < 9; col++) {
							if (col != currentcol) // skip current column
							{
								if (this[currentrow, col] == val) {
									if (!IsKnownElement(currentrow, col))
										this[currentrow, col] = 0;
									legal = false;
								}
							}
						}

						int cornerrow = currentrow / 3 * 3;
						int cornercol = currentcol / 3 * 3;
						for (int row = cornerrow; row < cornerrow + 3; row++) {
							for (int col = cornercol; col < cornercol + 3; col++) {
								if (col != currentcol || row != currentrow) // skip current column, row
								{
									if (this[row, col] == val) {
										if (!IsKnownElement(row, col))
											this[row, col] = 0;
										legal = false;
									}
								}
							}
						}

						if (!legal)
							this[currentrow, currentcol] = 0;
					}
				}
			}
		}
		#endregion
		#region public int this[int row, int column]
		public int this[int row, int column] {
			get {
				return _grid[row, column];
			}

			set {
				_grid[row, column] = value;
			}
		}
		#endregion
		#region public void CheckBoxes(int CM)
		public void CheckBoxes(int CM) {
			if (CM % 2 == 1)
				_ACH = true;
			else
				_ACH = false;
			CM /= 2;
			if (CM % 2 == 1)
				_EH = true;
			else
				_EH = false;
			CM /= 2;
			if (CM % 2 == 1)
				_T = true;
			else
				_T = false;
			CM /= 2;
			if (CM % 2 == 1)
				_TM = true;
			else
				_TM = false;
			CM /= 2;
			if (CM % 2 == 1)
				_E = true;
			else
				_E = false;
			CM /= 2;
			if (CM % 2 == 1)
				_M = true;
			else
				_M = false;
			CM /= 2;
			if (CM % 2 == 1)
				_H = true;
			else
				_H = false;
		}
		#endregion
		#region public bool ACH()
		public bool ACH() {
			return _ACH;
		}
		#endregion
		#region public bool EH()
		public bool EH() {
			return _EH;
		}
		#endregion
		#region public bool TM()
		public bool TM() {
			return _TM;
		}
		#endregion
		#region public bool T()
		public bool T() {
			return _T;
		}
		#endregion
		#region public bool E()
		public bool E() {
			return _E;
		}
		#endregion
		#region public bool M()
		public bool M() {
			return _M;
		}
		#endregion
		#region public bool H()
		public bool H() {
			return _H;
		}
		#endregion
	}
}

