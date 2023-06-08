using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DemonChess
{
    internal class Engine
    {
        private const int HashSize = 204857;

        private const int CastleNone = 0;
        private const int CastleShort = 1;
        private const int CastleLong = 2;
        private const int CastleBoth = 3;
        private const int NodeAll = -1;
        private const int NodePv = 0;
        private const int NodeCut = 1;
        private const int OpenGame = 0;
        private const int MiddleGame = 1;
        private const int EndGame = 2;
        private const int EndGamePawn = 3;

        private const int KnightUnit = 4;
        private const int BishopUnit = 6;
        private const int RookUnit = 7;
        private const int QueenUnit = 13;

        private const int HashfExact = 0;
        private const int HashfAlpha = 1;
        private const int HashfBeta = 2;
        private const int AttackKqr = 1;
        private const int AttackQr = 2;
        private const int AttackKqBwP = 3;
        private const int AttackKqBbP = 4;
        private const int AttackQb = 5;
        private const int AttackN = 6;
        private const int TimeNodesCheck = 10000;

        private const int KingAttackDistance = 40;
        private const int KnightMobOpening = 4;
        private const int KnightMobEndgame = 4;
        private const int BishopMobOpening = 5;
        private const int BishopMobEndgame = 5;
        private const int RookMobOpening = 2;
        private const int RookMobEndgame = 4;
        private const int QueenMobOpening = 1;
        private const int QueenMobEndgame = 2;

        private const int TrappedBishop = 200;

        private const int BlockedBishop = 10;
        private const int BlockedRook = 50;
        private const int RookOpenFile = 20;
        private const int RookOnSeventRank = 20;
        private static string _guiEngineMove = "";
        private static string _guiThinkerPvLineString = "";
        private static int _guiMoveTypeEngine;
        private static int _guiMoveTypeHuman;
        private static int _guiMoveTypePgn;
        private static int _guiMoveFromSidePgn;
        private static int _guiMoveFromValuePgn;
        private static int _guiMoveToSidePgn;
        private static int _guiMoveToValuePgn;
        private static int _guiMaxLevelDepth = 64;
        private static int _guiMovePromotionValue;
        private static int _guiEngineWin;

        private static int[] _arrSide = new int[200];
        private static int[] _arrValue = new int[200];
        private static int[] _arrPiecePossition = new int[200];
        private static int[,,,] _pst = new int[3, 7, 128, 4];

        private static int[] _arrPieceEvaluate = new int[7];

        private static Hashtable _kingRookMovedList = new Hashtable(200);
        private static int[] _pawnWhiteEvaluate = new int[128];
        private static int[] _pawnBlackEvaluate = new int[128];
        private static int[] _pawnEvaluateEnding = new int[128];
        private static int[] _bishopWhiteEvaluate = new int[128];
        private static int[] _bishopBlackEvaluate = new int[128];
        private static int[] _bishopEvaluateEnding = new int[128];
        private static int[] _knightWhiteEvaluate = new int[128];
        private static int[] _knightBlackEvaluate = new int[128];
        private static int[] _knightEvaluateEnding = new int[128];
        private static int[] _queenWhiteEvaluate = new int[128];
        private static int[] _queenBlackEvaluate = new int[128];
        private static int[] _queenEvaluateEnding = new int[128];
        private static int[] _kingWhiteEvaluate = new int[128];
        private static int[] _kingBlackEvaluate = new int[128];
        private static bool _isFirstKingWhiteMove = true;
        private static bool _isFirstKingBlackMove = true;
        private static bool _isFirstRookBlackRightMove = true;
        private static bool _isFirstRookBlackLeftMove = true;
        private static bool _isFirstRookWhiteLeftMove = true;
        private static bool _isFirstRookWhiteRightMove = true;
        private static TagHashe[] _arrZobristKey = new TagHashe[HashSize];
        private static int _age;
        private static bool _isOverTime;
        private static int _engineSide = 2;
        private static MoveList[,] _pvLine = new MoveList[256, 256];
        private static int[] _pvLength = new int[256];
        private static int _pvPly;
        private static int _maxDepth = 20;
        private static long _countNodes;

        private static readonly int[] BishopDelta = { -15, -17, 15, 17, 0, 0, 0, 0 };
        private static readonly int[] RookDelta = { -1, -16, 1, 16, 0, 0, 0, 0 };
        private static readonly int[] QueenDelta = { -15, -17, 15, 17, -1, -16, 1, 16 };
        private static readonly int[] KingDelta = { -15, -17, 15, 17, -1, -16, 1, 16 };
        private static readonly int[] KnightDelta = { 18, 33, 31, 14, -31, -33, -18, -14 };
        private static readonly int[] WpawnDelta = { -16, -32, -17, -15, 0, 0, 0, 0 };
        private static readonly int[] BpawnDelta = { 16, 32, 17, 15, 0, 0, 0, 0 };

        private static readonly int[] AttackArray =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
            0, 0, 0, 5, 0, 0, 5, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 5, 0,
            0, 0, 0, 5, 0, 0, 0, 0, 2, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0,
            5, 0, 0, 0, 2, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0,
            2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 6, 2, 6, 5, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 4, 1, 4, 6, 0, 0, 0, 0, 0,
            0, 2, 2, 2, 2, 2, 2, 1, 0, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0,
            0, 0, 6, 3, 1, 3, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 6,
            2, 6, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 2, 0, 0, 5,
            0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 2, 0, 0, 0, 5, 0, 0, 0,
            0, 0, 0, 5, 0, 0, 0, 0, 2, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0,
            0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 5, 0, 0, 5, 0, 0, 0, 0, 0, 0,
            2, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        private static Piece[] _arrPiece = new Piece[33];
        private static MoveList[] _arrMoveList = new MoveList[400];
        private static MoveList[] _arrMainMoveList = new MoveList[400];
        private static readonly MoveList[] ArrBestMoveList = new MoveList[400];
        private static MoveList[] _primeKillerMoveList = new MoveList[256];
        private static MoveList[] _secondKillerMoveList = new MoveList[256];

        private static int _countMainMoveList = -1;
        private static ulong[] _moveHashCode = new ulong[400];
        private static string _strMainMoveList = "";
        private static string[] _strOpeningBook = new string[50000];
        private static bool _useOpeningBook = true;
        private static int _countOpenbookRow;
        private static long _startTimeThinking;
        private static long _timePerMove = 1500000000000000;
        private static int _nextTimeCheck = TimeNodesCheck;
        private static int _rDepthNull = 3;
        private static int _whiteCastle;
        private static int _blackCastle;
        private static int _enPassant = -1;
        private static int _fifty;
        private static HistoryStruc[] _historyDat = new HistoryStruc[400];
        private static int _countHistoryDat;
        private static int[,] _historyMoveOrder = new int[127, 127];
        private static readonly int[,] HistoryHit = new int[127, 127];
        private static readonly int[,] HistoryTot = new int[127, 127];
        private static int _globalBestMove;

        private static ulong[] _wCastlingRights;
        private static ulong[] _bCastlingRights;
        private static ulong[] _enPasant;
        private static ulong[,,] _zobristPieces;
        private static ulong _zobrist;
        private static ulong[] _zobristSide;
        private static ulong _hashZobrist;

        private static Global _glb;
        private static PosVal _psval;
        private static string _lineSearchDepth = "";
        private static bool _engineThink;
        private static bool _myThinking;

        private static int _phase;
        private static int _gamePhase;
        private static int _phaseMix;
        private static int _scorce;
        private static int _scorceEnding;
        private static int _scorceB;
        private static int _scorceEndingB;

        private static readonly int[] KingAttackUnit = { 0, 0, 1, 1, 2, 4, 0 };

        private static readonly int[] KingAttackWeight =
        {
            0, 0, 128, 192, 224, 240, 248, 252, 254, 255, 256, 256, 256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
            256, 256, 256, 256, 256, 256, 256, 256, 256
        };

        private static readonly int[] PawnSameFile = { 10, 10, 20, 20 };
        private static readonly int[] PawnIsolated = { 10, 10, 20, 20 };

        private static int _totalAtackMyKing, _totalAtackOpKing, _countTotalAtackMyKing, _countTotalAtackOpKing;
        private static int _toMyKingIndex, _toOpKingIndex;
        private static int[,] _pawnRank = new int[3, 12];
        private static readonly int[] PassedPawnBonus = { 10, 20, 20, 30 };
        private readonly Thread _myThinker;

        private int _bestFromIndex;
        private int _bestFromPiecePossition;
        private int _bestFromSide;
        private int _bestFromValue;
        private int _bestToIndex;
        private int _bestToPiecePossition;
        private int _bestToValue;
        private bool _myForce;

        public Engine()
        {
            InitAll();
            InitOpenBook();
            InitZobrist();
            _hashZobrist = GetZobrist(1);
            _myThinking = false;
            _myThinker = new Thread(Thinker);
            _myThinker.Start();
        }

        public void Initialize()
        {
            ThinkOff();
            _myForce = false;
            InitAll();
            ClearHastable();
            _hashZobrist = GetZobrist(1);
        }

        private void InitAll()
        {
            InitVariable();
            InitPiece();
            ClearHastable();
            InitOpenBook();
        }

        private static void InitOpenBook()
        {
            try
            {
                _countOpenbookRow = 0;
                var sr = new StreamReader("opening.txt");

                while (!sr.EndOfStream)
                {
                    var strTemp = sr.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(strTemp)) continue;
                    _strOpeningBook[_countOpenbookRow] = strTemp;
                    _countOpenbookRow++;
                }

                sr.Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static string FindMoveInOpenBook()
        {
            var move = "";
            var rnd = new Random();

            if (_strMainMoveList.Trim() == "")
            {
                var indexOpening = rnd.Next(_countOpenbookRow);
                move = _strOpeningBook[indexOpening].Substring(0, 4);

                if (IsSquare(move) && IsSquare(move.Substring(2))) return move;
            }
            else
            {
                var i = 0;
                var j = 0;
                var tempOpenBook = new string[_countOpenbookRow];

                while (i < _countOpenbookRow)
                {
                    if (_strOpeningBook[i].StartsWith(_strMainMoveList) &&
                        _strOpeningBook[i].Length > _strMainMoveList.Trim().Length)
                    {
                        tempOpenBook[j] = _strOpeningBook[i];
                        j++;
                    }

                    i++;
                }

                if (j <= 0) return move;
                var k = rnd.Next(j);

                if (!tempOpenBook[k].StartsWith(_strMainMoveList) ||
                    tempOpenBook[k].Length <= _strMainMoveList.Trim().Length) return move;

                move = tempOpenBook[k].Substring(_strMainMoveList.Length, 4);

                return IsSquare(move) && IsSquare(move.Substring(2))
                    ? move
                    : throw new Exception("Error in Open Book, Found but not a move ");
            }

            return move;
        }

        public void Dispose()
        {
            _myThinker.Abort();
        }

        public static void ChangeSideToWhite()
        {
            _engineSide = 1;
        }

        public static void ChangeSideToBlack()
        {
            _engineSide = 2;
        }

        public static void Level(int moves, int seconds, int increment)
        {
            if (increment != 0)
            {
                _timePerMove = seconds * 10000000;
                return;
            }

            if (moves == 0) moves = 40;

            SetTime(seconds * 1000 / moves);
        }

        private static void SetTime(int milliseconds)
        {
            _timePerMove = (long)milliseconds * 10000;

            if (_timePerMove > 10000000) _timePerMove += 20000000;
        }

        private void InitVariable()
        {
            ThinkOff();
            _arrMainMoveList = new MoveList[400];
            _strMainMoveList = "";
            _countMainMoveList = -1;
            _strOpeningBook = new string[50000];
            _countOpenbookRow = 0;
            _rDepthNull = 3;
            _moveHashCode = new ulong[400];
            _myForce = false;
            _engineThink = false;
            _enPassant = -1;
            _fifty = 0;

            _whiteCastle = CastleBoth;
            _blackCastle = CastleBoth;

            _kingRookMovedList = new Hashtable(200);
            _isFirstKingBlackMove = true;
            _isFirstKingWhiteMove = true;
            _isFirstRookBlackLeftMove = true;
            _isFirstRookBlackRightMove = true;
            _isFirstRookWhiteLeftMove = true;
            _isFirstRookWhiteRightMove = true;

            _arrSide = new int[200];
            _arrValue = new int[200];
            _arrPiecePossition = new int[200];

            _glb = new Global();
            _arrSide = _glb.ArrSide;
            _arrValue = _glb.ArrValue;
            _arrPiecePossition = _glb.ArrPiecePossition;

            _arrPieceEvaluate = _glb.ArrPieceEvaluate;
            _pawnWhiteEvaluate = _glb.PawnWhiteEvaluate;
            _pawnBlackEvaluate = _glb.PawnBlackEvaluate;
            _pawnEvaluateEnding = _glb.PawnEvaluateEnding;

            _bishopWhiteEvaluate = _glb.BishopWhiteEvaluate;
            _bishopBlackEvaluate = _glb.BishopBlackEvaluate;
            _bishopEvaluateEnding = _glb.BishopEvaluateEnding;

            _knightWhiteEvaluate = _glb.KnightWhiteEvaluate;
            _knightBlackEvaluate = _glb.KnightBlackEvaluate;
            _knightEvaluateEnding = _glb.KnightEvaluateEnding;

            _queenWhiteEvaluate = _glb.QueenWhiteEvaluate;
            _queenBlackEvaluate = _glb.QueenBlackEvaluate;
            _queenEvaluateEnding = _glb.QueenEvaluateEnding;

            _kingWhiteEvaluate = _glb.KingWhiteEvaluate;
            _kingBlackEvaluate = _glb.KingBlackEvaluate;

            _primeKillerMoveList = new MoveList[256];
            _secondKillerMoveList = new MoveList[256];
            _arrMoveList = new MoveList[400];
            _age = 0;
            _historyDat = new HistoryStruc[400];
            _countHistoryDat = 0;
            _enPassant = -1;

            _psval = new PosVal();
            _psval.InitPst();
            _pst = _psval.Pst;

            _pvLine = new MoveList[256, 256];
            _pvLength = new int[256];

            _useOpeningBook = true;
            _guiMaxLevelDepth = 64;
        }

        private static void InitPiece()
        {
            _arrPiece = new Piece[33];
            _arrPiece[1].Index = 0;
            _arrPiece[1].Side = 2;
            _arrPiece[1].Value = 4;

            _arrPiece[2].Index = 1;
            _arrPiece[2].Side = 2;
            _arrPiece[2].Value = 3;

            _arrPiece[3].Index = 2;
            _arrPiece[3].Side = 2;
            _arrPiece[3].Value = 2;

            _arrPiece[4].Index = 3;
            _arrPiece[4].Side = 2;
            _arrPiece[4].Value = 5;

            _arrPiece[5].Index = 4;
            _arrPiece[5].Side = 2;
            _arrPiece[5].Value = 6;

            _arrPiece[6].Index = 5;
            _arrPiece[6].Side = 2;
            _arrPiece[6].Value = 2;

            _arrPiece[7].Index = 6;
            _arrPiece[7].Side = 2;
            _arrPiece[7].Value = 3;

            _arrPiece[8].Index = 7;
            _arrPiece[8].Side = 2;
            _arrPiece[8].Value = 4;

            for (var i = 1; i < 9; i++)
            {
                _arrPiece[i + 8].Index = 15 + i;
                _arrPiece[i + 8].Side = 2;
                _arrPiece[i + 8].Value = 1;
            }

            _arrPiece[17].Index = 112;
            _arrPiece[17].Side = 1;
            _arrPiece[17].Value = 4;

            _arrPiece[18].Index = 113;
            _arrPiece[18].Side = 1;
            _arrPiece[18].Value = 3;

            _arrPiece[19].Index = 114;
            _arrPiece[19].Side = 1;
            _arrPiece[19].Value = 2;

            _arrPiece[20].Index = 115;
            _arrPiece[20].Side = 1;
            _arrPiece[20].Value = 5;

            _arrPiece[21].Index = 116;
            _arrPiece[21].Side = 1;
            _arrPiece[21].Value = 6;

            _arrPiece[22].Index = 117;
            _arrPiece[22].Side = 1;
            _arrPiece[22].Value = 2;

            _arrPiece[23].Index = 118;
            _arrPiece[23].Side = 1;
            _arrPiece[23].Value = 3;

            _arrPiece[24].Index = 119;
            _arrPiece[24].Side = 1;
            _arrPiece[24].Value = 4;

            for (var i = 1; i < 9; i++)
            {
                _arrPiece[i + 24].Index = 95 + i;
                _arrPiece[i + 24].Side = 1;
                _arrPiece[i + 24].Value = 1;
            }

            for (var i = 1; i < 33; i++)
                _arrPiece[i].Status = 0;
        }

        private static void InitZobrist()
        {
            var rnd = new Random((int)17L);
            _wCastlingRights = new ulong[4];
            _bCastlingRights = new ulong[4];
            _zobristSide = new ulong[2];

            for (var i = 0; i < 4; i++)
            {
                _wCastlingRights[i] = (ulong)Math.Abs(rnd.Next() ^ ((long)rnd.Next() << 15) ^
                                                      ((long)rnd.Next() << 30) ^ ((long)rnd.Next() << 45) ^
                                                      ((long)rnd.Next() << 60));

                _bCastlingRights[i] = (ulong)Math.Abs(rnd.Next() ^ ((long)rnd.Next() << 15) ^
                                                      ((long)rnd.Next() << 30) ^ ((long)rnd.Next() << 45) ^
                                                      ((long)rnd.Next() << 60));
            }

            for (var i = 0; i < 2; i++)
                _zobristSide[i] = (ulong)Math.Abs(rnd.Next() ^ ((long)rnd.Next() << 15) ^ ((long)rnd.Next() << 30) ^
                                                  ((long)rnd.Next() << 45) ^ ((long)rnd.Next() << 60));

            _enPasant = new ulong[128];
            _zobristPieces = new ulong[6, 2, 128];

            for (var square = 0; square <= 127; square++)
                if ((square & 0x88) == 0)
                {
                    _enPasant[square] = (ulong)Math.Abs(rnd.Next() ^ ((long)rnd.Next() << 15) ^
                                                        ((long)rnd.Next() << 30) ^ ((long)rnd.Next() << 45) ^
                                                        ((long)rnd.Next() << 60));

                    for (var piece = 0; piece < 6; piece++)
                    {
                        _zobristPieces[piece, 0, square] = (ulong)Math.Abs(rnd.Next() ^ ((long)rnd.Next() << 15) ^
                                                                           ((long)rnd.Next() << 30) ^
                                                                           ((long)rnd.Next() << 45) ^
                                                                           ((long)rnd.Next() << 60));

                        _zobristPieces[piece, 1, square] = (ulong)Math.Abs(rnd.Next() ^ ((long)rnd.Next() << 15) ^
                                                                           ((long)rnd.Next() << 30) ^
                                                                           ((long)rnd.Next() << 45) ^
                                                                           ((long)rnd.Next() << 60));
                    }
                }

            _zobrist = 0;

            for (var i = 1; i < 33; i++)
                _zobrist ^= _zobristPieces[_arrPiece[i].Value - 1, _arrPiece[i].Side - 1, _arrPiece[i].Index];

            _whiteCastle = CastleBoth;
            _blackCastle = CastleBoth;
        }

        public void ThinkOn()
        {
            if (_myForce) return;

            if (_myThinking) return;

            _engineThink = true;
        }

        public static void ThinkOff()
        {
            if (!_myThinking) return;

            _engineThink = false;

            while (_myThinking)
                Thread.Sleep(1);
        }

        private static void ClearHastable()
        {
            _arrZobristKey = new TagHashe[HashSize];
        }

        private void Thinker()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1);

                    if (!_engineThink) continue;
                    _guiEngineMove = "";
                    var moveOpenBook = FindMoveInOpenBook();

                    if (!_useOpeningBook) moveOpenBook = "";

                    if (moveOpenBook.Trim() != "")
                    {
                        DoMove(moveOpenBook);
                        _engineThink = false;
                    }
                    else
                    {
                        _myThinking = true;
                        _startTimeThinking = DateTime.Now.Ticks;

                        _countNodes = 0;

                        var allMovesAble = new MoveList[200];
                        var fromI = 1;
                        var toI = 17;

                        if (_engineSide == 1)
                        {
                            fromI = 17;
                            toI = 33;
                        }

                        _isOverTime = false;
                        var totalMovesAble = 0;

                        if (IsInCheck(_engineSide))
                        {
                            var arrAttackKingIndex = new int[30];

                            for (var i = 0; i < arrAttackKingIndex.Length; i++)
                                arrAttackKingIndex[i] = -1;

                            var countAttacking = CountAllPieceCanAttackKing(_engineSide, ref arrAttackKingIndex);

                            if (countAttacking > 0)
                            {
                                if (countAttacking == 1)
                                    GenStopCheckEvasion(_engineSide, ref totalMovesAble, ref allMovesAble,
                                        ref arrAttackKingIndex);

                                GenKingEvasion(_engineSide, ref totalMovesAble, ref allMovesAble);
                            }
                        }
                        else
                        {
                            for (var i = fromI; i < toI; i++)
                                if (_arrPiece[i].Status == 0 && _arrPiece[i].Side == _engineSide)
                                {
                                    GenerateCaptureMoves(i, _arrPiece[i].Side, _arrPiece[i].Value,
                                        _arrPiece[i].Index, ref allMovesAble, ref totalMovesAble);

                                    GenerateMoves(i, _arrPiece[i].Side, _arrPiece[i].Value, _arrPiece[i].Index,
                                        ref allMovesAble, ref totalMovesAble);
                                }
                        }

                        _historyMoveOrder = new int[127, 127];

                        for (var i = 0; i < 127; i++)
                        for (var j = 0; j < 127; j++)
                        {
                            HistoryHit[i, j] = 1;
                            HistoryTot[i, j] = 1;
                        }

                        _arrMoveList = new MoveList[400];
                        OrderMove(ref allMovesAble, 1, totalMovesAble);

                        _primeKillerMoveList = new MoveList[256];
                        _secondKillerMoveList = new MoveList[256];

                        _globalBestMove = 0;

                        _phase = GetPhaseGame();
                        _gamePhase = GetPhaseGame();

                        const int alpha = -32000;
                        const int beta = -alpha;
                        _age++;

                        if (_age > 100) _age = 0;

                        _pvPly = 0;
                        _pvLine = new MoveList[256, 256];
                        _guiThinkerPvLineString = "";
                        _guiEngineWin = 0;

                        for (_maxDepth = 1; _maxDepth <= _guiMaxLevelDepth && _engineThink; _maxDepth++)
                        {
                            _nextTimeCheck = TimeNodesCheck;

                            var legalMoves = 0;
                            SearchRoot(alpha, beta, ref allMovesAble, totalMovesAble, ref legalMoves);
                            _isOverTime = _isOverTime && _engineThink;

                            if (_isOverTime) break;

                            if (legalMoves == 1) break;
                        }

                        _bestFromIndex = ArrBestMoveList[1].FromIndex;
                        _bestToIndex = ArrBestMoveList[1].ToIndex;
                        _bestFromSide = ArrBestMoveList[1].FromSide;
                        _bestFromValue = ArrBestMoveList[1].FromValue;
                        _bestToValue = ArrBestMoveList[1].ToValue;
                        _bestFromPiecePossition = ArrBestMoveList[1].FromPiecePossition;
                        _bestToPiecePossition = ArrBestMoveList[1].ToPiecePossition;

                        var mMoveType = IsMoveAble(_bestFromIndex, _bestToIndex, _bestFromSide, _bestFromValue);
                        _guiMoveTypeEngine = mMoveType;
                        _guiMovePromotionValue = ArrBestMoveList[1].PromotionValue;
                        _guiEngineWin = 0;

                        if (ArrBestMoveList[1].Evaluate > 30000) _guiEngineWin = 1;

                        if (ArrBestMoveList[1].Evaluate < -30000 && ArrBestMoveList[1].Evaluate != alpha)
                            _guiEngineWin = -1;

                        if (mMoveType < 0)
                        {
                            WriteAllVariableToCheck();

                            throw new Exception("Invalid move " + ToBoardString(ArrBestMoveList[1].FromIndex) +
                                                ToBoardString(ArrBestMoveList[1].ToIndex) + " move type=" +
                                                mMoveType);
                        }

                        MakeMove(mMoveType, ArrBestMoveList[1].PromotionValue, _bestFromSide, _bestFromValue,
                            _bestToValue, _bestFromIndex, _bestToIndex, _bestFromPiecePossition,
                            _bestToPiecePossition);

                        if (!_kingRookMovedList.ContainsKey(_bestFromPiecePossition))
                            _kingRookMovedList.Add(_bestFromPiecePossition, "1");

                        _countMainMoveList++;
                        _arrMainMoveList[_countMainMoveList] = ArrBestMoveList[1];
                        _moveHashCode[_countMainMoveList] = GetZobrist(3 - _bestFromSide);

                        _strMainMoveList += ToBoardString(ArrBestMoveList[1].FromIndex) +
                                            ToBoardString(ArrBestMoveList[1].ToIndex);

                        _guiEngineMove = ToBoardString(ArrBestMoveList[1].FromIndex) +
                                         ToBoardString(ArrBestMoveList[1].ToIndex);

                        _engineThink = false;
                        _myThinking = false;
                        _arrMoveList[1] = ArrBestMoveList[1];
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("Thread", StringComparison.Ordinal) < 0)
                    LogFile(ex + "- message: " + ex.Message + " -  source: " + ex.Source);
            }
        }

        private static bool IsDraw(int ply, int side)
        {
            var hash = GetZobrist(side);
            _ = ply - 1 - _historyDat[ply].Fitty;
            var i = _countHistoryDat - 2;

            while (i >= 4)
            {
                if (_historyDat[i].Zobrist == hash) return true;

                i -= 2;
            }

            return false;
        }

        private static int Full_new_depth(int depth, MoveList move)
        {
            var newDepth = depth - 1;
            var isRank7Th = move.FromValue == 1 && move.FromSide == 1 && move.ToIndex <= 23 && move.ToIndex >= 16;

            isRank7Th = isRank7Th ||
                        (move.FromValue == 1 && move.FromSide == 2 && move.ToIndex >= 96 && move.ToIndex <= 103);

            var isPromotion = move.MoveType == 1;

            if (isPromotion || isRank7Th) newDepth++;

            return newDepth;
        }

        private void SearchRoot(int alpha, int beta, ref MoveList[] allMovesAble, int totalMovesAble,
            ref int legalMoves)
        {
            var bestEval = int.MinValue;
            OrderMove(ref allMovesAble, 1, totalMovesAble);

            var oldAlpha = alpha;

            _arrMoveList = new MoveList[400];

            for (var i = 1; i <= totalMovesAble; i++)
            {
                _arrMoveList[1] = allMovesAble[i];

                MakeMove(allMovesAble[i].MoveType, allMovesAble[i].PromotionValue, allMovesAble[i].FromSide,
                    allMovesAble[i].FromValue, allMovesAble[i].ToValue,
                    allMovesAble[i].FromIndex, allMovesAble[i].ToIndex, allMovesAble[i].FromPiecePossition,
                    allMovesAble[i].ToPiecePossition);

                _moveHashCode[_countMainMoveList + 1] = GetZobrist(_engineSide);
                var iic = IsInCheck(_engineSide);

                if (iic)
                {
                    allMovesAble[i].Evaluate = -32000;

                    UnMakeMove(allMovesAble[i].MoveType, allMovesAble[i].FromSide,
                        allMovesAble[i].FromValue, allMovesAble[i].ToSide, allMovesAble[i].ToValue,
                        allMovesAble[i].FromIndex, allMovesAble[i].ToIndex, allMovesAble[i].FromPiecePossition,
                        allMovesAble[i].ToPiecePossition);

                    continue;
                }

                legalMoves++;
                var oldCountNodes = _countNodes;
                var detailStartTime = DateTime.Now.Ticks;

                var newDepth = _maxDepth - 1;
                int eval;

                if (i <= 1)
                {
                    eval = -AlphaBeta(-beta, -alpha, newDepth, 3 - _engineSide, false, NodePv, 2);
                }
                else
                {
                    eval = -AlphaBeta(-alpha - 1, -alpha, newDepth, 3 - _engineSide, false, NodeCut, 2);

                    if (eval > alpha) eval = -AlphaBeta(-beta, -alpha, newDepth, 3 - _engineSide, false, NodePv, 2);
                }

                _ = _countNodes - oldCountNodes;

                UnMakeMove(allMovesAble[i].MoveType, allMovesAble[i].FromSide,
                    allMovesAble[i].FromValue, allMovesAble[i].ToSide, allMovesAble[i].ToValue,
                    allMovesAble[i].FromIndex, allMovesAble[i].ToIndex, allMovesAble[i].FromPiecePossition,
                    allMovesAble[i].ToPiecePossition);

                _ = (DateTime.Now.Ticks - detailStartTime) / 10000;

                allMovesAble[i].Evaluate = eval <= alpha ? oldAlpha : eval >= beta ? beta : eval;

                _isOverTime = DateTime.Now.Ticks - _startTimeThinking > _timePerMove;

                if (_isOverTime) break;

                if (eval <= bestEval) continue;

                if (eval >= beta)
                {
                    if (!_isOverTime) RecordHash(_engineSide, _maxDepth, allMovesAble[i], eval, HashfBeta);

                    return;
                }

                bestEval = eval;
                _globalBestMove = i;

                if (eval <= alpha) continue;
                ArrBestMoveList[1] = allMovesAble[_globalBestMove];
                ArrBestMoveList[1].Evaluate = eval;
                alpha = eval;
                var finishDepthTime = (DateTime.Now.Ticks - _startTimeThinking) / 10000;

                _lineSearchDepth = ToBoardString(ArrBestMoveList[1].FromIndex) +
                                   ToBoardString(ArrBestMoveList[1].ToIndex) + " ";

                for (var i1 = 0; i1 < _pvLength[1]; i1++)
                    if (_pvLine[1, i1].FromSide != 0)
                        _lineSearchDepth += ToBoardStringFromMove(_pvLine[1, i1]) + " ";

                _lineSearchDepth = _maxDepth + "," + eval + "," + finishDepthTime + "," + _countNodes + "," +
                                   _lineSearchDepth;

                _guiThinkerPvLineString = _lineSearchDepth + "\r\n" + _guiThinkerPvLineString;
            }
        }

        private static ulong GetZobrist(int side)
        {
            ulong nZobrist = 0;
            var i = 0;

            try
            {
                for (i = 1; i < 33; i++)
                    if (_arrPiece[i].Status == 0)
                        nZobrist ^= _zobristPieces[_arrPiece[i].Value - 1, _arrPiece[i].Side - 1, _arrPiece[i].Index];

                nZobrist ^= _zobristSide[side - 1];
            }
            catch (Exception)
            {
                StreamWriter sw = new StreamWriter("getZobrist.txt");

                sw.WriteLine("side" + side + " i=" + i + " value:" + _arrPiece[i].Value + " sidePiece:" +
                             _arrPiece[i].Side + " index:" + _arrPiece[i].Index);

                sw.Close();
            }

            return nZobrist;
        }

        private static int ProbeHash(int side, int depth, ref MoveList hashMove, int alpha, int beta)
        {
            const int result = int.MinValue;

            _zobrist = GetZobrist(side);
            var hashkey = (int)(_zobrist % HashSize);

            if (_arrZobristKey[hashkey].Key != _zobrist) return result;
            var typeHash = _arrZobristKey[hashkey].HashType;
            hashMove = _arrZobristKey[hashkey].BestMove;

            if (depth > _arrZobristKey[hashkey].Depth) return result;

            switch (typeHash)
            {
                case HashfExact:
                    return _arrZobristKey[hashkey].Val;
                case HashfAlpha when _arrZobristKey[hashkey].Val <= alpha:
                    return _arrZobristKey[hashkey].Val;
                case HashfBeta when _arrZobristKey[hashkey].Val >= beta:
                    return _arrZobristKey[hashkey].Val;
                default:
                    return result;
            }
        }

        private static void RecordHash(int side, int depth, MoveList bestMove, int value, int hashType)
        {
            _zobrist = GetZobrist(side);
            var hashkey = (int)(_zobrist % HashSize);

            if (_arrZobristKey[hashkey].Depth > depth && _arrZobristKey[hashkey].Key != 0) return;
            _arrZobristKey[hashkey].Val = value;
            _arrZobristKey[hashkey].BestMove = bestMove;
            _arrZobristKey[hashkey].Depth = depth;
            _arrZobristKey[hashkey].HashType = hashType;
            _arrZobristKey[hashkey].SideToMove = side;
            _arrZobristKey[hashkey].Key = _zobrist;
        }

        private static bool IsGoodCap(MoveList move)
        {
            switch (move.MoveType)
            {
                case 2:
                case 1:
                    return true;
            }

            if (move.ToValue == 0) return See(move, 0, move.ToIndex, move.FromSide) >= 0;
            if (move.MoveType == 1) return true;

            if (move.FromValue != 6 && _arrPieceEvaluate[move.ToValue] >= _arrPieceEvaluate[move.FromValue])
                return true;

            return See(move, 0, move.ToIndex, move.FromSide) >= 0;
        }

        private static void GenerateStateMoves(int side, int ply, int state, ref int totalMovesAble,
            ref MoveList[] allMovesAble, ref StateMove[] arrStateMove, MoveList hashMove)
        {
            var fromIndex = 1;
            var toIndex = 17;

            if (side == 1)
            {
                fromIndex = 17;
                toIndex = 33;
            }

            int i;

            switch (state)
            {
                case 0:
                    if (hashMove.FromPiecePossition != 0 && hashMove.FromSide == side &&
                        ValidateHashMove(hashMove) >= 0)
                    {
                        totalMovesAble = 1;
                        allMovesAble[1] = hashMove;
                        allMovesAble[1].Evaluate = 20000;
                        arrStateMove[0].From = 1;
                        arrStateMove[0].To = 1;
                    }

                    break;
                case 1:
                    arrStateMove[1].From = totalMovesAble + 1;
                    for (i = fromIndex; i < toIndex; i++)
                        if (_arrPiece[i].Status == 0 && _arrPiece[i].Side == side)
                            GenerateCaptureMoves(i, side, _arrPiece[i].Value, _arrPiece[i].Index, ref allMovesAble,
                                ref totalMovesAble);

                    for (i = arrStateMove[1].From; i <= totalMovesAble; i++)
                    {
                        if (IsTheSameMove(hashMove, allMovesAble[i]))
                        {
                            allMovesAble[i].Evaluate = -100000;
                            continue;
                        }

                        switch (allMovesAble[i].MoveType)
                        {
                            case 1:
                                allMovesAble[i].Evaluate = 100000;
                                continue;
                            case 2:
                                allMovesAble[i].Evaluate = 90000;
                                continue;
                            default:
                                allMovesAble[i].Evaluate = IsGoodCap(allMovesAble[i])
                                    ? _arrPieceEvaluate[allMovesAble[i].ToValue] * 6 -
                                    _arrPieceEvaluate[allMovesAble[i].FromValue] + 500
                                    : _arrPieceEvaluate[allMovesAble[i].ToValue] * 6 -
                                      _arrPieceEvaluate[allMovesAble[i].FromValue] - 100000;
                                break;
                        }
                    }

                    OrderMove(ref allMovesAble, arrStateMove[1].From, totalMovesAble);
                    var found = false;
                    for (i = arrStateMove[1].From; i <= totalMovesAble; i++)
                        if (allMovesAble[i].Evaluate < 0)
                        {
                            arrStateMove[1].To = i - 1;
                            arrStateMove[4].From = i;
                            arrStateMove[4].To = totalMovesAble;
                            found = true;
                            break;
                        }

                    if (!found)
                    {
                        arrStateMove[1].To = totalMovesAble;
                        arrStateMove[4].From = 1;
                        arrStateMove[4].To = -1;
                    }

                    break;
                case 2:
                    arrStateMove[2].From = totalMovesAble + 1;
                    if (!IsTheSameMove(hashMove, _primeKillerMoveList[ply]) &&
                        ValidateKiller(_primeKillerMoveList[ply]) >= 0)
                    {
                        totalMovesAble++;
                        allMovesAble[totalMovesAble] = _primeKillerMoveList[ply];
                        allMovesAble[totalMovesAble].Evaluate = 5000;
                    }

                    if (!IsTheSameMove(hashMove, _secondKillerMoveList[ply]) &&
                        ValidateKiller(_secondKillerMoveList[ply]) >= 0)
                    {
                        totalMovesAble++;
                        allMovesAble[totalMovesAble] = _secondKillerMoveList[ply];
                        allMovesAble[totalMovesAble].Evaluate = 4000;
                    }

                    arrStateMove[2].To = totalMovesAble;
                    break;
                case 3:
                    arrStateMove[3].From = totalMovesAble + 1;
                    for (i = fromIndex; i < toIndex; i++)
                        if (_arrPiece[i].Status == 0 && _arrPiece[i].Side == side)
                            GenerateMoves(i, side, _arrPiece[i].Value, _arrPiece[i].Index, ref allMovesAble,
                                ref totalMovesAble);

                    for (i = arrStateMove[3].From; i <= totalMovesAble; i++)
                    {
                        if (IsTheSameMove(hashMove, allMovesAble[i]))
                        {
                            allMovesAble[i].Evaluate = -100000;
                            continue;
                        }

                        if (IsTheSameMove(_primeKillerMoveList[ply], allMovesAble[i]))
                        {
                            allMovesAble[i].Evaluate = -100000;
                            continue;
                        }

                        if (IsTheSameMove(_secondKillerMoveList[ply], allMovesAble[i]))
                        {
                            allMovesAble[i].Evaluate = -100000;
                            continue;
                        }

                        allMovesAble[i].Evaluate =
                            _historyMoveOrder[allMovesAble[i].FromIndex, allMovesAble[i].ToIndex];
                    }

                    OrderMove(ref allMovesAble, arrStateMove[3].From, totalMovesAble);
                    arrStateMove[3].To = totalMovesAble;
                    break;
            }
        }

        private static void GenKingEvasion(int side, ref int totalMovesAble, ref MoveList[] allMovesAble)
        {
            var delta = KingDelta;
            var kingIndex = side == 1 ? _arrPiece[21].Index : _arrPiece[5].Index;
            var kingPiece = side == 1 ? 21 : 5;

            foreach (var t in delta)
            {
                var toIndex = kingIndex + t;

                if ((toIndex & 0x88) == 0 && _arrSide[toIndex] != side)
                    CreateMoves(kingPiece, 0, 0, side, 6, _arrSide[toIndex], _arrValue[toIndex], kingIndex, toIndex,
                        ref allMovesAble, ref totalMovesAble);
            }
        }

        private static void GenStopCheckEvasion(int side, ref int totalMovesAble, ref MoveList[] allMovesAble,
            ref int[] arrAttackKingIndex)
        {
            var from = 1;
            var to = 17;

            if (side == 1)
            {
                from = 17;
                to = 33;
            }

            var delta = BishopDelta;
            int i;

            for (i = from; i < to; i++)
                if (_arrPiece[i].Status == 0 && _arrPiece[i].Side == side)
                {
                    var index = _arrPiece[i].Index;
                    var value = _arrPiece[i].Value;

                    if (value == 1 && side == 1) delta = WpawnDelta;

                    switch (value)
                    {
                        case 1 when side == 2:
                            delta = BpawnDelta;
                            break;
                        case 2:
                            delta = BishopDelta;
                            break;
                        case 3:
                            delta = KnightDelta;
                            break;
                        case 4:
                            delta = RookDelta;
                            break;
                        case 5:
                            delta = QueenDelta;
                            break;
                    }

                    int j;

                    for (j = 0; j < arrAttackKingIndex.Length; j++)
                        if (arrAttackKingIndex[j] != -1)
                        {
                            int k;

                            for (k = 0; k < delta.Length; k++)
                                if (delta[k] != 0)
                                {
                                    if (value == 2 || value == 4 || value == 5)
                                    {
                                        var fromIndex = index + delta[k];

                                        while ((fromIndex & 0x88) == 0)
                                        {
                                            if (_arrSide[fromIndex] == side) break;

                                            if (fromIndex == arrAttackKingIndex[j] && _arrSide[fromIndex] != side)
                                            {
                                                CreateMoves(i, 0, 0, side, value, _arrSide[fromIndex],
                                                    _arrValue[fromIndex], index, fromIndex, ref allMovesAble,
                                                    ref totalMovesAble);

                                                break;
                                            }

                                            if (_arrSide[fromIndex] != side && _arrSide[fromIndex] != 0) break;

                                            fromIndex += delta[k];
                                        }
                                    }

                                    if (value != 1 && value != 3) continue;

                                    {
                                        var fromIndex = index + delta[k];

                                        if ((fromIndex & 0x88) == 0 && _arrSide[fromIndex] != side &&
                                            fromIndex == arrAttackKingIndex[j] && Math.Abs(delta[k]) % 16 == 0 &&
                                            value == 1)
                                        {
                                            if (Math.Abs(delta[k]) == 16 && _arrSide[fromIndex] == 0)
                                                CreateMoves(i, 0, 0, side, value, _arrSide[fromIndex],
                                                    _arrValue[fromIndex], index, fromIndex, ref allMovesAble,
                                                    ref totalMovesAble);

                                            if (Math.Abs(delta[k]) == 32 && _arrSide[fromIndex] == 0)
                                            {
                                                var tempFromIndex = index + delta[k] / 2;

                                                if ((tempFromIndex & 0x88) == 0 && _arrSide[tempFromIndex] == 0)
                                                    CreateMoves(i, 0, 0, side, value, _arrSide[fromIndex],
                                                        _arrValue[fromIndex], index, fromIndex, ref allMovesAble,
                                                        ref totalMovesAble);
                                            }
                                        }

                                        if ((fromIndex & 0x88) == 0 && _arrSide[fromIndex] != side &&
                                            _arrSide[fromIndex] != 0 && fromIndex == arrAttackKingIndex[j] &&
                                            Math.Abs(delta[k]) % 16 != 0 && value == 1)
                                        {
                                            if ((side == 1 && fromIndex < 16) || (side == 2 && fromIndex >= 112))
                                                for (byte kkk = 2; kkk <= 5; kkk++)
                                                    CreateMoves(i, 1, kkk, side, value, _arrSide[fromIndex],
                                                        _arrValue[fromIndex], index, fromIndex, ref allMovesAble,
                                                        ref totalMovesAble);
                                            else
                                                CreateMoves(i, 0, 0, side, value, _arrSide[fromIndex],
                                                    _arrValue[fromIndex], index, fromIndex, ref allMovesAble,
                                                    ref totalMovesAble);
                                        }

                                        if ((fromIndex & 0x88) == 0 && fromIndex == arrAttackKingIndex[j] &&
                                            _arrSide[fromIndex] != side && value == 3)
                                            CreateMoves(i, 0, 0, side, value, _arrSide[fromIndex], _arrValue[fromIndex],
                                                index, fromIndex, ref allMovesAble, ref totalMovesAble);
                                    }
                                }
                        }
                }
        }

        private static void GenerateStateMovesQuiesSearch(int side, int state, ref int totalMovesAble,
            ref MoveList[] allMovesAble, ref StateMove[] arrStateMove)
        {
            var fromIndex = 1;
            var toIndex = 17;

            if (side == 1)
            {
                fromIndex = 17;
                toIndex = 33;
            }

            switch (state)
            {
                case 0:
                    break;
                case 1:
                    arrStateMove[1].From = totalMovesAble + 1;
                    arrStateMove[1].To = -1;
                    int i;
                    for (i = fromIndex; i < toIndex; i++)
                        if (_arrPiece[i].Status == 0 && _arrPiece[i].Side == side)
                            GenerateCaptureMoves(i, side, _arrPiece[i].Value, _arrPiece[i].Index,
                                ref allMovesAble,
                                ref totalMovesAble);

                    for (i = arrStateMove[1].From; i <= totalMovesAble; i++)
                        switch (allMovesAble[i].MoveType)
                        {
                            case 1:
                                allMovesAble[i].Evaluate = 100000;
                                continue;
                            case 2:
                                allMovesAble[i].Evaluate = 90000;
                                continue;
                            default:
                                allMovesAble[i].Evaluate = IsGoodCap(allMovesAble[i])
                                    ? _arrPieceEvaluate[allMovesAble[i].ToValue] * 6 -
                                    _arrPieceEvaluate[allMovesAble[i].FromValue] + 500
                                    : -100000;
                                break;
                        }

                    OrderMove(ref allMovesAble, arrStateMove[1].From, totalMovesAble);
                    arrStateMove[1].To = totalMovesAble;
                    break;
                case 2:
                    break;
            }
        }

        private static int CountAllPieceCanAttackKing(int side, ref int[] arrAttackKingIndex)
        {
            var count = 0;
            var lengthArrAttackKingIndex = 0;
            var kingIndex = side == 1 ? _arrPiece[21].Index : _arrPiece[5].Index;
            var delta = QueenDelta;
            int i;

            for (i = 0; i < delta.Length; i++)
                if (delta[i] != 0)
                {
                    var fromIndex = kingIndex + delta[i];
                    var found = false;

                    while ((fromIndex & 0x88) == 0)
                    {
                        if (_arrSide[fromIndex] == side) break;

                        if (_arrSide[fromIndex] != 0 && _arrSide[fromIndex] != side && _arrValue[fromIndex] != 1 &&
                            _arrValue[fromIndex] != 6 && _arrValue[fromIndex] != 3 &&
                            IsAttackedPseudoSquareBySquare(kingIndex, fromIndex, _arrSide[fromIndex],
                                _arrValue[fromIndex]))
                        {
                            arrAttackKingIndex[lengthArrAttackKingIndex] = fromIndex;
                            count++;
                            lengthArrAttackKingIndex++;
                            found = true;
                        }

                        if (found)
                        {
                            var tempFromIndex = fromIndex - delta[i];

                            while ((tempFromIndex & 0x88) == 0 && tempFromIndex != kingIndex &&
                                   _arrSide[tempFromIndex] == 0)
                            {
                                arrAttackKingIndex[lengthArrAttackKingIndex] = tempFromIndex;
                                lengthArrAttackKingIndex++;
                                tempFromIndex -= delta[i];
                            }

                            break;
                        }

                        if (_arrSide[fromIndex] != side && _arrSide[fromIndex] != 0) break;

                        fromIndex += delta[i];
                    }
                }

            delta = KnightDelta;

            for (i = 0; i < delta.Length; i++)
                if (delta[i] != 0)
                {
                    var fromIndex = kingIndex + delta[i];

                    if ((fromIndex & 0x88) != 0) continue;
                    if (_arrSide[fromIndex] == 0 || _arrSide[fromIndex] == side || _arrValue[fromIndex] != 3) continue;
                    arrAttackKingIndex[lengthArrAttackKingIndex] = fromIndex;
                    count++;
                    lengthArrAttackKingIndex++;
                }

            delta = side == 1 ? WpawnDelta : BpawnDelta;

            for (i = 2; i <= 3; i++)
                if (delta[i] != 0)
                {
                    var fromIndex = kingIndex + delta[i];

                    if ((fromIndex & 0x88) != 0) continue;
                    if (_arrSide[fromIndex] == 0 || _arrSide[fromIndex] == side || _arrValue[fromIndex] != 1) continue;
                    arrAttackKingIndex[lengthArrAttackKingIndex] = fromIndex;
                    count++;
                    lengthArrAttackKingIndex++;
                }

            return count;
        }

        private static int AlphaBeta(int alpha, int beta, int depth, int side, bool isNullMove, int nodeType, int ply)
        {
            var bestMove = new MoveList();
            var hashf = HashfAlpha;
            var hashMove = new MoveList();

            _countNodes += 1;
            _nextTimeCheck--;

            if (_nextTimeCheck <= 0 || _isOverTime)
            {
                _nextTimeCheck = TimeNodesCheck;

                if (_isOverTime || DateTime.Now.Ticks - _startTimeThinking > _timePerMove)
                {
                    _isOverTime = true;
                    return 0;
                }
            }

            int val;

            if (depth <= 0)
            {
                val = QuiesSearch(alpha, beta, side, ply);

                return val;
            }

            _pvLength[_pvPly] = _pvPly;

            if (IsDraw(ply, side)) return 0;

            var tempVal = ProbeHash(side, depth, ref hashMove, alpha, beta);
            if (tempVal != int.MinValue) return tempVal;

            if (ply >= 63) return Evaluate(side);

            var iic1 = IsInCheck(side);
            var mateThreat = false;
            int bestValue;

            if (!iic1 && isNullMove && _gamePhase != EndGamePawn && depth >= 2)
            {
                _rDepthNull = depth > 6 ? 3 : 2;

                MakeNullMove(side, 0, 0);

                var valNull = -AlphaBeta(-beta, -beta + 1, depth - 1 - _rDepthNull, 3 - side, false, -nodeType,
                    ply + 1);

                UnMakeNullMove();

                if (valNull >= beta)
                {
                    if (!_isOverTime) RecordHash(side, depth, hashMove, valNull, HashfBeta);

                    bestValue = valNull;
                    return bestValue;
                }

                if (valNull <= -31000) mateThreat = true;
            }

            if (_isOverTime) return 0;

            bestValue = int.MinValue;
            var allMovesAble = new MoveList[100];

            var iidDepth = depth - 2;

            if (hashMove.FromPiecePossition == 0 && depth >= 3)
            {
                _ = AlphaBeta(alpha, beta, iidDepth, side, false, NodeCut, ply);
                hashMove = _pvLine[ply, ply];
            }

            var atleastOneLegal = false;
            var arrStateMove = new StateMove[5];

            var searchedMoves = 0;
            var statusGenerateMove = 0;

            var totalMovesAble = 0;

            while (statusGenerateMove <= 4)
            {
                GenerateStateMoves(side, ply, statusGenerateMove, ref totalMovesAble, ref allMovesAble,
                    ref arrStateMove, hashMove);

                for (var j = arrStateMove[statusGenerateMove].From; j <= arrStateMove[statusGenerateMove].To; j++)
                    if (allMovesAble[j].FromSide != 0)
                    {
                        if (allMovesAble[j].Evaluate == -100000) continue;

                        _arrMoveList[ply] = allMovesAble[j];

                        var mMoveType = allMovesAble[j].MoveType;
                        var mPromotionValue = allMovesAble[j].PromotionValue;
                        var mFromSide = allMovesAble[j].FromSide;
                        var mFromValue = allMovesAble[j].FromValue;
                        var mToSide = allMovesAble[j].ToSide;
                        var mToValue = allMovesAble[j].ToValue;
                        var mFromIndex = allMovesAble[j].FromIndex;
                        var mToIndex = allMovesAble[j].ToIndex;
                        var mFromPiecePossition = allMovesAble[j].FromPiecePossition;
                        var mToPiecePossition = allMovesAble[j].ToPiecePossition;

                        MakeMove(mMoveType, mPromotionValue, mFromSide, mFromValue, mToValue,
                            mFromIndex, mToIndex, mFromPiecePossition, mToPiecePossition);

                        iic1 = IsInCheck(side);

                        if (iic1)
                        {
                            UnMakeMove(mMoveType, mFromSide, mFromValue, mToSide, mToValue,
                                mFromIndex, mToIndex, mFromPiecePossition, mToPiecePossition);

                            continue;
                        }

                        var extend = Full_new_depth(depth, allMovesAble[j]);

                        if (extend == depth - 1)
                        {
                            extend = 0;
                            iic1 = IsInCheck(3 - side);

                            if (iic1) extend = 1;
                        }
                        else
                        {
                            extend = 1;
                        }

                        if (mateThreat) extend = 1;

                        var newDepth = depth - 1 + extend;

                        atleastOneLegal = true;

                        if (searchedMoves <= 0)
                        {
                            val = -AlphaBeta(-beta, -alpha, newDepth, 3 - side, true, NodePv, ply + 1);
                        }
                        else
                        {
                            val = depth >= 2 && !iic1 && extend == 0 && allMovesAble[j].ToValue == 0 &&
                                  allMovesAble[j].MoveType != 2 && searchedMoves >= 3 && statusGenerateMove >= 3
                                ? -AlphaBeta(-alpha - 1, -alpha, depth - 2, 3 - side, true, NodeCut, ply + 1)
                                : alpha + 1;

                            if (val > alpha)
                            {
                                val = -AlphaBeta(-alpha - 1, -alpha, newDepth, 3 - side, true, NodeCut, ply + 1);

                                if (val > alpha)
                                    val = -AlphaBeta(-beta, -alpha, newDepth, 3 - side, true, NodeCut, ply + 1);
                            }
                        }

                        searchedMoves++;

                        UnMakeMove(mMoveType, mFromSide, mFromValue, mToSide, mToValue,
                            mFromIndex, mToIndex, mFromPiecePossition, mToPiecePossition);

                        if (val > bestValue)
                        {
                            bestValue = val;
                            bestMove = allMovesAble[j];

                            if (val > alpha)
                            {
                                alpha = val;
                                hashf = HashfExact;
                                _pvLine[_pvPly, _pvPly] = allMovesAble[j];

                                for (var k = _pvPly + 1; k < _pvLength[_pvPly + 1]; k++)
                                    _pvLine[_pvPly, k] = _pvLine[_pvPly + 1, k];

                                _pvLength[_pvPly] = _pvLength[_pvPly + 1];

                                if (val >= beta)
                                {
                                    if (!_isOverTime) RecordHash(side, depth, allMovesAble[j], val, HashfBeta);

                                    if (allMovesAble[j].ToValue != 0 || allMovesAble[j].MoveType == 2) return val;

                                    if (!IsTheSameMove(_primeKillerMoveList[ply], allMovesAble[j]))
                                    {
                                        _secondKillerMoveList[ply] = _primeKillerMoveList[ply];
                                        _primeKillerMoveList[ply] = allMovesAble[j];
                                    }

                                    _historyMoveOrder[allMovesAble[j].FromIndex, allMovesAble[j].ToIndex] +=
                                        depth * depth;

                                    return val;
                                }
                            }
                        }

                        if (nodeType == NodeCut) nodeType = NodeAll;
                    }

                statusGenerateMove++;
            }

            if (!atleastOneLegal) return IsInCheck(side) ? -31999 + ply : 0;

            if (!_isOverTime) RecordHash(side, depth, bestMove, bestValue, hashf);

            return bestValue;
        }

        private static int QuiesSearch(int alpha, int beta, int side, int depth)
        {
            _pvLength[_pvPly] = _pvPly;

            if (depth >= 63) return Evaluate(side);

            var val = Evaluate(side);
            var bestValue = val;

            if (val >= beta) return bestValue;

            if (val > alpha) alpha = val;

            var allMovesAble = new MoveList[40];
            var totalMovesAble = 0;

            const int stateGenMove = 1;
            var arrStateMove = new StateMove[3];
            var searchedMoves = 0;

            GenerateStateMovesQuiesSearch(side, stateGenMove, ref totalMovesAble, ref allMovesAble,
                ref arrStateMove);

            for (var j = arrStateMove[stateGenMove].From; j <= arrStateMove[stateGenMove].To; j++)
                if (allMovesAble[j].FromSide != 0)
                {
                    if (allMovesAble[j].Evaluate == -100000) continue;

                    _countNodes++;
                    _arrMoveList[depth] = allMovesAble[j];

                    var mMoveType = allMovesAble[j].MoveType;
                    var mPromotionValue = allMovesAble[j].PromotionValue;
                    var mFromSide = allMovesAble[j].FromSide;
                    var mFromValue = allMovesAble[j].FromValue;
                    var mToSide = allMovesAble[j].ToSide;
                    var mToValue = allMovesAble[j].ToValue;
                    var mFromIndex = allMovesAble[j].FromIndex;
                    var mToIndex = allMovesAble[j].ToIndex;
                    var mFromPiecePossition = allMovesAble[j].FromPiecePossition;
                    var mToPiecePossition = allMovesAble[j].ToPiecePossition;

                    MakeMove(mMoveType, mPromotionValue, mFromSide, mFromValue, mToValue, mFromIndex,
                        mToIndex, mFromPiecePossition, mToPiecePossition);

                    var iic1 = IsInCheck(side);

                    if (iic1)
                    {
                        UnMakeMove(mMoveType, mFromSide, mFromValue, mToSide, mToValue,
                            mFromIndex, mToIndex, mFromPiecePossition, mToPiecePossition);

                        continue;
                    }

                    val = -QuiesSearch(-beta, -alpha, 3 - side, depth + 1);

                    UnMakeMove(mMoveType, mFromSide, mFromValue, mToSide, mToValue, mFromIndex,
                        mToIndex, mFromPiecePossition, mToPiecePossition);

                    searchedMoves++;

                    if (val <= bestValue) continue;
                    bestValue = val;

                    if (val <= alpha) continue;
                    alpha = val;
                    _pvLine[_pvPly, _pvPly] = allMovesAble[j];

                    for (var k = _pvPly + 1; k < _pvLength[_pvPly + 1]; k++)
                        _pvLine[_pvPly, k] = _pvLine[_pvPly + 1, k];

                    _pvLength[_pvPly] = _pvLength[_pvPly + 1];

                    if (val >= beta) return val;
                }

            if (searchedMoves != 0) return bestValue;

            if (IsInCheck(side))
                return -32000 - depth;

            return bestValue;
        }

        private static int GetPhaseGame()
        {
            _phaseMix = 0;
            int i;

            for (i = 1; i <= 16; i++)
                if (_arrPiece[i].Status == 0 && _arrPiece[i].Value != 6)
                    switch (_arrPiece[i].Value)
                    {
                        case 4:
                            _phaseMix += 2;
                            break;
                        case 5:
                            _phaseMix += 4;
                            break;
                        default:
                        {
                            if (_arrPiece[i].Value != 1) _phaseMix += 1;
                            break;
                        }
                    }

            for (i = 17; i <= 32; i++)
                if (_arrPiece[i].Status == 0 && _arrPiece[i].Value != 6)
                    switch (_arrPiece[i].Value)
                    {
                        case 4:
                            _phaseMix += 2;
                            break;
                        case 5:
                            _phaseMix += 4;
                            break;
                        default:
                        {
                            if (_arrPiece[i].Value != 1) _phaseMix += 1;
                            break;
                        }
                    }

            return _phaseMix == 0 ? EndGamePawn : _phaseMix >= 22 ? OpenGame : _phaseMix >= 10 ? MiddleGame : EndGame;
        }

        private static int Evaluate(int side)
        {
            _totalAtackMyKing = 0;
            _totalAtackOpKing = 0;
            _countTotalAtackMyKing = 0;
            _countTotalAtackOpKing = 0;
            _toMyKingIndex = _arrPiece[5].Index;
            _toOpKingIndex = _arrPiece[21].Index;

            if (side == 1)
            {
                _toMyKingIndex = _arrPiece[21].Index;
                _toOpKingIndex = _arrPiece[5].Index;
            }
            else
            {
                _toMyKingIndex = _arrPiece[5].Index;
                _toOpKingIndex = _arrPiece[21].Index;
            }

            var sumA = 0;
            var sumA1 = 0;
            var sumA4 = 0;
            var sumA5 = 0;
            var sumAPos = 0;

            var sumB = 0;
            var sumB1 = 0;
            var sumB4 = 0;
            var sumB5 = 0;
            var sumBPos = 0;
            _phase = GetPhaseGame();
            _scorce = 0;
            _scorceB = 0;
            _scorceEnding = 0;
            _scorceEndingB = 0;
            var hasPawn = false;
            _pawnRank = new int[3, 12];

            int ii;

            for (ii = 0; ii <= 11; ii++)
            {
                _pawnRank[1, ii] = 0;
                _pawnRank[2, ii] = 7;
            }

            for (ii = 1; ii < 33; ii++)
                if (_arrPiece[ii].Value == 1 && _arrPiece[ii].Status == 0)
                {
                    var r = _arrPiece[ii].Index % 16 + 1;

                    if (_arrPiece[ii].Side == 1)
                    {
                        if (_arrPiece[ii].Index / 16 > _pawnRank[_arrPiece[ii].Side, r])
                            _pawnRank[_arrPiece[ii].Side, r] = _arrPiece[ii].Index / 16;
                    }
                    else
                    {
                        if (_arrPiece[ii].Index / 16 < _pawnRank[_arrPiece[ii].Side, r])
                            _pawnRank[_arrPiece[ii].Side, r] = _arrPiece[ii].Index / 16;
                    }
                }

            for (ii = 1; ii < 33; ii++)
                if (_arrPiece[ii].Status == 0)
                {
                    int temp;

                    if (_arrPiece[ii].Side == side)
                    {
                        _arrPiece[ii].AttackKing = 0;
                        _arrPiece[ii].DefenceKing = 0;
                        sumA += _arrPieceEvaluate[_arrPiece[ii].Value];
                        _scorce += _arrPieceEvaluate[_arrPiece[ii].Value];
                        _scorceEnding += _arrPieceEvaluate[_arrPiece[ii].Value];

                        temp = GetEvaluatePiece(_arrPiece[ii].Index, _arrPiece[ii].Side, _arrPiece[ii].Value,
                            ref _scorce,
                            ref _scorceEnding);

                        sumAPos += temp;

                        sumA1 += GetAllValueBelongPieceFromIndex(_arrPiece[ii].Side, _arrPiece[ii].Value,
                            _arrPiece[ii].Index, ref _scorce, ref _scorceEnding);

                        if (_arrPiece[ii].Value >= 2 && _arrPiece[ii].Value <= 5)
                        {
                            var attackingMark = TotalAtackKing(_arrPiece[ii].Side, _arrPiece[ii].Value,
                                _arrPiece[ii].Index, _toOpKingIndex);

                            if (attackingMark > 0)
                            {
                                _totalAtackOpKing += attackingMark;
                                _countTotalAtackOpKing++;
                            }
                        }

                        if (_arrPiece[ii].Value >= 2 && _arrPiece[ii].Value <= 5)
                            _ = KingAttackDistance /
                                (DistanceToTheKing(_arrPiece[ii].Side, _arrPiece[ii].Index) +
                                 1);

                        if (_arrPiece[ii].Value == 6)
                        {
                            sumA4 = SumKingSafeMark(_arrPiece[ii].Side, _arrPiece[ii].Index);
                            _scorce += sumA4;
                        }

                        if (_arrPiece[ii].Value != 1) continue;
                        hasPawn = true;

                        if (_arrPiece[ii].Side == 1)
                            sumA5 += Eval_light_pawn(_arrPiece[ii].Side, _arrPiece[ii].Index, ref _scorce,
                                ref _scorceEnding);
                        else
                            sumA5 += Eval_dark_pawn(_arrPiece[ii].Side, _arrPiece[ii].Index, ref _scorce,
                                ref _scorceEnding);
                    }
                    else
                    {
                        _arrPiece[ii].AttackKing = 0;
                        _arrPiece[ii].DefenceKing = 0;
                        sumB += _arrPieceEvaluate[_arrPiece[ii].Value];
                        _scorceB += _arrPieceEvaluate[_arrPiece[ii].Value];
                        _scorceEndingB += _arrPieceEvaluate[_arrPiece[ii].Value];

                        temp = GetEvaluatePiece(_arrPiece[ii].Index, _arrPiece[ii].Side, _arrPiece[ii].Value,
                            ref _scorceB,
                            ref _scorceEndingB);

                        sumBPos += temp;

                        sumB1 += GetAllValueBelongPieceFromIndex(_arrPiece[ii].Side, _arrPiece[ii].Value,
                            _arrPiece[ii].Index, ref _scorceB, ref _scorceEndingB);

                        if (_arrPiece[ii].Value >= 2 && _arrPiece[ii].Value <= 5)
                        {
                            var attackingMark = TotalAtackKing(_arrPiece[ii].Side, _arrPiece[ii].Value,
                                _arrPiece[ii].Index, _toMyKingIndex);

                            if (attackingMark > 0)
                            {
                                _totalAtackMyKing += attackingMark;
                                _countTotalAtackMyKing++;
                            }
                        }

                        if (_arrPiece[ii].Value >= 2 && _arrPiece[ii].Value <= 5)
                            _ = KingAttackDistance /
                                (DistanceToTheKing(_arrPiece[ii].Side, _arrPiece[ii].Index) +
                                 1);

                        if (_arrPiece[ii].Value == 6)
                        {
                            sumB4 = SumKingSafeMark(_arrPiece[ii].Side, _arrPiece[ii].Index);
                            _scorceB += sumB4;
                        }

                        if (_arrPiece[ii].Value != 1) continue;
                        hasPawn = true;

                        if (_arrPiece[ii].Side == 1)
                            sumB5 += Eval_light_pawn(_arrPiece[ii].Side, _arrPiece[ii].Index, ref _scorceB,
                                ref _scorceEndingB);
                        else
                            sumB5 += Eval_dark_pawn(_arrPiece[ii].Side, _arrPiece[ii].Index, ref _scorceB,
                                ref _scorceEndingB);
                    }
                }

            _ = sumA - sumB;

            if (Math.Abs(sumA - sumB) <= 325 && !hasPawn) return 0;

            _ = sumAPos - sumBPos;

            _ = sumA1 - sumB1;

            _ = sumA4 - sumB4;

            _ = _totalAtackOpKing * 20 * KingAttackWeight[_countTotalAtackOpKing] / 256 -
                _totalAtackMyKing * 20 * KingAttackWeight[_countTotalAtackMyKing] / 256;

            _ = sumA5 - sumB5;

            _scorce -= _scorceB;
            _scorceEnding -= _scorceEndingB;

            _scorce += _totalAtackOpKing * 20 * KingAttackWeight[_countTotalAtackOpKing] / 256 -
                       _totalAtackMyKing * 20 * KingAttackWeight[_countTotalAtackMyKing] / 256;

            _scorceEnding += _totalAtackOpKing * 20 * KingAttackWeight[_countTotalAtackOpKing] / 256 -
                             _totalAtackMyKing * 20 * KingAttackWeight[_countTotalAtackMyKing] / 256;

            if (_phaseMix > 24) _phaseMix = 24;

            var total = (_scorceEnding * (24 - _phaseMix) + _scorce * _phaseMix) / 24;

            return total;
        }

        private static int Eval_light_pawn(int side, int index, ref int scorce, ref int scorceEnding)
        {
            var r = index % 16 + 1;
            var sum = 0;

            if (_pawnRank[side, r] > index / 16)
            {
                sum -= PawnSameFile[_phase];
                scorce -= PawnSameFile[_phase];
                scorceEnding -= PawnSameFile[2];
            }

            if (_pawnRank[side, r - 1] == 0 && _pawnRank[side, r + 1] == 0)
            {
                sum -= PawnIsolated[_phase];
                scorce -= PawnIsolated[_phase];
                scorceEnding -= PawnIsolated[2];
            }
            else
            {
                if (_pawnRank[side, r - 1] < index / 16 && _pawnRank[side, r + 1] < index / 16)
                {
                    sum -= PawnIsolated[_phase] / 2;
                    scorce -= PawnIsolated[_phase] / 2;
                    scorceEnding -= PawnIsolated[2] / 2;
                }
            }

            if (_pawnRank[3 - side, r - 1] < index / 16 || _pawnRank[3 - side, r] < index / 16 ||
                _pawnRank[3 - side, r + 1] < index / 16) return sum;

            sum += (7 - index / 16) * PassedPawnBonus[_phase];
            scorce += (7 - index / 16) * PassedPawnBonus[_phase];
            scorceEnding += (7 - index / 16) * PassedPawnBonus[2];
            var j = index / 16;
            var found = false;

            for (var k = -j + index; k <= j + index; k++)
            {
                var col = k;

                while ((col & 0x88) == 0)
                {
                    if (_arrSide[col] != side && _arrValue[col] > 1)
                    {
                        found = true;
                        break;
                    }

                    col -= 16;
                }

                if (found) break;
            }

            if (found) return sum;
            sum += 5 * PassedPawnBonus[_phase];
            scorce += 5 * PassedPawnBonus[_phase];
            scorceEnding += 5 * PassedPawnBonus[2];

            return sum;
        }

        private static int Eval_dark_pawn(int side, int index, ref int scorce, ref int scorceEnding)
        {
            var r = index % 16 + 1;
            var sum = 0;

            if (_pawnRank[side, r] < index / 16)
            {
                sum -= PawnSameFile[_phase];
                scorce -= PawnSameFile[_phase];
                scorceEnding -= PawnSameFile[2];
            }

            if (_pawnRank[side, r - 1] == 7 && _pawnRank[side, r + 1] == 7)
            {
                sum -= PawnIsolated[_phase];
                scorce -= PawnIsolated[_phase];
                scorceEnding -= PawnIsolated[2];
            }
            else
            {
                if (_pawnRank[side, r - 1] > index / 16 && _pawnRank[side, r + 1] > index / 16)
                {
                    sum -= PawnIsolated[_phase] / 2;
                    scorce -= PawnIsolated[_phase] / 2;
                    scorceEnding -= PawnIsolated[2] / 2;
                }
            }

            if (_pawnRank[3 - side, r - 1] > index / 16 || _pawnRank[3 - side, r] > index / 16 ||
                _pawnRank[3 - side, r + 1] > index / 16) return sum;

            sum += index / 16 * PassedPawnBonus[_phase];
            scorce += index / 16 * PassedPawnBonus[_phase];
            scorceEnding += index / 16 * PassedPawnBonus[2];
            var j = 7 - index / 16;
            var found = false;

            for (var k = -j + index; k <= j + index; k++)
            {
                var col = k;

                while ((col & 0x88) == 0)
                {
                    if (_arrSide[col] != side && _arrValue[col] > 1)
                    {
                        found = true;
                        break;
                    }

                    col += 16;
                }

                if (found) break;
            }

            if (found) return sum;
            sum += 5 * PassedPawnBonus[_phase];
            scorce += 5 * PassedPawnBonus[_phase];
            scorceEnding += 5 * PassedPawnBonus[2];

            return sum;
        }

        private static int EvaluateKnight(int side, int index, IEnumerable<int> delta, ref int scorce,
            ref int scorceEnding)
        {
            var result = -KnightUnit;

            foreach (var t in delta)
                if (t != 0)
                {
                    var toIndex = index + t;

                    if ((toIndex & 0x88) != 0) continue;

                    if (_arrSide[toIndex] != 0)
                    {
                        if (_arrSide[toIndex] != side) result += 1;
                    }
                    else
                    {
                        result += 1;
                    }
                }

            scorce += result * KnightMobOpening;
            scorceEnding += result * KnightMobEndgame;
            return _phase <= MiddleGame ? result * KnightMobOpening : result * KnightMobEndgame;
        }

        private static int EvaluateBishop(int side, int index, IEnumerable<int> delta, ref int scorce,
            ref int scorceEnding)
        {
            var result = -BishopUnit;

            foreach (var t in delta)
                if (t != 0)
                {
                    var toIndex = index + t;
                    var isFound = true;

                    while ((toIndex & 0x88) == 0 && isFound)
                    {
                        if (_arrSide[toIndex] != 0)
                        {
                            if (_arrSide[toIndex] != side) result += 1;

                            isFound = false;
                        }
                        else
                        {
                            result += 1;
                        }

                        toIndex += t;
                    }

                    if ((toIndex & 0x88) != 0 || ((_arrValue[toIndex] != 5 || _arrSide[toIndex] != side) &&
                                                  (_arrSide[toIndex] == side || (_arrValue[toIndex] != 3 &&
                                                      _arrValue[toIndex] != 5 && _arrValue[toIndex] != 4)))) continue;

                    while ((toIndex & 0x88) == 0)
                    {
                        result += 1;

                        if (_arrSide[toIndex] != side && (_arrValue[toIndex] == 6 || _arrValue[toIndex] == 5 ||
                                                          _arrValue[toIndex] == 4)) result += 1;

                        toIndex += t;
                    }
                }

            scorce += result * BishopMobOpening;
            scorceEnding += result * BishopMobEndgame;
            return _phase <= MiddleGame ? result * BishopMobOpening : result * BishopMobEndgame;
        }

        private static int EvaluateRook(int side, int index, IEnumerable<int> delta, ref int scorce,
            ref int scorceEnding)
        {
            var result = -RookUnit;

            foreach (var t in delta)
                if (t != 0)
                {
                    var toIndex = index + t;
                    var isFound = true;

                    while ((toIndex & 0x88) == 0 && isFound)
                    {
                        if (_arrSide[toIndex] != 0)
                        {
                            if (_arrSide[toIndex] != side) result += 1;

                            isFound = false;
                        }
                        else
                        {
                            result += 1;
                        }

                        toIndex += t;
                    }
                }

            scorce += result * RookMobOpening;
            scorceEnding += result * RookMobEndgame;
            return _phase <= MiddleGame ? result * RookMobOpening : result * RookMobEndgame;
        }

        private static int EvaluateQueen(int side, int index, IEnumerable<int> delta, ref int scorce,
            ref int scorceEnding)
        {
            var result = -QueenUnit;

            foreach (var t in delta)
                if (t != 0)
                {
                    var toIndex = index + t;
                    var isFound = true;

                    while ((toIndex & 0x88) == 0 && isFound)
                    {
                        if (_arrSide[toIndex] != 0)
                        {
                            if (_arrSide[toIndex] != side) result += 1;

                            isFound = false;
                        }
                        else
                        {
                            result += 1;
                        }

                        toIndex += t;
                    }
                }

            scorce += result * QueenMobOpening;
            scorceEnding += result * QueenMobEndgame;
            return _phase <= MiddleGame ? result * QueenMobOpening : result * QueenMobEndgame;
        }

        private static int GetAllValueBelongPieceFromIndex(int side, int value, int index, ref int scorce,
            ref int scorceEnding)
        {
            switch (value)
            {
                case 1:
                    return 0;

                case 2:
                    return EvaluateBishop(side, index, BishopDelta, ref scorce, ref scorceEnding);
                case 3:
                    return EvaluateKnight(side, index, KnightDelta, ref scorce, ref scorceEnding);
                case 4:
                    return EvaluateRook(side, index, RookDelta, ref scorce, ref scorceEnding);
                case 5:
                    return EvaluateQueen(side, index, QueenDelta, ref scorce, ref scorceEnding);
                case 6:
                    return 0;
            }

            return 0;
        }

        private static int TotalAtackKing(int side, int value, int index, int kingIndex)
        {
            var delta = new int[8];
            var result = 0;

            switch (value)
            {
                case 1:
                    delta = side == 1 ? WpawnDelta : BpawnDelta;
                    break;
                case 2:
                    delta = BishopDelta;
                    break;
                case 3:
                    delta = KnightDelta;
                    break;
                case 4:
                    delta = RookDelta;
                    break;
                case 5:
                    delta = QueenDelta;
                    break;
                case 6:
                    delta = KingDelta;
                    break;
            }

            int i;

            if (value == 3)
            {
                for (i = 0; i < delta.Length; i++)
                    if (delta[i] != 0)
                    {
                        var toIndex = index + delta[i];

                        if (Math.Abs(delta[i]) == 32 && index >= 32 && index < 96) continue;

                        if ((toIndex & 0x88) != 0) continue;

                        if (_arrSide[toIndex] != 0)
                        {
                            if (_arrSide[toIndex] == _arrSide[index]) continue;

                            if (DistanceFromXtoY(toIndex, kingIndex) <= 1 && toIndex != kingIndex)
                                result += 1;
                        }
                        else
                        {
                            if (DistanceFromXtoY(toIndex, kingIndex) <= 1 && toIndex != kingIndex) result += 1;
                        }
                    }
            }
            else
            {
                var brq = value == 2 || value == 4 || value == 5;

                if (!brq) return result * KingAttackUnit[value];

                for (i = 0; i < delta.Length; i++)
                    if (delta[i] != 0)
                    {
                        var toIndex = index + delta[i];
                        var isFound = true;

                        while ((toIndex & 0x88) == 0 && isFound)
                        {
                            if (_arrSide[toIndex] != 0)
                            {
                                isFound = false;

                                switch (value)
                                {
                                    case 2 when _arrValue[toIndex] == 5:
                                    case 4 when _arrValue[toIndex] == 5:
                                        isFound = true;
                                        break;
                                }

                                if (_arrSide[toIndex] != _arrSide[index] && toIndex != kingIndex)
                                    if (DistanceFromXtoY(toIndex, kingIndex) <= 1)
                                        result += 1;
                            }
                            else
                            {
                                if (DistanceFromXtoY(toIndex, kingIndex) <= 1 && toIndex != kingIndex) result += 1;
                            }

                            toIndex += delta[i];
                        }
                    }
            }

            return result * KingAttackUnit[value];
        }

        private static int SumKingSafeMark(int side, int index)
        {
            var s = 0;
            var c = index % 16 + 1;

            if (_phaseMix <= 12 && _phase == 1) return s;

            if (_phase <= 1)
                s = side == 1 ? _kingWhiteEvaluate[index] : _kingBlackEvaluate[index];
            else if (_phaseMix <= 5) s = _pst[side, 6, index, 2];

            if (_phaseMix > 16 && side == 1 && index / 16 < 7) s -= 50;

            if (_phaseMix > 16 && side == 2 && index / 16 > 0) s -= 50;

            if (_phaseMix <= 12) return s;

            if (c <= 3)
            {
                s += Eval_king_lkp(side, 1);
                s += Eval_king_lkp(side, 2);
                s += Eval_king_lkp(side, 3);
            }
            else if (c > 5)
            {
                s += Eval_king_lkp(side, 8);
                s += Eval_king_lkp(side, 7);
                s += Eval_king_lkp(side, 6);
            }
            else
            {
                for (var j = c; j <= c + 2; j++)
                    if (_pawnRank[side, j] == 0 && _pawnRank[3 - side, j] != 0)
                        s -= 10;

                s -= 10;
            }

            return s;
        }

        private static int Eval_king_lkp(int side, int col)
        {
            var lkp = 0;

            if (side == 1)
            {
                switch (_pawnRank[1, col])
                {
                    case 6:
                        lkp = 0;
                        break;
                    case 5:
                        lkp -= 10;
                        break;
                    default:
                    {
                        if (_pawnRank[1, col] != 0)
                            lkp -= 20;
                        else
                            lkp -= 25;

                        break;
                    }
                }

                switch (_pawnRank[2, col])
                {
                    case 6:
                        lkp -= 15;
                        break;
                    case 5:
                        lkp -= 10;
                        break;
                    case 4:
                        lkp -= 5;
                        break;
                }
            }
            else
            {
                switch (_pawnRank[2, col])
                {
                    case 1:
                        lkp = 0;
                        break;
                    case 2:
                        lkp -= 10;
                        break;
                    default:
                    {
                        if (_pawnRank[2, col] != 0)
                            lkp -= 20;
                        else
                            lkp -= 25;

                        break;
                    }
                }

                switch (_pawnRank[1, col])
                {
                    case 1:
                        lkp -= 15;
                        break;
                    case 2:
                        lkp -= 10;
                        break;
                    case 3:
                        lkp -= 5;
                        break;
                }
            }

            return lkp;
        }

        private static int DistanceFromXtoY(int fromIndex, int toIndex)
        {
            var xFrom = fromIndex % 16 + 1;
            var yFrom = fromIndex / 16 + 1;
            var xTo = toIndex % 16 + 1;
            var yTo = toIndex / 16 + 1;

            return Math.Abs(xFrom - xTo) > Math.Abs(yFrom - yTo) ? Math.Abs(xFrom - xTo) : Math.Abs(yFrom - yTo);
        }

        private static int DistanceToTheKing(int side, int fromIndex)
        {
            var toIndex = _arrPiece[5].Index;

            if (side == 2) toIndex = _arrPiece[21].Index;

            var xFrom = fromIndex % 16 + 1;
            var yFrom = fromIndex / 16 + 1;
            var xTo = toIndex % 16 + 1;
            var yTo = toIndex / 16 + 1;

            return Math.Abs(xFrom - xTo) > Math.Abs(yFrom - yTo) ? Math.Abs(xFrom - xTo) : Math.Abs(yFrom - yTo);
        }

        private static int GetEvaluatePiece(int index, int side, int value, ref int scorce, ref int scorceEnding)
        {
            var result = 0;

            if (index < 0) return 0;

            switch (value)
            {
                case 1:

                    if (side == 1)
                    {
                        result = _pawnWhiteEvaluate[index];
                        scorce += _pawnWhiteEvaluate[index];
                        scorceEnding += _pawnEvaluateEnding[index];
                    }
                    else
                    {
                        result = _pawnBlackEvaluate[index];
                        scorce += _pawnBlackEvaluate[index];
                        scorceEnding += _pawnEvaluateEnding[index];
                    }

                    break;
                case 2:

                    if (side == 1)
                    {
                        result = _bishopWhiteEvaluate[index];
                        scorce += _bishopWhiteEvaluate[index];
                        scorceEnding += _bishopEvaluateEnding[index];

                        switch (index)
                        {
                            case 32 when _arrValue[49] == 1 && _arrSide[49] == 2:
                                result -= TrappedBishop / 2;
                                scorce -= TrappedBishop / 2;
                                scorceEnding -= TrappedBishop / 2;
                                break;
                            case 16 when _arrValue[33] == 1 && _arrSide[33] == 2:
                                result -= TrappedBishop;
                                scorce -= TrappedBishop;
                                scorceEnding -= TrappedBishop;
                                break;
                            case 39 when _arrValue[54] == 1 && _arrSide[54] == 2:
                                result -= TrappedBishop / 2;
                                scorce -= TrappedBishop / 2;
                                scorceEnding -= TrappedBishop / 2;
                                break;
                            case 23 when _arrValue[38] == 1 && _arrSide[38] == 2:
                                result -= TrappedBishop;
                                scorce -= TrappedBishop;
                                scorceEnding -= TrappedBishop;
                                break;
                        }

                        if (_arrValue[97] != 0 && _arrValue[99] != 0 && index == 114)
                        {
                            result -= BlockedBishop;
                            scorce -= BlockedBishop;
                            scorceEnding -= BlockedBishop;
                        }

                        if (_arrValue[100] != 0 && _arrValue[102] != 0 && index == 117)
                        {
                            result -= BlockedBishop;
                            scorce -= BlockedBishop;
                            scorceEnding -= BlockedBishop;
                        }
                    }
                    else
                    {
                        result = _bishopBlackEvaluate[index];
                        scorce += _bishopBlackEvaluate[index];
                        scorceEnding += _bishopEvaluateEnding[index];

                        switch (index)
                        {
                            case 80 when _arrValue[65] == 1 && _arrSide[65] == 1:
                                result -= TrappedBishop / 2;
                                scorce -= TrappedBishop / 2;
                                scorceEnding -= TrappedBishop / 2;
                                break;
                            case 96 when _arrValue[81] == 1 && _arrSide[81] == 1:
                                result -= TrappedBishop;
                                scorce -= TrappedBishop;
                                scorceEnding -= TrappedBishop;
                                break;
                            case 87 when _arrValue[70] == 1 && _arrSide[70] == 1:
                                result -= TrappedBishop / 2;
                                scorce -= TrappedBishop / 2;
                                scorceEnding -= TrappedBishop / 2;
                                break;
                            case 103 when _arrValue[86] == 1 && _arrSide[86] == 1:
                                result -= TrappedBishop;
                                scorce -= TrappedBishop;
                                scorceEnding -= TrappedBishop;
                                break;
                        }

                        if (_arrValue[17] != 0 && _arrValue[19] != 0 && index == 2)
                        {
                            result -= BlockedBishop;
                            scorce -= BlockedBishop;
                            scorceEnding -= BlockedBishop;
                        }

                        if (_arrValue[20] != 0 && _arrValue[22] != 0 && index == 5)
                        {
                            result -= BlockedBishop;
                            scorce -= BlockedBishop;
                            scorceEnding -= BlockedBishop;
                        }
                    }

                    break;
                case 3:

                    if (side == 1)
                    {
                        result = _knightWhiteEvaluate[index];
                        scorce += _knightWhiteEvaluate[index];
                        scorceEnding += _knightEvaluateEnding[index];
                    }
                    else
                    {
                        result = _knightBlackEvaluate[index];
                        scorce += _knightBlackEvaluate[index];
                        scorceEnding += _knightEvaluateEnding[index];
                    }

                    break;
                case 4:
                    var r = index % 16 + 1;
                    result = _pst[side, value, index, _phase];
                    scorce += _pst[side, value, index, _phase];
                    scorceEnding += _pst[side, value, index, 2];

                    if (side == 1)
                    {
                        if (_pawnRank[1, r] == 0)
                        {
                            if (_pawnRank[2, r] == 7)
                            {
                                result += RookOpenFile;
                                scorce += RookOpenFile;
                                scorceEnding += RookOpenFile;
                            }
                            else
                            {
                                result += RookOpenFile - 10;
                                scorce += RookOpenFile - 10;
                                scorceEnding += RookOpenFile - 10;
                            }
                        }

                        if (((_arrValue[114] == 6 && _arrSide[114] == 1) || (_arrValue[113] == 6 && _arrSide[113] == 1))
                            && (index == 112 || index == 96 || index == 113))
                        {
                            result -= BlockedRook;
                            scorce -= BlockedRook;
                        }

                        if (((_arrValue[117] == 6 && _arrSide[117] == 1) || (_arrValue[118] == 6 && _arrSide[118] == 1))
                            && (index == 119 || index == 103 || index == 118))
                        {
                            result -= BlockedRook;
                            scorce -= BlockedRook;
                        }

                        if (index / 16 == 1)
                        {
                            result += RookOnSeventRank;
                            scorce += RookOnSeventRank;
                            scorceEnding += RookOnSeventRank;
                        }
                    }
                    else
                    {
                        if (_pawnRank[2, r] == 7)
                        {
                            if (_pawnRank[1, r] != 0)
                            {
                                result += RookOpenFile;
                                scorce += RookOpenFile;
                                scorceEnding += RookOpenFile;
                            }
                            else
                            {
                                result += RookOpenFile - 10;
                                scorce += RookOpenFile - 10;
                                scorceEnding += RookOpenFile - 10;
                            }
                        }

                        if (((_arrValue[1] == 6 && _arrSide[1] == 2) || (_arrValue[2] == 6 && _arrSide[2] == 2))
                            && (index == 0 || index == 16 || index == 1))
                        {
                            result -= BlockedRook;
                            scorce -= BlockedRook;
                        }

                        if (((_arrValue[5] == 6 && _arrSide[5] == 2) || (_arrValue[6] == 6 && _arrSide[2] == 2))
                            && (index == 7 || index == 23 || index == 6))
                        {
                            result -= BlockedRook;
                            scorce -= BlockedRook;
                        }

                        if (index / 16 == 6)
                        {
                            result += RookOnSeventRank;
                            scorce += RookOnSeventRank;
                            scorceEnding += RookOnSeventRank;
                        }
                    }

                    break;
                case 5:
                    if (_phase <= 1) return 0;

                    if (side == 1)
                    {
                        result = _queenWhiteEvaluate[index];
                        scorce += _queenWhiteEvaluate[index];
                        scorceEnding += _queenEvaluateEnding[index];
                    }
                    else
                    {
                        result = _queenBlackEvaluate[index];
                        scorce += _queenBlackEvaluate[index];
                        scorceEnding += _queenEvaluateEnding[index];
                    }

                    break;
                case 6:
                    return 0;
            }

            return result;
        }

        private static void OrderMove(ref MoveList[] allMovesAble, int startIndex, int totalMovesAble)
        {
            QuickSortDesc(ref allMovesAble, startIndex, totalMovesAble);
        }

        private static void QuickSortDesc(ref MoveList[] a, int l, int r)
        {
            var temp = new MoveList[1];

            for (var i = l; i <= r - 1; i++)
            for (var j = i + 1; j <= r; j++)
                if (a[j].Evaluate > a[i].Evaluate)
                {
                    temp[0] = a[i];
                    a[i] = a[j];
                    a[j] = temp[0];
                }
        }

        private static void GenerateCaptureMoves(int pieceIndex, int side, int value, int index,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            switch (value)
            {
                case 1:
                    PawnGenerateCaptureMoves(pieceIndex, side, value, index, ref allMovesAble,
                        ref totalMovesAble);
                    if (side == 1)
                    {
                        if (index <= 23 && index >= 16)
                            PawnGenerateMoves(pieceIndex, side, value, index, ref allMovesAble,
                                ref totalMovesAble);
                    }
                    else
                    {
                        if (index <= 103 && index >= 96)
                            PawnGenerateMoves(pieceIndex, side, value, index, ref allMovesAble,
                                ref totalMovesAble);
                    }

                    break;
                case 2:
                    BishopRookGenerateCaptureMoves(pieceIndex, side, value, index, BishopDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 3:
                    KnightKingGenerateCaptureMoves(pieceIndex, side, value, index, KnightDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 4:
                    BishopRookGenerateCaptureMoves(pieceIndex, side, value, index, RookDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 5:
                    QueenGenerateCaptureMoves(pieceIndex, side, value, index, QueenDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 6:
                    KnightKingGenerateCaptureMoves(pieceIndex, side, value, index, KingDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
            }
        }

        private static void QueenGenerateCaptureMoves(int pieceIndex, int side, int value, int index, IList<int> delta,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            for (var j = 0; j < 8; j++)
                if (delta[j] != 0)
                {
                    var toIndex = index + delta[j];

                    while ((toIndex & 0x88) == 0 && _arrSide[toIndex] == 0)

                        toIndex += delta[j];

                    if ((toIndex & 0x88) == 0 && _arrSide[toIndex] != _arrSide[index])
                        CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                            toIndex,
                            ref allMovesAble, ref totalMovesAble);
                }
        }

        private static void KnightKingGenerateCaptureMoves(int pieceIndex, int side, int value, int index,
            IList<int> delta,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            for (var j = 0; j < 8; j++)
                if (delta[j] != 0)
                {
                    var toIndex = index + delta[j];

                    if ((toIndex & 0x88) == 0 && _arrSide[toIndex] != _arrSide[index] && _arrSide[toIndex] != 0)
                        CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                            toIndex,
                            ref allMovesAble, ref totalMovesAble);
                }
        }

        private static void BishopRookGenerateCaptureMoves(int pieceIndex, int side, int value, int index,
            IList<int> delta,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            for (var j = 0; j < 4; j++)
                if (delta[j] != 0)
                {
                    var toIndex = index + delta[j];

                    while ((toIndex & 0x88) == 0 && _arrSide[toIndex] == 0)

                        toIndex += delta[j];

                    if ((toIndex & 0x88) == 0 && _arrSide[toIndex] != side && _arrSide[toIndex] != 0)
                        CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                            toIndex,
                            ref allMovesAble, ref totalMovesAble);
                }
        }

        private static void PawnGenerateCaptureMoves(int pieceIndex, int side, int value, int index,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            if (side == 1)
            {
                for (var j = 2; j < 4; j++)
                    if (WpawnDelta[j] != 0)
                    {
                        var toIndex = index + WpawnDelta[j];

                        if ((toIndex & 0x88) == 0 && -WpawnDelta[j] % 16 != 0 && _arrSide[toIndex] != 0 &&
                            _arrSide[toIndex] != side)
                        {
                            if (toIndex <= 7)
                            {
                                CreateMoves(pieceIndex, 1, 5, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);

                                CreateMoves(pieceIndex, 1, 4, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);

                                CreateMoves(pieceIndex, 1, 3, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);

                                CreateMoves(pieceIndex, 1, 2, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);
                            }
                            else
                            {
                                CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);
                            }
                        }

                        if ((toIndex & 0x88) != 0 || toIndex != _enPassant || _enPassant >= 40 || _enPassant == -1)
                            continue;

                        var tempEnpassant = _enPassant;

                        CreateMoves(pieceIndex, 2, 0, side, value, 0, 0, index, tempEnpassant, ref allMovesAble,
                            ref totalMovesAble);
                    }
            }
            else
            {
                for (var j = 2; j < 4; j++)
                    if (BpawnDelta[j] != 0)
                    {
                        var toIndex = index + BpawnDelta[j];

                        if ((toIndex & 0x88) == 0 && BpawnDelta[j] % 16 != 0 && _arrSide[toIndex] != 0 &&
                            _arrSide[toIndex] != side)
                        {
                            if (toIndex >= 112)
                            {
                                CreateMoves(pieceIndex, 1, 5, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);

                                CreateMoves(pieceIndex, 1, 4, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);

                                CreateMoves(pieceIndex, 1, 3, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);

                                CreateMoves(pieceIndex, 1, 2, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);
                            }
                            else
                            {
                                CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                    toIndex, ref allMovesAble, ref totalMovesAble);
                            }
                        }

                        if ((toIndex & 0x88) != 0 || toIndex != _enPassant || _enPassant < 80 || _enPassant == -1)
                            continue;

                        var tempEnpassant = _enPassant;

                        CreateMoves(pieceIndex, 2, 0, side, value, 0, 0, index, tempEnpassant, ref allMovesAble,
                            ref totalMovesAble);
                    }
            }
        }

        private static void GenerateMoves(int pieceIndex, int side, int value, int index,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            switch (value)
            {
                case 1:
                    PawnGenerateMoves(pieceIndex, side, value, index, ref allMovesAble, ref totalMovesAble);
                    break;
                case 2:
                    BishopRookGenerateMoves(pieceIndex, side, value, index, BishopDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 3:
                    KnightKingGenerateMoves(pieceIndex, side, value, index, KnightDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 4:
                    BishopRookGenerateMoves(pieceIndex, side, value, index, RookDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 5:
                    QueenGenerateMoves(pieceIndex, side, value, index, QueenDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
                case 6:
                    KnightKingGenerateMoves(pieceIndex, side, value, index, KingDelta, ref allMovesAble,
                        ref totalMovesAble);
                    break;
            }
        }

        private static void QueenGenerateMoves(int pieceIndex, int side, int value, int index, IList<int> delta,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            for (var j = 0; j < 8; j++)
                if (delta[j] != 0)
                {
                    var toIndex = index + delta[j];

                    while ((toIndex & 0x88) == 0 && _arrSide[toIndex] == 0)
                    {
                        CreateMoves(pieceIndex, 0, 0, side, value, 0, 0, index, toIndex, ref allMovesAble,
                            ref totalMovesAble);

                        toIndex += delta[j];
                    }
                }
        }

        private static void KnightKingGenerateMoves(int pieceIndex, int side, int value, int index, IList<int> delta,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            for (var j = 0; j < 8; j++)
                if (delta[j] != 0)
                {
                    var toIndex = index + delta[j];

                    if ((toIndex & 0x88) == 0 && _arrSide[toIndex] == 0)
                        CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                            toIndex,
                            ref allMovesAble, ref totalMovesAble);
                }

            if (value != 6) return;

            if (index == 116 && _isFirstKingWhiteMove && _isFirstRookWhiteRightMove && _arrValue[117] == 0 &&
                _arrValue[118] == 0 && _arrValue[119] == 4 && _arrSide[119] == side &&
                _arrPiecePossition[119] == 24 &&
                _arrPiece[24].Status == 0 && !IsInCheck(1) && !HaveSomePieceCanMoveToIndex(2, 117) &&
                !HaveSomePieceCanMoveToIndex(2, 118))
                CreateMoves(pieceIndex, 3, 0, side, value, _arrSide[118], _arrValue[118], index, 118,
                    ref allMovesAble, ref totalMovesAble);

            switch (index)
            {
                case 116 when _isFirstKingWhiteMove && _isFirstRookWhiteLeftMove && _arrValue[115] == 0 &&
                              _arrValue[114] == 0 && _arrValue[113] == 0 && _arrValue[112] == 4 &&
                              _arrSide[112] == side &&
                              _arrPiecePossition[112] == 17 && _arrPiece[17].Status == 0 && !IsInCheck(1) &&
                              !HaveSomePieceCanMoveToIndex(2, 115) && !HaveSomePieceCanMoveToIndex(2, 114):
                    CreateMoves(pieceIndex, 4, 0, side, value, _arrSide[114], _arrValue[114], index, 114,
                        ref allMovesAble, ref totalMovesAble);
                    break;
                case 4 when _isFirstKingBlackMove && _isFirstRookBlackRightMove && _arrValue[5] == 0 &&
                            _arrValue[6] == 0 && _arrValue[7] == 4 && _arrSide[7] == side &&
                            _arrPiecePossition[7] == 8 &&
                            _arrPiece[8].Status == 0 && !IsInCheck(2) && !HaveSomePieceCanMoveToIndex(1, 5) &&
                            !HaveSomePieceCanMoveToIndex(1, 6):
                    CreateMoves(pieceIndex, 5, 0, side, value, _arrSide[6], _arrValue[6], index, 6, ref allMovesAble,
                        ref totalMovesAble);
                    break;
            }

            if (index == 4 && _isFirstKingBlackMove && _isFirstRookBlackLeftMove && _arrValue[3] == 0 &&
                _arrValue[2] == 0 && _arrValue[1] == 0 && _arrValue[0] == 4 && _arrSide[0] == side &&
                _arrPiecePossition[0] == 1 && _arrPiece[1].Status == 0 && !IsInCheck(2) &&
                !HaveSomePieceCanMoveToIndex(1, 3) && !HaveSomePieceCanMoveToIndex(1, 2))
                CreateMoves(pieceIndex, 6, 0, side, value, _arrSide[2], _arrValue[2], index, 2, ref allMovesAble,
                    ref totalMovesAble);
        }

        private static void BishopRookGenerateMoves(int pieceIndex, int side, int value, int index, IList<int> delta,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            for (var j = 0; j < 4; j++)
                if (delta[j] != 0)
                {
                    var toIndex = index + delta[j];

                    while ((toIndex & 0x88) == 0 && _arrSide[toIndex] == 0)
                    {
                        CreateMoves(pieceIndex, 0, 0, side, value, 0, 0, index, toIndex, ref allMovesAble,
                            ref totalMovesAble);

                        toIndex += delta[j];
                    }
                }
        }

        private static void PawnGenerateMoves(int pieceIndex, int side, int value, int index,
            ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            if (side == 1)
            {
                for (var j = 0; j < 2; j++)
                    if (WpawnDelta[j] != 0)
                    {
                        var toIndex = index + WpawnDelta[j];

                        if (index > 95 && (toIndex & 0x88) == 0 && WpawnDelta[j] == -32 && _arrSide[toIndex] == 0 &&
                            _arrSide[index - 16] == 0)
                            CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                toIndex, ref allMovesAble, ref totalMovesAble);

                        if ((toIndex & 0x88) != 0 || WpawnDelta[j] != -16 || _arrSide[toIndex] != 0) continue;

                        if (toIndex <= 7)
                        {
                            CreateMoves(pieceIndex, 1, 5, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);

                            CreateMoves(pieceIndex, 1, 4, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);

                            CreateMoves(pieceIndex, 1, 3, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);

                            CreateMoves(pieceIndex, 1, 2, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);
                        }
                        else
                        {
                            CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                toIndex, ref allMovesAble, ref totalMovesAble);
                        }
                    }
            }
            else
            {
                for (var j = 0; j < 2; j++)
                    if (BpawnDelta[j] != 0)
                    {
                        var toIndex = index + BpawnDelta[j];

                        if (index <= 23 && (toIndex & 0x88) == 0 && BpawnDelta[j] == 32 && _arrSide[toIndex] == 0 &&
                            _arrSide[index + 16] == 0)
                            CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                toIndex, ref allMovesAble, ref totalMovesAble);

                        if ((toIndex & 0x88) != 0 || BpawnDelta[j] != 16 || _arrSide[toIndex] != 0) continue;

                        if (toIndex >= 112)
                        {
                            CreateMoves(pieceIndex, 1, 5, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);

                            CreateMoves(pieceIndex, 1, 4, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);

                            CreateMoves(pieceIndex, 1, 3, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);

                            CreateMoves(pieceIndex, 1, 2, side, value, 0, 0, index, toIndex, ref allMovesAble,
                                ref totalMovesAble);
                        }
                        else
                        {
                            CreateMoves(pieceIndex, 0, 0, side, value, _arrSide[toIndex], _arrValue[toIndex], index,
                                toIndex, ref allMovesAble, ref totalMovesAble);
                        }
                    }
            }
        }

        private static void CreateMoves(int pieceIndex, int moveType, byte promotionValue, int fromSide, int fromValue,
            int toSide, int toValue, int fromIndex, int toIndex, ref MoveList[] allMovesAble, ref int totalMovesAble)
        {
            totalMovesAble += 1;
            allMovesAble[totalMovesAble].MoveType = moveType;
            allMovesAble[totalMovesAble].PromotionValue = promotionValue;
            allMovesAble[totalMovesAble].FromIndex = fromIndex;
            allMovesAble[totalMovesAble].ToIndex = toIndex;
            allMovesAble[totalMovesAble].FromPiecePossition = pieceIndex;

            allMovesAble[totalMovesAble].ToPiecePossition = moveType == 2
                ? fromSide == 1
                    ? Math.Abs(toIndex - fromIndex) == 15
                        ? _arrPiecePossition[fromIndex + 1]
                        : _arrPiecePossition[fromIndex - 1]
                    : Math.Abs(toIndex - fromIndex) == 17
                        ? _arrPiecePossition[fromIndex + 1]
                        : _arrPiecePossition[fromIndex - 1]
                : _arrPiecePossition[toIndex];

            allMovesAble[totalMovesAble].FromSide = fromSide;
            allMovesAble[totalMovesAble].FromValue = fromValue;
            allMovesAble[totalMovesAble].ToSide = toSide;
            allMovesAble[totalMovesAble].ToValue = toValue;

            allMovesAble[totalMovesAble].ToEnpassantIndex = fromValue == 1 && Math.Abs(fromIndex - toIndex) == 32
                ? fromIndex + (toIndex - fromIndex) / 2
                : 0;
        }

        public static void UndoUserMove()
        {
            if (_countMainMoveList < 0) return;

            _guiMoveFromSidePgn = _arrMainMoveList[_countMainMoveList].FromSide;
            _guiMoveFromValuePgn = _arrMainMoveList[_countMainMoveList].FromValue;
            _guiMoveToSidePgn = _arrMainMoveList[_countMainMoveList].ToSide;
            _guiMoveToValuePgn = _arrMainMoveList[_countMainMoveList].ToValue;
            _guiMoveTypePgn = _arrMainMoveList[_countMainMoveList].MoveType;
            UnDoAMoveList(_arrMainMoveList[_countMainMoveList]);

            if (_countMainMoveList >= 0) _countMainMoveList--;
        }

        public static int UserMove(string move)
        {
            var from = Square(move);
            var to = Square(move.Substring(2));

            var mMoveType = IsMoveAble(from, to, _arrSide[from], _arrValue[from]);

            if (mMoveType >= 0)
            {
                byte promotionValue = 0;

                if (mMoveType == 1)
                    if (move.Length == 5)
                    {
                        switch (move.Substring(4, 1).ToLower())
                        {
                            case "n":
                                promotionValue = 3;
                                break;
                            case "b":
                                promotionValue = 2;
                                break;
                            case "r":
                                promotionValue = 4;
                                break;
                            case "q":
                                promotionValue = 5;
                                break;
                        }

                        _guiMovePromotionValue = promotionValue;
                    }

                _countMainMoveList++;

                if (!_kingRookMovedList.ContainsKey(_arrPiecePossition[from]))
                    _kingRookMovedList.Add(_arrPiecePossition[from], "1");

                _arrMoveList[1].MoveType = mMoveType;
                _arrMoveList[1].PromotionValue = promotionValue;
                _arrMoveList[1].FromSide = _arrSide[from];
                _arrMoveList[1].FromValue = _arrValue[from];
                _arrMoveList[1].ToSide = _arrSide[to];
                _arrMoveList[1].ToValue = _arrValue[to];
                _arrMoveList[1].FromIndex = from;
                _arrMoveList[1].ToIndex = to;
                _arrMoveList[1].FromPiecePossition = _arrPiecePossition[from];
                _arrMoveList[1].ToPiecePossition = _arrPiecePossition[to];
                _arrMainMoveList[_countMainMoveList] = _arrMoveList[1];

                MakeMove(mMoveType, promotionValue, _arrSide[from], _arrValue[from], _arrValue[to], from,
                    to,
                    _arrPiecePossition[from], _arrPiecePossition[to]);

                _moveHashCode[_countMainMoveList] = GetZobrist(3 - _arrSide[to]);
                _strMainMoveList += move;
            }
            else
            {
                throw new Exception("Invalid move " + move);
            }

            return mMoveType;
        }

        private static void DoAMoveList(MoveList move)
        {
            MakeMove(move.MoveType, move.PromotionValue, move.FromSide, move.FromValue, move.ToValue,
                move.FromIndex, move.ToIndex, move.FromPiecePossition, move.ToPiecePossition);
        }

        private static void UnDoAMoveList(MoveList move)
        {
            UnMakeMove(move.MoveType, move.FromSide, move.FromValue, move.ToSide, move.ToValue,
                move.FromIndex, move.ToIndex, move.FromPiecePossition, move.ToPiecePossition);
        }

        private static void DoMove(string move)
        {
            var from = Square(move);
            var to = Square(move.Substring(2));

            var mMoveType = IsMoveAble(from, to, _arrSide[from], _arrValue[from]);

            if (mMoveType >= 0)
            {
                byte promotionValue = 0;

                if (mMoveType == 1)
                    if (move.Length == 5)
                        switch (move.Substring(4, 1))
                        {
                            case "n":
                                promotionValue = 3;
                                break;
                            case "b":
                                promotionValue = 2;
                                break;
                            case "r":
                                promotionValue = 4;
                                break;
                            case "q":
                                promotionValue = 5;
                                break;
                        }

                if (!_kingRookMovedList.ContainsKey(_arrPiecePossition[from]))
                    _kingRookMovedList.Add(_arrPiecePossition[from], "1");

                _arrMoveList[1].MoveType = mMoveType;
                _arrMoveList[1].PromotionValue = promotionValue;
                _arrMoveList[1].FromSide = _arrSide[from];
                _arrMoveList[1].FromValue = _arrValue[from];
                _arrMoveList[1].ToSide = _arrSide[to];
                _arrMoveList[1].ToValue = _arrValue[to];
                _arrMoveList[1].FromIndex = from;
                _arrMoveList[1].ToIndex = to;
                _arrMoveList[1].FromPiecePossition = _arrPiecePossition[from];
                _arrMoveList[1].ToPiecePossition = _arrPiecePossition[to];

                MakeMove(mMoveType, promotionValue, _arrSide[from], _arrValue[from], _arrValue[to], from,
                    to,
                    _arrPiecePossition[from], _arrPiecePossition[to]);

                _strMainMoveList += move;

                _guiMoveTypeEngine = mMoveType;
                _guiMovePromotionValue = _arrMoveList[1].PromotionValue;
                _guiEngineWin = 0;

                _countMainMoveList++;
                _arrMainMoveList[_countMainMoveList] = _arrMoveList[1];
                _moveHashCode[_countMainMoveList] = GetZobrist(3 - _arrMoveList[1].FromSide);
                _strMainMoveList += ToBoardString(_arrMoveList[1].FromIndex) + ToBoardString(_arrMoveList[1].ToIndex);
                _guiEngineMove = ToBoardString(_arrMoveList[1].FromIndex) + ToBoardString(_arrMoveList[1].ToIndex);
                _engineThink = false;
                _myThinking = false;
            }
            else
            {
                throw new Exception("Invalid move " + move);
            }
        }

        private static int Square(string square)
        {
            var temp = square.Substring(0, 2);
            temp = temp.ToLower();

            if (temp[0] < 'a' || temp[0] > 'h') throw new Exception("Invalid square");

            if (temp[1] < '1' || temp[1] > '8') throw new Exception("Invalid square");

            var result = temp[0] - 'a' + (temp[1] - '1') * 8;
            return (7 - result / 8) * 16 + result % 8;
        }

        private static bool IsSquare(string square)
        {
            var temp = square.Substring(0, 2);
            temp = temp.ToLower();

            return temp[0] >= 'a' && temp[0] <= 'h' && temp[1] >= '1' && temp[1] <= '8';
        }

        private static bool IsTheSameMove(MoveList a, MoveList b)
        {
            return a.FromSide == b.FromSide && a.FromValue == b.FromValue && a.FromIndex == b.FromIndex &&
                   a.ToIndex == b.ToIndex && a.FromPiecePossition == b.FromPiecePossition;
        }

        private static string ToBoardStringFromMove(MoveList move)
        {
            return ToBoardString(move.FromIndex) + ToBoardString(move.ToIndex);
        }

        private static string ToBoardString(int x)
        {
            string[] colname = { "a", "b", "c", "d", "e", "f", "g", "h" };
            var row = 8 - x / 16;
            var col = x % 16;
            return colname[col] + row;
        }

        private static int GetMax(int s1, int s2)
        {
            return s1 >= s2 ? s1 : s2;
        }

        private static int See(MoveList move, int depth, int toIndex, int side)
        {
            var value = 0;
            int from = 1, to = 17;

            if (depth == 0)
            {
                DoAMoveList(move);

                if (IsInCheck(side))
                {
                    UnDoAMoveList(move);
                    return -100000;
                }

                value = _arrPieceEvaluate[move.ToValue] - See(move, depth + 1, toIndex, 3 - side);
                UnDoAMoveList(move);
                return value;
            }

            if (side == 1)
            {
                from = 17;
                to = 33;
            }

            var minValue = int.MaxValue;
            var kkmin = -1;
            var moveType = -1;

            byte mPromotion = 0;

            for (var k = from; k < to; k++)
                if (_arrPiece[k].Status == 0 && _arrPiece[k].Side == side)
                {
                    var mark = IsMoveAble(_arrPiece[k].Index, toIndex, _arrPiece[k].Side, _arrPiece[k].Value);

                    if (mark < 0) continue;
                    if (_arrPieceEvaluate[_arrPiece[k].Value] >= minValue) continue;
                    minValue = _arrPieceEvaluate[_arrPiece[k].Value];
                    kkmin = k;
                    moveType = mark;
                }

            if (kkmin == -1) return value;
            var mFromSide = _arrPiece[kkmin].Side;
            var mFromValue = _arrPiece[kkmin].Value;
            var mToSide = _arrSide[toIndex];
            var mToValue = _arrValue[toIndex];
            var mFromIndex = _arrPiece[kkmin].Index;
            var mFromPiercePos = _arrPiecePossition[mFromIndex];
            var mToPiercePos = _arrPiecePossition[toIndex];

            if (moveType == 1) mPromotion = 5;

            MakeMove(moveType, mPromotion, mFromSide, mFromValue, mToValue, mFromIndex, toIndex,
                mFromPiercePos, mToPiercePos);

            if (IsInCheck(mFromSide))
            {
                UnMakeMove(moveType, mFromSide, mFromValue, mToSide, mToValue, mFromIndex,
                    toIndex, mFromPiercePos, mToPiercePos);

                return value;
            }

            value = GetMax(0, _arrPieceEvaluate[mToValue] - See(move, depth + 1, toIndex, 3 - side));

            UnMakeMove(moveType, mFromSide, mFromValue, mToSide, mToValue, mFromIndex, toIndex,
                mFromPiercePos, mToPiercePos);

            return value;
        }

        private static int ValidateKiller(MoveList move)
        {
            return _arrPiecePossition[move.FromIndex] != move.FromPiecePossition
                ? -1
                : _arrPiecePossition[move.ToIndex] != move.ToPiecePossition
                    ? -1
                    : _arrValue[move.FromIndex] != move.FromValue
                        ? -1
                        : _arrSide[move.FromIndex] != move.FromSide
                            ? -1
                            : _arrPiece[_arrPiecePossition[move.FromIndex]].Status == 1
                                ? -1
                                : _arrSide[move.ToIndex] != 0
                                    ? -1
                                    : IsMoveAble(move.FromIndex, move.ToIndex, move.FromSide, move.FromValue);
        }

        private static int ValidateHashMove(MoveList move)
        {
            return _arrPiecePossition[move.FromIndex] != move.FromPiecePossition
                ? -1
                : _arrPiecePossition[move.ToIndex] != move.ToPiecePossition
                    ? -1
                    : _arrValue[move.FromIndex] != move.FromValue
                        ? -1
                        : _arrSide[move.FromIndex] != move.FromSide
                            ? -1
                            : _arrPiece[_arrPiecePossition[move.FromIndex]].Status == 1
                                ? -1
                                : IsMoveAble(move.FromIndex, move.ToIndex, move.FromSide, move.FromValue);
        }

        private static bool IsAttackedPseudoSquareBySquare(int toIndex, int fromIndex, int side, int value)
        {
            if (side == 0 || value == 0) return false;

            if (AttackArray[toIndex - fromIndex + 128] == 0) return false;

            switch (value)
            {
                case 1:
                    break;
                case 2:
                    if (AttackArray[toIndex - fromIndex + 128] == AttackKqBbP ||
                        AttackArray[toIndex - fromIndex + 128] == AttackKqBwP ||
                        AttackArray[toIndex - fromIndex + 128] == AttackQb)
                        return true;

                    break;
                case 3:
                    if (AttackArray[toIndex - fromIndex + 128] == AttackN) return true;

                    break;
                case 4:
                    var r = toIndex % 16 == fromIndex % 16;
                    r = r || toIndex / 16 == fromIndex / 16;
                    if (r && (AttackArray[toIndex - fromIndex + 128] == AttackKqr ||
                              AttackArray[toIndex - fromIndex + 128] == AttackQr))
                        return true;

                    break;
                case 5:
                    if (AttackArray[toIndex - fromIndex + 128] == AttackKqBbP ||
                        AttackArray[toIndex - fromIndex + 128] == AttackKqBwP ||
                        AttackArray[toIndex - fromIndex + 128] == AttackQb ||
                        AttackArray[toIndex - fromIndex + 128] == AttackKqr ||
                        AttackArray[toIndex - fromIndex + 128] == AttackQr)
                        return true;

                    break;
                case 6:
                    if (AttackArray[toIndex - fromIndex + 128] == AttackKqBbP ||
                        AttackArray[toIndex - fromIndex + 128] == AttackKqBwP ||
                        AttackArray[toIndex - fromIndex + 128] == AttackKqr
                       )
                        return true;

                    break;
            }

            return false;
        }

        private static int IsMoveAble(int fromIndex, int toIndex, int fromSide, int fromValue)
        {
            if (_arrValue[fromIndex] != fromValue) return -1;

            if (_arrSide[fromIndex] != fromSide) return -2;

            if (_arrPiecePossition[fromIndex] == 0) return -3;

            if (_arrPiece[_arrPiecePossition[fromIndex]].Status == 1) return -4;

            if (AttackArray[toIndex - fromIndex + 128] == 0 && fromValue != 1) return -5;

            var r = IsAttackedPseudoSquareBySquare(toIndex, fromIndex, fromSide, fromValue);

            switch (fromValue)
            {
                case 1:
                    return CheckPawnIsMoveAble(fromIndex, toIndex, fromSide);
                case 2:
                    if (r) return CheckBishopRookQueenIsMoveAble(fromIndex, toIndex, fromValue);

                    break;
                case 3:
                    if (r) return CheckKnightKingIsMoveAble(fromIndex, toIndex, fromSide, fromValue);

                    break;
                case 4:
                    if (r) return CheckBishopRookQueenIsMoveAble(fromIndex, toIndex, fromValue);

                    break;
                case 5:
                    if (r) return CheckBishopRookQueenIsMoveAble(fromIndex, toIndex, fromValue);

                    break;
                case 6:
                    return CheckKnightKingIsMoveAble(fromIndex, toIndex, fromSide, fromValue);
            }

            return -1;
        }

        private static int CheckKnightKingIsMoveAble(int fromIndex, int toIndex, int fromSide, int fromValue)
        {
            var delta = KnightDelta;

            if (fromValue == 6) delta = KingDelta;

            if (fromValue == 6)
                switch (fromIndex)
                {
                    case 116 when toIndex == 118 && _isFirstKingWhiteMove && _isFirstRookWhiteRightMove &&
                                  _arrValue[117] == 0 && _arrValue[118] == 0 && _arrValue[119] == 4 &&
                                  _arrSide[119] == fromSide &&
                                  _arrPiecePossition[119] == 24 && _arrPiece[24].Status == 0 && !IsInCheck(1) &&
                                  !HaveSomePieceCanMoveToIndex(2, 117) && !HaveSomePieceCanMoveToIndex(2, 118):
                        return 3;
                    case 116 when toIndex == 114 && _isFirstKingWhiteMove && _isFirstRookWhiteLeftMove &&
                                  _arrValue[115] == 0 && _arrValue[114] == 0 && _arrValue[113] == 0 &&
                                  _arrValue[112] == 4 &&
                                  _arrSide[112] == fromSide && _arrPiecePossition[112] == 17 &&
                                  _arrPiece[17].Status == 0 &&
                                  !IsInCheck(1) && !HaveSomePieceCanMoveToIndex(2, 115) &&
                                  !HaveSomePieceCanMoveToIndex(2, 114):
                        return 4;
                    case 4 when toIndex == 6 && _isFirstKingBlackMove && _isFirstRookBlackRightMove &&
                                _arrValue[5] == 0 && _arrValue[6] == 0 && _arrValue[7] == 4 &&
                                _arrSide[7] == fromSide &&
                                _arrPiecePossition[7] == 8 && _arrPiece[8].Status == 0 && !IsInCheck(2) &&
                                !HaveSomePieceCanMoveToIndex(1, 5) && !HaveSomePieceCanMoveToIndex(1, 6):
                        return 5;
                    case 4 when toIndex == 2 && _isFirstKingBlackMove && _isFirstRookBlackLeftMove &&
                                _arrValue[3] == 0 && _arrValue[2] == 0 && _arrValue[1] == 0 && _arrValue[0] == 4 &&
                                _arrSide[0] == fromSide && _arrPiecePossition[0] == 1 && _arrPiece[1].Status == 0 &&
                                !IsInCheck(2) &&
                                !HaveSomePieceCanMoveToIndex(1, 3) && !HaveSomePieceCanMoveToIndex(1, 2):
                        return 6;
                }

            for (var j = 0; j < 8; j++)
                if (delta[j] != 0 && toIndex == fromIndex + delta[j])
                    if ((toIndex & 0x88) == 0 && _arrSide[toIndex] != _arrSide[fromIndex])
                        return 0;

            return -1;
        }

        private static bool IsInCheck(int side)
        {
            var fromIndex = 1;
            var toIndex = 17;
            int kingIndex;

            if (side == 2)
            {
                fromIndex = 17;
                toIndex = 33;
                kingIndex = 5;

                if (_arrPiece[5].Status != 0) return false;
            }
            else
            {
                kingIndex = 21;

                if (_arrPiece[21].Status != 0) return false;
            }

            for (var k = fromIndex; k < toIndex; k++)
                if (_arrPiece[k].Status == 0)
                    if (IsMoveAble(_arrPiece[k].Index, _arrPiece[kingIndex].Index, _arrPiece[k].Side,
                            _arrPiece[k].Value) >=
                        0)
                        return true;

            return false;
        }

        private static bool HaveSomePieceCanMoveToIndex(int side, int index)
        {
            var fromIndex = 17;
            var toIndex = 33;

            if (side == 2)
            {
                fromIndex = 1;
                toIndex = 17;
            }

            for (var k = fromIndex; k < toIndex; k++)
                if (_arrPiece[k].Status == 0 && _arrPiece[k].Value != 6)
                    if (IsMoveAble(_arrPiece[k].Index, index, _arrPiece[k].Side, _arrPiece[k].Value) >= 0)
                        return true;

            return false;
        }

        private static int CheckBishopRookQueenIsMoveAble(int fromIndex, int toIndex, int fromValue)
        {
            var delta = BishopDelta;

            switch (fromValue)
            {
                case 2:
                    delta = BishopDelta;
                    break;
                case 4:
                    delta = RookDelta;
                    break;
                case 5:
                    delta = QueenDelta;
                    break;
            }

            for (var j = 0; j < 8; j++)
                if (delta[j] != 0)
                {
                    var tempToIndex = fromIndex + delta[j];

                    while ((tempToIndex & 0x88) == 0 && _arrSide[tempToIndex] == 0)
                    {
                        if (tempToIndex == toIndex) return 0;

                        tempToIndex += delta[j];
                    }

                    if ((tempToIndex & 0x88) == 0 && _arrSide[tempToIndex] != _arrSide[fromIndex] &&
                        tempToIndex == toIndex) return 0;
                }

            return -1;
        }

        private static int CheckPawnIsMoveAble(int fromIndex, int toIndex, int fromSide)
        {
            if (fromSide == 1)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (WpawnDelta[j] != 0 && fromIndex + WpawnDelta[j] == toIndex)
                    {
                        if (fromIndex > 95 && WpawnDelta[j] == -32 && (toIndex & 0x88) == 0 && _arrSide[toIndex] == 0 &&
                            _arrSide[fromIndex - 16] == 0) return 0;

                        if ((toIndex & 0x88) == 0 && WpawnDelta[j] == -16 && _arrSide[toIndex] == 0)
                            return toIndex <= 7 ? 1 : 0;

                        if ((toIndex & 0x88) == 0 && -WpawnDelta[j] % 16 != 0 && _arrSide[toIndex] != 0 &&
                            _arrSide[toIndex] != fromSide) return toIndex <= 7 ? 1 : 0;
                    }

                    if ((toIndex & 0x88) == 0 && toIndex == _enPassant && _enPassant < 40 && _enPassant != -1) return 2;
                }
            }
            else
            {
                for (var j = 0; j < 4; j++)
                    if (BpawnDelta[j] != 0 && fromIndex + BpawnDelta[j] == toIndex)
                    {
                        if (fromIndex <= 23 && BpawnDelta[j] == 32 && (toIndex & 0x88) == 0 && _arrSide[toIndex] == 0 &&
                            _arrSide[fromIndex + 16] == 0) return 0;

                        if ((toIndex & 0x88) == 0 && BpawnDelta[j] == 16 && _arrSide[toIndex] == 0)
                            return toIndex >= 112 ? 1 : 0;

                        if ((toIndex & 0x88) == 0 && BpawnDelta[j] % 16 != 0 && _arrSide[toIndex] != 0 &&
                            _arrSide[toIndex] != fromSide) return toIndex >= 112 ? 1 : 0;
                    }

                if ((toIndex & 0x88) == 0 && toIndex == _enPassant && _enPassant >= 80 && _enPassant != -1) return 2;
            }

            return -1;
        }

        private static void MakeNullMove(int fromSide, int fromValue, int toValue)
        {
            _historyDat[_countHistoryDat].Enpasant = _enPassant;
            _historyDat[_countHistoryDat].Fitty = _fifty;
            _historyDat[_countHistoryDat].Zobrist = _hashZobrist;
            _countHistoryDat++;
            _pvPly++;

            if (toValue != 0 || fromValue == 1)
                _fifty = 0;
            else
                _fifty++;

            _enPassant = -1;
            _hashZobrist = GetZobrist(3 - fromSide);
        }

        private static void UnMakeNullMove()
        {
            _countHistoryDat--;
            _pvPly--;
            _enPassant = _historyDat[_countHistoryDat].Enpasant;
            _fifty = _historyDat[_countHistoryDat].Fitty;
            _hashZobrist = _historyDat[_countHistoryDat].Zobrist;
        }

        private static void MakeMove(int moveType, byte promotionValue, int fromSide, int fromValue,
            int toValue, int fromIndex, int toIndex, int fromPiecePossition, int toPiecePossition)
        {
            _historyDat[_countHistoryDat].Enpasant = _enPassant;
            _historyDat[_countHistoryDat].Fitty = _fifty;
            _historyDat[_countHistoryDat].Zobrist = _hashZobrist;
            _countHistoryDat++;
            _pvPly++;

            if (toValue != 0)
                _fifty = 0;
            else
                _fifty++;

            _enPassant = Math.Abs(toIndex - fromIndex) == 32 && fromValue == 1
                ? fromIndex + (toIndex - fromIndex) / 2
                : -1;

            if (fromValue == 6)
            {
                if (fromSide == 1)
                {
                    if (moveType == 3 || moveType == 4)
                    {
                        if (moveType == 3)
                        {
                            _isFirstKingWhiteMove = false;
                            _isFirstRookWhiteRightMove = false;
                            _arrSide[116] = 0;
                            _arrValue[116] = 0;
                            _arrPiecePossition[116] = 0;

                            _arrSide[118] = fromSide;
                            _arrValue[118] = 6;
                            _arrPiecePossition[118] = 21;
                            _arrPiece[21].Index = 118;

                            _arrSide[119] = 0;
                            _arrValue[119] = 0;
                            _arrPiecePossition[119] = 0;

                            _arrSide[117] = fromSide;
                            _arrValue[117] = 4;

                            _arrPiecePossition[117] = 24;
                            _arrPiece[24].Index = 117;

                            _whiteCastle = CastleNone;
                        }
                        else
                        {
                            _isFirstKingWhiteMove = false;
                            _isFirstRookWhiteLeftMove = false;
                            _arrSide[116] = 0;
                            _arrValue[116] = 0;
                            _arrPiecePossition[116] = 0;

                            _arrSide[114] = fromSide;
                            _arrValue[114] = 6;
                            _arrPiecePossition[114] = 21;
                            _arrPiece[21].Index = 114;

                            _arrSide[112] = 0;
                            _arrValue[112] = 0;
                            _arrPiecePossition[112] = 0;

                            _arrSide[115] = fromSide;
                            _arrValue[115] = 4;
                            _arrPiecePossition[115] = 17;
                            _arrPiece[17].Index = 115;

                            _whiteCastle = CastleNone;
                        }
                    }
                    else
                    {
                        _isFirstKingWhiteMove = false;
                        _arrSide[toIndex] = fromSide;
                        _arrValue[toIndex] = fromValue;
                        _arrSide[fromIndex] = 0;
                        _arrValue[fromIndex] = 0;

                        _arrPiece[toPiecePossition].Status = 1;
                        _arrPiece[fromPiecePossition].Index = toIndex;

                        _arrPiecePossition[toIndex] = fromPiecePossition;
                        _arrPiecePossition[fromIndex] = 0;

                        _whiteCastle = CastleNone;
                    }
                }
                else
                {
                    if (moveType == 5 || moveType == 6)
                    {
                        if (moveType == 5)
                        {
                            _isFirstKingBlackMove = false;
                            _isFirstRookBlackRightMove = false;
                            _arrSide[4] = 0;
                            _arrValue[4] = 0;
                            _arrPiecePossition[4] = 0;

                            _arrSide[6] = fromSide;
                            _arrValue[6] = 6;
                            _arrPiecePossition[6] = 5;
                            _arrPiece[5].Index = 6;

                            _arrSide[7] = 0;
                            _arrValue[7] = 0;
                            _arrPiecePossition[7] = 0;

                            _arrSide[5] = fromSide;
                            _arrValue[5] = 4;
                            _arrPiecePossition[5] = 8;
                            _arrPiece[8].Index = 5;

                            _blackCastle = CastleNone;
                        }
                        else
                        {
                            _isFirstKingBlackMove = false;
                            _isFirstRookBlackLeftMove = false;
                            _arrSide[4] = 0;
                            _arrValue[4] = 0;
                            _arrPiecePossition[4] = 0;

                            _arrSide[2] = fromSide;
                            _arrValue[2] = 6;
                            _arrPiecePossition[2] = 5;
                            _arrPiece[5].Index = 2;

                            _arrSide[0] = 0;
                            _arrValue[0] = 0;
                            _arrPiecePossition[0] = 0;

                            _arrSide[3] = fromSide;
                            _arrValue[3] = 4;
                            _arrPiecePossition[3] = 1;
                            _arrPiece[1].Index = 3;

                            _blackCastle = CastleNone;
                        }
                    }
                    else
                    {
                        _isFirstKingBlackMove = false;
                        _arrSide[toIndex] = fromSide;
                        _arrValue[toIndex] = fromValue;
                        _arrSide[fromIndex] = 0;
                        _arrValue[fromIndex] = 0;

                        _arrPiece[toPiecePossition].Status = 1;

                        _arrPiece[fromPiecePossition].Index = toIndex;

                        _arrPiecePossition[toIndex] = fromPiecePossition;
                        _arrPiecePossition[fromIndex] = 0;
                        _blackCastle = CastleNone;
                    }
                }
            }
            else
            {
                _arrSide[toIndex] = fromSide;
                _arrValue[toIndex] = fromValue;
                _arrSide[fromIndex] = 0;
                _arrValue[fromIndex] = 0;

                _arrPiece[toPiecePossition].Status = 1;
                _arrPiece[fromPiecePossition].Index = toIndex;

                _arrPiecePossition[toIndex] = fromPiecePossition;
                _arrPiecePossition[fromIndex] = 0;

                switch (moveType)
                {
                    case 1:
                        _arrValue[toIndex] = promotionValue;
                        _arrPiece[fromPiecePossition].Value = promotionValue;
                        break;
                    case 2 when fromSide == 1:
                    {
                        if (Math.Abs(fromIndex - toIndex) == 15)
                        {
                            _arrValue[fromIndex + 1] = 0;
                            _arrSide[fromIndex + 1] = 0;
                            _arrPiece[_arrPiecePossition[fromIndex + 1]].Status = 1;
                            _arrPiecePossition[fromIndex + 1] = 0;
                        }

                        if (Math.Abs(fromIndex - toIndex) == 17)
                        {
                            _arrValue[fromIndex - 1] = 0;
                            _arrSide[fromIndex - 1] = 0;
                            _arrPiece[_arrPiecePossition[fromIndex - 1]].Status = 1;
                            _arrPiecePossition[fromIndex - 1] = 0;
                        }

                        break;
                    }
                    case 2:
                    {
                        if (Math.Abs(fromIndex - toIndex) == 17)
                        {
                            _arrValue[fromIndex + 1] = 0;
                            _arrSide[fromIndex + 1] = 0;
                            _arrPiece[_arrPiecePossition[fromIndex + 1]].Status = 1;
                            _arrPiecePossition[fromIndex + 1] = 0;
                        }

                        if (Math.Abs(fromIndex - toIndex) == 15)
                        {
                            _arrValue[fromIndex - 1] = 0;
                            _arrSide[fromIndex - 1] = 0;
                            _arrPiece[_arrPiecePossition[fromIndex - 1]].Status = 1;
                            _arrPiecePossition[fromIndex - 1] = 0;
                        }

                        break;
                    }
                }

                switch (fromPiecePossition)
                {
                    case 8:
                        _isFirstRookBlackRightMove = false;
                        if (_blackCastle != CastleNone) _blackCastle = CastleLong;

                        break;
                    case 1:
                        _isFirstRookBlackLeftMove = false;
                        if (_blackCastle != CastleNone) _blackCastle = CastleShort;

                        break;
                    case 24:
                        _isFirstRookWhiteRightMove = false;
                        if (_whiteCastle != CastleNone) _whiteCastle = CastleLong;

                        break;
                    case 17:
                        _isFirstRookWhiteLeftMove = false;
                        if (_whiteCastle != CastleNone) _whiteCastle = CastleShort;

                        break;
                }
            }

            _hashZobrist = GetZobrist(3 - fromSide);
        }

        private static void UnMakeMove(int moveType, int fromSide, int fromValue, int toSide,
            int toValue, int fromIndex, int toIndex, int fromPiecePossition, int toPiecePossition)
        {
            _countHistoryDat--;
            _pvPly--;

            _enPassant = _historyDat[_countHistoryDat].Enpasant;
            _fifty = _historyDat[_countHistoryDat].Fitty;
            _hashZobrist = _historyDat[_countHistoryDat].Zobrist;

            if (fromValue == 6)
            {
                if (fromSide == 1)
                {
                    if (moveType == 3 || moveType == 4)
                    {
                        if (moveType == 3)
                        {
                            _isFirstKingWhiteMove = true;
                            _isFirstRookWhiteRightMove = true;
                            _whiteCastle = CastleShort;
                            _arrSide[116] = fromSide;
                            _arrValue[116] = 6;
                            _arrPiecePossition[116] = 21;
                            _arrPiece[21].Index = 116;

                            _arrSide[118] = 0;
                            _arrValue[118] = 0;
                            _arrPiecePossition[118] = 0;

                            _arrSide[119] = fromSide;
                            _arrValue[119] = 4;
                            _arrPiecePossition[119] = 24;
                            _arrPiece[24].Index = 119;

                            _arrSide[117] = 0;
                            _arrValue[117] = 0;
                            _arrPiecePossition[117] = 0;
                        }
                        else
                        {
                            _isFirstKingWhiteMove = true;
                            _isFirstRookWhiteLeftMove = true;
                            _whiteCastle = CastleLong;
                            _arrSide[116] = fromSide;
                            _arrValue[116] = 6;
                            _arrPiecePossition[116] = 21;
                            _arrPiece[21].Index = 116;

                            _arrSide[114] = 0;
                            _arrValue[114] = 0;
                            _arrPiecePossition[114] = 0;

                            _arrSide[112] = fromSide;
                            _arrValue[112] = 4;
                            _arrPiecePossition[112] = 17;
                            _arrPiece[17].Index = 112;

                            _arrSide[115] = 0;
                            _arrValue[115] = 0;
                            _arrPiecePossition[115] = 0;
                        }
                    }
                    else
                    {
                        if (!_kingRookMovedList.ContainsKey(21))
                        {
                            _isFirstKingWhiteMove = true;
                            _whiteCastle = CastleBoth;
                        }

                        _arrSide[fromIndex] = fromSide;
                        _arrValue[fromIndex] = fromValue;
                        _arrSide[toIndex] = toSide;
                        _arrValue[toIndex] = toValue;

                        _arrPiecePossition[fromIndex] = fromPiecePossition;
                        _arrPiecePossition[toIndex] = toPiecePossition;

                        _arrPiece[toPiecePossition].Status = 0;
                        _arrPiece[fromPiecePossition].Index = fromIndex;
                    }
                }
                else
                {
                    if (moveType == 5 || moveType == 6)
                    {
                        if (moveType == 5)
                        {
                            _isFirstKingBlackMove = true;
                            _isFirstRookBlackRightMove = true;
                            _blackCastle = CastleShort;

                            _arrSide[4] = fromSide;
                            _arrValue[4] = 6;
                            _arrPiecePossition[4] = 5;
                            _arrPiece[5].Index = 4;

                            _arrSide[6] = 0;
                            _arrValue[6] = 0;
                            _arrPiecePossition[6] = 0;

                            _arrSide[5] = 0;
                            _arrValue[5] = 0;
                            _arrPiecePossition[5] = 0;

                            _arrSide[7] = fromSide;
                            _arrValue[7] = 4;
                            _arrPiecePossition[7] = 8;
                            _arrPiece[8].Index = 7;
                        }
                        else
                        {
                            _isFirstKingBlackMove = true;
                            _isFirstRookBlackLeftMove = true;
                            _blackCastle = CastleLong;
                            _arrSide[4] = fromSide;
                            _arrValue[4] = 6;
                            _arrPiecePossition[4] = 5;
                            _arrPiece[5].Index = 4;

                            _arrSide[2] = 0;
                            _arrValue[2] = 0;
                            _arrPiecePossition[2] = 0;

                            _arrSide[0] = fromSide;
                            _arrValue[0] = 4;
                            _arrPiecePossition[0] = 1;
                            _arrPiece[1].Index = 0;

                            _arrSide[3] = 0;
                            _arrValue[3] = 0;
                            _arrPiecePossition[3] = 0;
                        }
                    }
                    else
                    {
                        if (!_kingRookMovedList.ContainsKey(5))
                        {
                            _isFirstKingBlackMove = true;
                            _blackCastle = CastleBoth;
                        }

                        _arrSide[fromIndex] = fromSide;
                        _arrValue[fromIndex] = fromValue;
                        _arrSide[toIndex] = toSide;
                        _arrValue[toIndex] = toValue;

                        _arrPiecePossition[fromIndex] = fromPiecePossition;
                        _arrPiecePossition[toIndex] = toPiecePossition;
                        _arrPiece[fromPiecePossition].Index = fromIndex;

                        _arrPiece[toPiecePossition].Status = 0;
                    }
                }
            }
            else
            {
                _arrSide[fromIndex] = fromSide;
                _arrValue[fromIndex] = fromValue;
                _arrSide[toIndex] = toSide;
                _arrValue[toIndex] = toValue;

                _arrPiecePossition[fromIndex] = fromPiecePossition;

                if (moveType != 2) _arrPiecePossition[toIndex] = toPiecePossition;

                _arrPiece[fromPiecePossition].Index = fromIndex;

                if (moveType != 2)
                {
                    _arrPiece[toPiecePossition].Status = 0;
                    _arrPiece[toPiecePossition].Index = toIndex;
                }

                switch (moveType)
                {
                    case 1:
                        _arrValue[toIndex] = toValue;
                        _arrValue[fromIndex] = 1;
                        _arrPiece[fromPiecePossition].Value = 1;
                        break;
                    case 2:
                    {
                        _arrPiecePossition[toIndex] = 0;

                        if (fromSide == 1)
                        {
                            if (Math.Abs(fromIndex - toIndex) == 15)
                            {
                                _arrValue[fromIndex + 1] = 1;
                                _arrSide[fromIndex + 1] = 2;
                                _arrPiece[toPiecePossition].Status = 0;
                                _arrPiecePossition[fromIndex + 1] = toPiecePossition;
                            }

                            if (Math.Abs(fromIndex - toIndex) == 17)
                            {
                                _arrValue[fromIndex - 1] = 1;
                                _arrSide[fromIndex - 1] = 2;
                                _arrPiece[toPiecePossition].Status = 0;
                                _arrPiecePossition[fromIndex - 1] = toPiecePossition;
                            }
                        }
                        else
                        {
                            if (Math.Abs(fromIndex - toIndex) == 17)
                            {
                                _arrValue[fromIndex + 1] = 1;
                                _arrSide[fromIndex + 1] = 1;
                                _arrPiece[toPiecePossition].Status = 0;
                                _arrPiecePossition[fromIndex + 1] = toPiecePossition;
                            }

                            if (Math.Abs(fromIndex - toIndex) == 15)
                            {
                                _arrValue[fromIndex - 1] = 1;
                                _arrSide[fromIndex - 1] = 1;
                                _arrPiece[toPiecePossition].Status = 0;
                                _arrPiecePossition[fromIndex - 1] = toPiecePossition;
                            }
                        }

                        break;
                    }
                }

                switch (fromPiecePossition)
                {
                    case 8:
                        if (!_kingRookMovedList.ContainsKey(8))
                        {
                            _isFirstRookBlackRightMove = true;

                            if (_blackCastle == CastleLong) _blackCastle = CastleBoth;

                            if (_blackCastle == CastleNone) _blackCastle = CastleShort;
                        }

                        break;
                    case 1:
                        if (!_kingRookMovedList.ContainsKey(1))
                        {
                            _isFirstRookBlackLeftMove = true;

                            if (_blackCastle == CastleShort) _blackCastle = CastleBoth;

                            if (_blackCastle == CastleNone) _blackCastle = CastleLong;
                        }

                        break;
                    case 24:
                        if (!_kingRookMovedList.ContainsKey(24))
                        {
                            _isFirstRookWhiteRightMove = true;

                            if (_whiteCastle == CastleLong) _whiteCastle = CastleBoth;

                            if (_whiteCastle == CastleNone) _whiteCastle = CastleShort;
                        }

                        break;
                    case 17:
                        if (!_kingRookMovedList.ContainsKey(17))
                        {
                            _isFirstRookWhiteLeftMove = true;

                            if (_whiteCastle == CastleShort) _whiteCastle = CastleBoth;

                            if (_whiteCastle == CastleNone) _whiteCastle = CastleLong;
                        }

                        break;
                }
            }
        }

        private static void WriteAllVariableToCheck()
        {
            var sw = new StreamWriter("arrSide.txt");

            for (var i = 0; i < _arrSide.Length; i++)
            {
                sw.Write(_arrSide[i] + " ");

                if ((i + 1) % 16 == 0) sw.WriteLine();
            }

            sw.Close();
            sw = new StreamWriter("arrValue.txt");

            for (var i = 0; i < _arrValue.Length; i++)
            {
                sw.Write(_arrValue[i] + " ");

                if ((i + 1) % 16 == 0) sw.WriteLine();
            }

            sw.Close();
            sw = new StreamWriter("arrPiecePossition.txt");

            for (var i = 0; i < _arrPiecePossition.Length; i++)
            {
                sw.Write(_arrPiecePossition[i].ToString().ToLower());

                sw.Write(_arrPiecePossition[i] > 10 ? " " : "   ");

                if ((i + 1) % 16 == 0) sw.WriteLine();
            }

            sw.Close();
            sw = new StreamWriter("arrPiece.txt");

            for (var i = 1; i < 33; i++)
                sw.WriteLine(i + " index:" + _arrPiece[i].Index + " status:" + _arrPiece[i].Status);

            sw.Close();
        }

        private static void LogFile(string error)
        {
            var sw = new StreamWriter("Error" + DateTime.Now.Ticks + ".txt");
            sw.WriteLine(error);
            sw.Close();
        }

        public static string GuIgetEngineMove()
        {
            return _guiEngineMove;
        }

        public static bool GuIgetEngineThink()
        {
            return _engineThink;
        }

        public static string GuIgetPvLineString()
        {
            return _guiThinkerPvLineString;
        }

        public static int GuiIsMoveAbleFromString(string move)
        {
            var from = Square(move);
            var to = Square(move.Substring(2));

            var mMoveType = IsMoveAble(from, to, _arrSide[from], _arrValue[from]);

            return mMoveType;
        }

        public static bool GuiCheckUserMove(string move)
        {
            var from = Square(move);
            var to = Square(move.Substring(2));

            var mMoveType = IsMoveAble(from, to, _arrSide[from], _arrValue[from]);
            _guiMoveTypeHuman = mMoveType;

            if (mMoveType < 0) return false;
            byte promotionValue = 0;

            if (mMoveType == 1)
                if (move.Length == 5)
                {
                    switch (move.Substring(4, 1).ToLower())
                    {
                        case "n":
                            promotionValue = 3;
                            break;
                        case "b":
                            promotionValue = 2;
                            break;
                        case "r":
                            promotionValue = 4;
                            break;
                        case "q":
                            promotionValue = 5;
                            break;
                    }

                    _guiMovePromotionValue = promotionValue;
                }

            _countMainMoveList++;

            if (!_kingRookMovedList.ContainsKey(_arrPiecePossition[from]))
                _kingRookMovedList.Add(_arrPiecePossition[from], "1");

            _arrMoveList[1].MoveType = mMoveType;
            _arrMoveList[1].PromotionValue = promotionValue;
            _arrMoveList[1].FromSide = _arrSide[from];
            _arrMoveList[1].FromValue = _arrValue[from];
            _arrMoveList[1].ToSide = _arrSide[to];
            _arrMoveList[1].ToValue = _arrValue[to];
            _arrMoveList[1].FromIndex = from;
            _arrMoveList[1].ToIndex = to;
            _arrMoveList[1].FromPiecePossition = _arrPiecePossition[from];
            _arrMoveList[1].ToPiecePossition = _arrPiecePossition[to];
            _arrMainMoveList[_countMainMoveList] = _arrMoveList[1];

            MakeMove(mMoveType, promotionValue, _arrSide[from], _arrValue[from], _arrValue[to], from,
                to,
                _arrPiecePossition[from], _arrPiecePossition[to]);

            if (IsInCheck(_arrMoveList[1].FromSide))
            {
                _countMainMoveList--;
                _kingRookMovedList.Remove(_arrMoveList[1].FromPiecePossition);

                UnMakeMove(mMoveType, _arrMoveList[1].FromSide, _arrMoveList[1].FromValue,
                    _arrMoveList[1].ToSide, _arrMoveList[1].ToValue, from, to, _arrMoveList[1].FromPiecePossition,
                    _arrMoveList[1].ToPiecePossition);

                return false;
            }

            _moveHashCode[_countMainMoveList] = GetZobrist(3 - _arrSide[to]);
            _strMainMoveList += move;

            return true;
        }

        public static int GUIgetValueOnSquare(string indexStr)
        {
            var index = int.Parse(indexStr);
            var index128 = index / 8 * 16 + index % 8;
            return _arrValue[index128];
        }

        public static int GUIgetSideOnSquare(string indexStr)
        {
            var index = int.Parse(indexStr);
            var index128 = index / 8 * 16 + index % 8;
            return _arrSide[index128];
        }

        public static int GuIgetMoveTypeHuman()
        {
            return _guiMoveTypeHuman;
        }

        public static int GuIgetMoveTypeEngine()
        {
            return _guiMoveTypeEngine;
        }

        public static int GuIgetMovePromotionValue()
        {
            return _guiMovePromotionValue;
        }

        public static int GuIgetEngineWin()
        {
            return _guiEngineWin;
        }

        public static int GuIgetMoveTypePgn()
        {
            return _guiMoveTypePgn;
        }

        public static int GuIgetMoveFromSidePgn()
        {
            return _guiMoveFromSidePgn;
        }

        public static int GuIgetMoveFromValuePgn()
        {
            return _guiMoveFromValuePgn;
        }

        public static int GuIgetMoveToSidePgn()
        {
            return _guiMoveToSidePgn;
        }

        public static int GuIgetMoveToValuePgn()
        {
            return _guiMoveToValuePgn;
        }

        public static Hashtable GuIgetAllTheMoveFromSide(int side, ref Hashtable hshTableMoveTypeInPgnFile,
            ref Hashtable hshTablePromotionValueInPgnFile)
        {
            var result = new Hashtable();
            var allMovesAble = new MoveList[200];
            var fromI = 1;
            var toI = 17;

            if (side == 1)
            {
                fromI = 17;
                toI = 33;
            }

            var totalMovesAble = 0;
            int i;

            for (i = fromI; i < toI; i++)
                if (_arrPiece[i].Status == 0 && _arrPiece[i].Side == side)
                {
                    GenerateCaptureMoves(i, _arrPiece[i].Side, _arrPiece[i].Value, _arrPiece[i].Index,
                        ref allMovesAble,
                        ref totalMovesAble);

                    GenerateMoves(i, _arrPiece[i].Side, _arrPiece[i].Value, _arrPiece[i].Index, ref allMovesAble,
                        ref totalMovesAble);
                }

            string[] pieceName = { "", "", "B", "N", "R", "Q", "K" };
            string[] colName = { "a", "b", "c", "d", "e", "f", "g", "h" };

            for (i = 1; i <= totalMovesAble; i++)
            {
                var excolname = "";
                var exrowname = "";

                for (var j = 1; j <= totalMovesAble; j++)
                    if (i != j && allMovesAble[i].FromValue == allMovesAble[j].FromValue &&
                        allMovesAble[i].ToIndex == allMovesAble[j].ToIndex)
                    {
                        if (allMovesAble[i].FromIndex / 16 == allMovesAble[j].FromIndex / 16 ||
                            allMovesAble[i].FromIndex % 16 != allMovesAble[j].FromIndex % 16)
                        {
                            excolname = colName[allMovesAble[i].FromIndex % 16];
                            break;
                        }

                        if (allMovesAble[i].FromIndex % 16 != allMovesAble[j].FromIndex % 16) continue;
                        var row = 8 - allMovesAble[i].FromIndex / 16;
                        exrowname = row.ToString();
                        break;
                    }

                var fullMove = "";
                var realMove = "";
                var cap = "";
                var rowDes = 8 - allMovesAble[i].ToIndex / 16;
                var rowFrom = 8 - allMovesAble[i].FromIndex / 16;

                if (allMovesAble[i].ToValue != 0) cap = "x";

                realMove += colName[allMovesAble[i].FromIndex % 16] + rowFrom + colName[allMovesAble[i].ToIndex % 16] +
                            rowDes;

                switch (allMovesAble[i].MoveType)
                {
                    case 0:
                        if (allMovesAble[i].FromValue == 1)
                        {
                            if (allMovesAble[i].ToValue != 0)
                                fullMove += colName[allMovesAble[i].FromIndex % 16] + "x" +
                                            colName[allMovesAble[i].ToIndex % 16] + rowDes;
                            else
                                fullMove += colName[allMovesAble[i].ToIndex % 16] + rowDes;
                        }
                        else
                        {
                            fullMove += pieceName[allMovesAble[i].FromValue] + excolname + exrowname + cap +
                                        colName[allMovesAble[i].ToIndex % 16] + rowDes;
                        }

                        break;
                    case 2:

                        fullMove += colName[allMovesAble[i].FromIndex % 16] + "x" +
                                    colName[allMovesAble[i].ToIndex % 16] + rowDes;

                        break;
                    case 1:
                        if (allMovesAble[i].ToValue != 0)
                            fullMove += colName[allMovesAble[i].FromIndex % 16] + "x" +
                                        colName[allMovesAble[i].ToIndex % 16] + rowDes +
                                        pieceName[allMovesAble[i].PromotionValue];
                        else
                            fullMove += colName[allMovesAble[i].FromIndex % 16] + rowDes +
                                        pieceName[allMovesAble[i].PromotionValue];

                        break;
                    case 3:
                        fullMove += "O-O";
                        break;
                    case 4:
                        fullMove += "O-O-O";
                        break;
                    case 5:
                        fullMove += "O-O";
                        break;
                    case 6:
                        fullMove += "O-O-O";
                        break;
                }

                if (!result.ContainsKey(fullMove)) result.Add(fullMove, realMove);

                if (!hshTableMoveTypeInPgnFile.ContainsKey(realMove))
                    hshTableMoveTypeInPgnFile.Add(realMove, allMovesAble[i].MoveType);

                if (!hshTablePromotionValueInPgnFile.ContainsKey(realMove))
                    hshTablePromotionValueInPgnFile.Add(realMove, allMovesAble[i].PromotionValue);
            }

            return result;
        }

        public static void GuIsetUseOpeningBook(bool val)
        {
            _useOpeningBook = val;
        }

        public static void GuiSetMaxDepth(int val)
        {
            _guiMaxLevelDepth = val;

            // if (GUIMaxLevelDepth <= 2)
            // {
            // GUIMaxLevelDepth = 2;
            // }
        }

        private struct TagHashe
        {
            public ulong Key;
            public int Depth;
            public int SideToMove;
            public MoveList BestMove;
            public int HashType;
            public int Val;
        }

        private struct Piece
        {
            public int Index;
            public int Side;
            public int Value;
            public int Status;
            public int AttackKing;
            public int DefenceKing;
        }

        private struct MoveList
        {
            public int FromIndex;
            public int ToIndex;
            public int FromSide;
            public int ToSide;
            public int FromValue;
            public int ToValue;
            public int FromPiecePossition;
            public int ToPiecePossition;
            public int Evaluate;
            public byte PromotionValue;
            public int MoveType;
            public int ToEnpassantIndex;
        }

        private struct StateMove
        {
            public int From;
            public int To;
        }

        private struct HistoryStruc
        {
            public int Enpasant;
            public ulong Zobrist;
            public int Fitty;
        }
    }
}