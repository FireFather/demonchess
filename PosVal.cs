namespace DemonChess
{
    internal class PosVal
    {
        private const int PawnFileOpening = 5;
        private const int KnightCentreOpening = 5;
        private const int KnightCentreEndgame = 5;
        private const int KnightRankOpening = 5;
        private const int KnightTrapped = 100;
        private const int BishopCentreOpening = 2;
        private const int BishopCentreEndgame = 3;
        private const int BishopBackRankOpening = 10;
        private const int BishopDiagonalOpening = 4;
        private const int RookFileOpening = 3;
        private const int QueenCentreOpening = 0;
        private const int QueenCentreEndgame = 4;
        private const int QueenBackRankOpening = 5;
        private const int KingCentreEndgame = 12;
        private const int KingFileOpening = 10;
        private const int KingRankOpening = 10;

        private static readonly int[] PawnFile = { -3, -1, 0, +1, +1, 0, -1, -3 };
        private static readonly int[] KnightLine = { -4, -2, 0, +1, +1, 0, -2, -4 };
        private static readonly int[] KnightRank = { -2, -1, 0, +1, +2, +3, +2, +1 };
        private static readonly int[] BishopLine = { -3, -1, 0, +1, +1, 0, -1, -3 };
        private static readonly int[] RookFile = { -2, -1, 0, +1, +1, 0, -1, -2 };
        private static readonly int[] QueenLine = { -3, -1, 0, +1, +1, 0, -1, -3 };
        private static readonly int[] KingLine = { -3, -1, 0, +1, +1, 0, -1, -3 };
        private static readonly int[] KingFile = { +3, +4, +2, 0, 0, +2, +4, +3 };
        private static readonly int[] KingRank = { +1, 0, -2, -3, -4, -5, -6, -7 };
        public int[,,,] Pst = new int[3, 7, 128, 4];

        public void InitPst()
        {
            Pst = new int[3, 7, 128, 3];

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[1, 1, i, 0] += PawnFile[Square_file(i)] * PawnFileOpening;
                Pst[1, 1, i, 1] = Pst[1, 1, i, 0];
            }

            Pst[1, 1, 83, 0] += 10;
            Pst[1, 1, 84, 0] += 10;
            Pst[1, 1, 67, 0] += 20;
            Pst[1, 1, 68, 0] += 20;
            Pst[1, 1, 51, 0] += 10;
            Pst[1, 1, 52, 0] += 10;

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[2, 1, i, 0] += Pst[1, 1, (7 - i / 16) * 16 + i % 16, 0];
                Pst[2, 1, i, 1] = Pst[2, 1, i, 0];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[1, 3, i, 0] += KnightLine[Square_file(i)] * KnightCentreOpening;
                Pst[1, 3, i, 0] += KnightLine[Square_rank(i)] * KnightCentreOpening;
                Pst[1, 3, i, 1] = Pst[1, 3, i, 0];
                Pst[1, 3, i, 2] += KnightLine[Square_file(i)] * KnightCentreEndgame;
                Pst[1, 3, i, 2] += KnightLine[Square_rank(i)] * KnightCentreEndgame;
            }

            for (var i = 0; i <= 127; i++)
                if ((i & 0x88) == 0)
                    Pst[1, 3, i, 0] += KnightRank[Square_rank(i)] * KnightRankOpening;

            Pst[1, 3, 112, 0] -= KnightTrapped;
            Pst[1, 3, 119, 0] -= KnightTrapped;

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[2, 3, i, 0] += Pst[1, 3, (7 - i / 16) * 16 + i % 16, 0];
                Pst[2, 3, i, 1] += Pst[2, 3, i, 0];
                Pst[2, 3, i, 2] += Pst[1, 3, (7 - i / 16) * 16 + i % 16, 2];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[1, 2, i, 0] += BishopLine[Square_file(i)] * BishopCentreOpening;
                Pst[1, 2, i, 0] += BishopLine[Square_file(i)] * BishopCentreOpening;
                Pst[1, 2, i, 1] = Pst[1, 2, i, 0];
                Pst[1, 2, i, 2] += BishopLine[Square_file(i)] * BishopCentreEndgame;
                Pst[1, 2, i, 2] += BishopLine[Square_file(i)] * BishopCentreEndgame;
            }

            for (var i = 0; i <= 7; i++)
            {
                Pst[1, 2, i, 0] -= BishopBackRankOpening;
                Pst[1, 2, i, 1] = Pst[1, 2, i, 0];
            }

            for (var i = 0; (i & 0x88) == 0; i += 17)
            {
                Pst[1, 2, i, 0] += BishopDiagonalOpening;
                Pst[1, 2, i, 1] += Pst[1, 2, i, 0];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[2, 2, i, 0] += Pst[1, 2, (7 - i / 16) * 16 + i % 16, 0];
                Pst[2, 2, i, 1] += Pst[2, 2, i, 0];
                Pst[2, 2, i, 2] += Pst[1, 2, (7 - i / 16) * 16 + i % 16, 2];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[1, 4, i, 0] += RookFile[Square_file(i)] * RookFileOpening;
                Pst[1, 4, i, 1] = Pst[1, 4, i, 0];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[2, 4, i, 0] += Pst[1, 4, (7 - i / 16) * 16 + i % 16, 0];
                Pst[2, 4, i, 1] += Pst[2, 4, i, 0];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[1, 5, i, 0] += QueenLine[Square_file(i)] * QueenCentreOpening;
                Pst[1, 5, i, 0] += QueenLine[Square_rank(i)] * QueenCentreOpening;
                Pst[1, 5, i, 1] = Pst[1, 5, i, 0];
                Pst[1, 5, i, 2] += QueenLine[Square_file(i)] * QueenCentreEndgame;
                Pst[1, 5, i, 2] += QueenLine[Square_rank(i)] * QueenCentreEndgame;
            }

            for (var i = 0; i <= 7; i++)
            {
                Pst[1, 5, i, 0] -= QueenBackRankOpening;
                Pst[1, 5, i, 1] = Pst[1, 5, i, 0];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[2, 5, i, 0] += Pst[1, 5, (7 - i / 16) * 16 + i % 16, 0];
                Pst[2, 5, i, 1] += Pst[2, 5, i, 0];
                Pst[2, 5, i, 2] += Pst[1, 5, (7 - i / 16) * 16 + i % 16, 2];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[1, 6, i, 2] += KingLine[Square_file(i)] * KingCentreEndgame;
                Pst[1, 6, i, 2] += KingLine[Square_rank(i)] * KingCentreEndgame;
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[1, 6, i, 0] += KingFile[Square_file(i)] * KingFileOpening;
                Pst[1, 6, i, 0] += KingRank[Square_rank(i)] * KingRankOpening;
                Pst[1, 6, i, 1] = Pst[1, 6, i, 0];
            }

            for (var i = 0; i <= 127; i++)
            {
                if ((i & 0x88) != 0) continue;
                Pst[2, 6, i, 0] += Pst[1, 6, (7 - i / 16) * 16 + i % 16, 0];
                Pst[2, 6, i, 1] += Pst[2, 6, i, 0];
                Pst[2, 6, i, 2] += Pst[1, 6, (7 - i / 16) * 16 + i % 16, 2];
            }
        }

        private static int Square_file(int index)
        {
            return index % 16;
        }

        private static int Square_rank(int index)
        {
            return index / 16;
        }
    }
}