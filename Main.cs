using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace DemonChess
{
    public partial class Main : Form
    {
        private static Engine _e;
        private static string _guiEngineMove = "";
        private static string _guipvLineString = "";
        private static string _guipvLineStringOld = "";
        private static int _maxDepthLevel = 64;
        private static string _previosCell = "";
        private static string _presentCell = "";
        private static Thread _myStatusThinker;
        private static int _totalTimeForWhite = 5;
        private static int _totalTimeForBlack = 5;
        private static int _totalTimeForWhiteForTick = 5;
        private static int _totalTimeForBlackForTick = 5;
        private static int _sideToMove = 1;
        private static int _engineSide = 2;
        private static bool _engineWin;
        private static int _humanSide = 1;
        private static bool _humanWin;
        private static bool _engineThink;
        private static bool _engineThinking;
        private static int _whiteClockTick = 600;
        private static int _whiteClockTickForThinking;
        private static int _blackClockTick = 600;
        private static int _blackClockTickForThinking;
        private static int _loadFirstTime;
        private static bool _startedNewGame;
        private static bool _startedLoadedGame;
        private static bool _inGameProcessing;
        private static Hashtable _hshTableCellNameToIndex = new Hashtable(64);
        private static string[] _listPgnFile;
        private static string[] _loadedPgnFile;
        private static string[] _realMoveListInPgn;
        private static int _lengthListPgnFile;
        private static int _indexOfMoveInPgnfile = -1;
        private static Hashtable _hshTableMoveTypeInPgnFile;
        private static Hashtable _hshTablePromotionValueInPgnFile;
        private static string _movePromotionPawn = "";

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (_loadFirstTime != 0) return;
            DrawBoard();
            InitMenu();
            InitListViewPvLine();
            _loadFirstTime = 1;
            _engineThink = false;
            _e = new Engine();
            Engine.Level(40, _totalTimeForWhite * 60, 0);
            Engine.ThinkOff();
            InitAll();
            _myStatusThinker = new Thread(GetThinkerStatus);
            _myStatusThinker.Start();
            ClearPgnFileView();
            ChooseWhiteToolStripMenuItem.Checked = true;
            ChooseBlackToolStripMenuItem.Checked = false;
        }

        private void InitMenu()
        {
            mnNewGame.Image = Image.FromFile(Application.StartupPath + "\\Images\\new.png");
            mnLoadGame.Image = Image.FromFile(Application.StartupPath + "\\Images\\load.png");
            mnSaveGame.Image = Image.FromFile(Application.StartupPath + "\\Images\\save.png");
            mnExitGame.Image = Image.FromFile(Application.StartupPath + "\\Images\\exit.png");
            mnTimeOption.Image = Image.FromFile(Application.StartupPath + "\\Images\\time.png");
            ChooseWhiteToolStripMenuItem.Image = Image.FromFile(Application.StartupPath + "\\Images\\playwhite.png");
            ChooseBlackToolStripMenuItem.Image = Image.FromFile(Application.StartupPath + "\\Images\\playblack.png");
            mnHelpPlayer.Image = Image.FromFile(Application.StartupPath + "\\Images\\help.png");
            mnLevel.Image = Image.FromFile(Application.StartupPath + "\\Images\\level.png");
            mnResignGame.Image = Image.FromFile(Application.StartupPath + "\\Images\\resign.png");
            mnIntroHelp.Image = Image.FromFile(Application.StartupPath + "\\Images\\about.png");
        }

        private void InitListViewPvLine()
        {
            LVPvLine.Items.Clear();
            LVPvLine.View = View.Details;
            _ = LVPvLine.Columns.Add("Depth", 30);
            _ = LVPvLine.Columns.Add("Eval", 70);
            _ = LVPvLine.Columns.Add("Time", 70);
            _ = LVPvLine.Columns.Add("Nodes", 90);
            _ = LVPvLine.Columns.Add("NPS", 90);
            _ = LVPvLine.Columns.Add("PV", 254);
        }

        private void InsertPvLine(string pvLine)
        {
            if (pvLine.Equals(_guipvLineStringOld)) return;

            if (pvLine.Trim().Equals("")) return;

            LVPvLine.Items.Clear();
            _ = new ListViewItem();

            var line = Regex.Split(pvLine, "\r\n");

            foreach (var t in line)
            {
                var items = Regex.Split(t, ",");

                if (!t.Trim().Equals(""))
                {

                    var lvi = new ListViewItem(items[0]);
                    var eval = float.Parse(items[1]) / 100;
                    var time = float.Parse(items[2]) / 1000;
                    uint nodes = uint.Parse(items[3]);

                    uint speed;
                    if (time == 0)
                        speed = 1;
                    else
                        speed = (uint)(nodes / time);

                    _ = lvi.SubItems.Add(eval.ToString(CultureInfo.InvariantCulture) + " cp");
                    _ = lvi.SubItems.Add(time + " s");
                    _ = lvi.SubItems.Add(nodes + " n");
                    _ = lvi.SubItems.Add(speed + " nps");
                    _ = lvi.SubItems.Add(items[4]);
                    _ = LVPvLine.Items.Add(lvi);
                }
            }
        }

        private void DrawBoard()
        {
            const int spaceLeft = 1;
            const int spaceTop = 1;
            int i;
            BoardPanel.Controls.Clear();
            _hshTableCellNameToIndex = new Hashtable(64);

            for (i = 0; i < 64; i++)
            {
                var pic = new PictureBox();
                var size = new Size(50, 50);
                pic.Location = new Point(spaceLeft + i % 8 * 50, i / 8 * 50 + spaceTop);

                pic.BackgroundImage = (i % 8 + i / 8) % 2 == 0
                    ? Image.FromFile(Application.StartupPath + "\\Images\\white.png")
                    : Image.FromFile(Application.StartupPath + "\\Images\\black.png");

                pic.Size = size;

                if (i < 10)
                {
                    pic.Name = "Picture" + GetSquareName("0" + i);
                    _hshTableCellNameToIndex.Add(GetSquareName("0" + i), i);
                }
                else
                {
                    pic.Name = "Picture" + GetSquareName(i.ToString());
                    _hshTableCellNameToIndex.Add(GetSquareName(i.ToString()), i);
                }

                pic.BorderStyle = BorderStyle.FixedSingle;
                pic.Click += Pic_Click;
                pic.MouseLeave += Pic_MouseOut;

                pic.Tag = i;
                ChoosePicForPiece(ref pic, i);

                BoardPanel.Controls.Add(pic);
            }
        }

        private static void ChoosePicForPiece(ref PictureBox pic, int i)
        {
            if (i >= 8 && i <= 15)
                pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackPawn.png");
            else if (i >= 48 && i <= 55)
                pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhitePawn.png");
            else
                switch (i)
                {
                    case 0:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                        break;
                    case 1:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKnight.png");
                        break;
                    case 2:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackBishop.png");
                        break;
                    case 3:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackQueen.png");
                        break;
                    case 4:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKing.png");
                        break;
                    case 5:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackBishop.png");
                        break;
                    case 6:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKnight.png");
                        break;
                    case 7:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                        break;
                    case 56:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                        break;
                    case 57:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKnight.png");
                        break;
                    case 58:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteBishop.png");
                        break;
                    case 59:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteQueen.png");
                        break;
                    case 60:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKing.png");
                        break;
                    case 61:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteBishop.png");
                        break;
                    case 62:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKnight.png");
                        break;
                    case 63:
                        pic.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                        break;
                }
        }

        private void MakePromotionPawn(int promotionValue)
        {
            switch (promotionValue)
            {
                case 2:
                    _movePromotionPawn += "b";
                    if (!Engine.GuiCheckUserMove(_movePromotionPawn)) return;

                    break;
                case 3:
                    _movePromotionPawn += "n";
                    if (!Engine.GuiCheckUserMove(_movePromotionPawn)) return;

                    break;
                case 4:
                    _movePromotionPawn += "r";
                    if (!Engine.GuiCheckUserMove(_movePromotionPawn)) return;

                    break;
                case 5:
                    _movePromotionPawn += "q";
                    if (!Engine.GuiCheckUserMove(_movePromotionPawn)) return;

                    break;
            }

            _inGameProcessing = true;
            _sideToMove = 3 - _sideToMove;
            _startedNewGame = true;
            LVPvLine.Items.Clear();
            _engineThinking = true;

            switch (_engineSide)
            {
                case 1:
                    Engine.Level(40, _totalTimeForWhiteForTick * 60 + _whiteClockTick / 10, 0);
                    break;
                case 2:
                    Engine.Level(40, _totalTimeForBlackForTick * 60 + _blackClockTick / 10, 0);
                    break;
            }

            switch (_sideToMove)
            {
                case 2:
                    Engine.ChangeSideToBlack();
                    break;
                case 1:
                    Engine.ChangeSideToWhite();
                    break;
            }

            _e.ThinkOn();

            ChangePicForMove(_movePromotionPawn, 1);
            AddMoveToPgnViewFile(_movePromotionPawn.Trim());
        }

        private static void Pic_MouseOut(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;
            var i = int.Parse(pic.Tag.ToString());

            pic.BackgroundImage = (i % 8 + i / 8) % 2 == 0
                ? Image.FromFile(Application.StartupPath + "\\Images\\white.png")
                : Image.FromFile(Application.StartupPath + "\\Images\\black.png");
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;
            _previosCell = _presentCell;
            _presentCell = pic.Name.Substring(pic.Name.Length - 2, 2);
            _movePromotionPawn = "";

            if (_previosCell.Equals(_presentCell) || _previosCell.Equals("")) return;
            var tempMove = _previosCell + _presentCell;

            if (Engine.GuiIsMoveAbleFromString(tempMove.Trim()) == 1)
            {
                _movePromotionPawn = tempMove;

                var pf = new PromotionForm
                {
                    PassPro = MakePromotionPawn
                };

                pf.Show();
                return;
            }

            if (_engineThinking || _humanSide != _sideToMove ||
                _humanSide != Engine.GUIgetSideOnSquare(_hshTableCellNameToIndex[_previosCell].ToString()) ||
                !Engine.GuiCheckUserMove(tempMove.Trim())) return;
            _inGameProcessing = true;
            HideButtonPgnView();
            _sideToMove = 3 - _sideToMove;
            _startedNewGame = true;
            LVPvLine.Items.Clear();
            _engineThinking = true;

            switch (_engineSide)
            {
                case 1:
                    Engine.Level(40, _totalTimeForWhiteForTick * 60 + _whiteClockTick / 10, 0);
                    break;
                case 2:
                    Engine.Level(40, _totalTimeForBlackForTick * 60 + _blackClockTick / 10, 0);
                    break;
            }

            switch (_sideToMove)
            {
                case 2:
                    Engine.ChangeSideToBlack();
                    break;
                case 1:
                    Engine.ChangeSideToWhite();
                    break;
            }

            _e.ThinkOn();

            AddMoveToPgnViewFile(tempMove.Trim());
            ChangePicForMove(tempMove.Trim(), Engine.GuIgetMoveTypeHuman());
        }

        private void AddMoveToPgnViewFile(string move)
        {
            if (_startedLoadedGame)
                for (var i = _indexOfMoveInPgnfile / 2 + 1; i < GridViewPgnFile.RowCount; i++)
                {
                    GridViewPgnFile.Rows[i].Cells[0].Value = "";
                    GridViewPgnFile.Rows[i].Cells[1].Value = "";
                    GridViewPgnFile.Rows[i].Cells[2].Value = "";
                }

            _indexOfMoveInPgnfile++;
            _realMoveListInPgn[_indexOfMoveInPgnfile] = move.Trim();

            if (_indexOfMoveInPgnfile % 2 == 1)
            {
                GridViewPgnFile.Rows[_indexOfMoveInPgnfile / 2].Cells[2].Value = move;
                GridViewPgnFile.Rows[_indexOfMoveInPgnfile / 2].Cells[0].Value = _indexOfMoveInPgnfile / 2 + 1;
            }
            else
            {
                GridViewPgnFile.Rows[_indexOfMoveInPgnfile / 2].Cells[1].Value = move;
                GridViewPgnFile.Rows[_indexOfMoveInPgnfile / 2].Cells[2].Value = "";
            }

            CheckRepeatMove(_realMoveListInPgn, _indexOfMoveInPgnfile);
        }

        private void CheckRepeatMove(IList<string> move, int m)
        {
            if (m < 10) return;

            var isRepeat = move[m].Equals(move[m - 4]) && move[m].Equals(move[m - 8]) &&
                           move[m - 1].Equals(move[m - 5]) && move[m - 1].Equals(move[m - 9]);
            isRepeat &= move[m - 3].Equals(move[m - 7]) && move[m - 2].Equals(move[m - 6]);

            if (isRepeat) _ = MessageBox.Show(@"Draw by 3-fold repetition");
        }

        private void ClearHightLightPicMove(string namePic)
        {
            if (namePic == null || namePic.Equals("")) return;

            var picName = namePic.Substring(2, 2);

            for (var i = 0; i < 64; i++)
            {
                var pic = (PictureBox)BoardPanel.Controls[i];

                if (!pic.Name.Equals("Picture" + picName)) continue;
                pic.BackgroundImage = (i % 8 + i / 8) % 2 == 0
                    ? Image.FromFile(Application.StartupPath + "\\Images\\white.png")
                    : Image.FromFile(Application.StartupPath + "\\Images\\black.png");

                return;
            }
        }

        private void ChangePicForMove(string move, int moveType)
        {
            if (move == null || move.Equals("")) return;

            var nameSource = move.Substring(0, 2);
            var nameDest = move.Substring(2, 2);
            var fromSide = Engine.GUIgetSideOnSquare(_hshTableCellNameToIndex[nameDest].ToString());
            var fromValue = Engine.GUIgetValueOnSquare(_hshTableCellNameToIndex[nameDest].ToString());

            if (moveType == 1) fromValue = 1;

            PictureBox picSource = null;
            PictureBox picDest = null;

            if (_indexOfMoveInPgnfile >= 1)
            {
                ClearHightLightPicMove(_realMoveListInPgn[_indexOfMoveInPgnfile]);
                ClearHightLightPicMove(_realMoveListInPgn[_indexOfMoveInPgnfile - 1]);

                if (_indexOfMoveInPgnfile >= 2) ClearHightLightPicMove(_realMoveListInPgn[_indexOfMoveInPgnfile - 2]);
            }

            int i;

            for (i = 0; i < BoardPanel.Controls.Count; i++)
            {
                var pic = (PictureBox)BoardPanel.Controls[i];

                if (pic.Name.Equals("Picture" + nameSource)) picSource = pic;

                if (pic.Name.Equals("Picture" + nameDest)) picDest = pic;

                if (picSource == null || picDest == null) continue;
                picSource.Image = null;
                picDest.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Images\\light.png");

                if (fromSide == 1)
                    switch (fromValue)
                    {
                        case 1:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhitePawn.png");
                            switch (moveType)
                            {
                                case 2:
                                {
                                    var desDelColName = int.Parse(nameDest.Substring(1, 1));
                                    desDelColName--;
                                    var enpassantNameCol = nameDest.Substring(0, 1) + desDelColName;
                                    PictureBox picEnpassant = null;

                                    for (var j = 0; j < BoardPanel.Controls.Count; j++)
                                    {
                                        var picEn = (PictureBox)BoardPanel.Controls[j];

                                        if (!picEn.Name.Equals("Picture" + enpassantNameCol.Trim())) continue;
                                        picEnpassant = picEn;
                                        break;
                                    }

                                    if (picEnpassant != null) picEnpassant.Image = null;
                                    break;
                                }
                                case 1:
                                    picDest.Image =
                                        Image.FromFile(Application.StartupPath + "\\Images\\WhiteQueen.png");
                                    break;
                            }

                            break;
                        case 2:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteBishop.png");
                            break;
                        case 3:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKnight.png");
                            break;
                        case 4:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                            break;
                        case 5:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteQueen.png");
                            break;
                        case 6:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKing.png");
                            switch (moveType)
                            {
                                case 3:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[63];
                                    picRook.Image = null;
                                    picRook = (PictureBox)BoardPanel.Controls[61];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                                    break;
                                }
                                case 4:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[56];
                                    picRook.Image = null;
                                    picRook = (PictureBox)BoardPanel.Controls[59];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                                    break;
                                }
                            }

                            break;
                    }
                else
                    switch (fromValue)
                    {
                        case 1:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackPawn.png");
                            switch (moveType)
                            {
                                case 2:
                                {
                                    var desDelColName = int.Parse(nameDest.Substring(1, 1));
                                    desDelColName++;
                                    var enpassantNameCol = nameDest.Substring(0, 1) + desDelColName;
                                    PictureBox picEnpassant = null;

                                    for (var j = 0; j < BoardPanel.Controls.Count; j++)
                                    {
                                        var picEn = (PictureBox)BoardPanel.Controls[j];

                                        if (!picEn.Name.Equals("Picture" + enpassantNameCol)) continue;
                                        picEnpassant = picEn;
                                        break;
                                    }

                                    if (picEnpassant != null) picEnpassant.Image = null;
                                    break;
                                }
                                case 1:
                                    picDest.Image =
                                        Image.FromFile(Application.StartupPath + "\\Images\\BlackQueen.png");
                                    break;
                            }

                            break;
                        case 2:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackBishop.png");
                            break;
                        case 3:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKnight.png");
                            break;
                        case 4:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                            break;
                        case 5:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackQueen.png");
                            break;
                        case 6:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKing.png");
                            switch (moveType)
                            {
                                case 5:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[7];
                                    picRook.Image = null;
                                    picRook = (PictureBox)BoardPanel.Controls[5];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                                    break;
                                }
                                case 6:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[0];
                                    picRook.Image = null;
                                    picRook = (PictureBox)BoardPanel.Controls[3];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                                    break;
                                }
                            }

                            break;
                    }

                return;
            }
        }

        private void UndoChangePicForMove(string move, int moveType, int fromSide, int fromValue,
            int toSide, int toValue)
        {
            if (move == null) return;

            var nameSource = move.Substring(0, 2);
            var nameDest = move.Substring(2, 2);
            PictureBox picSource = null;
            PictureBox picDest = null;

            for (var i = 0; i < BoardPanel.Controls.Count; i++)
            {
                var pic = (PictureBox)BoardPanel.Controls[i];

                if (pic.Name.Equals("Picture" + nameSource)) picSource = pic;

                if (pic.Name.Equals("Picture" + nameDest)) picDest = pic;

                if (picSource == null || picDest == null) continue;
                picDest.Image = null;

                if (fromSide == 1)
                    switch (fromValue)
                    {
                        case 1:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhitePawn.png");
                            if (moveType == 2)
                            {
                                var desDelColName = int.Parse(nameDest.Substring(1, 1));
                                desDelColName--;
                                var enpassantNameCol = nameDest.Substring(0, 1) + desDelColName;
                                PictureBox picEnpassant = null;

                                for (var j = 0; j < BoardPanel.Controls.Count; j++)
                                {
                                    var picEn = (PictureBox)BoardPanel.Controls[j];

                                    if (!picEn.Name.Equals("Picture" + enpassantNameCol.Trim())) continue;
                                    picEnpassant = picEn;
                                    break;
                                }

                                if (picEnpassant != null)
                                    picEnpassant.Image =
                                        Image.FromFile(Application.StartupPath + "\\Images\\BlackPawn.png");
                            }

                            break;
                        case 2:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteBishop.png");
                            break;
                        case 3:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKnight.png");
                            break;
                        case 4:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                            break;
                        case 5:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteQueen.png");
                            break;
                        case 6:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteKing.png");
                            switch (moveType)
                            {
                                case 3:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[63];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                                    picRook = (PictureBox)BoardPanel.Controls[61];
                                    picRook.Image = null;
                                    break;
                                }
                                case 4:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[56];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                                    picRook = (PictureBox)BoardPanel.Controls[59];
                                    picRook.Image = null;
                                    break;
                                }
                            }

                            break;
                    }
                else
                    switch (fromValue)
                    {
                        case 1:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackPawn.png");
                            if (moveType == 2)
                            {
                                var desDelColName = int.Parse(nameDest.Substring(1, 1));
                                desDelColName++;
                                var enpassantNameCol = nameDest.Substring(0, 1) + desDelColName;
                                PictureBox picEnpassant = null;

                                for (var j = 0; j < BoardPanel.Controls.Count; j++)
                                {
                                    var picEn = (PictureBox)BoardPanel.Controls[j];

                                    if (!picEn.Name.Equals("Picture" + enpassantNameCol)) continue;
                                    picEnpassant = picEn;
                                    break;
                                }

                                if (picEnpassant != null)
                                    picEnpassant.Image =
                                        Image.FromFile(Application.StartupPath + "\\Images\\WhitePawn.png");
                            }

                            break;
                        case 2:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackBishop.png");
                            break;
                        case 3:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKnight.png");
                            break;
                        case 4:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                            break;
                        case 5:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackQueen.png");
                            break;
                        case 6:
                            picSource.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackKing.png");
                            switch (moveType)
                            {
                                case 5:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[7];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                                    picRook = (PictureBox)BoardPanel.Controls[5];
                                    picRook.Image = null;
                                    break;
                                }
                                case 6:
                                {
                                    var picRook = (PictureBox)BoardPanel.Controls[0];
                                    picRook.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                                    picRook = (PictureBox)BoardPanel.Controls[3];
                                    picRook.Image = null;
                                    break;
                                }
                            }

                            break;
                    }

                if (toValue == 0) return;
                if (toSide == 1)
                    switch (toValue)
                    {
                        case 1:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhitePawn.png");
                            break;
                        case 2:
                            picDest.Image =
                                Image.FromFile(Application.StartupPath + "\\Images\\WhiteBishop.png");
                            break;
                        case 3:
                            picDest.Image =
                                Image.FromFile(Application.StartupPath + "\\Images\\WhiteKnight.png");
                            break;
                        case 4:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\WhiteRook.png");
                            break;
                        case 5:
                            picDest.Image =
                                Image.FromFile(Application.StartupPath + "\\Images\\WhiteQueen.png");
                            break;
                    }
                else
                    switch (toValue)
                    {
                        case 1:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackPawn.png");
                            break;
                        case 2:
                            picDest.Image =
                                Image.FromFile(Application.StartupPath + "\\Images\\BlackBishop.png");
                            break;
                        case 3:
                            picDest.Image =
                                Image.FromFile(Application.StartupPath + "\\Images\\BlackKnight.png");
                            break;
                        case 4:
                            picDest.Image = Image.FromFile(Application.StartupPath + "\\Images\\BlackRook.png");
                            break;
                        case 5:
                            picDest.Image =
                                Image.FromFile(Application.StartupPath + "\\Images\\BlackQueen.png");
                            break;
                    }

                return;
            }
        }

        private static string GetSquareName(string strIndex)
        {
            var squareName = "";
            string[] colName = { "a", "b", "c", "d", "e", "f", "g", "h" };

            try
            {
                var index = int.Parse(strIndex);
                squareName = colName[index % 8] + (8 - index / 8);
            }
            catch (Exception)
            {
                // ignored
            }

            return squareName;
        }

        private void HideButtonPgnView()
        {
            btnNextMove.Enabled = false;
            btnPreviousMove.Enabled = false;
            btnFirstMove.Enabled = false;
            btnLastMove.Enabled = false;
        }

        private void SetIndexOfPgnfile(int index)
        {
            _indexOfMoveInPgnfile = -1;
            _realMoveListInPgn = new string[500];

            var temp = _listPgnFile[index];

            if (temp == null || temp.Trim().Equals("")) return;

            btnNextMove.Enabled = true;
            btnPreviousMove.Enabled = true;
            btnFirstMove.Enabled = true;
            btnLastMove.Enabled = true;

            temp = Regex.Replace(temp, "\\{.*?\\}", "");
            temp = Regex.Replace(temp, "\"", "");
            var startIndex = temp.IndexOf("White", StringComparison.Ordinal);
            var endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
            var whiteName = temp.Substring(startIndex + 5, endIndex - startIndex - 5);
            WhitePlayer.Text = whiteName;

            startIndex = temp.IndexOf("Black", StringComparison.Ordinal);
            endIndex = temp.IndexOf("]", startIndex, StringComparison.Ordinal);
            var blackName = temp.Substring(startIndex + 5, endIndex - startIndex - 5);
            BlackPlayer.Text = blackName;

            temp = temp.Substring(temp.LastIndexOf("]", StringComparison.Ordinal) + 1);

            var clean = Regex.Replace(temp, "(^\\d\\.? )|( \\d+\\.?)", "").Trim();
            clean = clean.Replace("\r", " ");
            clean = clean.Replace("    ", " ");
            clean = clean.Replace("   ", " ");
            clean = clean.Replace("  ", " ");
            clean = clean.Replace("*", "");
            _loadedPgnFile = Regex.Split(clean.Trim(), " ");
            AddPgnFileToGrid(_loadedPgnFile);
        }

        private void AddPgnFileToGrid(IList<string> file)
        {
            ClearPgnFileView();
            var i = 0;

            while (i < file.Count)
            {
                _ = GridViewPgnFile.Rows.Add();
                GridViewPgnFile.Rows[i / 2].Cells[0].Value = i / 2 + 1;
                GridViewPgnFile.Rows[i / 2].Cells[1].Value = file[i];

                if (i + 1 < file.Count) GridViewPgnFile.Rows[i / 2].Cells[2].Value = file[i + 1];

                i += 2;
            }

            GridViewPgnFile.Rows[0].Cells[1].Selected = true;
        }

        private void InitAll()
        {
            _whiteClockTick = 1;
            _blackClockTick = 1;
            _whiteClockTickForThinking = 0;
            _blackClockTickForThinking = 0;

            _totalTimeForWhiteForTick = _totalTimeForWhite;
            _totalTimeForBlackForTick = _totalTimeForBlack;

            //whiteTotalTimetxt.Text = _totalTimeForWhiteForTick.ToString();
            whiteTotalTimetxt.Text = (_totalTimeForWhiteForTick + @":" + _whiteClockTick / 10);

            //blackTotalTimetxt.Text = _totalTimeForBlackForTick.ToString();
            blackTotalTimetxt.Text = (_totalTimeForBlackForTick + @":" + _blackClockTick / 10);

            _guiEngineMove = "";
            _listPgnFile = new string[200];
            _lengthListPgnFile = 0;
            _previosCell = "";
            _presentCell = "";
            _engineThinking = false;
            Engine.Level(40, _totalTimeForWhite * 60, 0);
            _indexOfMoveInPgnfile = -1;
            _realMoveListInPgn = new string[500];
            _inGameProcessing = false;
            _startedLoadedGame = false;
            _engineWin = false;
            _humanWin = false;
            _sideToMove = 1;
        }

        private void mnNewGame_Click(object sender, EventArgs e)
        {
            if (_inGameProcessing && !_engineThinking)
            {
                var mess = MessageBox.Show(@"Save game?", @"Save game", MessageBoxButtons.YesNo);

                if (mess == DialogResult.Yes) SaveFilePgn();
            }

            _sideToMove = 1;
            _engineThinking = false;
            DrawBoard();
            LVPvLine.Items.Clear();
            InitAll();
            _e.Initialize();
            _startedNewGame = true;
            _startedLoadedGame = false;
            ClearPgnFileView();
            ChooseWhiteToolStripMenuItem.Checked = true;
            ChooseBlackToolStripMenuItem.Checked = false;
        }

        private void ClearPgnFileView()
        {
            if (GridViewPgnFile.Rows.Count == 0) _ = GridViewPgnFile.Rows.Add();

            GridViewPgnFile.RowCount = 1;
            GridViewPgnFile.Rows[0].Cells[0].Value = "";
            GridViewPgnFile.Rows[0].Cells[1].Value = "";
            GridViewPgnFile.Rows[0].Cells[2].Value = "";

            for (var i = 0; i <= 200; i++)
                _ = GridViewPgnFile.Rows.Add();
        }

        private void GetThinkerStatus()
        {
            while (true)
            {
                Thread.Sleep(1);

                if (!_engineThinking) continue;
                _engineThink = Engine.GuIgetEngineThink();
                _guiEngineMove = Engine.GuIgetEngineMove();
                _guipvLineString = Engine.GuIgetPvLineString();

                if (_engineThink) continue;
                _sideToMove = 3 - _sideToMove;
                _engineThinking = false;
                ChangePicForMove(_guiEngineMove, Engine.GuIgetMoveTypeEngine());

                if (Engine.GuIgetMoveTypeEngine() == 1)
                    switch (Engine.GuIgetMovePromotionValue())
                    {
                        case 2:
                            _guiEngineMove += "b";
                            break;
                        case 3:
                            _guiEngineMove += "n";
                            break;
                        case 4:
                            _guiEngineMove += "r";
                            break;
                        case 5:
                            _guiEngineMove += "q";
                            break;
                    }

                AddMoveToPgnViewFile(_guiEngineMove);

                if (Engine.GuIgetEngineWin() == 1) _ = MessageBox.Show(@"Checkmate");

                if (Engine.GuIgetEngineWin() != -1) continue;
                _ = MessageBox.Show(@"Checkmate");
                _humanWin = true;
                return;
            }
        }

        private void mnTimeOption_Click(object sender, EventArgs e)
        {
            var frmOptionTime = new OptionTime
            {
                PassTime = SetTime
            };

            frmOptionTime.Show();

            _whiteClockTick = 0;
            _blackClockTick = 0;
            _whiteClockTickForThinking = 0;
            _blackClockTickForThinking = 0;
        }

        private void SetTime(int value)
        {
            _totalTimeForWhite = value;
            _totalTimeForWhiteForTick = _totalTimeForWhite;
            whiteTotalTimetxt.Text = _totalTimeForWhite.ToString();

            _totalTimeForBlack = value;
            _totalTimeForBlackForTick = _totalTimeForBlack;
            blackTotalTimetxt.Text = _totalTimeForBlack.ToString();

            Engine.Level(40, _totalTimeForWhite * 60, 0);
        }

        private void MatchTimer_Tick(object sender, EventArgs e)
        {
            if (!_startedNewGame) return;

            if (_sideToMove == 1)
            {
                _whiteClockTick -= 1;
                _whiteClockTickForThinking += 1;
                whiteTotalTimetxt.Text = _totalTimeForWhiteForTick + @":" + _whiteClockTick / 10;
                whiteThinkingTimetxt.Text = (_whiteClockTickForThinking / 10).ToString();

                if (_whiteClockTick <= 0)
                {
                    _whiteClockTick = 600;
                    _totalTimeForWhiteForTick -= 1;
                }

                _blackClockTickForThinking = 0;
            }
            else
            {
                _blackClockTick -= 1;
                _blackClockTickForThinking += 1;
                blackTotalTimetxt.Text = _totalTimeForBlackForTick + @":" + _blackClockTick / 10;
                blackThinkingTimetxt.Text = (_blackClockTickForThinking / 10).ToString();

                if (_blackClockTick <= 0)
                {
                    _blackClockTick = 600;
                    _totalTimeForBlackForTick -= 1;
                }

                _whiteClockTickForThinking = 0;
            }

            if (!_engineThink) return;
            InsertPvLine(_guipvLineString);
            _guipvLineStringOld = _guipvLineString;
        }

        private void mnExitGame_Click(object sender, EventArgs e)
        {
            ActiveForm?.Close();
        }

        private void mnLoadGame_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (_e != null)
            {
                _sideToMove = 1;
                _engineThinking = false;
                Engine.ThinkOff();
            }

            DrawBoard();
            _e?.Initialize();
            InitAll();

            _startedLoadedGame = true;
            var sr = new StreamReader(ofd.FileName);
            var contentPgn = sr.ReadToEnd();
            var firstIndex = 0;
            contentPgn += "       ";
            var i = 0;

            _lengthListPgnFile = 0;
            _listPgnFile = new string[200];

            contentPgn = contentPgn.Replace("\n", " ");
            contentPgn = Regex.Replace(contentPgn, "\\{.*?\\}", "");

            while (i < contentPgn.Length - 9)
            {
                if (contentPgn.Substring(i, 4).Equals(" 1-0") || contentPgn.Substring(i, 4).Equals(" 0-1") ||
                    contentPgn.Substring(i, 8).Equals(" 1/2-1/2")
                    || contentPgn.Substring(i, 4).Equals("*\r \r") || contentPgn.Substring(i, 3).Equals("*\r\r"))
                {
                    _listPgnFile[_lengthListPgnFile] = contentPgn.Substring(firstIndex, i - firstIndex + 1);
                    _lengthListPgnFile++;
                    firstIndex = contentPgn.IndexOf(" ", i + 1, StringComparison.Ordinal);
                }

                i++;
            }

            sr.Close();

            var lp = new ListPgn
            {
                PassValue = SetIndexOfPgnfile
            };

            lp.Show();
            lp.LoadPgnFile(_listPgnFile);
        }

        private void GoNextMovePgn()
        {
            if (_inGameProcessing) return;

            if (_loadedPgnFile == null) return;

            if (_indexOfMoveInPgnfile < _loadedPgnFile.Length - 1)
                _indexOfMoveInPgnfile++;
            else
                return;

            var move = _loadedPgnFile[_indexOfMoveInPgnfile];

            if (move.Equals("*") || move.Equals("") || move.IndexOf(".", StringComparison.Ordinal) >= 0) return;

            _sideToMove = 3 - _sideToMove;
            var side = _indexOfMoveInPgnfile % 2 == 0 ? 1 : 2;
            FindValueInGridPgnView(move, _indexOfMoveInPgnfile);

            move = ConvertToStandardNotationMove(side, move);

            if (move.Equals("")) return;

            var emove = move;
            _realMoveListInPgn[_indexOfMoveInPgnfile] = move;
            var proValue = int.Parse(_hshTablePromotionValueInPgnFile[move].ToString());

            if (proValue != 0)
                switch (proValue)
                {
                    case 2:
                        emove += "b";
                        break;
                    case 3:
                        emove += "n";
                        break;
                    case 4:
                        emove += "r";
                        break;
                    case 5:
                        emove += "q";
                        break;
                }

            _ = Engine.UserMove(emove);
            ChangePicForMove(move, int.Parse(_hshTableMoveTypeInPgnFile[move].ToString()));
        }

        private void btnNextMove_Click(object sender, EventArgs e)
        {
            GoNextMovePgn();
        }

        private void GoPreviousMovePgn()
        {
            if (_inGameProcessing) return;

            if (_loadedPgnFile == null) return;

            if (_indexOfMoveInPgnfile < 0) return;

            if (_realMoveListInPgn[_indexOfMoveInPgnfile] == null ||
                _realMoveListInPgn[_indexOfMoveInPgnfile].Trim().Equals(""))
            {
                _indexOfMoveInPgnfile--;
                return;
            }

            var presentMove = _loadedPgnFile[_indexOfMoveInPgnfile];

            if (presentMove.Trim().Equals("")) return;

            _sideToMove = 3 - _sideToMove;
            Engine.UndoUserMove();
            UndoChangePicForMove(_realMoveListInPgn[_indexOfMoveInPgnfile], Engine.GuIgetMoveTypePgn(),
                Engine.GuIgetMoveFromSidePgn(), Engine.GuIgetMoveFromValuePgn(),
                Engine.GuIgetMoveToSidePgn(), Engine.GuIgetMoveToValuePgn());

            if (_indexOfMoveInPgnfile > 0)
                _indexOfMoveInPgnfile--;
            else
                return;

            var move = _loadedPgnFile[_indexOfMoveInPgnfile];
            FindValueInGridPgnView(move, _indexOfMoveInPgnfile);
        }

        private void btnPreviousMove_Click(object sender, EventArgs e)
        {
            GoPreviousMovePgn();
        }

        private void FindValueInGridPgnView(string move, int index)
        {
            ClearSelectedInGridViewPgn();

            for (var i = 0; i < GridViewPgnFile.Rows.Count; i++)
                if (i == index / 2)
                {
                    if (_indexOfMoveInPgnfile % 2 == 0 &&
                        GridViewPgnFile.Rows[i].Cells[1].Value.ToString().Trim().Equals(move))
                    {
                        GridViewPgnFile.Rows[i].Cells[1].Selected = true;
                        break;
                    }

                    if (_indexOfMoveInPgnfile % 2 == 0 ||
                        !GridViewPgnFile.Rows[i].Cells[2].Value.ToString().Trim().Equals(move)) continue;
                    GridViewPgnFile.Rows[i].Cells[2].Selected = true;
                    break;
                }
        }

        private static bool IsRealMove(string move)
        {
            if (move.Length != 4) return false;

            var s1 = move.Substring(0, 1);
            var s2 = move.Substring(1, 1);
            var s3 = move.Substring(2, 1);
            var s4 = move.Substring(3, 1);
            string[] colName = { "a", "b", "c", "d", "e", "f", "g", "h" };
            var colSet = new Hashtable();
            var rowSet = new Hashtable();

            for (var i = 1; i <= 8; i++)
            {
                colSet.Add(colName[i - 1], "1");
                rowSet.Add(i.ToString(), "1");
            }

            return colSet.ContainsKey(s1) && colSet.ContainsKey(s3) && rowSet.ContainsKey(s2) && rowSet.ContainsKey(s4);
        }

        private string ConvertToStandardNotationMove(int side, string move)
        {
            move = move.Trim();
            var realMove = "";

            if (IsRealMove(move)) realMove = move;

            if (move.Equals("*") || move.Equals("")) return "";

            if (move.EndsWith("+")) move = move.Substring(0, move.Length - 1);

            if (move.IndexOf("=", StringComparison.Ordinal) >= 0) move = move.Replace("=", "");

            _hshTableMoveTypeInPgnFile = new Hashtable();
            _hshTablePromotionValueInPgnFile = new Hashtable();
            var getAllMoveAble = Engine.GuIgetAllTheMoveFromSide(side, ref _hshTableMoveTypeInPgnFile,
                ref _hshTablePromotionValueInPgnFile);

            foreach (var entry in getAllMoveAble.Cast<DictionaryEntry>().Where(entry =>
                         move.Equals(entry.Key.ToString())
                         || realMove.Equals(entry.Value.ToString())))
                return entry.Value.ToString();

            _ = MessageBox.Show(@"");
            throw new Exception("");
        }

        private void btnLastMove_Click(object sender, EventArgs e)
        {
            if (_loadedPgnFile == null) return;

            Cursor = Cursors.WaitCursor;

            try
            {
                while (_indexOfMoveInPgnfile < _loadedPgnFile.Length - 1)
                    GoNextMovePgn();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.ToString());
            }

            Cursor = Cursors.Default;
        }

        private void ClearSelectedInGridViewPgn()
        {
            for (var i = 0; i < GridViewPgnFile.Rows.Count; i++)
                GridViewPgnFile.Rows[i].Selected = false;
        }

        private void btnFirstMove_Click(object sender, EventArgs e)
        {
            if (_loadedPgnFile == null) return;

            while (_indexOfMoveInPgnfile > 0)
                GoPreviousMovePgn();
        }

        private void ChooseWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_engineThinking) return;

            _engineSide = 2;
            _humanSide = 1;
            BlackPlayer.Text = @"Demon";
            WhitePlayer.Text = @"Human";
            _startedNewGame = true;
            ChooseWhiteToolStripMenuItem.Checked = true;
            ChooseBlackToolStripMenuItem.Checked = false;

            if (_sideToMove != _engineSide) return;
            _engineThinking = true;
            Engine.Level(40, _totalTimeForBlackForTick * 60 + _blackClockTick / 10, 0);
            Engine.ChangeSideToBlack();
            _e.ThinkOn();
        }

        private void ChooseBlackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_engineThinking) return;

            _engineSide = 1;
            _humanSide = 2;
            BlackPlayer.Text = @"Human";
            WhitePlayer.Text = @"Demon";
            _startedNewGame = true;
            ChooseBlackToolStripMenuItem.Checked = true;
            ChooseWhiteToolStripMenuItem.Checked = false;

            if (_sideToMove != _engineSide) return;
            _engineThinking = true;
            Engine.Level(40, _totalTimeForWhiteForTick * 60 + _whiteClockTick / 10, 0);
            Engine.ChangeSideToWhite();
            _e.ThinkOn();
        }

        private void mnIntroHelp_Click(object sender, EventArgs e)
        {
            var ab = new About();
            ab.Show();
        }

        private void SaveFilePgn()
        {
            const string fileName = "games";
            var result = "";
            result += "[Event \"DemonChess Game\"]\n";
            result += "[Date \"" + DateTime.Now + "\"]\n";
            result += "[White \"" + WhitePlayer.Text + "\"]\n";
            result += "[Black \"" + BlackPlayer.Text + "\"]\n";

            if (_engineSide == 1)
            {
                if (Engine.GuIgetEngineWin() == 1 || _engineWin)
                    result += "[Result \"1-0\"]\n";
                else if (Engine.GuIgetEngineWin() == 0)
                    result += "[Result \"*\"]\n";
                else if (Engine.GuIgetEngineWin() == -1 || _humanWin) result += "[Result \"0-1\"]\n";
            }
            else
            {
                if (Engine.GuIgetEngineWin() == 1 || _engineWin)
                    result += "[Result \"0-1\"]\n";
                else if (Engine.GuIgetEngineWin() == 0)
                    result += "[Result \"*\"]\n";
                else if (Engine.GuIgetEngineWin() == -1 || _humanWin) result += "[Result \"1-0\"]\n";
            }

            for (var i = 0; i <= GridViewPgnFile.RowCount; i++)
                if ((GridViewPgnFile.Rows[i].Cells[0].Value != null &&
                     !GridViewPgnFile.Rows[i].Cells[0].Value.ToString().Equals("")) ||
                    (GridViewPgnFile.Rows[i].Cells[1].Value != null &&
                     !GridViewPgnFile.Rows[i].Cells[1].Value.ToString().Equals("")))
                {
                    result += i + 1 + ". " + GridViewPgnFile.Rows[i].Cells[1].Value + " " +
                              GridViewPgnFile.Rows[i].Cells[2].Value + " ";

                    if ((i + 1) % 6 == 0) result += "\n";
                }
                else
                {
                    break;
                }

            if (_engineSide == 1)
            {
                if (Engine.GuIgetEngineWin() == 1) result += "1-0";

                if (Engine.GuIgetEngineWin() == 0) result += "*";

                if (Engine.GuIgetEngineWin() == -1) result += "0-1";
            }
            else
            {
                if (Engine.GuIgetEngineWin() == 1) result += "0-1";

                if (Engine.GuIgetEngineWin() == 0) result += "*";

                if (Engine.GuIgetEngineWin() == -1) result += "1-0";
            }

            result += "\n";
            var sw = new StreamWriter(fileName + ".pgn", true);
            sw.WriteLine(result);
            sw.Close();
        }

        private void mnSaveGame_Click(object sender, EventArgs e)
        {
            SaveFilePgn();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_inGameProcessing && !_engineThinking)
            {
                var mess = MessageBox.Show(@"Save game?", @"Save game", MessageBoxButtons.YesNo);

                if (mess == DialogResult.Yes) SaveFilePgn();
            }

            if (_myStatusThinker.IsAlive) _myStatusThinker.Abort();

            _e.Dispose();
        }

        private void mnResign_Click(object sender, EventArgs e)
        {
            _engineWin = true;
        }

        private void mnUseOpeningBook_Click(object sender, EventArgs e)
        {
            if (mnUseOpeningBook.Checked)
            {
                mnUseOpeningBook.Checked = false;
                Engine.GuIsetUseOpeningBook(false);
            }
            else
            {
                mnUseOpeningBook.Checked = true;
                Engine.GuIsetUseOpeningBook(true);
            }
        }

        private void mnHelpPlayer_Click(object sender, EventArgs e)
        {
            _ = MessageBox.Show(
                @"To make a move: click once on the source (from) square & once on the destination (to) square",
                @"Guide");
        }

        private void mnLevel_Click(object sender, EventArgs e)
        {
            var ol = new OptionLevel
            {
                PassLevel = SetLevelDepth
            };

            if (_maxDepthLevel != 64) ol.SetLevelMaxDepth(_maxDepthLevel);

            ol.Show();
        }

        private static void SetLevelDepth(int val)
        {
            Engine.GuiSetMaxDepth(val);
            _maxDepthLevel = val;
        }

        private void CmdThinker_Click(object sender, EventArgs e)
        {
            if (_engineThinking) return;

            if (_sideToMove == 1)
            {
                _engineSide = 1;
                _humanSide = 2;
                Engine.ChangeSideToWhite();
            }
            else
            {
                _engineSide = 2;
                _humanSide = 1;
                Engine.ChangeSideToBlack();
            }

            _startedNewGame = true;
            ChooseWhiteToolStripMenuItem.Checked = false;
            ChooseBlackToolStripMenuItem.Checked = false;

            if (_sideToMove != _engineSide) return;
            _engineThinking = true;
            Engine.Level(40, _totalTimeForBlackForTick * 60 + _blackClockTick / 10, 0);
            _e.ThinkOn();
        }

        private void GridViewPgnFile_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void UserPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void LVPvLine_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void WhiteTotalTimetxt_TextChanged(object sender, EventArgs e)
        {
        }

        private void blackThinkingTimetxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void BoardPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}