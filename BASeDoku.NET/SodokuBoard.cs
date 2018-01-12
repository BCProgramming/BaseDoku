using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using BASeCamp.Elementizer;

namespace BASeDoku
{
    public class SodokuBoard : ISodokuBoardHandler
    {
        //This is for "Normal" Soduku.
        //The numbers 1 through 9 can only appear once in a row, column, or minigrid.


        //We might support variants later. They might be able to re-use some aspects of this one.

        //Sum Soduku has similar rules to Normal Sudoku, however it has an added quirk; in addition to those rules,
        //there are also designated "areas" or 2 to 5 connected cells. The given cells must add up to a number indicated- typically the area is marked by a dotted line.
        //Usually no number can appear more than once within a given area (which makes sense since otherwise it would just be a normal sodoku puzzle...)
        //unfortunately we lose out on a lot of possible solver logic, though it should be possible to reason about such a puzzle. Generators could take longer- though it would also be possible
        //to generate a normal puzzle and create areas within that "normal" that don't have any repeating values and indicate their sum to "cheat".
    


        //Each puzzle consists of a 9x9 Sudoku grid containing areas surrounded by gray or dotted lines. 
        //The object is to fill all empty squares so that the numbers 1 to 9 appear exactly once in each row, 
        //column and 3x3 box, and the sum of the numbers in each area is equal to the clue in the area’s top-left corner. 
        //In addition, no number may be used in the same area more than once.





        private Dictionary<Tuple<int, int>, SodokuCell> CellData = new Dictionary<Tuple<int, int>, SodokuCell>();
        private Dictionary<SodokuCell, Tuple<int, int>> CellDataReverse = new Dictionary<SodokuCell, Tuple<int, int>>();
        private Dictionary<Tuple<int, int>, MiniGrid> MiniGrids = new Dictionary<Tuple<int, int>, MiniGrid>();
        private Dictionary<MiniGrid, Tuple<int, int>> MiniGridsReverse = new Dictionary<MiniGrid, Tuple<int, int>>();
        public event EventHandler<EventArgs> PuzzleSolved;
        public int RowCount { get; } = 9;
        public int ColumnCount { get; } = 9;

        private  String GetBoardStateString()
        {
            StringBuilder sBoardData = new StringBuilder();
            for(int y=1;y<10;y++)
            {

                String RowData = String.Join(",", from r in GetRowEnumerable(y) select r.Value);
                sBoardData.Append(RowData + "|");
            }
            return sBoardData.ToString();
        }
        private void SetBoardStateString(String sBoardState)
        {
            String[] Rows = sBoardState.Split('|');
            for(int row=1;row<10;row++)
            {
                String currentRow = Rows[row - 1];
                String[] Cells = currentRow.Split(',');
                for(int cell=1;cell<10;cell++)
                {
                    Tuple<int,int> CellKey = new Tuple<int, int>(row,cell);
                    SodokuCell GetCell = CellData[CellKey];
                    GetCell.Value = int.Parse(Cells[cell - 1]);
                }
            }
        }

        public int[,] GetBoardState()
        {
            int[,] ConstructResult = new int[9, 9];
            for(int x=1;x<10;x++)
            {
                for(int y=1;y<10;y++)
                {
                    Tuple<int, int> KeyData = new Tuple<int, int>(x, y);
                    SodokuCell sc = CellData[KeyData];
                    ConstructResult[x - 1, y - 1] = sc.Value;
                }
            }
            return ConstructResult;
        }
        public void SetBoardState(int[,] StateData)
        {
            try
            {
                for(int x=1;x<10;x++)
                {
                    for(int y=1;y<10;y++)
                    {
                        Tuple<int, int> KeyData = new Tuple<int, int>(x, y);
                        SodokuCell sc = CellData[KeyData];
                        sc.Value = StateData[x - 1, y - 1];
                    }
                }



            }
            catch(Exception exx)
            {
                throw new ArgumentException("Statedata",exx);
            }

        }
        public static SodokuBoard GeneratePuzzle(int EmptyCells=45)
        {
            Debug.Print("Generating a Sodoku Puzzle...");
            Random rng = new Random();
            bool builtpuzzle = false;
            SodokuBoard buildBoard = null;
            int Attempts = 0;
            bool Completed = false;
            Action ThreadRoutine = () =>
            {
                try
                {

                    Attempts = 0;
                    Completed = false;

                    while (!builtpuzzle)
                    {
                        buildBoard = new SodokuBoard();

                        Attempts++;
                        Debug.Print("Attempt " + Attempts + " generating puzzle...");
                        builtpuzzle = buildBoard.BruteForce_Solve(0, true);
                    }
                    Completed = true;
                }
                catch (ThreadAbortException tae)
                {
                    //Completed = true;

                    //aborted...
                }
            };
            Thread PuzzleBuilder = new Thread((w)=>ThreadRoutine());
            PuzzleBuilder.Start();
            Stopwatch puzzletimer = new Stopwatch();
            puzzletimer.Start();
            Completed = false;
            int RestartCount = 0;
            while(!Completed)
            {
                if(puzzletimer.Elapsed.TotalSeconds > 10)
                {
                    Debug.Print("Puzzle took too long to generate.");
                    PuzzleBuilder.Abort();
                    puzzletimer.Restart();
                    PuzzleBuilder = new Thread((w) => ThreadRoutine());
                    PuzzleBuilder.Start();
                    RestartCount++;
                    Completed = false;
                    Debug.Print("Running generation again, restart count " + RestartCount);
                }
                Thread.Sleep(10);
            }
            //We choose random cells by taking the list of all Cells, and randomizing the list and taking the top 45.

            foreach(var clearcell in GetSymmetricalPositions(EmptyCells/2))
            {
                buildBoard.CellData[clearcell].Value = 0;
            }
           
            foreach(var lockcell in buildBoard.AllCells())
            {
                if (lockcell.Value != 0) lockcell.Locked = true;

            }

            return buildBoard;
        }
        private static IEnumerable<Tuple<int, int>> GetSymmetricalPositions(int perside = 20)
        {
            //generate a list of all tuples from 1,1 to 5,5.
            Random rng = new Random();
            bool completed = false;
            List<Tuple<int, int>> Entries = new List<Tuple<int, int>>();
            
            for(int y=1;y<6;y++)
            {
                for (int x = 1; x < 10; x++)
                {
                    Entries.Add(new Tuple<int, int>(x, y));
                    if (x==5 && y==5)
                    {
                        
                        completed = true;
                        break;
                    }
                }
                if (completed) break;
            }
            var firstsymmetric = Entries.OrderBy((r) => rng.Next()).Take(perside);
            foreach(var iterate in firstsymmetric)
            {
                yield return iterate;
                Tuple<int, int> Symmetrical = new Tuple<int, int>(10 - iterate.Item1, 10 - iterate.Item2);
                yield return Symmetrical;
            }


        }
        public static Random rng = new Random();
        public bool BruteForce_Solve(int RecursionCount,bool pRandomize = false)
        {
            //randomize will randomize the brute force algorithm a bit.
            //construct a new SodokuBoard as a working model, copying from this instance.
            //UnfilledCells are the initial cells we are working with..
            List<SodokuCell> UnfilledCells = null;
            /*if(pRandomize)
            {
                UnfilledCells = (from c in CellData.Values where c.Value == 0 select c).ToList();
            }*/
            UnfilledCells = (from c in CellData.Values where c.Value == 0 orderby GetValidValuesForCell(c).Count ascending select c).ToList();
            if (UnfilledCells.Count == 0) return false;
            Debug.Print("Brute forcing puzzle. Starting point:");
            Debug.Print(GetBoardStateString().Replace("|", "\n"));

            Debug.Print("Brute forcing with " + UnfilledCells.Count + " Unfilled cells.");
            if (UnfilledCells.Count == 1) return false;
            
            //if randomizing, randomize the cell order.
            if(pRandomize) Shuffle(UnfilledCells,rng);


            foreach (var UnfilledCell in UnfilledCells)
            {
                var CellValues = GetValidValuesForCell(UnfilledCell);
                //if needed, randomize the order of the cells values which dictates what order we try to fill them in as well.
                if (pRandomize) Shuffle(CellValues,rng);
                foreach (var possiblevalue in CellValues)
                {
                    Debug.Print("Copying current board state...");
                    Debug.Print("Choosing value " + possiblevalue + " for cell at position:(" + UnfilledCell.X + "," + UnfilledCell.Y + ")");
                    //copy our "original" board state...
                    SodokuBoard sb = new SodokuBoard(this);
                    var fillcell = sb.GetCellAtPosition(UnfilledCell.X, UnfilledCell.Y);
                    fillcell.Value = possiblevalue;
                    //set the given cell to this value.
                    //now, attempt to solve the board. Note that this will be recursive, as SolvePuzzle will attempt to bruteforce as a last result as well.
                    if (sb.SolvePuzzle(RecursionCount+1,pRandomize))
                    {
                        //if it was successful- we are done. We want to get the boardstate of sb into ourselves now.
                        SetBoardState(sb.GetBoardState());
                        return true;
                    }
                    else
                    {
                        Debug.Print("No solution was found with value " + fillcell.Value + " at position: (" + UnfilledCell.X + ", " + UnfilledCell.Y + ") Recursion Level:" + RecursionCount);
                    }
                }
            }
            return false;
           
        }
        public void Save(String sTargetFile,bool sLockCells=false)
        {
            XDocument SaveDocument = new XDocument();
            XElement RootNode = new XElement("SodokuBoard");
            SaveDocument.Add(RootNode);
            foreach(var LoopCell in CellData)
            {
                if(LoopCell.Value.Value>0)
                {
                    XElement BoardCell = new XElement("BoardCell",
                        new XAttribute("X",LoopCell.Key.Item1),
                        new XAttribute("Y", LoopCell.Key.Item2),
                        new XAttribute("Value", LoopCell.Value.Value),
                        new XAttribute("Locked", sLockCells || LoopCell.Value.Locked)
                        );
                    RootNode.Add(BoardCell);
                }
            }
            SaveDocument.Save(sTargetFile);

        }
        public void Load(String sSourceFile)
        {
            Clear();
            XDocument LoadDocument = XDocument.Load(sSourceFile);
            XElement RootNode = LoadDocument.Root;
            if(RootNode.Name=="SodokuBoard")
            {
                foreach(XElement BoardCellNode in RootNode.Elements("BoardCell"))
                {
                    int XAttr = BoardCellNode.GetAttributeInt("X");
                    int YAttr = BoardCellNode.GetAttributeInt("Y");
                    int ValueAttr = BoardCellNode.GetAttributeInt("Value");
                    bool isLocked = BoardCellNode.GetAttributeBool("Locked");
                    Tuple<int, int> UseKey = new Tuple<int, int>(XAttr, YAttr);
                    if(CellData.ContainsKey(UseKey))
                    {
                        var GrabBoardCell = CellData[UseKey];
                        GrabBoardCell.Locked = isLocked;
                        GrabBoardCell.Value = ValueAttr;
                    }
                }
            }
        }
        public SodokuBoard(SodokuBoard CloneSource)
        {
            InitializeBoard();
            foreach(var cell in CellData)
            {
                Tuple<int, int> CellKey = cell.Key;
                cell.Value.Value = CloneSource.CellData[CellKey].Value;
            }
        }
        public SodokuBoard()
        {
            InitializeBoard();
        }
      
        public void Clear()
        {
            CellData = new Dictionary<Tuple<int, int>, SodokuCell>();
            CellDataReverse = new Dictionary<SodokuCell, Tuple<int, int>>();
            MiniGrids = new Dictionary<Tuple<int, int>, MiniGrid>();
            MiniGridsReverse = new Dictionary<MiniGrid, Tuple<int, int>>();
            InitializeBoard();
        }
        private void InitializeBoard()
        {
            for(int x = 1; x < 10; x++)
            {
                for(int y = 1; y < 10; y++)
                {
                    Tuple<int, int> BuildKey = new Tuple<int, int>(x, y);
                    SodokuCell NewCell = new SodokuCell(this,x,y);
                    CellData.Add(BuildKey,NewCell);
                    CellDataReverse.Add(NewCell,BuildKey);
                }
            }
            for(int x=1;x<=3;x++)
            {
                for(int y=1;y<=3;y++)
                {
                    Tuple<int, int> GridTuple = new Tuple<int, int>(x,y);
                    MiniGrid pm = new MiniGrid(this,x,y);
                    MiniGrids.Add(GridTuple,pm);
                    MiniGridsReverse.Add(pm,GridTuple);
                }
            }


        }
        public MiniGrid GetMiniGrid(int MiniGridX,int MiniGridY)
        {
            if (MiniGridX < 1 || MiniGridX > 3) throw new ArgumentException("MiniGridX");
            if(MiniGridY < 1 || MiniGridY > 3) throw new ArgumentException("MiniGridY");
            
            Tuple<int, int> GridKey = new Tuple<int, int>(MiniGridX, MiniGridY);
            return MiniGrids[GridKey];
        }
        public MiniGrid GetMiniGrid(SodokuCell Cell)
        {
            if (CellDataReverse.ContainsKey(Cell))
            {
                var CellPos = CellDataReverse[Cell];
                return GetMiniGridFromFullPosition(CellPos.Item1, CellPos.Item2);
            }
            return null;
        }
        public MiniGrid GetMiniGridFromFullPosition(int GridX, int GridY)
        {
            if (GridX < 1 || GridX > 9) throw new ArgumentException("GridX");
            if (GridY < 1 || GridY > 9) throw new ArgumentException("GridY");
            var useX = Math.Min((GridX-1) / 3 +1 , 3);
            var useY = Math.Min(((GridY-1) / 3)+1 , 3);
            return GetMiniGrid(useX,useY);
                
        }
        public IList<int> GetValidValuesForCell(SodokuCell cell)
        {
            //when handling for a specific SodokuCell, we set the cell's value temporarily to 0.
            int OriginalValue = cell.Value;
            try
            {
                cell.Value = 0;

                Tuple<int, int> PositionData = CellDataReverse[cell];
                var ReturnResult = GetValidValuesForCell(PositionData.Item1, PositionData.Item2);
                return ReturnResult;
            }
            finally
            {
                cell.Value = OriginalValue;
            }

        }
        public IList<int> GetValidValuesForCell(int pX,int pY)
        {
            ValidateGridPosition(pX,pY);
            HashSet<int> FoundValues = new HashSet<int>();
            foreach(var enumcol in GetColumnEnumerable(pX))
            {
                if(!FoundValues.Contains(enumcol.Value)) FoundValues.Add(enumcol.Value);
            }
            foreach(var enumrow in GetRowEnumerable(pY))
            {
                if (!FoundValues.Contains(enumrow.Value)) FoundValues.Add(enumrow.Value);
            }
            foreach(var enumgrid in GetMiniGridFromFullPosition(pX,pY).AllCells())
            {
                if (!FoundValues.Contains(enumgrid.Value)) FoundValues.Add(enumgrid.Value);
            }

            return Enumerable.Range(1, 9).Where((w) => !FoundValues.Contains(w)).ToList();

        }
        /// <summary>
        /// Searches the SodokuBoard to try to find Twins. ModifiedPossibleValues will be adjusted based on discovered twins to eliminate those possibilities
        /// from the possible values of other cells in the appropriate row/column/minigrid.
        /// </summary>
        /// <param name="ModifiedPossibleValues"></param>
        /// <returns></returns>
        private int FindTwins(ref Dictionary<SodokuCell, List<int>> ModifiedPossibleValues)
        {
            //"Twins" are when multiple cells in the given set have two possible values that are the same.
            //these are important because if there are two cells in a column/row/minigrid that only have two possible values, we can eliminate both of those possible values from the 
            //possibilities of any of the other values in that column/row/minigrid.
            //returns t he number of Twins that were found in total.
            return FindGroups(ref ModifiedPossibleValues, 2);
        }

        private int FindTriplets(ref Dictionary<SodokuCell,List<int>> ModifiedPossibleValues)
        {
            return FindGroups(ref ModifiedPossibleValues, 3);
        }
        private int FindGroups(ref Dictionary<SodokuCell,List<int>> ModifiedPossibleValues,int GroupCount=2)
        {
            int TotalCount = 0;
            for (int x = 1; x < 10; x++)
            {
                TotalCount += FindGroups_Set(GetColumnEnumerable(x), ref ModifiedPossibleValues,GroupCount);
            }

            for (int y = 1; y < 10; y++)
            {
                TotalCount += FindGroups_Set(GetRowEnumerable(y), ref ModifiedPossibleValues,GroupCount);
            }

            foreach (var mg in MiniGrids.Values)
            {
                TotalCount += FindGroups_Set(mg.AllCells(), ref ModifiedPossibleValues,GroupCount);
            }
            return TotalCount;
        }
        private int FindGroups_Set(IEnumerable<SodokuCell> Cells, ref Dictionary<SodokuCell,List<int>> ModifiedPossibleValues,int CheckCount=2)
        {//Finds Twins, Triplets,etc in a given set of cells, and adjusts the possible values for other cells.
            
            int AffectedCells = 0;
            List<SodokuCell> GroupCells = new List<SodokuCell>();
            Dictionary<String, int> GroupData = new Dictionary<string, int>();
            //get all cells that have only the given number of possible values.
            foreach(var iterate in Cells)
            {
                if (iterate.Value == 0)
                {
                    var grabpossible = ModifiedPossibleValues[iterate];
                    if (grabpossible.Count == CheckCount)
                    {
                        String KeyData = String.Join("", grabpossible);
                        
                        GroupCells.Add(iterate);
                        if (GroupData.ContainsKey(KeyData))
                            GroupData[KeyData]++;
                        else
                            GroupData.Add(KeyData,1);
                    }
                }
            }
            //if we have no cells with two possibilities, then we are done.
            if (GroupCells.Count == 0) return 0;
            var dupepossible = ModifiedPossibleValues;
            List<SodokuCell> grabtwins = new List<SodokuCell>();
            foreach(var tc in GroupCells)
            {
                var possibles = ModifiedPossibleValues[tc];
                var KeyData = String.Join("", possibles);
                if(GroupData.ContainsKey(KeyData))
                {
                    if(GroupData[KeyData]==CheckCount)
                    {
                        //if we have a number of entries  for a key that is equal to the group count, then we have a "grouplet" (twin, triplet, quadruplet, etc)
                        foreach(var stripcell in Cells)
                        {
                            var strippossible = ModifiedPossibleValues[stripcell];
                            if(strippossible.Count > CheckCount)
                            {
                                foreach (var removepossible in possibles)
                                {
                                    if (strippossible.Contains(removepossible))
                                    {
                                        AffectedCells++;
                                        strippossible.Remove(removepossible);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return AffectedCells;

        }
        private int ColumnRowMinigridElimination(Dictionary<SodokuCell,List<int>> ModifiedPossibleValues)
        {

            //Modified Possible Values is in place for Twins and Triplets  which do some preprocessing beyond GetValidValuesForCell() to eliminate possible entries.

            int ValueChangeCount = 0;
            bool ValueChanged = false;
            do
            {
                ValueChanged = false;
                foreach (var iteratecell in AllCells())
                {
                    if (iteratecell.Value == 0)
                    {
                        IList<int> validvalues = null;
                        validvalues = ModifiedPossibleValues[iteratecell];
                       
                        if (validvalues.Count == 1)
                        {
                            iteratecell.Value = validvalues.First();
                            ValueChanged = true;
                            ValueChangeCount++;
                        }
                        else if (!validvalues.Any())
                        {
                            Tuple<int, int> Position = CellDataReverse[iteratecell];
                            throw new ArgumentException("No Solution found for puzzle. Cell at " + Position.Item1 + "," + Position.Item2 + " Has no valid values.");
                        }
                    }
                }
            } while (ValueChanged);
            return ValueChangeCount;
        }
        private int HuntLoneRangers(Dictionary<SodokuCell, List<int>> ModifiedPossibleValues)
        {
            int RunningChangeCount = 0;
            for(int x=1;x<10;x++)
            {
                RunningChangeCount+= LoneRangerSearch_Column(ModifiedPossibleValues,x);
            }
            for (int y = 1; y < 10; y++)
            {
                RunningChangeCount+=LoneRangerSearch_Row(ModifiedPossibleValues,y);
            }

            foreach(var mg in MiniGrids.Values)
            {
                RunningChangeCount += LoneRangerSearch_Minigrid(ModifiedPossibleValues,mg);
            }
            return RunningChangeCount;
        }
        private int LoneRangerSearch_Column(Dictionary<SodokuCell, List<int>> ModifiedPossibleValues,int i)
        {
            return LoneRangerSearch(ModifiedPossibleValues,GetColumnEnumerable(i));
        }
        private int LoneRangerSearch_Row(Dictionary<SodokuCell, List<int>> ModifiedPossibleValues,int i)
        {
            return LoneRangerSearch(ModifiedPossibleValues,GetRowEnumerable(i));
        }
        private int LoneRangerSearch_Minigrid(Dictionary<SodokuCell, List<int>> ModifiedPossibleValues,MiniGrid mg)
        {
            return LoneRangerSearch(ModifiedPossibleValues,mg.AllCells());
        }
        private int LoneRangerSearch(Dictionary<SodokuCell, List<int>> ModifiedPossibleValues, IEnumerable<SodokuCell> SearchEntries)
        {
            //a "lone Ranger" search tries to find instances where a number only appears as a possible value within one of the cells.
            //For a data structure we will have a series of buckets:
            //number is the main key; each  entries has a list of the cells that have that number as a possible value.
            int changedCount = 0;
            Dictionary<int, List<SodokuCell>> Possibles = new Dictionary<int, List<SodokuCell>>();
            for(int i = 1;i < 10;i++)
            {
                Possibles.Add(i, new List<SodokuCell>());
            }

            //go through each Cell, get the possible values, and add it to the list at that index.
            foreach(var iteratecell in SearchEntries)
            {
                List<int> ValidValues = null;
                if (iteratecell.Value > 0)
                {
                    Possibles[iteratecell.Value].Add(iteratecell);
                }
                else
                {
                    ValidValues = ModifiedPossibleValues[iteratecell];
                    foreach (var validentry in ValidValues)
                    {
                        Possibles[validentry].Add(iteratecell);
                    }
                }
            }

            //now, we go through the main dictionary, and look at each one. If a possible value only has one cell, we assign the value to it.

            for(int i=1;i<10;i++)
            {
                if(Possibles[i].Count==1)
                {
                    if (Possibles[i].First().Value == 0)
                    {
                        Possibles[i].First().Value = i;
                        changedCount++;
                    }
                }
            }
            return changedCount;
        }
        public bool SolvePuzzle(int RecursionCount,bool pRandomize=false,bool NoBrute = false)
        {
            
            Dictionary<SodokuCell, List<int>> ModifiedPossibleValues = new Dictionary<SodokuCell, List<int>>();
            int ChangeCount = 0;
            foreach(var iterateCell in AllCells())
            {
                ModifiedPossibleValues.Add(iterateCell,new List<int>(GetValidValuesForCell(iterateCell)));
            }
            int unfounditerationcount = 0;
            bool StillSolving = true;
            int OuterloopUnchangedCount = 0;
            ModifiedPossibleValues = new Dictionary<SodokuCell, List<int>>();
            foreach (var iterateCell in AllCells())
            {
                ModifiedPossibleValues.Add(iterateCell, new List<int>(GetValidValuesForCell(iterateCell)));
            }
            while (StillSolving)
            {
                do
                {
                  
                    ChangeCount = 0;

                    try
                    {
                        var CRMEChange = ColumnRowMinigridElimination(ModifiedPossibleValues);
                        ChangeCount += CRMEChange;
                    }
                    catch(Exception exp)
                    {
                        return false;
                    }
                    var RangerCount = HuntLoneRangers(ModifiedPossibleValues);
                    ChangeCount += RangerCount;

                    int checkfortwins = FindTwins(ref ModifiedPossibleValues);
                    ChangeCount += checkfortwins;

                    if(checkfortwins>0)
                    {
                        var CRMEChange2 = ColumnRowMinigridElimination(ModifiedPossibleValues);
                        ChangeCount += CRMEChange2;
                    }
                    int checkfortriplets = FindTriplets(ref ModifiedPossibleValues);
                    ChangeCount += checkfortriplets;
                    if(checkfortriplets>0)
                    {
                        var CRMEChange3 = ColumnRowMinigridElimination(ModifiedPossibleValues);
                        ChangeCount += CRMEChange3;
                    }

                    if (ChangeCount > 0) unfounditerationcount = 0;
                    unfounditerationcount++;

                }
                while (ChangeCount > 0 && unfounditerationcount < 100);

                bool testfinished = IsPuzzleSolved();
                if (!testfinished)
                {
                    ModifiedPossibleValues = new Dictionary<SodokuCell, List<int>>();
                    foreach (var iterateCell in AllCells())
                    {
                        ModifiedPossibleValues.Add(iterateCell, new List<int>(GetValidValuesForCell(iterateCell)));
                    }
                    int outertests = 0;
                    
                    if (outertests == 0) OuterloopUnchangedCount++;
                    if (OuterloopUnchangedCount >= 10) StillSolving = false;
                }
                else
                {
                    StillSolving = false;
                }
            }
            var isSolved = IsPuzzleSolved();
            if (!isSolved && !NoBrute)
            {
                var brutesolve = BruteForce_Solve(RecursionCount + 1, pRandomize);
                if (brutesolve) return true;
            }
            return isSolved;
        }
        private bool HasAllValues(IEnumerable<SodokuCell> Cells)
        {
            List<int> CheckValues = Enumerable.Range(1, 9).ToList();
            foreach (var ColumnItem in Cells)
            {
                if (CheckValues.Contains(ColumnItem.Value))
                    CheckValues.Remove(ColumnItem.Value);
            }

            return CheckValues.Count == 0;
        }

        public bool IsPuzzleSolved()
        {
            
            for (int col=1;col<10;col++)
            {
                var GrabColData = GetColumn(col);
                if (!HasAllValues(GrabColData)) return false;
            

            }
            
            for(int row=1;row<10;row++)
            {
                var GrabRowData = GetRow(row);
                if (!HasAllValues(GrabRowData)) return false;
            }

            foreach(var LoopGrid in MiniGrids.Values)
            {
                var GrabGridData = LoopGrid.AllCells();
                if (!HasAllValues(GrabGridData)) return false;
            }
            return true;

        }

        public SodokuCell GetCellAtPosition(int pX,int pY)
        {
            ValidateGridPosition(pX, pY);
            Tuple<int, int> FindKey = new Tuple<int, int>(pX, pY);
            if (CellData.ContainsKey(FindKey)) return CellData[FindKey];
            return null;
        }

        private static void ValidateGridPosition(int pX, int pY)
        {
            if (pX < 1 || pX > 9) throw new ArgumentException("pX");
            if (pY < 1 || pY > 9) throw new ArgumentException("pY");
        }

        private IEnumerable<SodokuCell> GetRowEnumerable (int pRow)
        {
            if(pRow > RowCount || pRow < 1) throw new ArgumentException("pRow");
            for(int x=1;x<=ColumnCount;x++)
            {
                var GrabCell = GetCellAtPosition(x, pRow);
                yield return GrabCell;
            }
        }
        private IEnumerable<SodokuCell> GetColumnEnumerable(int pColumn)
        {
            if(pColumn > ColumnCount || pColumn < 1) throw new ArgumentException("pColumn");
            for(int y=1;y<=RowCount;y++)
            {
                var GrabCell = GetCellAtPosition(pColumn, y);
                yield return GrabCell;
            }
        }
        public IEnumerable<SodokuCell> AllCells()
        {
            return CellData.Values;
        }
        public IList<SodokuCell> GetRow(int pRow)
        {
            List<SodokuCell> BuildRow = new List<SodokuCell>();
            foreach(SodokuCell iterate in GetRowEnumerable(pRow))
            {
                BuildRow.Add(iterate);
            }
            return BuildRow;
        }
        public IList<SodokuCell> GetColumn(int pColumn)
        {
            List<SodokuCell> BuildColumn = new List<SodokuCell>();
            foreach(SodokuCell iterate in GetColumnEnumerable(pColumn))
            {
                BuildColumn.Add(iterate);
            }
            return BuildColumn;
        }
        public void CellEvent(SodokuCellEvent eventarg)
        {
            var temp = PuzzleSolved;
            if (temp != null)
            {
                if (eventarg is SodokuCellEvent_Changed)
                {
                    if (IsPuzzleSolved())
                    {

                        temp.Invoke(this, new EventArgs());
                    }

                }
            }
            //throw new NotImplementedException();
        }
        public void ClearHighlight()
        {
            foreach(var clearcell in AllCells())
            {
                clearcell.Highlighted = false;
            }
        }
        public void SetHighlight(IEnumerable<SodokuCell> NewHighlight)
        {
            ClearHighlight();
            foreach(var iterateentry in NewHighlight)
            {
                iterateentry.Highlighted = true;
            }
        }

        public SodokuCell HitTest(float pX,float pY,float pWidth,float pHeight)
        {
            float CellHeight = pHeight / 9;
            float CellWidth = pWidth / 9;
            int XCell = (int)(pX / CellWidth)+1;
            int YCell = (int)(pY / CellHeight)+1;

            return GetCellAtPosition(XCell, YCell);


        }
        


        private static void Shuffle<T>(IList<T> list, Random rnd)
        {
            for (var i = 0; i < list.Count; i++)
                Swap(list, i, rnd.Next(i, list.Count));
        }

        private static void Swap<T>(IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

    }
    
    
}
